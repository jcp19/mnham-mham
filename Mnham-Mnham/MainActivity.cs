using System;

using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Gms.Common;
using Android.Gms.Common.Apis;
using Android.Gms.Location;
using Android.Graphics;
using Android.Locations;
using Android.OS;
using Android.Speech;
using Android.Views;
using Android.Widget;

using Mikepenz.MaterialDrawer;
using Mikepenz.MaterialDrawer.Models;
using Mikepenz.MaterialDrawer.Models.Interfaces;
using Mikepenz.MaterialDrawer.Utils;
using Mikepenz.Typeface;

namespace Mnham_Mnham
{
    [Activity(Label = "Mnham Mnham", MainLauncher = true, Icon = "@drawable/icon", ScreenOrientation = ScreenOrientation.Portrait, Theme = "@style/AppTheme")]
    public class MainActivity : Activity, AccountHeader.IOnAccountHeaderListener, Drawer.IOnDrawerItemClickListener,
        GoogleApiClient.IConnectionCallbacks, GoogleApiClient.IOnConnectionFailedListener
    {
        public static readonly MnhamMnham Facade = new MnhamMnham();

        private AccountHeader cabecalhoDrawer;
        private Drawer drawer;
        private DrawerBuilder drawerBuilder;
        private EditText editTextPesquisa;
        private GoogleApiClient clienteApiGoogle;
        private ImageButton botaoPesquisa;
        private ImageButton botaoVoz;
        private LocationRequest locRequest;
        private PrimaryDrawerItem itemLogin;
        private PrimaryDrawerItem itemRegCliente;
        private PrimaryDrawerItem itemRegProprietario;
        private PrimaryDrawerItem itemPrefs;
        private PrimaryDrawerItem itemNaoPrefs;
        private PrimaryDrawerItem itemTerminarSessao;
        private ProfileDrawerItem itemUtilizador;
        private SecondaryDrawerItem itemDefinicoes;
        private SecondaryDrawerItem itemSobre;
        private TextView titulo;

        private bool aGravar;
        private bool temMicrofone;

        const int PROFILE_SETTING = 1;
        const int PERFIL_NAO_AUTENTICADO = 2;
        const int AUTENTICADO = 3;
        const int VOZ = 10;

        //=====================================================================
        // MÉTODOS QUE AUXILIAM A INICIALIZAÇÃO DA ACTIVITY
        //=====================================================================
        private void InicializarTitulo()
        {
            SetContentView(Resource.Layout.Main);
            titulo = FindViewById<TextView>(Resource.Id.textViewTitulo);
            Typeface tf = null;

            tf = Typeface.CreateFromAsset(Application.Context.Assets, "fonts/yellowtail-regular.ttf");
            if (tf != null)
                titulo.SetTypeface(tf, TypefaceStyle.Normal);
        }

        private void InicializarBotaoPesquisa()
        {
            editTextPesquisa = FindViewById<EditText>(Resource.Id.editTextPesquisa);
            botaoPesquisa = FindViewById<ImageButton>(Resource.Id.botaoPesquisa);
            botaoPesquisa.Click += HandlerBotaoPesquisa;
        }

        private void InicializarBotaoVoz()
        {
            this.botaoVoz = FindViewById<ImageButton>(Resource.Id.botaoVoz);
            string mic = PackageManager.FeatureMicrophone;

            if (mic != "android.hardware.microphone")
            {
                temMicrofone = false;
                botaoVoz.Enabled = false;
            }
            else
            {
                temMicrofone = true;
                botaoVoz.Click += HandlerBotaoVoz;
            }
        }

        // Inicializa o itemUtilizador e o cabecalhoDrawer cabeçalho do "drawer" lateral (invocado no OnCreate).
        private void InicializarCabecalho(string email, Bundle estadoGravado)
        {
            itemUtilizador = new ProfileDrawerItem();
            itemUtilizador.WithName(email.Equals("") ? "Não Autenticado" : "Email");
            itemUtilizador.WithIcon(Resource.Drawable.profile3);
            itemUtilizador.WithIdentifier(email.Equals("") ? PERFIL_NAO_AUTENTICADO : AUTENTICADO);
            if (!email.Equals(""))
                itemUtilizador.WithEmail(email);

            cabecalhoDrawer = new AccountHeaderBuilder()
                .WithActivity(this)
                .WithHeaderBackground(Resource.Drawable.header)
                .AddProfiles(itemUtilizador)
                .WithSelectionListEnabledForSingleProfile(false)
                .WithOnAccountHeaderListener(this)
                .WithSavedInstance(estadoGravado)
                .Build();
        }

        // Inicializa os items de definições e "sobre" (usado no OnCreate).
        private void InicializarDefinicoesESobre()
        {
            itemSobre = new SecondaryDrawerItem();
            itemSobre.WithName("Sobre");
            itemSobre.WithIcon(FontAwesome.Icon.FawBook);
            itemSobre.WithIdentifier(6);
            itemSobre.WithSelectable(true);
            itemSobre.WithTag("Bullhorn");

            itemDefinicoes = new SecondaryDrawerItem();
            itemDefinicoes.WithName("Definições");
            itemDefinicoes.WithIcon(FontAwesome.Icon.FawCog);
            itemDefinicoes.WithIdentifier(7);
            itemDefinicoes.WithSelectable(true);
        }

        private void InicializarItemsLoginRegisto()
        {
            itemLogin = new PrimaryDrawerItem();
            itemLogin.WithName("Login");
            itemLogin.WithIcon(GoogleMaterial.Icon.GmdPerson);
            itemLogin.WithIdentifier(1);
            itemLogin.WithSelectable(true);

            itemRegCliente = new PrimaryDrawerItem();
            itemRegCliente.WithName("Registar Cliente");
            itemRegCliente.WithIcon(GoogleMaterial.Icon.GmdPersonAdd);
            itemRegCliente.WithIdentifier(2);
            itemRegCliente.WithTag("RegCliente");
            itemRegCliente.WithSelectable(true);

            itemRegProprietario = new PrimaryDrawerItem();
            itemRegProprietario.WithName("Registar Proprietário");
            itemRegProprietario.WithIcon(GoogleMaterial.Icon.GmdPersonAdd);
            itemRegProprietario.WithIdentifier(2);
            itemRegProprietario.WithTag("RegProprietário");
            itemRegProprietario.WithSelectable(true);
            drawerBuilder.AddDrawerItems(
                itemLogin,
                itemRegCliente,
                itemRegProprietario,
                new DividerDrawerItem(),
                itemDefinicoes,
                itemSobre,
                new DividerDrawerItem()
            );
        }

        private void InicializarItemsCliente()
        {
            itemPrefs = new PrimaryDrawerItem();
            itemPrefs.WithName("Preferências");
            itemPrefs.WithIcon(GoogleMaterial.Icon.GmdPersonAdd);
            itemPrefs.WithIdentifier(3);
            itemPrefs.WithSelectable(true);

            itemNaoPrefs = new PrimaryDrawerItem();
            itemNaoPrefs.WithName("Não Preferências");
            itemNaoPrefs.WithIcon(GoogleMaterial.Icon.GmdPersonAdd);
            itemNaoPrefs.WithIdentifier(4);
            itemNaoPrefs.WithSelectable(true);

            itemTerminarSessao = new PrimaryDrawerItem();
            itemTerminarSessao.WithName("Terminar sessão");
            itemTerminarSessao.WithIcon(GoogleMaterial.Icon.GmdExitToApp);
            itemTerminarSessao.WithIdentifier(5);
            itemTerminarSessao.WithSelectable(true);

            drawerBuilder.AddDrawerItems(
                itemPrefs,
                itemNaoPrefs,
                itemTerminarSessao,
                new DividerDrawerItem(),
                itemDefinicoes,
                itemSobre,
                new DividerDrawerItem()
            );
        }

        //=====================================================================
        // HANDLERS
        //=====================================================================
        public void HandlerBotaoPesquisa(object obj, EventArgs args)
        {
            string pedido = editTextPesquisa.Text;
            Location localizacao = ObterLocalizacao();

            if (localizacao != null)
                Facade.EfetuarPedido(pedido, localizacao);
            else
                Toast.MakeText(this, "Localização indisponível.", ToastLength.Short);
        }

        public void HandlerBotaoVoz(object obj, EventArgs args)
        {
            Toast.MakeText(this, "Clique no botão de voz.", ToastLength.Short).Show();

            aGravar = !aGravar;
            if (aGravar)
            {
                var intentVoz = new Intent(RecognizerIntent.ActionRecognizeSpeech);
                intentVoz.PutExtra(RecognizerIntent.ExtraLanguageModel, RecognizerIntent.LanguageModelFreeForm);
                intentVoz.PutExtra(RecognizerIntent.ExtraPrompt, Application.Context.GetString(Resource.String.mensagemFalarAgora));
                intentVoz.PutExtra(RecognizerIntent.ExtraSpeechInputCompleteSilenceLengthMillis, 1500);
                intentVoz.PutExtra(RecognizerIntent.ExtraSpeechInputPossiblyCompleteSilenceLengthMillis, 1500);
                intentVoz.PutExtra(RecognizerIntent.ExtraMaxResults, 1);
                intentVoz.PutExtra(RecognizerIntent.ExtraLanguage, "pt-PT");
                StartActivityForResult(intentVoz, VOZ);
            }
            // O resultado reconhecimento de voz é processado no método OnActivityResult.
        }

        //=====================================================================
        // PROCESSAMENTO DO RESULTADO DE ATIVIDADES
        //=====================================================================
        protected override void OnActivityResult(int codigoPedido, Result codigoRes, Intent dados)
        {
            if (codigoPedido == VOZ)
            {
                aGravar = false;
                if (codigoRes == Result.Ok)
                {
                    var listaRes = dados.GetStringArrayListExtra(RecognizerIntent.ExtraResults);

                    if (listaRes.Count != 0)
                    {
                        string textoPedido = editTextPesquisa.Text + listaRes[0];

                        editTextPesquisa.Text = textoPedido;
                        Location localizacao = ObterLocalizacao();

                        if (localizacao != null)
                            Facade.EfetuarPedido(editTextPesquisa.Text, localizacao);
                        else
                            Toast.MakeText(this, "Localização indisponível. Não é possível prosseguir com o pedido.", ToastLength.Short);
                    }
                    else
                    {
                        editTextPesquisa.Text = "";
                    }
                }
            }
            base.OnActivityResult(codigoPedido, codigoRes, dados);
        }

        private void InicializarClienteLocalizacao()
        {

        }

        //=====================================================================
        // CICLO DE VIDA DA ATIVIDADE
        //=====================================================================
        protected override void OnCreate(Bundle estadoGravado)
        {
            base.OnCreate(estadoGravado);

            aGravar = false; // Não está a ser feito reconhecimento de voz.

            InicializarTitulo();
            InicializarBotaoPesquisa();
            // Se o dispositivo tiver microfone, associa um "click handler" ao botão de voz, se não, desativa-o
            InicializarBotaoVoz();

            string email = Intent.GetStringExtra("utilizador_email") ?? "";
            InicializarCabecalho(email, estadoGravado);
            InicializarDefinicoesESobre();

            drawerBuilder = new DrawerBuilder()
                .WithActivity(this)
                .WithAccountHeader(cabecalhoDrawer); // Adicionar o cabeçalho inicializado anteriormente.

            if (email.Equals("")) // Utilizador não autenticado
                InicializarItemsLoginRegisto();
            else  // TODO: Testar se o utilizador é um proprietário ou um cliente e criar items diferentes para cada caso!
                InicializarItemsCliente();

            drawer = drawerBuilder.WithOnDrawerItemClickListener(this)
                                  .WithSavedInstance(estadoGravado)
                                  .WithShowDrawerOnFirstLaunch(true)
                                  .Build();

            // Havendo vários tipos de DrawerItems, pode-se fazer caching desses items para se obter um melhor desempenho
            // ao fazer scroll. A cache deve ser inicializada depois do DrawerBuilder ser construido.
            RecyclerViewCacheUtil.Instance.WithCacheSize(2).Init(drawer);

            // Só se ativa a seleção quando não se está a recriar a "activity"
            if (estadoGravado == null)
            {
                // Definir a seleção como tendo identificador -1
                drawer.SetSelection(-1, false);

                // Definir o perfil ativo
                cabecalhoDrawer.SetActiveProfile(itemUtilizador, false);
            }

            if (ServicosGooglePlayEstaoDisponiveis())
            {
                // a MainActivity é o contexto implementa as interfaces IConnectionCallabacks e IOnConnectionFailedListener
                GoogleApiClient clienteApiGoogle = new GoogleApiClient.Builder(this, this, this)
                    .AddApi(LocationServices.API)
                    .Build();
            }
        }

        protected override void OnStart()
        {
            base.OnStart();

            if (temMicrofone == false)
            {
                var alerta = new AlertDialog.Builder(botaoVoz.Context);
                alerta.SetTitle("Não foi detetado qualquer microfone no seu dispositivo. Não será possível efetuar pedidos por voz.");
                alerta.SetPositiveButton("OK", (sender, e) => { return; });
                alerta.Create().Show();
            }

            /* if (clienteApiGoogle != null)
                clienteApiGoogle.Connect(); */
        }

        protected override void OnPause()
        {
            base.OnPause();

            if (clienteApiGoogle != null && clienteApiGoogle.IsConnected)
                clienteApiGoogle.Disconnect();
        }

        protected override void OnResume()
        {
            base.OnResume();

            if (clienteApiGoogle != null)
            {
                clienteApiGoogle.Connect();
            }
            else if (ServicosGooglePlayEstaoDisponiveis())
            {
                clienteApiGoogle = new GoogleApiClient.Builder(this, this, this)
                    .AddApi(LocationServices.API)
                    .Build();

                clienteApiGoogle.Connect();
            }
        }

        protected override void OnSaveInstanceState(Bundle outState)
        {
            // Adicionar ao bundle os valores da "Drawer" que precisam de ser guardados.
            outState = drawer.SaveInstanceState(outState);
            // Adicionar ao "bundle" os valores do cabeçalho da conta que precisam de ser guardados.
            outState = cabecalhoDrawer.SaveInstanceState(outState);
            base.OnSaveInstanceState(outState);
        }

        //=====================================================================
        // MÉTODOS RELACIONADOS COM OS GOOGLE PLAY SERVICES
        //=====================================================================
        private bool ServicosGooglePlayEstaoDisponiveis()
        {
            GoogleApiAvailability disponibilidadeApiGoogle = GoogleApiAvailability.Instance;
            int resQuery = disponibilidadeApiGoogle.IsGooglePlayServicesAvailable(this);

            return (resQuery == ConnectionResult.Success);
        }

        private Location ObterLocalizacao()
        {
            Location loc;

            if (clienteApiGoogle != null && clienteApiGoogle.IsConnected)
            {
                loc = LocationServices.FusedLocationApi.GetLastLocation(clienteApiGoogle);
                Console.WriteLine(loc.ToString());
            }
            else
                loc = null;

            return loc;
        }

        public void OnConnected(Bundle connectionHint)
        {
            Toast.MakeText(this, "A ligação aos serviços do Google Play foi bem sucedida.", ToastLength.Short);
        }

        public void OnConnectionSuspended(int cause)
        {
            string msg;

            switch (cause)
            {
                case GoogleApiClient.ConnectionCallbacks.CauseNetworkLost:
                    msg = "A ligação ao serviço da Google Play foi abaixo por falta de rede.";
                    break;
                case GoogleApiClient.ConnectionCallbacks.CauseServiceDisconnected:
                    msg = "O serviço da Google Play foi terminado.";
                    break;
                default:
                    msg = "O serviço da Google Play foi suspenso por causa desconhecida.";
                    break;
            }
            Toast.MakeText(this, msg, ToastLength.Short);
        }

        public void OnConnectionFailed(ConnectionResult result)
        {
            AlertDialog.Builder alerta = new AlertDialog.Builder(this);

            alerta.SetTitle("Não foi possível ligar aos serviços do Google Play. Código do erro: " + result.ErrorCode);
            alerta.SetPositiveButton("OK", (sender, e) => { return; });
            alerta.Create().Show();
        }

        //=====================================================================
        // RESPOSTA A EVENTOS DO DRAWER
        //=====================================================================
        public bool OnItemClick(View view, int position, IDrawerItem drawerItem)
        {
            // Existem várias razões para o "drawerItem" ser nulo:
            //   --> clique no cabeçalho;
            //   --> clique no rodapé;
            // Esses dois items não contêm um "drawerItem"
            if (drawerItem != null)
            {
                switch (drawerItem.Identifier)
                {
                    case 1: // Login
                        // Toast.MakeText(this, "Clique em Login", ToastLength.Short).Show();

                        StartActivity(typeof(LoginActivity));
                        break;
                    case 2: // Registo como cliente
                        if (drawerItem.Tag.Equals("RegCliente"))
                        {
                            // Toast.MakeText(this, "Clique em 'Registar Cliente'", ToastLength.Short).Show();
                            StartActivity(typeof(RegistoClienteActivity));
                        }
                        /* else
                        {
                            Toast.MakeText(this, string.Format("Clique em 'Registar Proprietário'"), ToastLength.Short).Show();
                        }*/
                        break;
                    case 3: // Preferências
                        // Toast.MakeText(this, "Clique em 'Preferências'", ToastLength.Short).Show();
                        StartActivity(typeof(RegistarPreferenciasActivity));
                        break;
                    case 4: // Não preferências
                        // Toast.MakeText(this, "Clique em 'Não Preferências'", ToastLength.Short).Show();
                        StartActivity(typeof(RegistarPreferenciasActivity));
                        break;
                    case 5: // Terminar sessão
                        /* Sobre ActivityFlags: https://developer.xamarin.com/api/type/Android.Content.ActivityFlags/ */
                        Facade.TerminarSessao();
                        Toast.MakeText(this, "Sessão terminada com sucesso.", ToastLength.Short).Show();

                        var intent = new Intent(this, typeof(MainActivity));
                        intent.SetFlags(ActivityFlags.NewTask | ActivityFlags.ClearTask);
                        StartActivity(intent);
                        break;
                    case 6: // Sobre
                        Toast.MakeText(this, "Clique em 'Sobre'", ToastLength.Short).Show();
                        break;
                    case 7: // Definições
                        Toast.MakeText(this, "Clique em 'Definições'", ToastLength.Short).Show();
                        break;
                }
                drawerItem.WithSetSelected(false);
            }
            return false;
        }

        public bool OnProfileChanged(View view, IProfile profile, bool current)
        {
            // sample usage of the onProfileChanged listener
            // if the clicked item has the identifier 1 add a new profile ;)
            if (profile is IDrawerItem && profile.Identifier == PROFILE_SETTING)
            {
                int count = 100 + cabecalhoDrawer.Profiles.Count + 1;
                IProfile newProfile = new ProfileDrawerItem().WithNameShown(true).WithName("Batman" + count).WithEmail("batman" + count + "@gmail.com").WithIcon(Resource.Drawable.profile3);
                newProfile.WithIdentifier(count);
                if (cabecalhoDrawer.Profiles != null)
                {
                    //we know that there are 2 setting elements. set the new profile above them ;)
                    cabecalhoDrawer.AddProfile(newProfile, cabecalhoDrawer.Profiles.Count - 2);
                }
                else
                {
                    cabecalhoDrawer.AddProfiles(newProfile);
                }
            }
            // false if you have not consumed the event and it should close the drawer
            return false;
        }
    }
}

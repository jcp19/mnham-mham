using System;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Views;
using Android.Widget;
using Android.Content.PM;

using Mikepenz.Typeface;
using Mikepenz.MaterialDrawer.Utils;

using Mikepenz.MaterialDrawer;
using Mikepenz.MaterialDrawer.Models;
using Mikepenz.MaterialDrawer.Models.Interfaces;
using Android.Graphics;

namespace Mnham_Mnham
{
    [Activity(Label = "Mnham Mnham", MainLauncher = true, Icon = "@drawable/icon", ScreenOrientation = ScreenOrientation.Portrait, Theme = "@style/AppTheme")]
    public class MainActivity: Activity, AccountHeader.IOnAccountHeaderListener, Drawer.IOnDrawerItemClickListener
    {
        public readonly static MnhamMnham Facade = new MnhamMnham();

        private AccountHeader headerResultado;
        private Drawer result;
        private DrawerBuilder drawerBuilder;
        private ProfileDrawerItem itemUtilizador;
        private SecondaryDrawerItem itemDefinicoes;
        private SecondaryDrawerItem itemSobre;
        private PrimaryDrawerItem itemLogin;
        private PrimaryDrawerItem itemRegCliente;
        private PrimaryDrawerItem itemRegProprietario;
        private PrimaryDrawerItem itemPrefs;
        private PrimaryDrawerItem itemNaoPrefs;
        private PrimaryDrawerItem itemTerminarSessao;

        const int PROFILE_SETTING = 1;
        const int PERFIL_NAO_AUTENTICADO = 2;
        const int AUTENTICADO = 3;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.Main);
            EditText editTextAlim = (EditText)FindViewById(Resource.Id.editText1);
            /* Make the drawable right clickable
            editTextAlim.Click += (sender,e) =>
            {
                (sender as EditText).OnTouchEvent(e);
            } */

            TextView titulo = FindViewById<TextView>(Resource.Id.textView1);
            Typeface tf = null;

            tf = Typeface.CreateFromAsset(Application.Context.Assets, "fonts/yellowtail-regular.ttf");
            if (tf != null)
                titulo.SetTypeface(tf, TypefaceStyle.Normal);
            string email = Intent.GetStringExtra("utilizador_email") ?? "";
            InicializarCabecalho(email, savedInstanceState);
            // Inicializa items que estão sempre lá.
            InicializarDefinicoesESobre();
            /* TODO: register hardcoded string in strings.xml*/

            drawerBuilder = new DrawerBuilder()
                .WithActivity(this)
                .WithAccountHeader(headerResultado); //set the AccountHeader we created earlier for the header

            if (email.Equals(""))
                InicializarItemsLoginRegisto();
            else
                InicializarItemsUtilizadorAutenticado();

            result = drawerBuilder.WithOnDrawerItemClickListener(this)
                                  .WithSavedInstance(savedInstanceState)
                                  .WithShowDrawerOnFirstLaunch(true)
                                  .Build();

            // Havendo vários tipos de DrawerItems, pode-se fazer caching desses items para ter melhor desempenho ao fazer scroll.
            // A cache deve ser inicializada depois do DrawerBuilder ser criado.
            RecyclerViewCacheUtil.Instance.WithCacheSize(2).Init(result);

            // Só se ativa a seleção quando não se está a recriar a "activity"
            if (savedInstanceState == null)
            {
                // Definir a seleção como tendo identificador -1
                result.SetSelection(-1, false);

                // Definir o perfil ativo
                headerResultado.SetActiveProfile(itemUtilizador, false);
            }
        }

        // Inicializa o itemUtilizador e o headerResultado, localizados no cabeçalho do "drawer" lateral.
        private void InicializarCabecalho(string email, Bundle savedInstanceState)
        {
            itemUtilizador = new ProfileDrawerItem();
            itemUtilizador.WithName(email.Equals("") ? "Não Autenticado" : "Email");
            itemUtilizador.WithIcon(Resource.Drawable.profile3);
            itemUtilizador.WithIdentifier(email.Equals("") ? PERFIL_NAO_AUTENTICADO : AUTENTICADO);
            if (!email.Equals(""))
                itemUtilizador.WithEmail(email);

            headerResultado = new AccountHeaderBuilder()
                .WithActivity(this)
                .WithHeaderBackground(Resource.Drawable.header)
                .AddProfiles(itemUtilizador)
                .WithSelectionListEnabledForSingleProfile(false)
                .WithOnAccountHeaderListener(this)
                .WithSavedInstance(savedInstanceState)
                .Build();
        }

        // Inicializa os items de definições e "sobre"
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
            ); // add the items we want to use With our Drawer
        }

        // TODO: Testar se o utilizador é um proprietário ou um cliente e criar items diferentes para cada caso!
        private void InicializarItemsUtilizadorAutenticado()
        {
            itemPrefs = new PrimaryDrawerItem();
            itemPrefs.WithName("Preferências");
            itemPrefs.WithIcon(GoogleMaterial.Icon.GmdPerson);
            itemPrefs.WithIdentifier(3);
            itemPrefs.WithSelectable(true);

            itemNaoPrefs = new PrimaryDrawerItem();
            itemNaoPrefs.WithName("Não Preferências");
            itemNaoPrefs.WithIcon(GoogleMaterial.Icon.GmdPersonAdd);
            itemNaoPrefs.WithIdentifier(4);
            itemNaoPrefs.WithSelectable(true);

            itemTerminarSessao = new PrimaryDrawerItem();
            itemTerminarSessao.WithName("Terminar sessão");
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
                        Toast.MakeText(this, "Clique em Login", ToastLength.Short).Show();

                        StartActivity(typeof(LoginActivity));
                        break;
                    case 2: // Registo como cliente
                        if (drawerItem.Tag.Equals("RegCliente"))
                        {
                            Toast.MakeText(this, "Clique em 'Registar Cliente'", ToastLength.Short).Show();
                            StartActivity(typeof(RegistoClienteActivity));
                        }
                        /* else
                        {
                            Toast.MakeText(this, string.Format("Clique em 'Registar Proprietário'"), ToastLength.Short).Show();
                        }*/
                        break;
                    case 3: // Preferências
                        Toast.MakeText(this, "Clique em 'Preferências'", ToastLength.Short).Show();
                        StartActivity(typeof(RegistarPreferenciasActivity));
                        break;
                    case 4: // Não preferências
                        Toast.MakeText(this, "Clique em 'Não Preferências'", ToastLength.Short).Show();
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
                int count = 100 + headerResultado.Profiles.Count + 1;
                IProfile newProfile = new ProfileDrawerItem().WithNameShown(true).WithName("Batman" + count).WithEmail("batman" + count + "@gmail.com").WithIcon(Resource.Drawable.profile3);
                newProfile.WithIdentifier(count);
                if (headerResultado.Profiles != null)
                {
                    //we know that there are 2 setting elements. set the new profile above them ;)
                    headerResultado.AddProfile(newProfile, headerResultado.Profiles.Count - 2);
                }
                else
                {
                    headerResultado.AddProfiles(newProfile);
                }
            }
            // false if you have not consumed the event and it should close the drawer
            return false;
        }

        protected override void OnSaveInstanceState(Bundle outState)
        {
            // Adicionar ao bundle os valores da "Drawer" que precisam de ser guardados.
            outState = result.SaveInstanceState(outState);
            // Adicionar ao "bundle" os valores do cabeçalho da conta que precisam de ser guardados.
            outState = headerResultado.SaveInstanceState(outState);
            base.OnSaveInstanceState(outState);
        }
    }
}

using System;
using System.Collections.Generic;
using Android.App;
using Android.Content;
using Android.Graphics;
using Android.Locations;
using Android.OS;
using Android.Text;
using Android.Widget;

namespace Mnham_Mnham
{
    [Activity(Label = "PerfilAlimentoActivity", Icon = "@drawable/icon", Theme = "@style/AppTheme")]
    public class PerfilAlimentoActivity : Activity
    {
        private AlimentoEstabelecimento alimentoEstabelecimento;
        private ImageView foto;
        private TextView textViewAlimento, textViewPreco, labelIngredientes, textViewIngredientes;
        private TextView textViewRestaurante, textViewDistancia, textViewRua, textViewContacto;
        private RatingBar barraRating;
        private Button botaoHorario, botaoComentarios, botaoPartilha;
        private ImageButton botaoMapa;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.AlimentoPerfil);

            foto = FindViewById<ImageView>(Resource.Id.picAlimentoPerfilImageView);
            textViewAlimento = FindViewById<TextView>(Resource.Id.nomeAlimentoPerfilTextView);
            barraRating = FindViewById<RatingBar>(Resource.Id.ratingAlimentoPerfilRatingBar);
            textViewRestaurante = FindViewById<TextView>(Resource.Id.restauranteAlimentoPerfilTextView);
            textViewDistancia = FindViewById<TextView>(Resource.Id.distanciaAlimentoPerfilTextView);
            textViewRua = FindViewById<TextView>(Resource.Id.ruaAlimentoPerfilTextView);
            textViewContacto = FindViewById<TextView>(Resource.Id.contactoAlimentoPerfilTextView);
            textViewPreco = FindViewById<TextView>(Resource.Id.precoAlimentoPerfilTextView);
            labelIngredientes = FindViewById<TextView>(Resource.Id.ingrAlimentoPerfilTextView);
            textViewIngredientes = FindViewById<TextView>(Resource.Id.ingredientesAlimentoPerfilTextView);

            textViewRestaurante.Click += HandlerClickRestaurante;
            // Botões
            botaoHorario = FindViewById<Button>(Resource.Id.horarioPerfilButton);
            botaoComentarios = FindViewById<Button>(Resource.Id.comentariosPerfilButton);
            botaoPartilha = FindViewById<Button>(Resource.Id.partilhaPerfilButton);
            botaoMapa = FindViewById<ImageButton>(Resource.Id.direcoesAlimentoPerfilButton);

            botaoHorario.Click += HandlerBotaoHorario;
            botaoComentarios.Click += HandlerBotaoComentarios;
            botaoPartilha.Click += HandlerBotaoPartilha;
            botaoMapa.Click += HandlerBotaoMapa;

            alimentoEstabelecimento = (AlimentoEstabelecimento)Intent.GetParcelableExtra("alimentoEstabelecimento");
            byte[] fotoAlimento = alimentoEstabelecimento.Alimento.Foto;
            float? preco = alimentoEstabelecimento.Alimento.Preco;
            ISet<string> ingredientes = alimentoEstabelecimento.Alimento.Ingredientes;

            // Campos opcionais
            if (fotoAlimento != null)
            {
                foto.SetImageBitmap(BitmapFactory.DecodeByteArray(fotoAlimento, 0, fotoAlimento.Length));
            }
            if (preco.HasValue)
            {
                textViewPreco.Text = preco + "€";
            }
            if (ingredientes != null)
            {
                textViewIngredientes.Text = string.Join(", ", ingredientes);
            }
            // Campos obrigatórios
            textViewAlimento.Text = alimentoEstabelecimento.Alimento.Designacao;
            barraRating.Rating = alimentoEstabelecimento.Alimento.ClassificacaoMedia;
            textViewRestaurante.SetTextColor(Color.ParseColor("#4286f4"));
            textViewRestaurante.Text = alimentoEstabelecimento.Estabelecimento.Nome;
            textViewDistancia.Text = string.Format("{0:F2} Km", alimentoEstabelecimento.Distancia / 1000.0f);
            textViewRua.Text = alimentoEstabelecimento.Estabelecimento.Morada;
            textViewContacto.Text = alimentoEstabelecimento.Estabelecimento.ContactoTel;

            textViewIngredientes.Selected = true;
            textViewIngredientes.Click += (sender, args) => { textViewIngredientes.Selected = !textViewIngredientes.Selected; };
        }

        private void HandlerClickRestaurante(object sender, EventArgs e)
        {
            var intent = new Intent(this, typeof(PerfilEstabelecimentoActivity));

            intent.PutExtra("estabelecimento", alimentoEstabelecimento.Estabelecimento);
            StartActivity(intent);
        }

        // Handlers
        private void HandlerBotaoPartilha(object sender, EventArgs e)
        {
            AlertDialog.Builder builder = new AlertDialog.Builder(this);
            builder.SetTitle("Mensagem a partilhar");

            EditText input = new EditText(this);
            // Specify the type of input expected; this, for example, sets the input as a password, and will mask the text
            input.InputType = InputTypes.TextFlagMultiLine | InputTypes.ClassText;
            builder.SetView(input);

            builder.SetPositiveButton("OK", (o, args) =>
            {
                if (input.Text.Trim().Length > 0)
                {
                    var sendIntent = new Intent();
                    sendIntent.SetAction(Intent.ActionSend);
                    sendIntent.PutExtra(Intent.ExtraText, input.Text);
                    sendIntent.SetType("text/plain");
                    StartActivity(sendIntent);
                }
            })
            .SetNegativeButton("Cancelar", (o, args) => { })
            .Create()
            .Show();
        }

        private void HandlerBotaoComentarios(object sender, EventArgs e)
        {
            Toast.MakeText(this, "Clique nos comentários.", ToastLength.Short);
        }

        private void HandlerBotaoHorario(object sender, EventArgs e)
        {
            string horario = alimentoEstabelecimento.Estabelecimento.Horario ?? "Informação indisponível.";

            horario = horario.Replace(";", "\n");
            AlertDialog dialogHorario = new AlertDialog.Builder(this)
                .SetTitle("Horário de " + alimentoEstabelecimento.Estabelecimento.Nome)
                .SetMessage(horario)
                .SetPositiveButton("OK", (emissor, evento) => { })
                .Create();

            dialogHorario.Show();
        }

        private void HandlerBotaoMapa(object sender, EventArgs a)
        {
            Location localizacaoEstabelecimento = alimentoEstabelecimento.Estabelecimento.Coords;
            string uri = string.Format("http://maps.google.com/maps?daddr={0:F},{1:F}",
                localizacaoEstabelecimento.Latitude, localizacaoEstabelecimento.Longitude);

            var intent = new Intent(Intent.ActionView, Android.Net.Uri.Parse(uri));
            StartActivity(intent);
        }
    }
}

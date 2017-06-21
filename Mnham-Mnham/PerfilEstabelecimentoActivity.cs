using Android.App;
using Android.OS;

using System;
using Android.Content;
using Android.Graphics;
using Android.Locations;
using Android.Text;
using Android.Widget;

namespace Mnham_Mnham
{
    [Activity(Label = "PerfilEstabelecimentoActivity", Icon = "@drawable/icon", Theme = "@style/AppTheme")]
    public class PerfilEstabelecimentoActivity : Activity
    {
        private Estabelecimento estabelecimento;
        private TextView nomeRestTextView, ruaTextView, contactoTextView, descricaoTextView;
        private RatingBar restRatingBar;
        private Button botaoHorario, botaoComentarios, botaoPartilha;
        private ImageView foto;
        private ImageButton botaoMapa;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.PerfilEstabelecimento);

            nomeRestTextView = FindViewById<TextView>(Resource.Id.nomeRestPerfilTextView);
            restRatingBar = FindViewById<RatingBar>(Resource.Id.ratingRestPerfilRatingBar);
            ruaTextView = FindViewById<TextView>(Resource.Id.ruaRestPerfilTextView);
            contactoTextView = FindViewById<TextView>(Resource.Id.contactoRestPerfilTextView);
            descricaoTextView = FindViewById<TextView>(Resource.Id.descricaoRestPerfilTextView);
            foto = FindViewById<ImageView>(Resource.Id.restauranteImageView);

            // Botões
            botaoHorario = FindViewById<Button>(Resource.Id.horarioRestPerfilButton);
            botaoComentarios = FindViewById<Button>(Resource.Id.comentariosRestPerfilButton);
            botaoPartilha = FindViewById<Button>(Resource.Id.partilhaRestPerfilButton);
            botaoMapa = FindViewById<ImageButton>(Resource.Id.direccoesEstabelecimentoPerfilButton);

            botaoHorario.Click += HandlerBotaoHorario;
            botaoComentarios.Click += HandlerBotaoComentarios;
            botaoPartilha.Click += HandlerBotaoPartilha;
            botaoMapa.Click += HandlerBotaoMapa;

            estabelecimento = (Estabelecimento)Intent.GetParcelableExtra("estabelecimento");
            byte[] fotoEstabelecimento = estabelecimento.Foto;

            // Campos opcionais
            if (fotoEstabelecimento != null)
            {
                foto.SetImageBitmap(BitmapFactory.DecodeByteArray(fotoEstabelecimento, 0, fotoEstabelecimento.Length));
            }
            nomeRestTextView.Text = estabelecimento.Nome;
            restRatingBar.Rating = estabelecimento.ClassificacaoMedia;
            ruaTextView.Text = estabelecimento.Morada;
            contactoTextView.Text = estabelecimento.ContactoTel;
            descricaoTextView.Text = estabelecimento.Descricao ?? "";
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
            string horario = estabelecimento.Horario ?? "Informação indisponível.";

            horario = horario.Replace(";", "\n");
            AlertDialog dialogHorario = new AlertDialog.Builder(this)
                .SetTitle("Horário de " + estabelecimento.Nome)
                .SetMessage(horario)
                .SetPositiveButton("OK", (emissor, evento) => { })
                .Create();

            dialogHorario.Show();
        }

        private void HandlerBotaoMapa(object sender, EventArgs a)
        {
            Location localizacaoEstabelecimento = estabelecimento.Coords;
            string uri = string.Format("http://maps.google.com/maps?daddr={0:F},{1:F}",
                localizacaoEstabelecimento.Latitude, localizacaoEstabelecimento.Longitude);

            var intent = new Intent(Intent.ActionView, Android.Net.Uri.Parse(uri));
            StartActivity(intent);
        }
    }
}

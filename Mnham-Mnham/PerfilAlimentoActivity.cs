using System;
using System.Collections.Generic;
using Android.App;
using Android.Gms.Maps;
using Android.Graphics;
using Android.OS;
using Android.Widget;

namespace Mnham_Mnham
{
    [Activity(Label = "PerfilAlimentoActivity", Icon = "@drawable/icon", Theme = "@style/AppTheme")]
    public class PerfilAlimentoActivity : Activity, IOnMapReadyCallback
    {
        private AlimentoEstabelecimento alimentoEstabelecimento;
        private ImageView foto;
        private TextView textViewAlimento, textViewPreco, labelIngredientes, textViewIngredientes;
        private TextView textViewRestaurante, textViewDistancia, textViewRua, textViewContacto;
        private RatingBar barraRating;
        private Button botaoHorario, botaoComentarios, botaoPartilha;
        private Fragment mapa;

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
            textViewContacto = FindViewById<TextView>(Resource.Id.distanciaAlimentoPerfilTextView);
            textViewPreco = FindViewById<TextView>(Resource.Id.precoAlimentoPerfilTextView);
            labelIngredientes = FindViewById<TextView>(Resource.Id.ingrAlimentoPerfilTextView);
            textViewIngredientes = FindViewById<TextView>(Resource.Id.ingredientesAlimentoPerfilTextView);
            // Botões
            botaoHorario = FindViewById<Button>(Resource.Id.horarioPerfilButton);
            botaoComentarios = FindViewById<Button>(Resource.Id.comentariosPerfilButton);
            botaoPartilha = FindViewById<Button>(Resource.Id.partilhaPerfilButton);

            botaoHorario.Click += HandlerBotaoHorario;
            botaoComentarios.Click += HandlerBotaoComentarios;
            botaoPartilha.Click += HandlerBotaoPartilha;

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
                textViewPreco.Text = preco.ToString();
            }
            if (ingredientes != null)
            {
                textViewIngredientes.Text = string.Join(", ", ingredientes);
            }
            // Campos obrigatórios
            textViewAlimento.Text = alimentoEstabelecimento.Alimento.Designacao;
            barraRating.Rating = alimentoEstabelecimento.Alimento.ClassificacaoMedia;
            textViewRestaurante.Text = alimentoEstabelecimento.Estabelecimento.Nome;
            textViewDistancia.Text = alimentoEstabelecimento.Distancia.ToString();
            textViewRua.Text = alimentoEstabelecimento.Estabelecimento.Morada;
            textViewContacto.Text = alimentoEstabelecimento.Estabelecimento.ContactoTel;
        }

        // Handlers
        private void HandlerBotaoPartilha(object sender, EventArgs e)
        {
            Toast.MakeText(this, "Clique na partilha.", ToastLength.Short);
        }

        private void HandlerBotaoComentarios(object sender, EventArgs e)
        {
            Toast.MakeText(this, "Clique nos comentários.", ToastLength.Short);
        }

        private void HandlerBotaoHorario(object sender, EventArgs e)
        {
            string horario = alimentoEstabelecimento.Estabelecimento.Horario ?? "Informação indisponível.";

            horario.Replace(";", "\n");
            AlertDialog dialogHorario = new AlertDialog.Builder(this)
                .SetTitle("Horário de " + alimentoEstabelecimento.Estabelecimento.Nome)
                .SetMessage(horario)
                .SetPositiveButton("OK", (emissor, evento) => { })
                .Create();

            dialogHorario.Show();
        }

        public void OnMapReady(GoogleMap googleMap)
        {
            throw new NotImplementedException();
        }
    }
}
     
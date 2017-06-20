using System;

using Android.App;
using Android.OS;
using Android.Views;
using Android.Widget;
using Android.Support.V7.Widget;
using System.Collections.Generic;
using Android.Content;
using Android.Database;
using Android.Graphics;
using Android.Locations;
using Newtonsoft.Json;

namespace Mnham_Mnham
{
    [Activity(Label = "ResultadosActivity", Theme = "@style/AppTheme")]
    public class ResultadosActivity : Activity
    {
        private bool sqlEx; // indica se ocorreu uma SqlException ao se efetuar pedido.
        private string pedido;
        private Location localizacao;
        private RecyclerView mRecyclerView;
        private RecyclerView.LayoutManager mLayoutManager;
        private ResultadosAdapter mAdapter;
        private List<AlimentoEstabelecimento> resultados;
        private UtilitarioApiGoogle utilitarioApiGoogle;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            pedido = Intent.GetStringExtra("pedido");

            utilitarioApiGoogle = MnhamMnhamApp.ObterUtilitarioApiGoogle();
            localizacao = utilitarioApiGoogle.ObterLocalizacao();
            if (localizacao != null)
            {
                try
                {
                    resultados = MainActivity.Facade.EfetuarPedido(pedido, localizacao);
                    sqlEx = false;
                }
                catch (SQLException)
                {
                    resultados = new List<AlimentoEstabelecimento>(0);
                    sqlEx = true;
                }
            }
            else
                resultados = new List<AlimentoEstabelecimento>(0);

            SetContentView(Resource.Layout.ResultadosLayout);

            mRecyclerView = FindViewById<RecyclerView>(Resource.Id.resultadosRecyclerView);

            mLayoutManager = new LinearLayoutManager(this);
            mRecyclerView.SetLayoutManager(mLayoutManager);

            mAdapter = new ResultadosAdapter(resultados);
            mAdapter.ItemClick += OnItemClick;
            mRecyclerView.SetAdapter(mAdapter);
        }

        protected override void OnStart()
        {
            base.OnStart();

            if (localizacao == null)
                Toast.MakeText(this, "Localização indisponível. Não é possível prosseguir com o pedido.", ToastLength.Short).Show();
            else if (sqlEx) // se ocorreu uma SqlException
                Toast.MakeText(this, "Não foi possível efetuar o pedido. Verifique se tem ligação à Internet.", ToastLength.Short).Show();
        }

        void OnItemClick(object sender, int position)
        {
            AlimentoEstabelecimento itemSelecionado = resultados[position];
            string strItemSelecionado = JsonConvert.SerializeObject(itemSelecionado);
            var intent = new Intent(this, typeof(PerfilAlimentoActivity));

            // handler for the itemClick event;
            // It should just start a PerfilAlimentoActivity
            // of the item clicked
            // if we pass with an intent the info that we already
            // have on the item to the new activity we wont need
            // to retrieve those bits of info from the database
            intent.PutExtra("alimentoEstabelecimento", strItemSelecionado);
            StartActivity(intent);
        }
    }

    public class ResultadoViewHolder : RecyclerView.ViewHolder
    {
        // Add declaration for every view of a result:
        public ImageView FoodImage { get; }
        public TextView FoodName { get; }
        public RatingBar Rating { get; }
        public TextView RestaurantName { get; }
        public TextView Distance { get; }
        public TextView Street { get; }
        public TextView Contact { get; }
        public TextView Price { get; }

        public ResultadoViewHolder(View itemView, Action<int> listener) : base(itemView)
        {
            FoodImage = itemView.FindViewById<ImageView>(Resource.Id.picAlimentoResImageView);
            FoodName = itemView.FindViewById<TextView>(Resource.Id.nomeAlimentoResTextView);
            Rating = itemView.FindViewById<RatingBar>(Resource.Id.ratingAlimentoResRatingBar);
            RestaurantName = itemView.FindViewById<TextView>(Resource.Id.restauranteAlimentoResTextView);
            Distance = itemView.FindViewById<TextView>(Resource.Id.distanciaAlimentoResTextView);
            Street = itemView.FindViewById<TextView>(Resource.Id.ruaAlimentoResTextView);
            Contact = itemView.FindViewById<TextView>(Resource.Id.contactoAlimentoResTextView);
            Price = itemView.FindViewById<TextView>(Resource.Id.precoAlimentoResTextView);

            itemView.Click += (sender, e) => listener(AdapterPosition);
        }
    }

    public class ResultadosAdapter : RecyclerView.Adapter
    {
        // event handler for item clicks
        public event EventHandler<int> ItemClick;

        public IList<AlimentoEstabelecimento> Resultados;
        public override int ItemCount => Resultados.Count;

        public ResultadosAdapter(IList<AlimentoEstabelecimento> resultados)
        {
            this.Resultados = resultados;
        }

        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            // loads the data at the specified position into the views whose 
            // references are stored in the given view holder
            AlimentoEstabelecimento alimentoEstabelecimento = Resultados[position];
            ResultadoViewHolder vh = holder as ResultadoViewHolder;
            byte[] foto = alimentoEstabelecimento.Alimento.Foto;
            float? preco = alimentoEstabelecimento.Alimento.Preco;

            // Campos opcionais
            if (foto != null)
            {
                vh.FoodImage.SetImageBitmap(BitmapFactory.DecodeByteArray(foto, 0, foto.Length));
            }
            if (preco.HasValue)
            {
                vh.Price.Text = preco.ToString();
            }
            // Campos obrigatórios
            vh.FoodName.Text = alimentoEstabelecimento.Alimento.Designacao;
            vh.Rating.Rating = alimentoEstabelecimento.Alimento.ClassificacaoMedia;
            vh.RestaurantName.Text = alimentoEstabelecimento.Estabelecimento.Nome;
            vh.Distance.Text = alimentoEstabelecimento.Distancia.ToString();
            vh.Street.Text = alimentoEstabelecimento.Estabelecimento.Morada;
            vh.Contact.Text = alimentoEstabelecimento.Estabelecimento.ContactoTel;
        }

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            // instantiates the item layout file and the view holder
            View itemView = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.AlimentoCardView, parent, false);

            ResultadoViewHolder vh = new ResultadoViewHolder(itemView, OnClick);
            return vh;
        }

        void OnClick(int position)
        {
            ItemClick?.Invoke(this, position);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Support.V7.Widget;

namespace Mnham_Mnham
{
    [Activity(Label = "PreferenciasActivity", Theme = "@style/AppTheme")]
    public class PreferenciasActivity : Activity
    {
        private RecyclerView mRecyclerView;
        private RecyclerView.LayoutManager mLayoutManager;
        private PreferenciasAdapter mAdapter;
        private IList<Preferencia> preferencias;
        private Button adicionarPreferencia;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your application here
            preferencias = MainActivity.Facade.ConsultarPreferencias();

            SetContentView(Resource.Layout.PreferenciasLayout);

            mRecyclerView = FindViewById<RecyclerView>(Resource.Id.preferenciasRecyclerView);

            mLayoutManager = new LinearLayoutManager(this);
            mRecyclerView.SetLayoutManager(mLayoutManager);

            mAdapter = new PreferenciasAdapter(preferencias);
            mAdapter.ItemClick += OnItemClick;
            mRecyclerView.SetAdapter(mAdapter);

            adicionarPreferencia = FindViewById<Button>(Resource.Id.adicionarPreferenciaButton);
            adicionarPreferencia.Click += AdicionarPreferencia;
        }

        void AdicionarPreferencia(object sender, EventArgs e)
        {
            var intent = new Intent(this, typeof(RegistarPreferenciasActivity));
            StartActivity(intent);
        }

        void OnItemClick(object sender, int position)
        {
            Preferencia itemSelecionado = preferencias[position];
            /*var intent = new Intent(this, typeof(ResultadosActivity));

            intent.PutExtra("pedido", itemSelecionado.Pedido);
            StartActivity(intent);*/
        }
    }

    public class PreferenciaViewHolder : RecyclerView.ViewHolder
    {
        // Add declaration for every view of a result:
        public TextView Alimento { get; }
        public TextView Ingrediente { get; }

        public PreferenciaViewHolder(View itemView, Action<int> listener) : base(itemView)
        {
            Alimento = itemView.FindViewById<TextView>(Resource.Id.alimentoResTextView);
            Ingrediente = itemView.FindViewById<TextView>(Resource.Id.ingredienteResTextView);

            itemView.Click += (sender, e) => listener(AdapterPosition);
        }
    }

    public class PreferenciasAdapter : RecyclerView.Adapter
    {
        // event handler for item clicks
        public event EventHandler<int> ItemClick;

        public IList<Preferencia> Preferencias;
        public override int ItemCount => Preferencias.Count;

        public PreferenciasAdapter(IList<Preferencia> preferencias)
        {
            this.Preferencias = preferencias;
        }

        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            // loads the data at the specified position into the views whose 
            // references are stored in the given view holder
            Preferencia pref = Preferencias[position];
            PreferenciaViewHolder vh = holder as PreferenciaViewHolder;
            vh.Alimento.Text = pref.DesignacaoAlimento;
            vh.Ingrediente.Text = pref.DesignacaoIngrediente;
        }

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            // instantiates the item layout file and the view holder
            View itemView = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.TendenciaCardView, parent, false);

            PreferenciaViewHolder vh = new PreferenciaViewHolder(itemView, OnClick);
            return vh;
        }

        void OnClick(int position)
        {
            ItemClick?.Invoke(this, position);
        }
    }
}
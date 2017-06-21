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
    [Activity(Label = "HistoricoActivity", Theme = "@style/AppTheme")]
    public class HistoricoActivity : Activity
    {
        private RecyclerView mRecyclerView;
        private RecyclerView.LayoutManager mLayoutManager;
        private HistoricoAdapter mAdapter;
        private IList<Pedido> historico;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your application here
            historico = MainActivity.Facade.ConsultarHistorico();

            SetContentView(Resource.Layout.ListLayout);

            mRecyclerView = FindViewById<RecyclerView>(Resource.Id.listRecyclerView);

            mLayoutManager = new LinearLayoutManager(this);
            mRecyclerView.SetLayoutManager(mLayoutManager);

            mAdapter = new HistoricoAdapter(historico);
            mAdapter.ItemClick += OnItemClick;
            mRecyclerView.SetAdapter(mAdapter);
        }

        void OnItemClick(object sender, int position)
        {
            Pedido itemSelecionado = historico[position];
            var intent = new Intent(this, typeof(ResultadosActivity));

            intent.PutExtra("pedido", itemSelecionado.Termo);
            StartActivity(intent);
        }
    }

    public class HistoricoViewHolder : RecyclerView.ViewHolder
    {
        // Add declaration for every view of a result:
        public TextView Request { get; }
        public TextView Date { get; }

        public HistoricoViewHolder(View itemView, Action<int> listener) : base(itemView)
        {
            Request = itemView.FindViewById<TextView>(Resource.Id.pedidoResTextView);
            Date = itemView.FindViewById<TextView>(Resource.Id.dataResTextView);

            itemView.Click += (sender, e) => listener(AdapterPosition);
        }
    }

    public class HistoricoAdapter : RecyclerView.Adapter
    {
        // event handler for item clicks
        public event EventHandler<int> ItemClick;

        public IList<Pedido> Historico;
        public override int ItemCount => Historico.Count;

        public HistoricoAdapter(IList<Pedido> historico)
        {
            this.Historico = historico;
        }

        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            // loads the data at the specified position into the views whose 
            // references are stored in the given view holder
            Pedido tend = Historico[position];
            HistoricoViewHolder vh = holder as HistoricoViewHolder;
            vh.Request.Text = tend.Termo;
            vh.Date.Text = tend.Data.ToString();
        }

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            // instantiates the item layout file and the view holder
            View itemView = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.HistoricoCardView, parent, false);

            HistoricoViewHolder vh = new HistoricoViewHolder(itemView, OnClick);
            return vh;
        }

        void OnClick(int position)
        {
            ItemClick?.Invoke(this, position);
        }
    }
}
﻿using System;
using System.Collections.Generic;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Views;
using Android.Widget;
using Android.Support.V7.Widget;

namespace Mnham_Mnham
{
    [Activity(Label = "TendenciasActivity", Theme = "@style/AppTheme")]
    public class TendenciasActivity : Activity
    {
        private RecyclerView mRecyclerView;
        private RecyclerView.LayoutManager mLayoutManager;
        private TendenciasAdapter mAdapter;
        private List<Tendencia> tendencias;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            tendencias = MainActivity.Facade.ObterTendencias();
            
            SetContentView(Resource.Layout.ListLayout);

            mRecyclerView = FindViewById<RecyclerView>(Resource.Id.listRecyclerView);

            mLayoutManager = new LinearLayoutManager(this);
            mRecyclerView.SetLayoutManager(mLayoutManager);

            mAdapter = new TendenciasAdapter(tendencias);
            mAdapter.ItemClick += OnItemClick;
            mRecyclerView.SetAdapter(mAdapter);
        }

        void OnItemClick(object sender, int position)
        {
            Tendencia itemSelecionado = tendencias[position];
            var intent = new Intent(this, typeof(ResultadosActivity));

            intent.PutExtra("pedido", itemSelecionado.Pedido);
            StartActivity(intent);
        }

    }

    public class TendenciaViewHolder : RecyclerView.ViewHolder
    {
        // Add declaration for every view of a result:
        public TextView Request { get; }
        public TextView Occurrences { get; }

        public TendenciaViewHolder(View itemView, Action<int> listener) : base(itemView)
        {
            Request = itemView.FindViewById<TextView>(Resource.Id.pedidoResTextView);
            Occurrences = itemView.FindViewById<TextView>(Resource.Id.repeticoesResTextView);

            itemView.Click += (sender, e) => listener(AdapterPosition);
        }
    }

    public class TendenciasAdapter : RecyclerView.Adapter
    {
        // event handler for item clicks
        public event EventHandler<int> ItemClick;

        public IList<Tendencia> Tendencias;
        public override int ItemCount => Tendencias.Count;

        public TendenciasAdapter(IList<Tendencia> tendencias)
        {
            this.Tendencias = tendencias;
        }

        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            // loads the data at the specified position into the views whose 
            // references are stored in the given view holder
            Tendencia tend = Tendencias[position];
            TendenciaViewHolder vh = holder as TendenciaViewHolder;
            vh.Request.Text = tend.Pedido;
            vh.Occurrences.Text = tend.Repeticoes.ToString();
        }

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            // instantiates the item layout file and the view holder
            View itemView = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.TendenciaCardView, parent, false);

            TendenciaViewHolder vh = new TendenciaViewHolder(itemView, OnClick);
            return vh;
        }

        void OnClick(int position)
        {
            ItemClick?.Invoke(this, position);
        }
    }

}
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
    [Activity(Label = "ResultadosActivity", Theme = "@style/AppTheme")]
    public class ResultadosActivity : Activity
    {
        RecyclerView mRecyclerView;
        RecyclerView.LayoutManager mLayoutManager;
        ResultadosAdapter mAdapter;
        // add list of results declaration

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your application here

            // get list of results
            // from Intent, most certainly



            SetContentView(Resource.Layout.ResultadosLayout);

            mRecyclerView = FindViewById<RecyclerView>(Resource.Id.resultadosRecyclerView);

            mLayoutManager = new LinearLayoutManager(this);
            mRecyclerView.SetLayoutManager(mLayoutManager);

            mAdapter = new ResultadosAdapter();
            mAdapter.ItemClick += OnItemClick;
            mRecyclerView.SetAdapter(mAdapter);

        }

        void OnItemClick(Object sender, int position)
        {
            // handler for the itemClick event;
            // It should just start a PerfilAlimentoActivity
            // of the item clicked
            // if we pass with an intent the info that we already
            // have on the item to the new activity we wont need
            // to retrieve those bits of info from the database
        }
    }

    public class ResultadoViewHolder : RecyclerView.ViewHolder
    {
        // Add declaration for every view of a result:

        public ImageView foodImage { get; private set; }
        public TextView foodName { get; private set; }
        public RatingBar rating { get; private set; }
        public TextView restaurantName { get; private set; }
        public TextView distance { get; private set; }
        public TextView street { get; private set; }
        public TextView contact { get; private set; }
        public TextView price { get; private set; }

        public ResultadoViewHolder(View itemView) : base(itemView)
        {
            foodImage = itemView.FindViewById<ImageView>(Resource.Id.picAlimentoResImageView);
            foodName = itemView.FindViewById<TextView>(Resource.Id.nomeAlimentoResTextView);
            rating = itemView.FindViewById<RatingBar>(Resource.Id.ratingAlimentoResRatingBar);
            restaurantName = itemView.FindViewById<TextView>(Resource.Id.restauranteAlimentoResTextView);
            distance = itemView.FindViewById<TextView>(Resource.Id.distanciaAlimentoResTextView);
            street = itemView.FindViewById<TextView>(Resource.Id.ruaAlimentoResTextView);
            contact = itemView.FindViewById<TextView>(Resource.Id.contactoAlimentoResTextView);
            price = itemView.FindViewById<TextView>(Resource.Id.precoAlimentoResTextView);

        }
    }

    public class ResultadosAdapter : RecyclerView.Adapter
    {
        // event handler for item clicks
        public event EventHandler<int> ItemClick;

        // TODO: needs reference to underlying data set
        // which in this case is the list of cookings (and its characteristics)
        // resulted from a search  (it would be really nice if was in fact a list, or a wrapper of a list)
        // * add here *

        public ResultadosAdapter()
        {
            // The list of results should be passed to the constructor
        }

        public override int ItemCount
        {
            // returns the number of items in the data source 
            get
            {
                // return *length of result list*;
                throw new NotImplementedException();
            }
        }

        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            // loads the data at the specified position into the views whose 
            // references are stored in the given view holder
            ResultadoViewHolder vh = holder as ResultadoViewHolder;

            /*
            vh.foodImage.SetImageResource(GET IMAGE FROM SOURCE);
            vh.foodName.Text = GET NAME FROM SOURCE;
            vh.rating.setRating( GET RATING FROM SOURCE );
            vh.restaurantName.Text = GET NAME FROM SOURCE;
            vh.distance.Text = GET DISTANCE FROM SOURCE;
            vh.street.Text = GET STREET FROM SOURCE;
            vh.contact.Text = GET CONTACT FROM SOURCE;
            vh.price.Text = GET PRICE FROM SOURCE:
            */
        }

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            // instantiates the item layout file and the view holder
            View itemView = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.AlimentoCardView, parent, false);
            
            ResultadoViewHolder vh = new ResultadoViewHolder(itemView);
            return vh;
        }

        void OnClick(int position)
        {
            ItemClick?.Invoke(this, position);
        }
    }
}
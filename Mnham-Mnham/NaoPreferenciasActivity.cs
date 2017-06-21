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
    [Activity(Label = "NaoPreferenciasActivity", Theme = "@style/AppTheme")]
    public class NaoPreferenciasActivity : Activity
    {
        private RecyclerView mRecyclerView;
        private RecyclerView.LayoutManager mLayoutManager;
        private NaoPreferenciasAdapter mAdapter;
        private IList<Preferencia> naoPreferencias;
        private Button adicionarNaoPreferencia;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your application here
            naoPreferencias = MainActivity.Facade.ConsultarNaoPreferencias();

            SetContentView(Resource.Layout.PreferenciasLayout);

            mRecyclerView = FindViewById<RecyclerView>(Resource.Id.preferenciasRecyclerView);

            mLayoutManager = new LinearLayoutManager(this);
            mRecyclerView.SetLayoutManager(mLayoutManager);

            mAdapter = new NaoPreferenciasAdapter(naoPreferencias);
            mAdapter.ItemClick += OnItemClick;
            mRecyclerView.SetAdapter(mAdapter);

            adicionarNaoPreferencia = FindViewById<Button>(Resource.Id.adicionarPreferenciaButton);
            adicionarNaoPreferencia.Text = "Adicionar Não Preferências";
            adicionarNaoPreferencia.Click += AdicionarNaoPreferencia;
        }

        void AdicionarNaoPreferencia(object sender, EventArgs e)
        {
            var intent = new Intent(this, typeof(RegistarPreferenciasActivity));
            intent.PutExtra("naoPreferencia", true);
            StartActivity(intent);
        }

        void OnItemClick(object sender, int position)
        {
            Preferencia itemSelecionado = naoPreferencias[position];
            var removerNaoPreferencia = new AlertDialog.Builder(this);
            removerNaoPreferencia.SetMessage("Remover Não Preferência?");
            removerNaoPreferencia.SetNeutralButton("Sim", delegate {
                MainActivity.Facade.RemoverNaoPreferencia(itemSelecionado.DesignacaoIngrediente, itemSelecionado.DesignacaoAlimento);
            });
            removerNaoPreferencia.SetNegativeButton("Não", delegate { });

            // Show the alert dialog to the user and wait for response.
            removerNaoPreferencia.Show();
        }
    }

    public class NaoPreferenciaViewHolder : RecyclerView.ViewHolder
    {
        // Add declaration for every view of a result:
        public TextView Alimento { get; }
        public TextView Ingrediente { get; }

        public NaoPreferenciaViewHolder(View itemView, Action<int> listener) : base(itemView)
        {
            Alimento = itemView.FindViewById<TextView>(Resource.Id.alimentoResTextView);
            Ingrediente = itemView.FindViewById<TextView>(Resource.Id.ingredienteResTextView);

            itemView.Click += (sender, e) => listener(AdapterPosition);
        }
    }

    public class NaoPreferenciasAdapter : RecyclerView.Adapter
    {
        // event handler for item clicks
        public event EventHandler<int> ItemClick;

        public IList<Preferencia> NaoPreferencias;
        public override int ItemCount => NaoPreferencias.Count;

        public NaoPreferenciasAdapter(IList<Preferencia> naoPreferencias)
        {
            this.NaoPreferencias = naoPreferencias;
        }

        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            // loads the data at the specified position into the views whose 
            // references are stored in the given view holder
            Preferencia nPref = NaoPreferencias[position];
            Console.WriteLine(holder);
            NaoPreferenciaViewHolder vh = holder as NaoPreferenciaViewHolder;
            vh.Alimento.Text = nPref.DesignacaoAlimento;
            vh.Ingrediente.Text = nPref.DesignacaoIngrediente;
        }

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            // instantiates the item layout file and the view holder
            View itemView = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.PreferenciaCardView, parent, false);

            NaoPreferenciaViewHolder vh = new NaoPreferenciaViewHolder(itemView, OnClick);
            return vh;
        }

        void OnClick(int position)
        {
            ItemClick?.Invoke(this, position);
        }
    }
}
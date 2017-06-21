using Android.App;
using Android.Graphics;
using Android.OS;
using Android.Widget;
using System;

namespace Mnham_Mnham
{
    [Activity(Label = "Mnham_Mnham", Theme = "@style/AppTheme")]
    public class RegistarPreferenciasActivity : Activity
    {
        private TextView titulo;
        private Spinner selecionarUmOuTodos;
        private CheckBox naoPrefCheckBox;
        private TextView alimentoTextView;
        private TextView preferenciasTextView;
        private Button botaoConfirmar;

        private bool naoPreferencia;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            naoPreferencia = Intent.GetBooleanExtra("naoPreferencia", false);

            SetContentView(Resource.Layout.AdicionarPreferenciasLayout);

            titulo = FindViewById<TextView>(Resource.Id.addPrefTitleTextView);
            if (naoPreferencia)
            {
                titulo.Text = "Adicionar Não Preferência";
            }

            selecionarUmOuTodos = FindViewById<Spinner>(Resource.Id.addPrefSpinner);
            //naoPrefCheckBox = FindViewById<CheckBox>(Resource.Id.addPrefCheckBox);
            alimentoTextView = FindViewById<TextView>(Resource.Id.alimentoAddPrefEditText);
            preferenciasTextView = FindViewById<TextView>(Resource.Id.prefsAddPrefEditText);
            botaoConfirmar = FindViewById<Button>(Resource.Id.confirmarAddPrefButton);

            Typeface tf = null;

            tf = Typeface.CreateFromAsset(Application.Context.Assets, "fonts/yellowtail-regular.ttf");
            if (tf != null)
                titulo.SetTypeface(tf, TypefaceStyle.Normal);

            var adapter = ArrayAdapter.CreateFromResource(this, Resource.Array.adicionara, Android.Resource.Layout.SimpleSpinnerItem);
            adapter.SetDropDownViewResource(Android.Resource.Layout.SimpleSpinnerDropDownItem);
            selecionarUmOuTodos.Adapter = adapter;

            if (selecionarUmOuTodos.SelectedItem.ToString().Equals("Todos"))
            {
                alimentoTextView.Clickable = false;
            }

            botaoConfirmar.Click += delegate {
                string ingredientes = preferenciasTextView.Text;
                char[] del = { ';' };
                string[] ingrs = ingredientes.Split(del);
                string alimento = "";
                if (selecionarUmOuTodos.SelectedItem.ToString().Equals("Alimento"))
                {
                    alimento = alimentoTextView.Text;
                }
                foreach (string s in ingrs)
                {
                    Preferencia p = new Preferencia(s.Trim(), alimento);
                    if (!naoPreferencia)
                    {
                        MainActivity.Facade.RegistarPreferencia(p);
                    }
                    else
                    {
                        Console.WriteLine("AQUI!!!");
                        MainActivity.Facade.RegistarNaoPreferencia(p);
                    }
                    
                }
                Finish();
            };

            

        }
    }
}
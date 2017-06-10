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

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.AdicionarPreferenciasLayout);

            titulo = FindViewById<TextView>(Resource.Id.addPrefTitleTextView);
            selecionarUmOuTodos = FindViewById<Spinner>(Resource.Id.addPrefSpinner);
            naoPrefCheckBox = FindViewById<CheckBox>(Resource.Id.addPrefCheckBox);
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
        }
    }
}
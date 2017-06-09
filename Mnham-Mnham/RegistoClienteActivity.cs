using Android.App;
using Android.OS;
using Android.Widget;
using Android.Graphics;

using System;
using Android.Content;

namespace Mnham_Mnham
{
    [Activity(Label = "RegistoClienteActivity", Icon = "@drawable/icon", Theme = "@style/AppTheme")]
    public class RegistoClienteActivity : Activity
    {
        private TextView titulo;
        private EditText editTextNome, editTextEmail, editTextPassword, editTextConfirmaPassword;
        private Spinner spinnerGenero;
        private Button botaoConfirmar, botaoCancelar;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.RegistoClienteLayout);

            Typeface tf = null;
            titulo = FindViewById<TextView>(Resource.Id.regClienteTextView);

            tf = Typeface.CreateFromAsset(Application.Context.Assets, "fonts/yellowtail-regular.ttf");
            if (tf != null)
                titulo.SetTypeface(tf, TypefaceStyle.Normal);

            editTextNome = FindViewById<EditText>(Resource.Id.nomeRegClienteEditText);
            editTextEmail = FindViewById<EditText>(Resource.Id.emailRegClienteEditText);
            editTextPassword = FindViewById<EditText>(Resource.Id.passwordRegClienteEditText);
            editTextConfirmaPassword = FindViewById<EditText>(Resource.Id.confirmaPassRegClienteEditText);

            spinnerGenero = FindViewById<Spinner>(Resource.Id.generoRegClienteSpinner);

            var adapter = ArrayAdapter.CreateFromResource(this, Resource.Array.generos, Android.Resource.Layout.SimpleSpinnerItem);
            adapter.SetDropDownViewResource(Android.Resource.Layout.SimpleSpinnerDropDownItem);
            spinnerGenero.Adapter = adapter;

            botaoConfirmar = FindViewById<Button>(Resource.Id.confirmRegClienteButton);
            botaoCancelar = FindViewById<Button>(Resource.Id.cancelRegClienteButton);

            botaoConfirmar.Click += HandlerRegistoCliente;

            botaoCancelar.Click += delegate
            {
                Finish();
            };
        }

        private void HandlerRegistoCliente(object obj, EventArgs args)
        {
            if (editTextNome.Text.Equals("") || editTextEmail.Equals("") ||
                editTextPassword.Text.Equals("") || editTextConfirmaPassword.Equals(""))
            {
                Toast.MakeText(this, "Por favor, preencha todos os campos.", ToastLength.Short).Show();
                return;
            }

            // Só chegamos a este ponto se todos os campos estiverem preenchidos.
            string nome, email, genero;
            string palavraPasse, confirmacaoPalavraPasse;

            palavraPasse = editTextPassword.Text;
            confirmacaoPalavraPasse = editTextConfirmaPassword.Text;
            if (!palavraPasse.Equals(confirmacaoPalavraPasse))
            {
                Toast.MakeText(this, "As palavras-passe introduzidas não coincidem.", ToastLength.Short).Show();
                return;
            }
            nome = editTextNome.Text;
            email = editTextEmail.Text;
            genero = spinnerGenero.SelectedItem.ToString();

            bool res = MainActivity.Facade.RegistarCliente(new Cliente(genero[0], email, nome, palavraPasse));
            if (res == true)
            {
                var intent = new Intent(this, typeof(MainActivity));
                intent.SetFlags(ActivityFlags.NewTask | ActivityFlags.ClearTask);
                Toast.MakeText(this, "Registo efetuado com sucesso.", ToastLength.Short).Show();

                StartActivity(intent);
            }
            else
                Toast.MakeText(this, "Já existe um utilizador com o e-mail indicado.", ToastLength.Short).Show();
        }
    }
}

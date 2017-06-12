using System;

using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Graphics;
using Android.OS;
using Android.Widget;

namespace Mnham_Mnham
{
    [Activity(Label = "Mnham.Droid", Icon = "@drawable/icon", ScreenOrientation = ScreenOrientation.Portrait, Theme = "@style/AppTheme")]
    public class LoginActivity : Activity
    {
        private EditText emailEditText;
        private EditText passwordEditText;
        private Button botaoConfirmar;
        private Button botaoCancelar;

        //=====================================================================
        // HANDLERS
        //=====================================================================
        private void HandlerLogin(object sender, EventArgs e)
        {
            string email = emailEditText.Text;
            string palavraPasse = passwordEditText.Text;

            if (!email.Equals("") && !palavraPasse.Equals(""))
            {
                Intent intent;
                int res = MainActivity.Facade.IniciarSessao(email, palavraPasse);

                switch (res)
                {
                    case 0:
                        Toast.MakeText(this, "Pelo menos uma das credenciais introduzidas é inválida.", ToastLength.Short).Show();
                        break;
                    case 1:
                        intent = new Intent(this, typeof(MainActivity));
                        intent.SetFlags(ActivityFlags.NewTask | ActivityFlags.ClearTask);
                        intent.PutExtra("utilizador_email", email);

                        Toast.MakeText(this, "Login bem sucedido.", ToastLength.Short).Show();
                        StartActivity(intent);
                        break;
                    /* case 2:
                        msgLogin = "Login bem sucedido.";
                        intent = new Intent(this, typeof(MainActivity));
                        intent.PutExtra("user_email", email);
                        StartActivity(intent);
                        break;
                     */
                }
            }
            else
                Toast.MakeText(this, "Por favor, preencha todos os campos.", ToastLength.Short).Show();
        }

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            SetContentView(Resource.Layout.LoginLayout);

            TextView loginTextView = FindViewById<TextView>(Resource.Id.loginTextView);
            Typeface tf = null;
            tf = Typeface.CreateFromAsset(Application.Context.Assets, "fonts/yellowtail-regular.ttf");
            if (tf != null)
                loginTextView.SetTypeface(tf, TypefaceStyle.Normal);

            emailEditText = FindViewById<EditText>(Resource.Id.emailLoginEditText);
            passwordEditText = FindViewById<EditText>(Resource.Id.passwordLoginEditText);

            botaoConfirmar = FindViewById<Button>(Resource.Id.botaoConfirmarLogin);
            botaoConfirmar.Click += HandlerLogin;

            botaoCancelar = FindViewById<Button>(Resource.Id.botaoCancelarLogin);
            botaoCancelar.Click += delegate
            {
                Finish();
            };
        }
    }
}

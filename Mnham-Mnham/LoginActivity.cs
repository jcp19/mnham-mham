using Android.App;
using Android.Content;
using Android.Widget;
using Android.OS;
using Android.Content.PM;
using Android.Graphics;

using System;

namespace Mnham_Mnham
{
    [Activity(Label = "Mnham.Droid", Icon = "@drawable/icon", ScreenOrientation = ScreenOrientation.Portrait, Theme = "@style/AppTheme")]
    public class LoginActivity : Activity
    {
        private EditText emailEditText;
        private EditText passwordEditText;
        private Button confirmButton;
        private Button cancelButton;

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

            confirmButton = FindViewById<Button>(Resource.Id.confirmLoginButton);
            confirmButton.Click += HandlerLogin;

            cancelButton = FindViewById<Button>(Resource.Id.cancelLoginButton);
            cancelButton.Click += delegate
            {
                Finish();
            };
        }

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
                        intent.PutExtra("user_email", email);

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
            {
                Toast toastErro = Toast.MakeText(this, "Por favor, preencha todos os campos.", ToastLength.Short);
                toastErro.Show();
            }
        }
    }
}

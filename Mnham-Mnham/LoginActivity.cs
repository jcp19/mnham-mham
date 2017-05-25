using System;

using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using System.Collections.Generic;
using Android.Content.PM;
using Android.Graphics;

namespace Mnham.Droid
{
    [Activity(Label = "Mnham.Droid", Icon = "@drawable/icon", ScreenOrientation = ScreenOrientation.Portrait, Theme = "@style/AppTheme")]
    public class MainActivity : Activity
    {

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.LoginLayout);

            TextView loginTextView = FindViewById<TextView>(Resource.Id.loginTextView);
            Typeface tf = null;
            tf = Typeface.CreateFromAsset(Application.Context.Assets, "fonts/yellowtail-regular.ttf");
            if (tf != null)
                loginTextView.SetTypeface(tf, TypefaceStyle.Normal);

            EditText emailEditText = FindViewById<EditText>(Resource.Id.emailLoginEditText);
            EditText passwordEditText = FindViewById<EditText>(Resource.Id.passwordLoginEditText);

            Button confirmButton = FindViewById<Button>(Resource.Id.confirmLoginButton);
            confirmButton.Click += delegate
            {
                string email = emailEditText.Text;
                string password = passwordEditText.Text;
                if (!email.Equals("") && !passwordEditText.Equals(""))
                {
                    var intent = new Intent(this, typeof(RelativeActivity));
                    intent.PutExtra("user_email", email);
                    StartActivity(intent);
                    this.Finish();
                }
                else
                {
                    Toast errorMessage = Toast.MakeText(this, "Login Failed", ToastLength.Short);
                    errorMessage.Show();
                }
            };

            Button cancelButton = FindViewById<Button>(Resource.Id.cancelLoginButton);
            cancelButton.Click += delegate
            {
                Finish();
            };
        }
    }
}
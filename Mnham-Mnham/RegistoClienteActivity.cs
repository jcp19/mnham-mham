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

namespace Mnham_Mnham
{
    [Activity(Label = "RegistoClienteActivity", Icon = "@drawable/icon", Theme = "@style/AppTheme")]
    public class RegistoClienteActivity : Activity
    {
        EditText nameEditText, emailEditText, passwordEditText, confirmaPasswordEditText;
        Spinner generoSpinner;
        Button confirmButton, cancelButton;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.RegistoClienteLayout);

            // Create your application here
            nameEditText = FindViewById<EditText>(Resource.Id.nomeRegClienteEditText);
            emailEditText = FindViewById<EditText>(Resource.Id.emailRegClienteEditText);
            passwordEditText = FindViewById<EditText>(Resource.Id.passwordRegClienteEditText);
            confirmaPasswordEditText = FindViewById<EditText>(Resource.Id.confirmaPassRegClienteEditText);

            generoSpinner = FindViewById<Spinner>(Resource.Id.generoRegClienteSpinner);

            var adapter = ArrayAdapter.CreateFromResource(this, Resource.Array.generos, Android.Resource.Layout.SimpleSpinnerItem);
            adapter.SetDropDownViewResource(Android.Resource.Layout.SimpleSpinnerDropDownItem);
            generoSpinner.Adapter = adapter;

            confirmButton = FindViewById<Button>(Resource.Id.confirmRegClienteButton);
            cancelButton = FindViewById<Button>(Resource.Id.cancelRegClienteButton);

            /* TODO: delegate click for confirmButton */

            cancelButton.Click += delegate
            {
                Finish();
            };
        }
    }
}
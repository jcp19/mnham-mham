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

namespace Mnham.Droid
{
    [Activity(Label = "RegistoPropActivity", Icon = "@drawable/icon", Theme = "@style/AppTheme")]
    public class RegistoPropActivity : Activity
    {
        EditText nameEditText, emailEditText, passwordEditText, confirmaPasswordEditText, phoneNumberEditText;
        Spinner generoSpinner;
        Button confirmButton, cancelButton;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.RegistoPropLayout);

            // Create your application here
            nameEditText = FindViewById<EditText>(Resource.Id.nomeRegPropEditText);
            emailEditText = FindViewById<EditText>(Resource.Id.emailRegPropEditText);
            passwordEditText = FindViewById<EditText>(Resource.Id.passwordRegPropEditText);
            confirmaPasswordEditText = FindViewById<EditText>(Resource.Id.confirmaPassRegPropEditText);
            phoneNumberEditText = FindViewById<EditText>(Resource.Id.phoneNumberRegPropEditText);

            generoSpinner = FindViewById<Spinner>(Resource.Id.generoRegPropSpinner);

            var adapter = ArrayAdapter.CreateFromResource(this, Resource.Array.generos, Android.Resource.Layout.SimpleSpinnerItem);
            adapter.SetDropDownViewResource(Android.Resource.Layout.SimpleSpinnerDropDownItem);
            generoSpinner.Adapter = adapter;

            confirmButton = FindViewById<Button>(Resource.Id.confirmRegPropButton);
            cancelButton = FindViewById<Button>(Resource.Id.cancelRegPropButton);

            cancelButton.Click += delegate
            {
                Finish();
            };

        }
    }
}
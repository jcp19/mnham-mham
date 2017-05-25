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
using Android.Text;
using Android.Graphics;
using Java.Lang;
using Android.Content.Res;

using Mikepenz.Typeface;
using Mikepenz.MaterialDrawer.Utils;

using Mikepenz.Iconics;
using Mikepenz.MaterialDrawer;
using Mikepenz.MaterialDrawer.Models;
using Mikepenz.MaterialDrawer.Models.Interfaces;
using Android.Content.PM;

namespace Mnham_Mnham
{

    [Activity(Label = "Mnham Mnham", MainLauncher = true, Icon = "@drawable/icon", ScreenOrientation = ScreenOrientation.Portrait, Theme = "@style/AppTheme")]
    public class RelativeActivity : Activity, AccountHeader.IOnAccountHeaderListener, Drawer.IOnDrawerItemClickListener
    {
        public TextView alimTextView;
        AccountHeader headerResult = null;
        Drawer result = null;
        DrawerBuilder drawerBuilder = null;
        const int PROFILE_SETTING = 1;
        const int PERFIL_NAO_AUTENTICADO = 2;
        const int AUTENTICADO = 3;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your application here
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Relative);

            string email = Intent.GetStringExtra("user_email") ?? "";

            alimTextView = (TextView)FindViewById(Resource.Id.textView1);

            Typeface tf = null;
            tf = Typeface.CreateFromAsset(Application.Context.Assets, "fonts/yellowtail-regular.ttf");
            if (tf != null)
                alimTextView.SetTypeface(tf, TypefaceStyle.Normal);

            EditText alimEditText = (EditText)FindViewById(Resource.Id.editText1);
            /* Make the drawable right clickable
            alimEditText.Click += (sender,e) =>
            {
                (sender as EditText).OnTouchEvent(e);
            } */

            var user = new ProfileDrawerItem();
            user.WithName(email.Equals("") ? "Não Autenticado" : "Nome");
            user.WithIcon(Resource.Drawable.profile3);
            user.WithIdentifier(email.Equals("") ? PERFIL_NAO_AUTENTICADO : AUTENTICADO);
            if (!email.Equals(""))
                user.WithEmail(email);


            headerResult = new AccountHeaderBuilder()
                .WithActivity(this)
                .WithHeaderBackground(Resource.Drawable.header)
                .AddProfiles(user)
                .WithSelectionListEnabledForSingleProfile(false)
                .WithOnAccountHeaderListener(this)
                .WithSavedInstance(savedInstanceState)
                .Build();

            /* TODO: register hardcoded string in strings.xml*/

            // items that are always there
            var item10 = new SecondaryDrawerItem();
            item10.WithName("Definições");
            item10.WithIcon(FontAwesome.Icon.FawCog);
            item10.WithIdentifier(20);
            item10.WithSelectable(true);
            //
            var item11 = new SecondaryDrawerItem();
            item11.WithName("Sobre");
            item11.WithIcon(FontAwesome.Icon.FawBook);
            item11.WithIdentifier(10);
            item10.WithSelectable(true);
            item11.WithTag("Bullhorn");
            drawerBuilder = new DrawerBuilder()
                .WithActivity(this)
                .WithAccountHeader(headerResult); //set the AccountHeader we created earlier for the header

            if (email.Equals(""))
            {
                var item1 = new PrimaryDrawerItem();
                item1.WithName("Login");
                item1.WithIcon(GoogleMaterial.Icon.GmdPerson);
                item1.WithIdentifier(1);
                item1.WithSelectable(true);

                var item2 = new PrimaryDrawerItem();
                item2.WithName("Registar Cliente");
                item2.WithIcon(GoogleMaterial.Icon.GmdPersonAdd);
                item2.WithIdentifier(2);
                item2.WithTag("RegCliente");
                item2.WithSelectable(true);

                var item3 = new PrimaryDrawerItem();
                item3.WithName("Registar Proprietário");
                item3.WithIcon(GoogleMaterial.Icon.GmdPersonAdd);
                item3.WithIdentifier(2);
                item3.WithTag("RegProprietário");
                item3.WithSelectable(true);
                drawerBuilder.AddDrawerItems(
                    item1,
                    item2,
                    item3,
                    new DividerDrawerItem(),
                    item10,
                    item11,
                    new DividerDrawerItem()
                ); // add the items we want to use With our Drawer
            }
            else
            {
                var item1 = new PrimaryDrawerItem();
                item1.WithName("Preferências");
                item1.WithIcon(GoogleMaterial.Icon.GmdPerson);
                item1.WithIdentifier(3);
                item1.WithSelectable(true);

                var item2 = new PrimaryDrawerItem();
                item2.WithName("Não Preferências");
                item2.WithIcon(GoogleMaterial.Icon.GmdPersonAdd);
                item2.WithIdentifier(4);
                item2.WithSelectable(true);

                var item3 = new PrimaryDrawerItem();
                item3.WithName("Terminar sessão");
                item3.WithIdentifier(5);
                item3.WithSelectable(true);

                drawerBuilder.AddDrawerItems(
                    item1,
                    item2,
                    item3,
                    new DividerDrawerItem(),
                    item10,
                    item11,
                    new DividerDrawerItem()
                );
            }

            result = drawerBuilder.WithOnDrawerItemClickListener(this)
                         .WithSavedInstance(savedInstanceState)
                         .WithShowDrawerOnFirstLaunch(true)
                         .Build();

            //if you have many different types of DrawerItems you can magically pre-cache those items to get a better scroll performance
            //make sure to init the cache after the DrawerBuilder was created as this will first clear the cache to make sure no old elements are in
            RecyclerViewCacheUtil.Instance.WithCacheSize(2).Init(result);

            //only set the active selection or active profile if we do not recreate the activity
            if (savedInstanceState == null)
            {
                // set the selection to the item with the identifier 11
                result.SetSelection(-1, false);

                //set the active profile
                headerResult.SetActiveProfile(user, false);
            }
        }

        public bool OnItemClick(View view, int position, IDrawerItem drawerItem)
        {
            //check if the drawerItem is set.
            //there are different reasons for the drawerItem to be null
            //--> click on the header
            //--> click on the footer
            //those items don't contain a drawerItem

            if (drawerItem != null)
            {
                Toast message;
                switch (drawerItem.Identifier)
                {
                    case 1:
                        message = Toast.MakeText(this, "Clicked Login", ToastLength.Short);
                        message.Show();
                        StartActivity(typeof(MainActivity));
                        break;
                    case 2:
                        string m;

                        if (drawerItem.Tag.Equals("RegCliente"))
                        {
                            //m = "Registar Cliente";
                            StartActivity(typeof(RegistoClienteActivity));
                        }
                        else
                        {
                            m = "Registar Proprietário";
                            message = Toast.MakeText(this, string.Format("Clicked {0}", m), ToastLength.Short);
                            message.Show();
                        }
                        break;
                    case 3:
                        message = Toast.MakeText(this, "Clicked Preferencias", ToastLength.Short);
                        message.Show();
                        break;
                    case 4:
                        message = Toast.MakeText(this, "Clicked Não Preferencias", ToastLength.Short);
                        message.Show();
                        break;
                    case 5:

                        /* About ActivityFlags: */
                        /* https://developer.xamarin.com/api/type/Android.Content.ActivityFlags/ */
                        message = Toast.MakeText(this, "Clicked Terminar Sessão", ToastLength.Short);
                        message.Show();
                        var intent = new Intent(this, typeof(RelativeActivity));
                        intent.SetFlags(ActivityFlags.NewTask | ActivityFlags.ClearTask);
                        StartActivity(intent);
                        Console.WriteLine("Reached finish");
                        break;
                    case 10:
                        message = Toast.MakeText(this, "Clicked Definições", ToastLength.Short);
                        message.Show();
                        break;
                    case 20:
                        message = Toast.MakeText(this, "Clicked Sobre", ToastLength.Short);
                        message.Show();
                        break;
                }
                drawerItem.WithSetSelected(false);
            }

            return false;
        }

        public bool OnProfileChanged(View view, IProfile profile, bool current)
        {
            //sample usage of the onProfileChanged listener
            //if the clicked item has the identifier 1 add a new profile ;)
            if (profile is IDrawerItem && profile.Identifier == PROFILE_SETTING)
            {
                int count = 100 + headerResult.Profiles.Count + 1;
                IProfile newProfile = new ProfileDrawerItem().WithNameShown(true).WithName("Batman" + count).WithEmail("batman" + count + "@gmail.com").WithIcon(Resource.Drawable.profile3);
                newProfile.WithIdentifier(count);
                if (headerResult.Profiles != null)
                {
                    //we know that there are 2 setting elements. set the new profile above them ;)
                    headerResult.AddProfile(newProfile, headerResult.Profiles.Count - 2);
                }
                else
                {
                    headerResult.AddProfiles(newProfile);
                }
            }

            //false if you have not consumed the event and it should close the drawer
            return false;
        }

        protected override void OnSaveInstanceState(Bundle outState)
        {
            //add the values which need to be saved from the drawer to the bundle
            outState = result.SaveInstanceState(outState);
            //add the values which need to be saved from the accountHeader to the bundle
            outState = headerResult.SaveInstanceState(outState);
            base.OnSaveInstanceState(outState);
        }
    }


}


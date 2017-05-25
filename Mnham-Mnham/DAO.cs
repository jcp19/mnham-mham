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
    class DAO
    {
        private SqlConnection sqlCon;

        public DAO()
        {
            sqlCon = new SqlConnection("Data Source=DESKTOP-3SNVUJ2;Initial Catalog=mnham-mnham;Integrated Security=True");
        }

        public DAO(string queryString)
        {
            sqlCon = new SqlConnection(queryString);
        }

    }
}
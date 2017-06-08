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
using System.Data;
using System.Data.SqlClient;

namespace Mnham_Mnham
{
    class DAO
    {
        // MUDAR IP, testar acesso à BD
        private static string my_ip = "";
        private static string SQL_STRING = "Data Source=" + my_ip + ";Network Library=DBMSSOCN;Initial Catalog = mnham-mnham;"; // User ID = myUsername; Password=myPassword;";
            
        /* "Data Source=DESKTOP-3SNVUJ2;Initial Catalog=mnham-mnham;Integrated Security=True"; */
        protected SqlConnection sqlCon;

        public DAO()
        {
            sqlCon = new SqlConnection(SQL_STRING);
        }

        public DAO(string queryString)
        {
            sqlCon = new SqlConnection(queryString);
        }

    }
}
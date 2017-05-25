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
    class ClienteDAO : DAO
    {
        private SqlConnection sqlCon;

        public ClienteDAO() : base()
        {
        }
        
        public ClienteDAO(string queryString) : base(queryString)
        {
        }

        public Cliente get(string email, string passwordMD5)
        {
            SqlCommand cmd = new SqlCommand(/* put values here */);
        }

        static int Main()
        {
            DAO d = new Mnham_Mnham.DAO();
            SqlCommand cmd = new SqlCommand();
            SqlDataReader reader;

            cmd.CommandText = "SELECT * FROM Cliente";
            cmd.CommandType = CommandType.Text;
            cmd.Connection = d.sqlCon;

            d.sqlCon.Open();

            reader = cmd.ExecuteReader();
            // Data is accessible through the DataReader object here.
            while (reader.Read())
            {
                IDataRecord data = reader;
                Console.WriteLine(String.Format("{0}, {1}", data[0], data[1]));
            }
            d.sqlCon.Close();

            return 0;
        }
    }
}
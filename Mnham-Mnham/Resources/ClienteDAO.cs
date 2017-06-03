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
        public ClienteDAO() : base()
        {
        }
        
        public ClienteDAO (string queryString) : base(queryString)
        {
        }

        public Cliente Get (string email)
        {
            SqlCommand cmd = new SqlCommand("SELECT * FROM Cliente WHERE email = @email", base.sqlCon);
            cmd.Parameters.Add("@email", SqlDbType.NVarChar, 50);
            cmd.Parameters["@email"].Value = email;

            bool contains;
            SqlDataReader reader = cmd.ExecuteReader();
            Cliente c = null;

            try
            {
                contains = reader.Read();
                if(contains)
                {
                    c = new Cliente(/* construtor aqui */);
                }
            }
            finally
            {
                reader.Close();
            }
            return c;
        }

        public bool Contains(string email, string password)
        {
            SqlCommand cmd = new SqlCommand("SELECT email FROM Cliente WHERE email = @email AND palavra_passe = @pp", base.sqlCon);

            cmd.Parameters.Add("@email", SqlDbType.NVarChar, 50);
            cmd.Parameters["@email"].Value = email;

            cmd.Parameters.Add("@pp", SqlDbType.Char, 32);
            cmd.Parameters["@pp"].Value = password;
            SqlDataReader reader = cmd.ExecuteReader();
            bool contains = false;

            try
            {
                contains = reader.Read();
            }
            finally
            {
                reader.Close();
            }
            return contains;
        }

        internal bool TryGetValue(string email, out Cliente cliente)
        {
            cliente = Get(email);
            return (cliente == null);
        }
    }
}
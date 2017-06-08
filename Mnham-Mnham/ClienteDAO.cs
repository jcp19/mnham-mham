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
        private PreferenciaDAO preferencias;
        private NaoPreferenciaDAO naoPreferencias;

        public ClienteDAO() : base()
        {
        }
        
        public ClienteDAO(string queryString) : base(queryString)
        {
        }

        public Cliente ObterPorEmail(string email)
        {
            SqlCommand cmd = new SqlCommand("SELECT * FROM Cliente WHERE email = @email", base.sqlCon);
            cmd.Parameters.Add("@email", SqlDbType.NVarChar, 50);
            cmd.Parameters["@email"].Value = email;

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

            Cliente c = null;
            if (contains)
            {
                c = new Cliente(Convert.ToInt32(reader["id"]), Convert.ToChar(reader["genero"]), email, reader["nome"].ToString(), reader["palavra_passe"].ToString());
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

        public bool ContemEmail(string email)
        {
            Cliente c = ObterPorEmail(email);
            return c != null;
        }

        public Cliente ObterPorId(int id)
        {
            SqlCommand cmd = new SqlCommand("SELECT * FROM Cliente WHERE id = @id", base.sqlCon);
            cmd.Parameters.Add("@id", SqlDbType.NVarChar, 50);
            cmd.Parameters["@id"].Value = id;

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

            Cliente c = null;
            if (contains)
            {
                c = new Cliente(id, Convert.ToChar(reader["genero"]), reader["email"].ToString(), reader["nome"].ToString(), reader["palavra_passe"].ToString());
            }

            return c;
        }

        public bool AdicionarCliente(Cliente cliente)
        // ideia: passar cliente como ref e atualizar o seu id
        {
            // ignora o id do cliente e cria um novo
            bool adicionou = true;
            SqlCommand cmd = new SqlCommand("INSERT INTO Cliente(genero,email,nome,palavra_passe) VALUES (@genero, @email, @nome, @palavra_passe);", base.sqlCon);
            cmd.Parameters.Add("@genero", SqlDbType.Char, 1);
            cmd.Parameters["@genero"].Value = cliente.Genero;

            cmd.Parameters.Add("@email", SqlDbType.NVarChar, 50);
            cmd.Parameters["@email"].Value = cliente.Email;

            cmd.Parameters.Add("@nome", SqlDbType.NVarChar, 75);
            cmd.Parameters["@nome"].Value = cliente.Nome;

            cmd.Parameters.Add("@palavra_passe", SqlDbType.Char, 32);
            cmd.Parameters["@palavra_passe"].Value = cliente.PalavraPasse;

            try
            {
                cmd.Connection.Open();
                cmd.ExecuteNonQuery();
            }
            catch (SqlException)
            {
                // se ja existe um cliente registado com o mesmo email ou 
                // ha algum problema no acesso à BD
                adicionou = false;
            }
            finally
            {
                cmd.Connection.Close();
            }
            return adicionou;
        }
        public bool AdicionarPreferencia(int clienteAutenticado, Preferencia preferencia)
        {
            return preferencias.AdicionarPreferencia(clienteAutenticado, preferencia);
        }

        public bool AdicionarNaoPreferencia(int clienteAutenticado, Preferencia naoPreferencia)
        {
            return naoPreferencias.AdicionarNaoPreferencia(clienteAutenticado, naoPreferencia);
        }

        public bool RemoverPreferencia(int clienteAutenticado, Preferencia preferencia)
        {
            return preferencias.RemoverPreferencia(clienteAutenticado, preferencia);
        }

        public bool RemoverNaoPreferencia(int clienteAutenticado, Preferencia naoPreferencia)
        {
            return naoPreferencias.RemoverNaoPreferencia(clienteAutenticado, naoPreferencia);
        }

        internal List<Preferencia> ConsultarPreferencias(int clienteAutenticado)
        {
            return preferencias.ConsultarPreferencias(clienteAutenticado);
        }

        internal List<Preferencia> ConsultarNaoPreferencias(int clienteAutenticado)
        {
            return naoPreferencias.ConsultarNaoPreferencias(clienteAutenticado);
        }

        internal void EditarDados(Cliente cliente)
        {
            throw new NotImplementedException();
        }
    }
}
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace Mnham_Mnham
{
    public class ClienteDAO
    {
        private readonly PreferenciaDAO preferencias;
        private readonly NaoPreferenciaDAO naoPreferencias;

        public ClienteDAO()
        {
            preferencias = new PreferenciaDAO();
            naoPreferencias = new NaoPreferenciaDAO();
        }

        public Cliente ObterPorEmail(string email)
        {
            Cliente c = null;

            using (var sqlCon = new SqlConnection(DAO.CONECTION_STRING))
            {
                string txtCmd = "SELECT * FROM Cliente WHERE email = @email";

                sqlCon.Open();
                using (var cmd = new SqlCommand(txtCmd, sqlCon))
                {
                    cmd.Parameters.Add("@email", SqlDbType.NVarChar, 50);
                    cmd.Parameters["@email"].Value = email;

                    using (var reader = cmd.ExecuteReader())
                    {
                        bool existe = reader.Read();

                        if (existe)
                        {
                            int id = Convert.ToInt32(reader["id"]);
                            char genero = Convert.ToChar(reader["genero"]);
                            string nome = reader["nome"].ToString();
                            string palavraPasse = reader["palavra_passe"].ToString();

                            c = new Cliente(id, genero, email, nome, palavraPasse);
                        }
                    }
                }
            }
            return c;
        }

        public bool Contains(string email, string password)
        {
            using (var sqlCon = new SqlConnection(DAO.CONECTION_STRING))
            {
                string txtCmd = "SELECT email FROM Cliente WHERE email = @email AND palavra_passe = @pass";

                sqlCon.Open();
                using (var cmd = new SqlCommand(txtCmd, sqlCon))
                {
                    cmd.Parameters.Add("@email", SqlDbType.NVarChar, 50);
                    cmd.Parameters.Add("@pass", SqlDbType.VarChar);

                    cmd.Parameters["@email"].Value = email;
                    cmd.Parameters["@pass"].Value = password;

                    using (var reader = cmd.ExecuteReader())
                    {
                        return reader.Read();
                    }
                }
            }
        }

        public bool ContemEmail(string email)
        {
            return (ObterPorEmail(email) != null);
        }

        public Cliente ObterPorId(int id)
        {
            Cliente c = null;

            using (var sqlCon = new SqlConnection(DAO.CONECTION_STRING))
            {
                string txtCmd = "SELECT * FROM Cliente WHERE id = @id";

                sqlCon.Open();
                using (var cmd = new SqlCommand(txtCmd, sqlCon))
                {
                    cmd.Parameters.Add("@id", SqlDbType.NVarChar, 50);
                    cmd.Parameters["@id"].Value = id;

                    using (var reader = cmd.ExecuteReader())
                    {
                        bool existe = reader.Read();


                        if (existe)
                        {
                            char genero = Convert.ToChar(reader["genero"]);
                            string email = reader["email"].ToString();
                            string nome = reader["nome"].ToString();
                            string palavraPasse = reader["palavra_passe"].ToString();

                            c = new Cliente(id, genero, email, nome, palavraPasse);
                            ISet<Preferencia> prefs = new HashSet<Preferencia>(preferencias.ConsultarPreferencias(id));
                            ISet<Preferencia> naoPrefs = new HashSet<Preferencia>(naoPreferencias.ConsultarNaoPreferencias(id));
                            c.Preferencias = prefs;
                            c.NaoPreferencias = naoPrefs;
                        }
                    }
                }
            }
            return c;
        }

        public bool AdicionarCliente(Cliente cliente)
        // ideia: passar cliente como ref e atualizar o seu id
        {
            bool adicionou = true;

            using (var sqlCon = new SqlConnection(DAO.CONECTION_STRING))
            {
                // ignora o id do cliente e cria um novo
                string txtCmd = @"INSERT INTO Cliente(genero,email,nome,palavra_passe)
                                  VALUES (@genero, @email, @nome, @palavra_passe);";

                sqlCon.Open();
                using (var cmd = new SqlCommand(txtCmd, sqlCon))
                {
                    cmd.Parameters.Add("@genero", SqlDbType.Char, 1);
                    cmd.Parameters.Add("@email", SqlDbType.NVarChar, 50);
                    cmd.Parameters.Add("@nome", SqlDbType.NVarChar, 75);
                    cmd.Parameters.Add("@palavra_passe", SqlDbType.VarChar);

                    cmd.Parameters["@genero"].Value = cliente.Genero;
                    cmd.Parameters["@email"].Value = cliente.Email;
                    cmd.Parameters["@nome"].Value = cliente.Nome;
                    cmd.Parameters["@palavra_passe"].Value = cliente.PalavraPasse;

                    try
                    {
                        cmd.ExecuteNonQuery();
                    }
                    catch (SqlException)
                    {
                        // se ja existe um cliente registado com o mesmo email ou 
                        // ha algum problema no acesso à BD
                        adicionou = false;
                    }
                }
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

        public IList<Preferencia> ConsultarPreferencias(int clienteAutenticado)
        {
            return preferencias.ConsultarPreferencias(clienteAutenticado);
        }

        public IList<Preferencia> ConsultarNaoPreferencias(int clienteAutenticado)
        {
            return naoPreferencias.ConsultarNaoPreferencias(clienteAutenticado);
        }

        // POR FAZER!!
        public void EditarDados(Cliente cliente)
        {
            throw new NotImplementedException();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace Mnham_Mnham
{
    internal class ProprietarioDAO
    {
        EstabelecimentoDAO estabelecimentos;

        internal Proprietario ObterPorEmail(string email)
        {
            Proprietario p = null;

            using (SqlConnection sqlCon = new SqlConnection(DAO.CONECTION_STRING))
            {
                SqlCommand cmd = new SqlCommand("SELECT * FROM Proprietario WHERE email = @email", sqlCon);
                cmd.Parameters.Add("@email", SqlDbType.NVarChar, 50);
                cmd.Parameters["@email"].Value = email;

                cmd.Connection.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                bool existe = reader.Read();
                reader.Close();

                if (existe)
                {
                    p = new Proprietario(Convert.ToInt32(reader["id"]), Convert.ToChar(reader["genero"]), email, reader["nome"].ToString(), reader["palavra_passe"].ToString(), reader["contacto_tel"].ToString());
                }
            }
            return p;
        }

        internal bool AdicionarProprietario(Proprietario proprietario)
        {
            using (SqlConnection sqlCon = new SqlConnection(DAO.CONECTION_STRING))
            {
                // ignora o id do proprietario e cria um novo
                bool adicionou = true;

                SqlCommand cmd = new SqlCommand("INSERT INTO Proprietario(email,palavra_passe,nome,contacto_tel,genero) VALUES (@email, @palavra_passe, @nome, @contacto_tel, @genero);", sqlCon);
                cmd.Parameters.Add("@genero", SqlDbType.Char, 1);
                cmd.Parameters["@genero"].Value = proprietario.Genero;

                cmd.Parameters.Add("@email", SqlDbType.NVarChar, 50);
                cmd.Parameters["@email"].Value = proprietario.Email;

                cmd.Parameters.Add("@nome", SqlDbType.NVarChar, 75);
                cmd.Parameters["@nome"].Value = proprietario.Nome;

                cmd.Parameters.Add("@palavra_passe", SqlDbType.Char, 32);
                cmd.Parameters["@palavra_passe"].Value = proprietario.PalavraPasse;

                cmd.Parameters.Add("@contacto_tel", SqlDbType.Char, 19);
                cmd.Parameters["@contacto_tel"].Value = proprietario.ContactoTel;

                cmd.Connection.Open();

                try
                {
                    cmd.ExecuteNonQuery();
                }
                catch (SqlException)
                {
                    // se ja existe um proprietario registado com o mesmo email ou 
                    // ha algum problema no acesso à BD
                    adicionou = false;
                }
                finally
                {
                    cmd.Connection.Close();
                }
                return adicionou;
            }
        }

        internal bool RemoverAlimento(int idAlimento)
        {
            return estabelecimentos.RemoverAlimento(idAlimento);
        }


        // POR FAZER!!!
        internal bool EditarDados(Proprietario proprietario)
        {
            throw new NotImplementedException();
        }

        internal Proprietario ObterPorId(int proprietarioAutenticado)
        {
            using (SqlConnection sqlCon = new SqlConnection(DAO.CONECTION_STRING))
            {
                SqlCommand cmd = new SqlCommand("SELECT * FROM Proprietario WHERE id = @id", sqlCon);
                cmd.Parameters.Add("@id", SqlDbType.NVarChar, 50);
                cmd.Parameters["@id"].Value = proprietarioAutenticado;

                cmd.Connection.Open();
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

                Proprietario p = null;
                if (contains)
                {
                    p = new Proprietario(proprietarioAutenticado, Convert.ToChar(reader["genero"]), reader["email"].ToString(), reader["nome"].ToString(), reader["palavra_passe"].ToString(), reader["contacto_tel"].ToString());
                }

                return p;
            }
        }

        internal IList<Estabelecimento> ConsultarEstabelecimentos(int proprietarioAutenticado)
        {
            return estabelecimentos.ConsultarEstabelecimentos(proprietarioAutenticado);
        }

        internal List<Alimento> ConsultarAlimentos(int idEstabelecimento)
        {
            return ConsultarAlimentos(idEstabelecimento);
        }

        internal bool RegistarAlimento(int idEstabelecimento, Alimento alim)
        {
            return estabelecimentos.RegistarAlimento(idEstabelecimento, alim);
        }

        internal bool EditarFotoAlimento(int idAlimento, byte[] foto)
        {
            return estabelecimentos.EditarFotoAlimento(idAlimento, foto);
        }

        internal bool AdicionarIngredientesAlimento(int idAlimento, List<string> designacaoIngredientes)
        {
            return estabelecimentos.AdicionarIngredientesAlimento(idAlimento, designacaoIngredientes);
        }

        internal bool RemoverIngredientesAlimento(int idAlimento, List<string> designacaoIngredientes)
        {
            return estabelecimentos.RemoverIngredientesAlimento(idAlimento, designacaoIngredientes);
        }
    }
}
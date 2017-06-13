using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace Mnham_Mnham
{
    public class ProprietarioDAO
    {
        private readonly EstabelecimentoDAO estabelecimentos;

        public ProprietarioDAO()
        {
            estabelecimentos = new EstabelecimentoDAO();
        }

        public Proprietario ObterPorEmail(string email)
        {
            Proprietario p = null;

            using (var sqlCon = new SqlConnection(DAO.CONECTION_STRING))
            {
                string txtCmd = "SELECT * FROM Proprietario WHERE email = @email";

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
                            string contactoTel = reader["contacto_tel"]?.ToString();
                            p = new Proprietario(id, genero, email, nome, palavraPasse, contactoTel);
                        }
                    }
                }
            }
            return p;
        }

        public bool AdicionarProprietario(Proprietario proprietario)
        {
            using (var sqlCon = new SqlConnection(DAO.CONECTION_STRING))
            {
                // ignora o id do proprietario e cria um novo
                bool adicionou = true;
                string txtCmd = @"INSERT INTO Proprietario(email,palavra_passe,nome,contacto_tel,genero)
                                  VALUES (@email, @palavra_passe, @nome, @contacto_tel, @genero);";

                sqlCon.Open();
                using (var cmd = new SqlCommand(txtCmd, sqlCon))
                {
                    cmd.Parameters.Add("@genero", SqlDbType.Char, 1);
                    cmd.Parameters.Add("@email", SqlDbType.NVarChar, 50);
                    cmd.Parameters.Add("@nome", SqlDbType.NVarChar, 75);
                    cmd.Parameters.Add("@palavra_passe", SqlDbType.Char, 30);
                    cmd.Parameters.Add("@contacto_tel", SqlDbType.Char, 19);

                    cmd.Parameters["@genero"].Value = proprietario.Genero;
                    cmd.Parameters["@email"].Value = proprietario.Email;
                    cmd.Parameters["@nome"].Value = proprietario.Nome;
                    cmd.Parameters["@palavra_passe"].Value = proprietario.PalavraPasse;
                    cmd.Parameters["@contacto_tel"].Value = proprietario.ContactoTel;

                    try
                    {
                        cmd.ExecuteNonQuery();
                    }
                    catch (SqlException)
                    {
                        // já existe um Proprietario registado com o mesmo email ou houve algum problema no acesso à BD
                        adicionou = false;
                    }
                }
                return adicionou;
            }
        }

        public bool RemoverAlimento(int idAlimento)
        {
            return estabelecimentos.RemoverAlimento(idAlimento);
        }


        // POR FAZER!!!
        public bool EditarDados(Proprietario proprietario)
        {
            throw new NotImplementedException();
        }

        public Proprietario ObterPorId(int proprietarioAutenticado)
        {
            Proprietario p = null;

            using (var sqlCon = new SqlConnection(DAO.CONECTION_STRING))
            {
                string txtCmd = "SELECT * FROM Proprietario WHERE id = @id";

                sqlCon.Open();
                using (var cmd = new SqlCommand(txtCmd, sqlCon))
                {
                    cmd.Parameters.Add("@id", SqlDbType.NVarChar, 50);
                    cmd.Parameters["@id"].Value = proprietarioAutenticado;

                    using (var reader = cmd.ExecuteReader())
                    {
                        bool encontrado = reader.Read();

                        if (encontrado)
                        {
                            char genero = Convert.ToChar(reader["genero"]);
                            string email = reader["email"].ToString();
                            string nome = reader["nome"].ToString();
                            string palavraPasse = reader["palavra_passe"].ToString();
                            string contactoTel = reader["contacto_tel"]?.ToString();

                            p = new Proprietario(proprietarioAutenticado, genero, email, nome, palavraPasse, contactoTel);
                        }
                    }
                }
            }
            return p;
        }

        public IList<Estabelecimento> ConsultarEstabelecimentos(int proprietarioAutenticado)
        {
            return estabelecimentos.ConsultarEstabelecimentos(proprietarioAutenticado);
        }

        public IList<Alimento> ConsultarAlimentos(int idEstabelecimento)
        {
            return estabelecimentos.ConsultarAlimentos(idEstabelecimento);
        }

        public bool RegistarAlimento(int idEstabelecimento, Alimento alim)
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
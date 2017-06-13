using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;

namespace Mnham_Mnham
{
    public class ClassificacaoAlimentoDAO
    {
        public bool ClassificarAlimento(int idAlimento, Classificacao cla)
        {
            bool inseriu = true;

            using (var sqlCon = new SqlConnection(DAO.CONECTION_STRING))
            {
                string txtCmd = @"INSERT INTO ClassificacaoAlimento(id_cliente, id_alimento, valor, comentario, data)
                                  VALUES (@id_c, @id_a, @valor, @comentario, @data)";

                sqlCon.Open();
                using (var cmd = new SqlCommand(txtCmd, sqlCon))
                {
                    cmd.Parameters.Add("@id_c", SqlDbType.Int);
                    cmd.Parameters.Add("@id_a", SqlDbType.Int);
                    cmd.Parameters.Add("@valor", SqlDbType.Int);
                    cmd.Parameters.Add("@comentario", SqlDbType.NVarChar, 150);
                    cmd.Parameters.Add("@data", SqlDbType.DateTime);

                    cmd.Parameters["@id_c"].Value = cla.IdAutor;
                    cmd.Parameters["@id_a"].Value = idAlimento;
                    cmd.Parameters["@valor"].Value = cla.Avaliacao;
                    cmd.Parameters["@comentario"].Value = (object)cla.Comentario ?? DBNull.Value;
                    cmd.Parameters["@data"].Value = cla.Data;

                    try
                    {
                        cmd.ExecuteNonQuery();
                    }
                    catch (SqlException)
                    {
                        inseriu = false;
                    }
                }
            }
            return inseriu;
        }

        public void RemoverClassificacaoAlimento(int idAlimento, int clienteAutenticado)
        {
            using (var sqlCon = new SqlConnection(DAO.CONECTION_STRING))
            {
                string txtCmd = "DELETE FROM ClassificacaoAlimento WHERE id_cliente = @id_c AND id_alimento = @id_a";

                sqlCon.Open();
                using (var cmd = new SqlCommand(txtCmd, sqlCon))
                {
                    cmd.Parameters.Add("@id_c", SqlDbType.Int);
                    cmd.Parameters.Add("@id_a", SqlDbType.Int);

                    cmd.Parameters["@id_c"].Value = clienteAutenticado;
                    cmd.Parameters["@id_a"].Value = idAlimento;

                    cmd.ExecuteNonQuery();
                }
            }
        }

        public IList<Classificacao> ConsultarClassificacoesAlimentos(int clienteAutenticado)
        {
            IList<Classificacao> l = new List<Classificacao>();

            using (var sqlCon = new SqlConnection(DAO.CONECTION_STRING))
            {
                string txtCmd = "SELECT * FROM ClassificacaoAlimento WHERE id_cliente = @id_c";

                sqlCon.Open();
                using (var cmd = new SqlCommand(txtCmd, sqlCon))
                {
                    cmd.Parameters.Add("@id_c", SqlDbType.Int);
                    cmd.Parameters["@id_c"].Value = clienteAutenticado;

                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            int avaliacao = Convert.ToInt32(reader["valor"]);
                            string comentario = reader["comentario"]?.ToString();
                            DateTime data = Convert.ToDateTime(reader["data"]);

                            l.Add(new Classificacao(avaliacao, comentario, clienteAutenticado, data));
                        }
                    }
                }
            }
            return l;
        }

        public IList<Classificacao> ClassificacaoAlimento(int idAlimento)
        {
            using (var sqlCon = new SqlConnection(DAO.CONECTION_STRING))
            {
                sqlCon.Open();
                return ClassificacaoAlimento(idAlimento, sqlCon);
            }
        }

        // Recebe uma conexão aberta e não a fecha.
        public IList<Classificacao> ClassificacaoAlimento(int idAlimento, SqlConnection sqlCon)
        {
            IList<Classificacao> l = new List<Classificacao>();
            string txtCmd = "SELECT * FROM ClassificacaoAlimento WHERE id_alimento = @id_a";

            using (var cmd = new SqlCommand(txtCmd, sqlCon))
            {
                cmd.Parameters.Add("@id_a", SqlDbType.Int);
                cmd.Parameters["@id_a"].Value = idAlimento;

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        int avaliacao = Convert.ToInt32(reader["valor"]);
                        string comentario = reader["comentario"]?.ToString();
                        int idCliente = Convert.ToInt32(reader["id_cliente"]);
                        DateTime data = Convert.ToDateTime(reader["data"]);

                        l.Add(new Classificacao(avaliacao, comentario, idCliente, data));
                    }
                }
            }
            return l;
        }

        public float ObterClassificacaoMedia(int idAlimento)
        {
            using (SqlConnection sqlCon = new SqlConnection(DAO.CONECTION_STRING))
            {
                sqlCon.Open();
                return ObterClassificacaoMedia(idAlimento, sqlCon);
            }
        }

        // Recebe uma conexão aberta e não a fecha.
        public float ObterClassificacaoMedia(int idAlimento, SqlConnection sqlCon)
        {
            float res;
            string txtCmd = "SELECT AVG(valor) AS a FROM ClassificacaoAlimento WHERE id_alimento = @id_a";

            using (var cmd = new SqlCommand(txtCmd, sqlCon))
            {
                cmd.Parameters.Add("@id_a", SqlDbType.Int);
                cmd.Parameters["@id_a"].Value = idAlimento;

                using (var reader = cmd.ExecuteReader())
                {
                    reader.Read();
                    res = (float)((reader["a"] == null) ? 0.0 : Convert.ToDouble(reader["a"]));
                }
            }
            return res;
        }

    }
}

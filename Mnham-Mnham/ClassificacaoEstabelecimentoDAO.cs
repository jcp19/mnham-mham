using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace Mnham_Mnham
{
    public class ClassificacaoEstabelecimentoDAO
    {
        public bool ClassificarEstabelecimento(int idEstabelecimento, Classificacao cla)
        {
            bool inseriu = true;

            using (var sqlCon = new SqlConnection(DAO.CONECTION_STRING))
            {
                string txtCmd =
                    @"INSERT INTO ClassificacaoEstabelecimento(id_cliente, id_estabelecimento, valor, comentario, data)
                      VALUES (@id_c, @id_e, @valor, @comentario, @data)";

                sqlCon.Open();
                using (var cmd = new SqlCommand(txtCmd, sqlCon))
                {
                    cmd.Parameters.Add("@id_c", SqlDbType.Int);
                    cmd.Parameters.Add("@id_e", SqlDbType.Int);
                    cmd.Parameters.Add("@valor", SqlDbType.Int);
                    cmd.Parameters.Add("@comentario", SqlDbType.NVarChar, 150);
                    cmd.Parameters.Add("@data", SqlDbType.DateTime);

                    cmd.Parameters["@id_c"].Value = cla.IdAutor;
                    cmd.Parameters["@id_e"].Value = idEstabelecimento;
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

        internal IList<Classificacao> ClassificacoesEstabelecimento(int idEstabelecimento, SqlConnection sqlCon)
        {
            IList<Classificacao> l = new List<Classificacao>();
            string txtCmd = "SELECT * FROM ClassificacaoEstabelecimento WHERE id_estabelecimento = @id_e";

            using (var cmd = new SqlCommand(txtCmd, sqlCon))
            {
                cmd.Parameters.Add("@id_e", SqlDbType.Int);
                cmd.Parameters["@id_e"].Value = idEstabelecimento;

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

        internal float ClassificacaoMedia(int idEstabelecimento, SqlConnection sqlCon)
        {
            float res;
            string txtCmd = "SELECT AVG(valor) AS a FROM ClassificacaoEstabelecimento WHERE id_estabelecimento = @id_e";

            using (var cmd = new SqlCommand(txtCmd, sqlCon))
            {
                cmd.Parameters.Add("@id_e", SqlDbType.Int);
                cmd.Parameters["@id_e"].Value = idEstabelecimento;

                using (var reader = cmd.ExecuteReader())
                {
                    reader.Read();
                    res = (float)(reader["a"] == null ? 0.0 : Convert.ToDouble(reader["a"]));
                }
            }
            return res;
        }

        public void RemoverClassificacaoEstabelecimento(int idEstabelecimento, int clienteAutenticado)
        {
            using (var sqlCon = new SqlConnection(DAO.CONECTION_STRING))
            {
                string txtCmd = @"DELETE FROM ClassificacaoEstabelecimento
                                  WHERE id_cliente = @id_c AND id_estabelecimento = @id_e";

                sqlCon.Open();
                using (var cmd = new SqlCommand(txtCmd, sqlCon))
                {
                    cmd.Parameters.Add("@id_c", SqlDbType.Int);
                    cmd.Parameters.Add("@id_e", SqlDbType.Int);

                    cmd.Parameters["@id_c"].Value = clienteAutenticado;
                    cmd.Parameters["@id_e"].Value = idEstabelecimento;

                    cmd.ExecuteNonQuery();
                }
            }
        }

        public IList<Classificacao> ConsultarClassificacoesEstabelecimentos(int clienteAutenticado)
        {
            IList<Classificacao> l = new List<Classificacao>();

            using (var sqlCon = new SqlConnection(DAO.CONECTION_STRING))
            {
                string txtCmd = "SELECT * FROM ClassificacaoEstabelecimento WHERE id_cliente = @id_c";

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
    }
}

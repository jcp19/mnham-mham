using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace Mnham_Mnham
{
    class ClassificacaoEstabelecimentoDAO
    {
        public bool ClassificarEstabelecimento(int idEstabelecimento, Classificacao cla)
        {
            bool inseriu = true;
            using (SqlConnection sqlCon = new SqlConnection(DAO.CONECTION_STRING))
            {
                SqlCommand cmd = new SqlCommand("INSERT INTO ClassificacaoEstabelecimento(id_cliente, id_estabelecimento, valor, comentario, data) VALUES (@id_c, @id_e, @valor, @comentario, @data)", sqlCon);

                cmd.Parameters.Add("@id_c", SqlDbType.Int);
                cmd.Parameters.Add("@id_e", SqlDbType.Int);
                cmd.Parameters.Add("@valor", SqlDbType.Int);
                cmd.Parameters.Add("@comentario", SqlDbType.NVarChar, 150);
                cmd.Parameters.Add("@data", SqlDbType.DateTime);

                cmd.Parameters["@id_c"].Value = cla.IdAutor;
                cmd.Parameters["@id_e"].Value = idEstabelecimento;
                cmd.Parameters["@valor"].Value = cla.Avaliacao;
                cmd.Parameters["@comentario"].Value = cla.Comentario;
                cmd.Parameters["@data"].Value = cla.Data;

                cmd.Connection.Open();
                try
                {
                    cmd.ExecuteNonQuery();
                }
                catch (SqlException)
                {
                    inseriu = false;
                }
            }
            return inseriu;
        }

        public void RemoverClassificacaoEstabelecimento(int idEstabelecimento, int clienteAutenticado)
        {
            using (SqlConnection sqlCon = new SqlConnection(DAO.CONECTION_STRING))
            {
                SqlCommand cmd = new SqlCommand("DELETE FROM ClassificacaoEstabelecimento WHERE id_cliente = @id_c AND id_estabelecimento = @id_e", sqlCon);
                cmd.Parameters.Add("@id_c", SqlDbType.Int);
                cmd.Parameters.Add("@id_e", SqlDbType.Int);

                cmd.Parameters["@id_c"].Value = clienteAutenticado;
                cmd.Parameters["@id_e"].Value = idEstabelecimento;

                sqlCon.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public IList<Classificacao> ConsultarClassificacoesEstabelecimentos(int clienteAutenticado)
        {
            IList<Classificacao> l = new List<Classificacao>();

            using (SqlConnection sqlCon = new SqlConnection(DAO.CONECTION_STRING))
            {
                SqlCommand cmd = new SqlCommand("SELECT * FROM ClassificacaoEstabelecimento WHERE id_cliente = @id_c", sqlCon);
                cmd.Parameters.Add("@id_c", SqlDbType.Int);
                cmd.Parameters["@id_c"].Value = clienteAutenticado;

                cmd.Connection.Open();

                var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    int avaliacao = Convert.ToInt32(reader["valor"]);
                    string comentario = reader["comentario"].ToString();
                    // int idAlimento = Convert.ToInt32(reader["id_alimento"]);
                    DateTime data = Convert.ToDateTime(reader["data"]);
                    l.Add(new Classificacao(avaliacao, comentario, clienteAutenticado, data));
                }
            }
            return l;
        }
    }
}
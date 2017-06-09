using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;

namespace Mnham_Mnham
{
    class ClassificacaoAlimentoDAO { 
        public bool ClassificarAlimento(int idAlimento, Classificacao cla)
        {
            bool inseriu = true;
            using (SqlConnection sqlCon = new SqlConnection(DAO.CONECTION_STRING))
            {
                SqlCommand cmd = new SqlCommand("INSERT INTO ClassificacaoAlimento(id_cliente, id_alimento, valor, comentario, data) VALUES (@id_c, @id_a, @valor, @comentario, @data)", sqlCon);

                cmd.Parameters.Add("@id_c", SqlDbType.Int);
                cmd.Parameters.Add("@id_a", SqlDbType.Int);
                cmd.Parameters.Add("@valor", SqlDbType.Int);
                cmd.Parameters.Add("@comentario", SqlDbType.NVarChar, 150);
                cmd.Parameters.Add("@data", SqlDbType.DateTime);

                cmd.Parameters["@id_c"].Value = cla.IdAutor;
                cmd.Parameters["@id_a"].Value = idAlimento;
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

        public void RemoverClassificacaoAlimento(int idAlimento, int clienteAutenticado)
        {
            using (SqlConnection sqlCon = new SqlConnection(DAO.CONECTION_STRING))
            {
                SqlCommand cmd = new SqlCommand("DELETE FROM ClassificacaoAlimento WHERE id_cliente = @id_c AND id_alimento = @id_a", sqlCon);
                cmd.Parameters.Add("@id_c", SqlDbType.Int);
                cmd.Parameters.Add("@id_a", SqlDbType.Int);

                cmd.Parameters["@id_c"].Value = clienteAutenticado;
                cmd.Parameters["@id_a"].Value = idAlimento;

                sqlCon.Open();
                cmd.ExecuteNonQuery();      
            }
        }

        public List<Classificacao> ConsultarClassificacoesAlimentos(int clienteAutenticado)
        {
            List<Classificacao> l = new List<Classificacao>();
            using (SqlConnection sqlCon = new SqlConnection(DAO.CONECTION_STRING))
            {
                SqlCommand cmd = new SqlCommand("SELECT * FROM ClassificacaoAlimento WHERE id_cliente = @id_c", sqlCon);
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
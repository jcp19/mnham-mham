using System;
using System.Collections.Generic;
using System.Data;
using System.SqlClient;

namespace Mnham_Mnham
{
    class NaoPreferenciaDAO
    {
        public void AdicionarNaoPreferencia(int clienteAutenticado, Preferencia naoPreferencia)
        {
            using (SqlConnection sqlCon = new SqlConnection(DAO.CONECTION_STRING))
            {
                SqlCommand cmd = new SqlCommand("INSERT INTO NaoPreferencia(id_cliente, designacao_ingrediente, desingacao_alimento) VALUES (@id_c, @d_ing, @d_al)", sqlCon);

                cmd.Parameters.Add("@id_c", SqlDbType.Int);
                cmd.Parameters.Add("@d_ing", SqlDbType.NVarChar, 75);
                cmd.Parameters.Add("@d_al", SqlDbType.NVarChar, 75);

                cmd.Parameters["@id_c"].Value = clienteAutenticado;
                cmd.Parameters["@d_ing"].Value = naoPreferencia.DesignacaoIngrediente;
                cmd.Parameters["@d_al"].Value = naoPreferencia.DesignacaoAlimento;

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
        }

        public void RemoverNaoPreferencia(int clienteAutenticado, Preferencia naoPreferencia)
        {
            using (SqlConnection sqlCon = new SqlConnection(DAO.CONECTION_STRING))
            {
                SqlCommand cmd = new SqlCommand("DELETE FROM NaoPreferencia WHERE id_cliente = @id_c AND designacao_ingrediente = @d_ing AND desingacao_alimento = @d_al", sqlCon);
                cmd.Parameters.Add("@id_c", SqlDbType.Int);
                cmd.Parameters.Add("@d_ing", SqlDbType.NVarChar, 75);
                cmd.Parameters.Add("@d_al", SqlDbType.NVarChar, 75);

                cmd.Parameters["@id_c"].Value = clienteAutenticado;
                cmd.Parameters["@d_ing"].Value = naoPreferencia.DesignacaoIngrediente;
                cmd.Parameters["@d_al"].Value = naoPreferencia.DesignacaoAlimento;

                sqlCon.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public List<Preferencia> ConsultarNaoPreferencias(int clienteAutenticado)
        {
            List<Preferencia> l = new List<Classificacao>();
            using (SqlConnection sqlCon = new SqlConnection(DAO.CONECTION_STRING))
            {
                SqlCommand cmd = new SqlCommand("SELECT * FROM NaoPreferencia WHERE id_cliente = @id_c", sqlCon);
                cmd.Parameters.Add("@id_c", SqlDbType.Int);
                cmd.Parameters["@id_c"].Value = clienteAutenticado;

                cmd.Connection.Open();

                var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    l.Add(new Preferencia( reader["designacao_ingrediente"].ToString(), reader["designacao_alimento"].ToString()));
                }
                reader.Close();

            }
            return l;
        }
    }
}
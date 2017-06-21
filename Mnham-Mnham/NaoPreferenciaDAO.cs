using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace Mnham_Mnham
{
    public class NaoPreferenciaDAO
    {
        public bool AdicionarNaoPreferencia(int clienteAutenticado, Preferencia naoPreferencia)
        {
            using (SqlConnection sqlCon = new SqlConnection(DAO.CONECTION_STRING))
            {
                bool inseriu;
                SqlCommand cmd = new SqlCommand("INSERT INTO NaoPreferencia(id_cliente, designacao_ingrediente, designacao_alimento) VALUES (@id_c, @d_ing, @d_al)", sqlCon);

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
                    inseriu = true;
                }
                catch (SqlException)
                {
                    inseriu = false;
                }
                return inseriu;
            }
        }

        public bool RemoverNaoPreferencia(int clienteAutenticado, Preferencia naoPreferencia)
        {
            bool removeu;

            using (SqlConnection sqlCon = new SqlConnection(DAO.CONECTION_STRING))
            {
                SqlCommand cmd = new SqlCommand("DELETE FROM NaoPreferencia WHERE id_cliente = @id_c AND designacao_ingrediente = @d_ing AND designacao_alimento = @d_al", sqlCon);
                cmd.Parameters.Add("@id_c", SqlDbType.Int);
                cmd.Parameters.Add("@d_ing", SqlDbType.NVarChar, 75);
                cmd.Parameters.Add("@d_al", SqlDbType.NVarChar, 75);

                cmd.Parameters["@id_c"].Value = clienteAutenticado;
                cmd.Parameters["@d_ing"].Value = naoPreferencia.DesignacaoIngrediente;
                cmd.Parameters["@d_al"].Value = naoPreferencia.DesignacaoAlimento;

                sqlCon.Open();
                try
                {
                    cmd.ExecuteNonQuery();
                    removeu = true;
                }
                catch (SqlException)
                {
                    removeu = false;
                }
                return removeu;
            }
        }

        public IList<Preferencia> ConsultarNaoPreferencias(int clienteAutenticado)
        {
            IList<Preferencia> l = new List<Preferencia>();

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
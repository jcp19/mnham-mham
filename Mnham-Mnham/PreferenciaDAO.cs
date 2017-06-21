using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace Mnham_Mnham
{
    public class PreferenciaDAO
    {
        public bool AdicionarPreferencia(int clienteAutenticado, Preferencia preferencia)
        {
            bool inseriu;

            using (var sqlCon = new SqlConnection(DAO.CONECTION_STRING))
            {
                string txtCmd = @"INSERT INTO Preferencia(id_cliente, designacao_ingrediente, designcacao_alimento)
                                  VALUES (@id_c, @d_ing, @d_al)";

                sqlCon.Open();
                using (var cmd = new SqlCommand(txtCmd, sqlCon))
                {
                    cmd.Parameters.Add("@id_c", SqlDbType.Int);
                    cmd.Parameters.Add("@d_ing", SqlDbType.NVarChar, 75);
                    cmd.Parameters.Add("@d_al", SqlDbType.NVarChar, 75);

                    cmd.Parameters["@id_c"].Value = clienteAutenticado;
                    cmd.Parameters["@d_ing"].Value = preferencia.DesignacaoIngrediente;
                    cmd.Parameters["@d_al"].Value = preferencia.DesignacaoAlimento;

                    try
                    {
                        cmd.ExecuteNonQuery();
                        inseriu = true;
                    }
                    catch (SqlException)
                    {
                        inseriu = false;
                    }
                }
            }
            return inseriu;
        }

        public bool AdicionarNaoPreferencia(int clienteAutenticado, Preferencia naoPreferencia)
        {
            bool inseriu;

            using (var sqlCon = new SqlConnection(DAO.CONECTION_STRING))
            {
                string txtCmd = @"INSERT INTO NaoPreferencia(id_cliente, designacao_ingrediente, designcacao_alimento)
                                  VALUES (@id_c, @d_ing, @d_al)";

                sqlCon.Open();
                using (var cmd = new SqlCommand(txtCmd, sqlCon))
                {
                    cmd.Parameters.Add("@id_c", SqlDbType.Int);
                    cmd.Parameters.Add("@d_ing", SqlDbType.NVarChar, 75);
                    cmd.Parameters.Add("@d_al", SqlDbType.NVarChar, 75);

                    cmd.Parameters["@id_c"].Value = clienteAutenticado;
                    cmd.Parameters["@d_ing"].Value = naoPreferencia.DesignacaoIngrediente;
                    cmd.Parameters["@d_al"].Value = naoPreferencia.DesignacaoAlimento;

                    try
                    {
                        cmd.ExecuteNonQuery();
                        inseriu = true;
                    }
                    catch (SqlException)
                    {
                        inseriu = false;
                    }
                }
            }
            return inseriu;
        }

        public bool RemoverPreferencia(int clienteAutenticado, Preferencia preferencia)
        {
            bool removeu;

            using (var sqlCon = new SqlConnection(DAO.CONECTION_STRING))
            {
                string txtCmd = @"DELETE FROM Preferencia
                                  WHERE id_cliente = @id_c AND designacao_ingrediente = @d_ing AND designcacao_alimento = @d_al";

                sqlCon.Open();
                using (var cmd = new SqlCommand(txtCmd, sqlCon))
                {
                    cmd.Parameters.Add("@id_c", SqlDbType.Int);
                    cmd.Parameters.Add("@d_ing", SqlDbType.NVarChar, 75);
                    cmd.Parameters.Add("@d_al", SqlDbType.NVarChar, 75);

                    cmd.Parameters["@id_c"].Value = clienteAutenticado;
                    cmd.Parameters["@d_ing"].Value = preferencia.DesignacaoIngrediente;
                    cmd.Parameters["@d_al"].Value = preferencia.DesignacaoAlimento;

                    try
                    {
                        cmd.ExecuteNonQuery();
                        removeu = true;
                    }
                    catch (SqlException)
                    {
                        removeu = false;
                    }
                }
            }
            return removeu;
        }

        public IList<Preferencia> ConsultarPreferencias(int clienteAutenticado)
        {
            IList<Preferencia> l = new List<Preferencia>();

            using (var sqlCon = new SqlConnection(DAO.CONECTION_STRING))
            {
                string txtCmd = "SELECT * FROM Preferencia WHERE id_cliente = @id_c";

                sqlCon.Open();
                using (var cmd = new SqlCommand(txtCmd, sqlCon))
                {
                    cmd.Parameters.Add("@id_c", SqlDbType.Int);
                    cmd.Parameters["@id_c"].Value = clienteAutenticado;

                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            l.Add(new Preferencia(reader["designacao_ingrediente"].ToString(), reader["designcacao_alimento"].ToString()));
                        }
                    }
                }
            }
            return l;
        }
    }
}

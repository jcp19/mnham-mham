using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace Mnham_Mnham
{
    public class PedidoDAO
    {
        public void AdicionarPedido(Pedido pedido)
        {
            using (var sqlCon = new SqlConnection(DAO.CONECTION_STRING))
            {
                string txtCmd = "INSERT INTO Pedido(id_cliente, termo, data) VALUES (@id_c, @termo, @data)";

                sqlCon.Open();
                using (var cmd = new SqlCommand(txtCmd, sqlCon))
                {
                    cmd.Parameters.Add("@id_c", SqlDbType.Int);
                    cmd.Parameters.Add("@termo", SqlDbType.NVarChar, 150);
                    cmd.Parameters.Add("@data", SqlDbType.DateTime);

                    cmd.Parameters["@id_c"].Value = pedido.IdCliente;
                    cmd.Parameters["@termo"].Value = pedido.Termo;
                    cmd.Parameters["@data"].Value = pedido.Data;

                    cmd.ExecuteNonQuery();
                }
            }
        }

        public IList<Pedido> ObterPedidos(int clienteAutenticado)
        {
            IList<Pedido> lPedidos = new List<Pedido>();

            using (var sqlCon = new SqlConnection(DAO.CONECTION_STRING))
            {
                string txtCmd = "SELECT * FROM Pedido WHERE id_cliente = @id_c";

                sqlCon.Open();
                using (var cmd = new SqlCommand(txtCmd, sqlCon))
                {
                    cmd.Parameters.Add("@id_c", SqlDbType.Int);
                    cmd.Parameters["@id_c"].Value = clienteAutenticado;

                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            DateTime data = Convert.ToDateTime(reader["data"]);
                            string termo = reader["termo"].ToString();
                            lPedidos.Add(new Pedido(data, termo, clienteAutenticado));
                        }
                    }
                }
            }
            return lPedidos;
        }

        public IList<string> ObterPedidosUltimaSemana()
        {
            IList<string> pedidosUltimaSemana = new List<string>();

            using (var sqlCon = new SqlConnection(DAO.CONECTION_STRING))
            {
                DateTime umaSemanaAtras = DateTime.Today.AddDays(-7);
                string dataFormSql = umaSemanaAtras.ToString("yyyy-mm-dd hh-mm-ss");
                string txtCmd = "SELECT termo FROM Pedido WHERE Pedido.data >= '" + dataFormSql + "\'";

                using (var cmd = new SqlCommand(txtCmd, sqlCon))
                {
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            pedidosUltimaSemana.Add(reader["termo"].ToString());
                        }
                    }
                }
            }
            return pedidosUltimaSemana;
        }
    }
}

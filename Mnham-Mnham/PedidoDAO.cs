using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace Mnham_Mnham
{
    class PedidoDAO
    {
        public void AdicionarPedido(Pedido pedido)
        {
            using (SqlConnection sqlCon = new SqlConnection(DAO.CONECTION_STRING))
            {
                SqlCommand cmd = new SqlCommand("INSERT INTO Pedido(id_cliente, termo, data) VALUES (@id_c, @termo, @data)", sqlCon);

                cmd.Parameters.Add("@id_c", SqlDbType.Int);
                cmd.Parameters.Add("@termo", SqlDbType.NVarChar, 150);
                cmd.Parameters.Add("@data", SqlDbType.DateTime);

                cmd.Parameters["@id_c"].Value = pedido.IdCliente;
                cmd.Parameters["@termo"].Value = pedido.Termo;
                cmd.Parameters["@data"].Value = pedido.Data;

                cmd.Connection.Open();
                cmd.ExecuteNonQuery();
                
            }
        }

        internal IList<Pedido> ObterPedidos(int clienteAutenticado)
        {
            IList<Pedido> l = new List<Pedido>();

            using (SqlConnection sqlCon = new SqlConnection(DAO.CONECTION_STRING))
            {
                SqlCommand cmd = new SqlCommand("SELECT * FROM Pedido WHERE id_cliente = @id_c", sqlCon);
                cmd.Parameters.Add("@id_c", SqlDbType.Int);
                cmd.Parameters["@id_c"].Value = clienteAutenticado;

                cmd.Connection.Open();

                var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    l.Add(new Pedido(Convert.ToDateTime(reader["data"]), reader["termo"].ToString(), clienteAutenticado));
                }
                reader.Close();
            }
            return l;
        }

        internal IEnumerable<string> ObterPedidosUltimaSemana()
        {
            throw new NotImplementedException();
        }
    }
}
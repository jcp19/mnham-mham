using System;
using System.Collections.Generic;

namespace Mnham_Mnham
{
    class PedidoDAO : DAO
    {
        public PedidoDAO() : base()
        {

        }

        public PedidoDAO(string connectionString) : base(connectionString) { }

        internal bool AdicionarPedido(Pedido pedido)
        {
            throw new NotImplementedException();
        }

        internal List<Pedido> ObterPedidos(int clienteAutenticado)
        {
            throw new NotImplementedException();
        }

        internal IEnumerable<string> ObterPedidosUltimaSemana()
        {
            throw new NotImplementedException();
        }
    }
}
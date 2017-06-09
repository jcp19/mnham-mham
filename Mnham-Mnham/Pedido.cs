using System;

namespace Mnham_Mnham
{
    public class Pedido
    {
        private DateTime data;
        private string termo;
        private int idCliente;

        public DateTime Data { get { return data; } }
        public string Termo { get { return termo; } }
        public int IdCliente { get { return idCliente; } }

        private Pedido() { }

        public Pedido(string termo, int idCliente) : this(DateTime.Now, termo, idCliente) { }

        public Pedido(DateTime data, string termo, int idCliente)
        {
            this.data = data;
            this.termo = termo;
            this.idCliente = idCliente;
        }
    }
}
using System;
public class Pedido {
	private DateTime data;
	private string termo;
	private int idCliente;

    public Pedido(string termo, int idCliente)
    {
        this.termo = termo;
        this.idCliente = idCliente;
        this.data = DateTime.Now;
    }

    public Pedido(DateTime data, string termo, int idCliente)
    {
        this.termo = termo;
        this.idCliente = idCliente;
        this.data = data;
    }
}

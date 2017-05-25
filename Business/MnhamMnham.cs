using System;
public class MnhamMnham {
	private int idUtilizadorAutenticado;
	private bool utilizadorEProprietario;

	public AlimentoEstabelecimento[] EfetuarPedido(ref string termo) {
        RegistaPedidoHistorico(termo);
        PedidoProcessado pedidoProcessado = new PedidoProcessado(termo);
        Cliente cliente;

        List<string> preferencias;
        List<string> naoPreferencias;
        if (clientes.TryGetValue(idUtilizadorAutenticado, cliente))
        {
            // cliente
            preferencias = cliente.ObterPreferencias(pedidoProcessado.ObterNomeAlimento());
            List<string> preferenciasPedido = pedidoProcessado.ObterPreferencias();
            preferencias.AddRange(preferenciasPedido);

            naoPreferencias = cliente.ObterNaoPreferencias(pedidoProcessado.ObterNomeAlimento());
            List<string> naoPreferenciasPedido = pedidoProcessado.ObterNaoPreferencias();
            naoPreferencias.AddRange(naoPreferenciasPedido);
        }
        else
        {
            // utilizador não autenticado
            preferencias = pedidoProcessado.ObterPreferencias();
            naoPreferencias = pedidoProcessado.ObterNaoPreferencias();
        }

        // Obter localização !!

        List<AlimentoEstabelecimento> listaAEs = new List<AlimentoEstabelecimento>();
        foreach(Estabelecimento e in estabelecimentos.Values())
        {
            List<Alimento> alimentos = e.ObtemAlimentos(pedidoProcessado.ObterNomeAlimento());
            foreach(Alimento a in alimentos)
            {
                if(a.ContemNaoPreferencias(naoPreferencias) == false)
                {
                    int nPreferencias = a.QuantasPreferenciasContem(preferencias);
                    AlimentoEstabelecimento ae = new AlimentoEstabelecimento(e, a, nPreferencias);
                    listaAEs.Add(ae);
                }
            }
        }

        listaAEs.sort();

        return listaAEs;
	}
	private void RegistaPedidoHistorico(ref string termo) {
        Pedido pedido = new Pedido(termo, idUtilizadorAutenticado);
        List<Pedido> pedidosCliente;
        if(pedidos.TryGetValue(idUtilizadorAutenticado, pedidosCliente))
        {
            // cliente
            pedidosCliente.Add(pedido);
            pedidos.Remove(idUtilizadorAutenticado);
            pedidos.Add(idUtilizadorAutenticado, pedidosCliente);
        }
        else
        {
            // utilizador não autenticado
            // Como guardar??
        }
	}
	public void RegistarPreferenciaGeral(ref String designacaoPreferencia) {
		throw new System.Exception("Not implemented");
	}
	public void RegistarPreferenciaAlimento(ref string designacaoPreferencia, ref string designacaoAlimento) {
		throw new System.Exception("Not implemented");
	}
	public void RegistarNaoPreferenciaGeral(ref String designacaoPreferencia) {
		throw new System.Exception("Not implemented");
	}
	public void RegistarNaoPreferenciaAlimento(ref string designacaoPreferencia, ref string designacaoAlimento) {
		throw new System.Exception("Not implemented");
	}
	public AlimentoEstabelecimento ConsultarAlimento(ref int idAlimento, ref int idEstabelecimento) {
		throw new System.Exception("Not implemented");
	}
	public Estabelecimento ConsultarEstabelecimento(ref int idEstabelecimento) {
		throw new System.Exception("Not implemented");
	}
	public string[] ConsultarHistorico() {
		throw new System.Exception("Not implemented");
	}
	public void ClassificarAlimento(ref int idAlimento, ref int idEstabelecimento, ref int classificacao) {
		throw new System.Exception("Not implemented");
	}
	public void ClassificarAlimento(ref int idAlimento, ref int idEstabelecimento, ref int classificacao, ref string comentario) {
		throw new System.Exception("Not implemented");
	}
	public int RegistarAlimento(ref int idEstabelecimento, ref string nomeAlimento, ref float preco) {
		throw new System.Exception("Not implemented");
	}
	public void AssociaFotoAlimento(ref int idEstabelecimento, ref int idAlimento, ref Image photo) {
		throw new System.Exception("Not implemented");
	}
	public void AssociaIngredienteAlimento(ref int idEstabelecimento, ref int idAlimento, ref string designacaoIngrediente) {
		throw new System.Exception("Not implemented");
	}
	public void RemoverClassificacaoEstabelecimento(ref int idEstabelecimento) {
		throw new System.Exception("Not implemented");
	}
	public void RemoveAlimento(ref int idEstabelecimento, ref int idAlimento) {
		throw new System.Exception("Not implemented");
	}
	public void RemovePreferencia(ref string designacaoIngrediente, ref string designacaoAlimento) {
		throw new System.Exception("Not implemented");
	}
	public void RemoveNaoPreferencia(ref string designacaoIngrediente, ref string designacaoAlimento) {
		throw new System.Exception("Not implemented");
	}
	public string[] ObterTendencias() {
		throw new System.Exception("Not implemented");
	}
	public void ClassificarEstabelecimento(ref int idEstabelecimento, ref int classificacao) {
		throw new System.Exception("Not implemented");
	}
	public void ClassificarEstabelecimento(ref int idEstabelecimento, ref int classificacao, ref string comentario) {
		throw new System.Exception("Not implemented");
	}
	public void RemoverClassificacaoAlimento(ref int idAlimento, ref int idEstabelecimento) {
		throw new System.Exception("Not implemented");
	}

	private Dictionary<int, Proprietario> proprietarios;
	private Dictionary<int, Estabelecimento> estabelecimentos;
	private Dictionary<int, List<Pedido>> pedidos;
	private Dictionary<int, Cliente> clientes;

}

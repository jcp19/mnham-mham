using System;
public class Estabelecimento {
	private string nome;
	private string contacto_tel;
	private string coords;
	private string horario;
	private string tipo;
	private string descricao;
	private bool aceita_reservas;
	private bool tem_mb;
	private bool tem_takeaway;
	private bool tem_serv_mesa;
	private bool tem_esplanada;
	private bool tem_parque_estac;
	private bool tem_tv;
	private bool tem_wifi;
	private bool tem_zona_fum;
	private int id;
	private string morada;

	public List<Alimento> ObtemAlimentos(ref string nomeAlimento) {
        List<Alimento> alims = new List<Alimento>();
        foreach (Alimento a in alimentos)
        {
            if (a.ObterDesignacao().Compare(nomeAlimento) == 0)
            {
                alims.Add(a);
            }
        }
        return alims;
	}
	public Alimento ObtemAlimentoPorId(ref int id) {
		throw new System.Exception("Not implemented");
	}
	public void AssociaFotoAlimento(ref int idAlimento, ref Image photo) {
		throw new System.Exception("Not implemented");
	}
	public void AssociaIngredienteAlimento(ref int idAlimento, ref string designacaoIngrediente) {
		throw new System.Exception("Not implemented");
	}
	public void RemoverClassificacaoEstabelecimento(ref int idAutor) {
		throw new System.Exception("Not implemented");
	}
	public void RemoveAlimento(ref int idAlimento) {
		throw new System.Exception("Not implemented");
	}
	public void ClassificarAlimento(ref int idAlimento, ref int idCliente, ref int classificacao, ref string comentario) {
		throw new System.Exception("Not implemented");
	}
	public void ClassificareEstabelecimento(ref int idCliente, ref int classificacao, ref string comentario) {
		throw new System.Exception("Not implemented");
	}
	public void ClassificarAlimento(ref int idAlimento, ref int idCliente, ref int classificacao) {
		throw new System.Exception("Not implemented");
	}
	public void ClassificarEstabelecimento(ref int idCliente, ref int classificacao) {
		throw new System.Exception("Not implemented");
	}
	public void RemoverClassificacaoAlimento(ref int idAlimento, ref int idCliente) {
		throw new System.Exception("Not implemented");
	}

    public int compareTo(Estabelecimento e)
    {
        // Classificação do estabelecimento !!
        return 0;
    }

    private List<Alimento> alimentos;
	private Classificacao classificacao;
	private Image foto;


}

using System;
public class Alimento {
	private int id;
	private string designacao;
	private float preco;
	private List<string> ingredientes;

	public bool ContemNaoPreferencias(ref List<string> naoPreferencias) {
        foreach(string naoPref in naoPreferencias)
        {
            if (ingredientes.Contains(naoPref)) // Alterar!!
            {
                return true;
            }
        }
        return false;
	}
	public int QuantasPreferenciasContem(ref List<string> preferencias) {
        int n = 0;
        foreach(string pref in preferencias)
        {
            if (ingredientes.Contains(pref)) //Alterar!!
            {
                n++;
            }
        }
        return n;
	}
	public void ClassificarAlimento(ref int idCliente, ref int classificacao, ref string comentario) {
		throw new System.Exception("Not implemented");
	}
	public void ClassificarAlimento(ref int idCliente, ref int classificacao) {
		throw new System.Exception("Not implemented");
	}
	public void RemoverClassificacaoAlimento(ref int idCliente) {
		throw new System.Exception("Not implemented");
	}

    public string ObterDesignacao()
    {
        return this.designacao;
    }

    public int compareTo(Alimento a)
    {
        // Classificação do alimento !!
        return 0; 
    }

	private List<Classificacao> classificacoes;
	private Image foto;


}

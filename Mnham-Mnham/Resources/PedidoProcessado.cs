using System;
using System.Collections.Generic;

public class PedidoProcessado {
	private string nomeAlimento;
	private List<string> preferencias;
	private List<string> naoPreferencias;

	public PedidoProcessado(ref string pedido) {
        char[] delimitadores = {' '};
        string[] palavras = pedido.Split(delimitadores);

        preferencias = new List<string>();
        naoPreferencias = new List<string>();

        System.Text.StringBuilder palavra = new System.Text.StringBuilder();
        int aConstruir = 0; // 0 -> nomeAlimento; 1 -> preferencias; 2 -> nãoPreferencias
        for (int i = 0; i < palavras.Length; i++)
        {
            switch (palavras[i])
            {
                case "com":
                    GuardarPalavra(palavra.ToString(), aConstruir);
                    palavra = new System.Text.StringBuilder();
                    aConstruir = 1;
                    break;
                case "sem":
                    GuardarPalavra(palavra.ToString(), aConstruir);
                    palavra = new System.Text.StringBuilder();
                    aConstruir = 2;
                    break;
                default:
                    palavra.Append(palavra[i]);
                    palavra.Append(" ");
                    break;
            }
        }
	}

    private void GuardarPalavra(string palavra, int aConstruir)
    {
        switch (aConstruir)
        {
            case 0:
                this.nomeAlimento = palavra;
                break;
            case 1:
                preferencias.Add(palavra);
                break;
            case 2:
                naoPreferencias.Add(palavra);
                break;
        }
    }

    public string ObterNomeAlimento()
    {
        return this.nomeAlimento;
    }

    public List<string> ObterPreferencias()
    {
        return this.preferencias;
    }

    public List<string> ObterNaoPreferencias()
    {
        return this.naoPreferencias;
    }
}

using System;

public class PedidoProcessado
{
    /** Vari�veis de inst�ncia. */
    private string nomeAlimento;
    private IList<string> preferencias;
    private IList<string> naoPreferencias;

    /** Propriedades. */
    public string NomeAlimento { get; }
    public string Preferencias
    {
        get { return new List<string>(preferencias); }
    }
    public string NaoPreferencias
    {
        get { return new List<string>(naoPreferencias); }
    }

    /** Classe de constantes usadas para manter um registo do que est� a ser processado. */
    private class Contexto
    {
        public const int NomeAlimento = 0;
        public const int Preferencias = 1;
        public const int NaoPreferencias = 2;
    }

    public PedidoProcessado(string pedido)
    {
        char[] delimitadores = { ' ' };
        string[] palavras = pedido.Split(delimitadores);

        this.preferencias = new List<string>();
        this.naoPreferencias = new List<string>();

        Text.StringBuilder palavra = new Text.StringBuilder();
        int contexto = Contexto.NomeAlimento;

        for (int i = 0; i < palavras.Length; i++)
        {
            switch (palavras[i])
            {
                case "com":
                    GuardarPalavra(palavra.ToString(), contexto);
                    palavra.Clear();
                    contexto = Contexto.Preferencias;
                    break;
                case "sem":
                    GuardarPalavra(palavra.ToString(), contexto);
                    palavra.Clear();
                    contexto = Contexto.NaoPreferencias;
                    break;
                case ",":
                case "e":
                    GuardarPalavra(palavra.ToString(), contexto);
                    palavra.Clear();
                    break;
                default:
                    palavra.Append(palavra[i]);
                    palavra.Append(" ");
                    break;
            }
        }
        if (palavra.Length > 0)
            GuardarPalavra(palavra.ToString(), contexto);
    }

    private void GuardarPalavra(string palavra, int contexto)
    {
        if (palavra.Length > 0)
        {
            switch (contexto)
            {
                case Contexto.NomeAlimento:
                    nomeAlimento = palavra;
                    break;
                case Contexto.Preferencias:
                    preferencias.Add(palavra);
                    break;
                case Contexto.NaoPreferencias:
                    naoPreferencias.Add(palavra);
                    break;
            }
        }
    }
}
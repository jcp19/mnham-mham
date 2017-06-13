using System;
using System.Collections.Generic;

namespace Mnham_Mnham
{
    public class PedidoProcessado
    {
        /** Variáveis de instância. */
        private string nomeAlimento;
        private readonly ISet<string> preferencias;
        private readonly ISet<string> naoPreferencias;

        /** Propriedades. */
        public string NomeAlimento { get { return nomeAlimento; } }
        public ISet<string> Preferencias { get { return new HashSet<string>(preferencias); } }
        public ISet<string> NaoPreferencias { get { return new HashSet<string>(naoPreferencias); } }

        /** Classe de constantes usadas para manter um registo do que está a ser processado. */
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

            this.preferencias = new HashSet<string>();
            this.naoPreferencias = new HashSet<string>();

            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            int contexto = Contexto.NomeAlimento;

            foreach (string pal in palavras)
            {
                switch (pal)
                {
                    case "com":
                        GuardarPalavra(sb.ToString(), contexto);
                        sb.Clear();
                        contexto = Contexto.Preferencias;
                        break;
                    case "sem":
                        GuardarPalavra(sb.ToString(), contexto);
                        sb.Clear();
                        contexto = Contexto.NaoPreferencias;
                        break;
                    case ",":
                    case "e":
                        GuardarPalavra(sb.ToString(), contexto);
                        sb.Clear();
                        break;
                    default:
                        sb.Append(pal);
                        sb.Append(" ");
                        break;
                }
            }
            if (sb.Length > 0)
                GuardarPalavra(sb.ToString(), contexto);
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
}

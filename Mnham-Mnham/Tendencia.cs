using System;

namespace Mnham_Mnham
{
    public class Tendencia : IComparable
    {
        private string pedido;
        private int repeticoes;

        public int Repeticoes { get { return repeticoes; } set { repeticoes = value; } }

        public Tendencia(string s)
        {
            this.pedido = s;
            this.repeticoes = 0;
        }

        public void inc()
        {
            this.repeticoes++;
        }

        public int CompareTo(object obj)
        {
            if (obj == null)
                return 1;

            Tendencia alimento = obj as Tendencia;
            if (alimento != null)
            {
                float aval1 = this.repeticoes;
                float aval2 = alimento.Repeticoes;

                return aval1.CompareTo(aval2);
            }
            else
                throw new ArgumentException("O objeto passado como argumento não é uma Tendencia.");
        }
    }
}

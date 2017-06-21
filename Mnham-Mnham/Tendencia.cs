using System;

namespace Mnham_Mnham
{
    public class Tendencia : IComparable<Tendencia>
    {
        private string pedido;
        private int repeticoes;

        public int Repeticoes { get { return repeticoes; } set { repeticoes = value; } }

        public string Pedido { get { return pedido; } }

        public Tendencia(string pedido)
        {
            this.pedido = pedido;
            this.repeticoes = 1;
        }

        public void inc()
        {
            ++this.repeticoes;
        }

        public int CompareTo(Tendencia tend)
        {
            if (tend == null)
                return 1;

            float reps1 = this.repeticoes;
            float reps2 = tend.repeticoes;

            int comp =  reps2.CompareTo(reps1);
            if (comp == 0)
            {
                string pedido1 = this.Pedido;
                string pedido2 = tend.Pedido;

                comp = pedido1.CompareTo(pedido2);
            }
            return comp;
        }

        /*public int CompareTo(object obj)
        {
            if (obj == null)
                return 1;

            Tendencia tend = obj as Tendencia;
            if (tend != null)
            {
                float reps1 = this.repeticoes;
                float reps2 = tend.repeticoes;

                return reps2.CompareTo(reps1);
            }
            else
                throw new ArgumentException("O objeto passado como argumento não é uma Tendencia.");
        }*/
    }
}

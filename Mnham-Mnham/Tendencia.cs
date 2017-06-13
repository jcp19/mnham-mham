using System;

namespace Mnham_Mnham
{
    public class Tendencia : IComparable, IComparable<Tendencia>
    {
        private string pedido;
        private int repeticoes;

        public int Repeticoes { get { return repeticoes; } set { repeticoes = value; } }

        public Tendencia(string pedido)
        {
            this.pedido = pedido;
            this.repeticoes = 0;
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

            return reps1.CompareTo(reps2);
        }

        public int CompareTo(object obj)
        {
            if (obj == null)
                return 1;

            Tendencia tend = obj as Tendencia;
            if (tend != null)
            {
                float reps1 = this.repeticoes;
                float reps2 = tend.repeticoes;

                return reps1.CompareTo(reps2);
            }
            else
                throw new ArgumentException("O objeto passado como argumento não é uma Tendencia.");
        }
    }
}

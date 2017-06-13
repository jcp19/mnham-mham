using System;

namespace Mnham_Mnham
{
    public class AlimentoEstabelecimento : IComparable, IComparable<AlimentoEstabelecimento>
    {
        private readonly int numeroPreferenciasVerificadas;
        private readonly Estabelecimento estabelecimento;
        private readonly Alimento alimento;
        private float distancia;

        public int NumeroPreferenciasVerificadas { get { return numeroPreferenciasVerificadas; } }
        public Estabelecimento Estabelecimento { get { return estabelecimento; } }
        public Alimento Alimento { get { return alimento; } }
        public float Distancia { get { return distancia; } set { distancia = value; } }

        public AlimentoEstabelecimento(int numeroPreferenciasVerificadas, float distancia, Estabelecimento estabelecimento, Alimento alimento)
        {
            this.numeroPreferenciasVerificadas = numeroPreferenciasVerificadas;
            this.distancia = distancia;
            this.estabelecimento = estabelecimento;
            this.alimento = alimento;
        }

        public AlimentoEstabelecimento(Estabelecimento estabelecimento, Alimento alimento)
        {
            this.estabelecimento = estabelecimento;
            this.alimento = alimento;
        }

        public int CompareTo(AlimentoEstabelecimento ae)
        {
            if (ae == null)
                return 1;

            int res = numeroPreferenciasVerificadas.CompareTo(ae.numeroPreferenciasVerificadas);

            if (res == 0) // em caso de empate do número de preferências.
            {
                res = alimento.CompareTo(ae.alimento);
                if (res == 0) // em caso de empate na comparação de alimentos.
                {
                    res = estabelecimento.CompareTo(ae.estabelecimento);
                    if (res == 0) // em caso de empate na comparação de estabelecimentos
                    {
                        res = distancia.CompareTo(ae.distancia);
                    }
                }
            }
            return res;
        }

        public int CompareTo(object obj)
        {
            if (obj == null)
                return 1;

            AlimentoEstabelecimento ae = obj as AlimentoEstabelecimento;
            if (ae == null)
                throw new ArgumentException("O alimento passado com argumento não é um AlimentoEstabelecimento");

            int res = numeroPreferenciasVerificadas.CompareTo(ae.numeroPreferenciasVerificadas);

            if (res == 0)
            {
                res = alimento.CompareTo(ae.alimento);
                if (res == 0)
                {
                    res = estabelecimento.CompareTo(ae.estabelecimento);
                    if (res == 0)
                    {
                        res = distancia.CompareTo(ae.Distancia);
                    }
                }
            }
            return res;
        }
    }
}

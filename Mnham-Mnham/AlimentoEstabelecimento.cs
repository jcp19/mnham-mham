using System;

namespace Mnham_Mnham
{
    public class AlimentoEstabelecimento : IComparable, IComparable<AlimentoEstabelecimento>
    {
        private int numeroPreferenciasVerificadas;
        private Estabelecimento estabelecimento;
        private Alimento alimento;

        public int NumeroPreferenciasVerificadas { get; }
        public Estabelecimento Estabelecimento { get; }
        public Alimento Alimento { get; }

        public AlimentoEstabelecimento(int numeroPreferenciasVerificadas, Estabelecimento estabelecimento, Alimento alimento)
        {
            this.numeroPreferenciasVerificadas = numeroPreferenciasVerificadas;
            this.estabelecimento = estabelecimento;
            this.alimento = alimento;
        }

        public int CompareTo(AlimentoEstabelecimento ae)
        {
            if (ae == null)
                return 1;

            int res = this.numeroPreferenciasVerificadas.CompareTo(ae.numeroPreferenciasVerificadas);

            if (res == 0) // em caso de empate do número de preferências.
            {
                res = this.alimento.CompareTo(ae.alimento);
                if (res == 0) // em caso de empate na comparação de alimentos.
                {
                    res = this.estabelecimento.CompareTo(ae.estabelecimento);
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

            // Chegamos aqui se (ae != null)
            int res = this.numeroPreferenciasVerificadas.CompareTo(ae.numeroPreferenciasVerificadas);

            if (res == 0)
            {
                res = this.alimento.CompareTo(ae.alimento);
                if (res == 0)
                {
                    res = this.estabelecimento.CompareTo(ae.estabelecimento);
                }
            }
            return res;
        }
    }
}
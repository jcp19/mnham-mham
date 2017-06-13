using System;

namespace Mnham_Mnham
{
    public sealed class Classificacao
    {
        private readonly int avaliacao;
        private readonly string comentario;
        private readonly int idAutor;
        private readonly DateTime data;

        public int Avaliacao { get { return avaliacao; } }
        public string Comentario { get { return comentario; } }
        public int IdAutor { get { return idAutor; } }
        public DateTime Data { get { return data; } }

        public const int AvaliacaoMin = 1;
        public const int AvaliacaoMax = 5;

        // Evita instanciação sem os atributos obrigatórios (avaliacao e idAutor).
        private Classificacao() { }

        public Classificacao(int avaliacao, int idAutor) : this(avaliacao, null, idAutor, DateTime.Now) { }

        public Classificacao(int avaliacao, string comentario, int idAutor) : this(avaliacao, comentario, idAutor, DateTime.Now) { }

        public Classificacao(int avaliacao, string comentario, int idAutor, DateTime data)
        {
            if ((avaliacao < Classificacao.AvaliacaoMin) || (avaliacao > Classificacao.AvaliacaoMax))
            {
                string msgErro = string.Format("A classificação tem de ser um inteiro no intervalo [{0},{1}].",
                                                Classificacao.AvaliacaoMin, Classificacao.AvaliacaoMax);

                throw new ArgumentOutOfRangeException(msgErro);
            }
            this.avaliacao = avaliacao;
            this.comentario = comentario;
            this.idAutor = idAutor;
            this.data = data;
        }

        public Classificacao(Classificacao original)
        {
            avaliacao = original.avaliacao;
            comentario = original.comentario;
            idAutor = original.idAutor;
            data = original.data;
        }

        public Classificacao Clone()
        {
            return new Classificacao(this);
        }
    }
}

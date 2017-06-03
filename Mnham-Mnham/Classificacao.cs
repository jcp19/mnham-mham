using System;

public class Classificacao
{
    private int avaliacao;
    private string comentario;
    private int idAutor;
    private DateTime data;

    public int Avaliacao { get; }
    public string Comentario { get; }
    public int IdAutor { get; }
    public DateTime Data { get; }

    public const int AvaliacaoMin = 1;
    public const int AvaliacaoMax = 5;

    // Evita instanciação sem os atributos obrigatórios (avaliacao e idAutor).
    private Classificacao() { }

    public Classificacao(int avaliacao, int idAutor)
    {
        this(avaliacao, null, idAutor, DateTime.Now);
    }

    public Classificacao(int avaliacao, string comentario, int idAutor)
    {
        this(avaliacao, comentario, idAutor, DateTime.Now);
    }

    public Classificacao(int avaliacao, string comentario, int idAutor, DateTime date)
    {
        if ((avaliacao < Classificacao.AvaliacaoMin) || (classificacao > Classificacao.AvaliacaoMax))
        {
            string msgErro = string.Format("A classificação tem de ser um inteiro no intervalo [{0},{1}].", Classificacao.AvaliacaoMin, Classificacao.AvaliacaoMax);

            throw new ArgumentOutOfRangeException(msgErro);
        }
        this.avaliacao;
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

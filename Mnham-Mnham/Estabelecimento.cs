using System;

public class Estabelecimento : IComparable, IComparable<Estabelecimento>
{
    private int id;
    private string nome;
	private string contactoTel;
	private string coords;
	private string horario;
	private string tipo;
	private string descricao;
	private bool? aceitaReservas;
    private bool? temMb;
	private bool? temTakeaway;
	private bool? temServMesa;
	private bool? temEsplanada;
	private bool? temParqueEstac;
	private bool? temTv;
	private bool? temWifi;
	private bool? temZonaFum;
    private bool? permanFechado;
	private string morada;
    private IDictionary<int, Alimento> alimentos;
    private IDictionary<int, Classificacao> classificacoes;
    private Image foto;

    public int Id { get; }
    public string Nome { get; }
    public string ContactoTel { get; }
    public string Coords { get; }
    public string Horario { get; }
    public string Tipo { get; }
    public string Descricao { get; }
    public bool? AceitaReservas { get; }
    public bool? TemMb { get; set;  }
    public bool? TemTakeaway { get; set; }
    public bool? TemServMesa { get; set; }
    public bool? TemEsplanada { get; set; }
    public bool? TemParqueEstac { get; set; }
    public bool? TemTv { get; set; }
    public bool? TemWifi { get; }
    public bool? TemZonaFum { get; }
    public bool? PermanFechado { get; }
    public string Morada { get; }
    public Image foto { get; }

    public Estabelecimento(int id, string nome, string contactoTel, string coords, string horario, bool permanFechado)
    {
        this.id = id;
        this.nome = nome;
        this.contactoTel = contactoTel;
        this.coords = coords;
        this.horario = horario;
        this.permanFechado = permanFechado;
        this.alimentos = new Dictionary<int, Alimento>();
        this.classificacoes = new Dictionary<int, Classificacao>();
    }

	public IList<Alimento> ObtemAlimentos(string nomeAlimento)
    {
        IList<Alimento> res = new List<Alimento>();

        foreach (Alimento a in alimentos.Values)
        {
            if (a.Designacao.Contains(nomeAlimento))
            {
                res.Add(a.Clone());
            }
        }
        return res;
	}

	public Alimento ObtemAlimentoPorId(int id)
    {
        return alimentos[id].Clone();
	}

	public void AssociaFotoAlimento(int idAlimento, Image foto)
    {
        alimentos[id].Foto = foto.Clone();
	}

	public void AssociaIngredienteAlimento(int idAlimento, string designacaoIngrediente)
    {
        Alimento a = alimentos[idAlimento];

        a.AdicionaIngrediente(designacaoIngrediente);
	}

    public bool RemoverClassificacaoAlimento(int idAlimento, int idCliente)
    {
        return alimentos[idAlimento].RemoverClassificacaoAlimento();
    }

    public bool RemoverClassificacaoEstabelecimento(int idAutor)
    {
        return classificacoes.Remove(idAutor);
	}

	public void RemoveAlimento(int idAlimento)
    {
        return alimentos.Remove(idAlimento);
	}

	public void ClassificarAlimento(int idAlimento, int idCliente, int avaliacao, string comentario)
    {
        alimentos[idAlimento].ClassificarAlimento(idCliente, avaliacao, comentario);
	}

    public void ClassificarAlimento(int idAlimento, int idCliente, int avaliacao)
    {
        alimentos[idAlimento].ClassificarAlimento(idCliente, avaliacao);
    }

    public void ClassificareEstabelecimento(int idCliente, int avaliacao, string comentario)
    {
        classificacoes[idCliente] = new Classificacao(avaliacao, comentario, idCliente);
	}

	public void ClassificarEstabelecimento(int idCliente, int avaliacao)
    {
        classificacoes[idCliente] = new Classificacao(avaliacao, idCliente);
	}

    public float ObterAvaliacaoMedia()
    {
        int total = 0;
        float soma = 0.0f;

        foreach (var classificacao in classificacoes.Values)
        {
            soma += classificacao.Avaliacao;
            ++total;
        }
        return soma / total;
    }

    public int CompareTo(Estabelecimento e)
    {
        
    }
}

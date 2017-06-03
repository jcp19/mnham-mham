using System;

public class Alimento : IComparable, IComparable<Alimento>
{
	private int id;
	private string designacao;
	private float? preco;
	private ISet<string> ingredientes;
    private IDictionary<int, Classificacao> classificacoes;
    private Image foto;

    public int Id { get; }
    public string Designacao { get; set; }
    public float Preco { get; set; }
    
    // Assegura que não é possível criar alimentos sem especificar os seus atributos.
    private Alimento() {}

    public Alimento(int id, string designacao, float? preco, ISet<string> ingredientes, Image foto)
    {
        if (preco != null && preco < 0.0f)
            throw new ArgumentOutOfRangeException("O preço do alimento não pode ser negativo.");

        this.id = id;
        this.designacao = designacao;
        this.ingredientes = (ingredientes == null) ? new HashSet<string>() : new HashSet<string>(ingredientes);
        this.classificacoes = new Dictionary<int, Classificacao>();
        this.foto = foto;
    }

    public Alimento(Alimento original)
    {
        id = original.id;
        designacao = original.designacao;
        preco = original.preco;
        ingredientes = (original.ingredientes == null) ? null : new HashSet<string>(original.ingredientes);

        if (original.classificacoes != null)
        {
            classificacoes = new Dictionary<int, Classificacao>(original.classificacoes.Count);
            foreach (KeyValuePair<int, Classificacao> entrada in original.classificacoes)
                classificacoes.add(entrada.Key, entrada.Value.Clone());
        }
        foto = original.foto.Clone();
    }

    public bool ContemNaoPreferencias(List<string> naoPreferencias)
    {
        foreach (string naoPref in naoPreferencias)
        {
            foreach (string ingr in ingredientes)
            {
                if (ingr.Contains(naoPref))
                    return true;
            }
        }
        return false;
	}

	public int QuantasPreferenciasContem(List<string> preferencias)
    {
        int n = 0;

        foreach (string pref in preferencias)
        {
            foreach (string ingr in ingredientes)
            {
                if (ingredientes.Contains(pref))
                    n++;
            }
        }
        return n;
	}

    public void AdicionaIngrediente(string designacaoIngrediente)
    {
        ingredientes.Add(designacaoIngrediente);
    }

    public void ClassificarAlimento(int idCliente, int avaliacao, string comentario)
    {
        classificacoes[idCliente] = new Classificacao(avaliacao, idCliente, comentario);
	}

	public void ClassificarAlimento(int idCliente, int avaliacao)
    {
        classificacoes[idCliente] = new Classificacao(avaliacao, idCliente);
    }

	public bool RemoverClassificacaoAlimento(int idCliente)
    {
        return classificacoes.Remove(idCliente);    
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

    public Alimento Clone()
    {
        return new Alimento(this);
    }

    public int CompareTo(Alimento a)
    {
        float aval1 = this.ObterAvaliacaoMedia();
        float aval2 = a.ObterAvaliacaoMedia();

        // Classificação do alimento !!
        if (aval1 < aval2)
            return -1;
        else if (aval1 > aval2)
            return 1;
        else
            return 0;
    }
}

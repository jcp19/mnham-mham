using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Mnham_Mnham
{
    public class Alimento : IComparable, IComparable<Alimento>
    {
        // Variáveis de instância
        private int id;
        private string designacao;
        private float? preco;
        private readonly ISet<string> ingredientes;
        private byte[] foto;
        private int idEstabelecimento;
        private readonly IList<Classificacao> classificacoes;
        private float classificacaoMedia;

        // Propriedades
        public int Id => id;
        public string Designacao { get { return designacao; } set { designacao = value; } }
        public float? Preco { get { return preco; } set { preco = value; } }
        public ISet<string> Ingredientes => ingredientes;
        
        public byte[] Foto
        {
            get
            {
                byte[] copia = null;

                if (foto != null)
                {
                    copia = new byte[foto.Length];
                    Array.Copy(foto, copia, foto.Length);
                }
                return copia;
            }
            set
            {
                if (value != null)
                {
                    foto = new byte[value.Length];
                    Array.Copy(value, foto, value.Length);
                }
                else
                    foto = null;
            }
        }
        public int IdEstabelecimento { get { return idEstabelecimento; } set { idEstabelecimento = value; } }
        public float ClassificacaoMedia { get { return classificacaoMedia; } set { classificacaoMedia = value; } }
        public IList<Classificacao> Classificacoes { get { return classificacoes; } }

        // Assegura que não é possível criar alimentos sem especificar os seus atributos.
        private Alimento() { }

        [JsonConstructor]
        public Alimento(int id, string designacao, ISet<string> ingredientes, int idEstabelecimento, float? preco = null, byte[] foto = null)
        {
            if (preco != null && preco < 0.0f)
                throw new ArgumentOutOfRangeException("O preço do alimento não pode ser negativo.");

            this.id = id;
            this.designacao = designacao;
            this.preco = preco;
            this.ingredientes = (ingredientes == null) ? new HashSet<string>() : new HashSet<string>(ingredientes);
            this.classificacoes = new List<Classificacao>();
            this.idEstabelecimento = -1; // id desconhecido
            this.Foto = foto; // usa o setter
            this.classificacaoMedia = 0.0f;
            this.idEstabelecimento = idEstabelecimento;
        }

        public Alimento(string designacao, ISet<string> ingredientes, float? preco = null, byte[] foto = null) :
            this(-1, designacao, ingredientes, -1, preco, foto)
        {

        }

        public Alimento(Alimento original)
        {
            this.id = original.id;
            this.designacao = original.designacao;
            this.preco = original.preco;
            this.ingredientes = (original.ingredientes == null) ? null : new HashSet<string>(original.ingredientes);

            if (original.classificacoes != null)
                this.classificacoes = new List<Classificacao>(original.classificacoes);

            this.idEstabelecimento = original.idEstabelecimento;
            this.Foto = original.foto; // usa o setter
            this.classificacaoMedia = original.classificacaoMedia;
        }

        public bool ContemNaoPreferencias(ISet<string> naoPreferencias)
        {
            foreach (string naoPref in naoPreferencias)
            {
                foreach (string ingr in ingredientes)
                {
                    if (ingr.IndexOf(naoPref, StringComparison.CurrentCultureIgnoreCase) >= 0)
                    {
                        return true;
                    }
                    Console.WriteLine(">>>>> INGREDIENTE_ALIMENTO: " + ingr);
                    Console.WriteLine(">>>>> NAO_PREFERENCIA: " + naoPref);
                }
            }
            return false;
        }

        public int QuantasPreferenciasContem(ISet<string> preferencias)
        {
            int n = 0;

            foreach (string pref in preferencias)
            {
                foreach (string ingr in ingredientes)
                {
                    if (ingr.IndexOf(pref, StringComparison.CurrentCultureIgnoreCase) >= 0)
                        n++;
                }
            }
            return n;
        }

        public void AdicionarClassificacoes(IEnumerable<Classificacao> classificacoes)
        {
            foreach (var c in classificacoes)
            {
                this.classificacoes[c.IdAutor] = c.Clone();
            }
        }

        public void AdicionarIngrediente(string designacaoIngrediente)
        {
            ingredientes.Add(designacaoIngrediente);
        }

        public void AdicionaIngrediente(string designacaoIngrediente)
        {
            ingredientes.Add(designacaoIngrediente);
        }

        public void AdicionarIngredientes(IEnumerable<string> ingredientes)
        {
            foreach (var ingr in ingredientes)
                this.ingredientes.Add(ingr);
        }

        public void ClassificarAlimento(int idCliente, int avaliacao, string comentario)
        {
            classificacoes[idCliente] = new Classificacao(avaliacao, comentario, idCliente);
        }

        public void ClassificarAlimento(int idCliente, int avaliacao)
        {
            classificacoes[idCliente] = new Classificacao(avaliacao, idCliente);
        }

        /*public bool RemoverClassificacaoAlimento(int idCliente)
        {
            return classificacoes.Remove(idCliente);
        }*/

        public float ObterAvaliacaoMedia()
        {
            int total = 0, soma = 0;

            foreach (var classificacao in classificacoes)
            {
                soma += classificacao.Avaliacao;
                ++total;
            }
            return (total > 0) ? (soma / (float)total) : 0.0f;
        }

        public Alimento Clone()
        {
            return new Alimento(this);
        }

        public int CompareTo(Alimento alimento)
        {
            if (alimento == null)
                return 1;

            float aval1 = this.classificacaoMedia;
            float aval2 = alimento.classificacaoMedia;

            return aval1.CompareTo(aval2);
        }

        public int CompareTo(object obj)
        {
            if (obj == null)
                return 1;

            Alimento alimento = obj as Alimento;
            if (alimento != null)
            {
                float aval1 = this.classificacaoMedia;
                float aval2 = alimento.classificacaoMedia;

                return aval1.CompareTo(aval2);
            }
            else
                throw new ArgumentException("O objeto passado como argumento não é um Alimento.");
        }
    }
}

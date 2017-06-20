using Android.Locations;
using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Mnham_Mnham
{
    public class Estabelecimento : IComparable, IComparable<Estabelecimento>
    {
        private readonly int id;
        private string nome;
        private string contactoTel;
        //private string coords;
        private Location coords;
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
        private List<Classificacao> classificacoes;
        private byte[] foto;
        private float classificacaoMedia;

        public int Id { get { return id; } }
        public string Nome { get { return nome; } }
        public string ContactoTel { get { return contactoTel; } }
        public Location Coords { get { return coords; } }
        public string Horario { get { return horario; } }
        public string Tipo { get { return tipo; } }
        public string Descricao { get { return descricao; } }
        public bool? AceitaReservas { get { return aceitaReservas; } }
        public bool? TemMb { get { return temMb; } set { temMb = value; } }
        public bool? TemTakeaway { get { return temTakeaway; } set { temTakeaway = value; } }
        public bool? TemServMesa { get { return temServMesa; } set { temServMesa = value; } }
        public bool? TemEsplanada { get { return temEsplanada; } set { temEsplanada = value; } }
        public bool? TemParqueEstac { get { return temParqueEstac; } set { temParqueEstac = value; } }
        public bool? TemTv { get { return temTv; } set { temTv = value; } }
        public bool? TemWifi { get { return temWifi; } set { temWifi = value; } }
        public bool? TemZonaFum { get { return temZonaFum; } set { temZonaFum = value; } }
        public bool? PermanFechado { get { return permanFechado; } set { permanFechado = value; } }
        public string Morada { get { return morada; } }
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

        public float ClassificacaoMedia { get { return classificacaoMedia; } set { classificacaoMedia = value; } }
        public object Classificacoes { get { return classificacoes; } }

        private Estabelecimento() { }

        [JsonConstructor]
        public Estabelecimento(int id, string nome, string contactoTel, string morada, double latitude, double longitude, string horario, bool permanFechado)
        {
            this.id = id;
            this.nome = nome;
            this.contactoTel = contactoTel;
            this.coords = new Location("");
            this.coords.Latitude = latitude;
            this.coords.Longitude = longitude;
            this.horario = horario;
            this.morada = morada;
            this.permanFechado = permanFechado;
            this.alimentos = new Dictionary<int, Alimento>();
            this.classificacoes = new List<Classificacao>();
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

        public void AssociaFotoAlimento(int idAlimento, byte[] foto)
        {
            alimentos[idAlimento].Foto = foto;
        }

        public void AssociaIngredienteAlimento(int idAlimento, string designacaoIngrediente)
        {
            Alimento a = alimentos[idAlimento];

            a.AdicionaIngrediente(designacaoIngrediente);
        }

        public void AdicionarClassificacoes(IEnumerable<Classificacao> classificacoes)
        {
            foreach (var c in classificacoes)
            {
                this.classificacoes[c.IdAutor] = c.Clone();
            }
        }

        /*public bool RemoverClassificacaoAlimento(int idAlimento, int idCliente)
        {
            return alimentos[idAlimento].RemoverClassificacaoAlimento(idCliente);
        }*/

        /*public bool RemoverClassificacaoEstabelecimento(int idAutor)
        {
            return classificacoes.Remove(idAutor);
        }*/

        public bool RemoveAlimento(int idAlimento)
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

        public void ClassificarEstabelecimento(int idCliente, int avaliacao, string comentario)
        {
            classificacoes[idCliente] = new Classificacao(avaliacao, comentario, idCliente);
        }

        public void ClassificarEstabelecimento(int idCliente, int avaliacao)
        {
            classificacoes[idCliente] = new Classificacao(avaliacao, idCliente);
        }

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

        public int CompareTo(Estabelecimento estabelecimento)
        {
            if (estabelecimento == null)
                return 1;

            float aval1 = this.ObterAvaliacaoMedia();
            float aval2 = estabelecimento.ObterAvaliacaoMedia();

            return aval1.CompareTo(aval2);
        }

        public int CompareTo(object obj)
        {
            if (obj == null)
                return 1;

            Estabelecimento estabelecimento = obj as Estabelecimento;
            if (estabelecimento != null)
            {
                float aval1 = this.ObterAvaliacaoMedia();
                float aval2 = estabelecimento.ObterAvaliacaoMedia();

                return aval1.CompareTo(aval2);
            }
            else
                throw new ArgumentException("O objeto passado como argumento não é um Estabelecimento.");
        }
    }
}
using System;
using Android.OS;
using Java.Interop;

namespace Mnham_Mnham
{
    public sealed class Classificacao : Java.Lang.Object, IParcelable
    {
        private readonly int avaliacao;
        private readonly string comentario;
        private readonly int idAutor;
        private readonly DateTime data;

        public int Avaliacao => avaliacao;
        public string Comentario => comentario;
        public int IdAutor => idAutor;
        public DateTime Data => data;

        public const int AvaliacaoMin = 1;
        public const int AvaliacaoMax = 5;

        public Classificacao() { }

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

        // Implementação de Parcelable
        public int DescribeContents()
        {
            return 0;
        }

        public void WriteToParcel(Parcel dest, ParcelableWriteFlags flags)
        {
            dest.WriteInt(avaliacao);
            dest.WriteString(comentario);
            dest.WriteInt(idAutor);
            dest.WriteLong(data.Ticks);
        }

        private Classificacao(Parcel source)
        {
            avaliacao = source.ReadInt();
            comentario = source.ReadString();
            idAutor = source.ReadInt();
            data = new DateTime(source.ReadLong());
        }

        private static readonly CriadorParcelableGenerico<Classificacao> creator
            = new CriadorParcelableGenerico<Classificacao>((parcel) => new Classificacao(parcel));

        [ExportField("CREATOR")]
        public static CriadorParcelableGenerico<Classificacao> ObterCreator()
        {
            return creator;
        }
    }
}

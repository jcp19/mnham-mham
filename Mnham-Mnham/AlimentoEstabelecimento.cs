using System;
using Android.OS;
using Java.Interop;
using Java.Lang;
using IComparable = System.IComparable;

namespace Mnham_Mnham
{
    public class AlimentoEstabelecimento : Java.Lang.Object, IParcelable, IComparable, IComparable<AlimentoEstabelecimento>
    {
        private readonly int numeroPreferenciasVerificadas;
        private readonly Estabelecimento estabelecimento;
        private readonly Alimento alimento;
        private float distancia;

        public int NumeroPreferenciasVerificadas => numeroPreferenciasVerificadas;
        public Estabelecimento Estabelecimento => estabelecimento;
        public Alimento Alimento => alimento;
        public float Distancia { get { return distancia; } set { distancia = value; } }

        public AlimentoEstabelecimento() { }

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

            int res = ae.numeroPreferenciasVerificadas.CompareTo(numeroPreferenciasVerificadas);

            if (res == 0) // em caso de empate do número de preferências.
            {
                res = ae.alimento.CompareTo(alimento);
                if (res == 0) // em caso de empate na comparação de alimentos.
                {
                    res = ae.estabelecimento.CompareTo(estabelecimento);
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

            int res = ae.numeroPreferenciasVerificadas.CompareTo(numeroPreferenciasVerificadas);

            if (res == 0)
            {
                res = ae.alimento.CompareTo(alimento);
                if (res == 0)
                {
                    res = ae.estabelecimento.CompareTo(ae.estabelecimento);
                    if (res == 0)
                    {
                        res = distancia.CompareTo(ae.Distancia);
                    }
                }
            }
            return res;
        }

        public int DescribeContents()
        {
            return 0;
        }

        public void WriteToParcel(Parcel dest, ParcelableWriteFlags flags)
        {
            dest.WriteInt(numeroPreferenciasVerificadas);
            dest.WriteParcelable(estabelecimento, flags);
            dest.WriteParcelable(alimento, flags);
            dest.WriteFloat(distancia);
        }

        private AlimentoEstabelecimento(Parcel source)
        {
            ClassLoader estabelecimentoClassLoader = new Estabelecimento().Class.ClassLoader;
            ClassLoader alimentoClassLoader = new Alimento().Class.ClassLoader;

            numeroPreferenciasVerificadas = source.ReadInt();
            estabelecimento = (Estabelecimento)source.ReadParcelable(estabelecimentoClassLoader);
            alimento = (Alimento)source.ReadParcelable(alimentoClassLoader);
            distancia = source.ReadFloat();
        }

        private static readonly CriadorParcelableGenerico<AlimentoEstabelecimento> creator
            = new CriadorParcelableGenerico<AlimentoEstabelecimento>((parcel) => new AlimentoEstabelecimento(parcel));

        [ExportField("CREATOR")]
        public static CriadorParcelableGenerico<AlimentoEstabelecimento> ObterCreator()
        {
            return creator;
        }
    }
}

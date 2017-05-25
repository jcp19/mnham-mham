using System;
public class AlimentoEstabelecimento {
	private int numeroDePreferenciasVerificadas;

	private Estabelecimento estabelecimento;
	private Alimento alimento;


    public int compareTo(AlimentoEstabelecimento ae)
    {
        int comp =  this.numeroDePreferenciasVerificadas.CompareTo(ae.ObterNumeroPreferencias());
        if (comp != 0)
        {
            return comp;
        }
        comp = this.alimento.compareTo(ae.ObterAlimento());
        if (comp != 0)
        {
            return comp;
        }
        comp = this.estabelecimento.compareTo(ae.ObterEstabelecimento());
        return comp;
    }
}

using System;
public class Cliente {
	private int id;
	private char genero;
	private string email;
	private string nome;
	private string palavraPasse;

	public List<string> ObterNaoPreferencias(ref string nomeAlimento) {
        List<string> naoPrefs = new List<string>();
        foreach(Preferencia p in naoPreferencias)
        {
            if (p.ObterDesignacaoAlimento().Compare(nomeAlimento) == 0) //ALTERAR!!
            {
                naoPrefs.Add(p.ObterDesignacaoIngrediente());
            }
        }
        return naoPrefs;
	}
	public List<string> ObterPreferencias(ref string nomeAlimento) {
        List<string> prefs = new List<string>();
        foreach (Preferencia p in preferencias)
        {
            if (p.ObterDesignacaoAlimento().Compare(nomeAlimento) == 0) //ALTERAR!!
            {
                prefs.Add(p.ObterDesignacaoIngrediente());
            }
        }
        return prefs;
    }
	public void RegistarPreferenciaGeral(ref String designacaoPreferencia) {
		throw new System.Exception("Not implemented");
	}
	public void RegistarPreferenciaAlimento(ref string designacaoPreferencia, ref string designacaoAlimento) {
		throw new System.Exception("Not implemented");
	}
	public void RegistarNaoPreferenciaGeral(ref String designacaoPreferencia) {
		throw new System.Exception("Not implemented");
	}
	public void RegistarNaoPreferenciaAlimento(ref string designacaoPreferencia, ref string designacaoAlimento) {
		throw new System.Exception("Not implemented");
	}
	public void RemovePreferencia(ref string designacaoIngrediente, ref string designacaoAlimento) {
		throw new System.Exception("Not implemented");
	}
	public void RemoveNaoPreferencia(ref string designacaoIngrediente, ref string designacaoAlimento) {
		throw new System.Exception("Not implemented");
	}

	private List<Preferencia> preferencias;
	private List<Preferencia> naoPreferencias;


}

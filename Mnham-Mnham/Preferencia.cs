using System;

public class Preferencia
{
	private string designacaoIngrediente;
	private string designacaoAlimento;

    public string DesignacaoIngrediente { get; }
    public string DesignacaoAlimento { get; }

    private Preferencia() {}

    public Preferencia(string designacaoIngrediente)
    {
        this(designacaoIngrediente, "");
    }

    public Preferencia(string designacaoIngrediente, string designacaoAlimento)
    {
        this.designacaoIngrediente = designacaoIngrediente;
        this.designacaoAlimento = designacaoAlimento;
    }

    public override bool Equals(object obj)
    {
        if (obj == null || obj.GetType() != Preferencia)
            return false;

        Preferencia pref = (Preferencia)obj;

        return designacaoIngrediente.Equals(pref.designacaoIngrediente) && designacaoAlimento.Equals(pref.designacaoAlimento);
    }
}

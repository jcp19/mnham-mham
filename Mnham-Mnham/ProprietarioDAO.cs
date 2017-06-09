using System;
using System.Collections.Generic;

namespace Mnham_Mnham
{
    internal class ProprietarioDAO
    {
        EstabelecimentoDAO estabelecimentos;

        internal Proprietario ObterPorEmail(string email)
        {
            throw new NotImplementedException();
        }

        internal bool AdicionarProprietario(Proprietario proprietario)
        {
            throw new NotImplementedException();
        }

        internal bool RemoverAlimento(int idAlimento)
        {
            return estabelecimentos.RemoverAlimento(idAlimento);
        }

        internal bool EditarDados(Proprietario proprietario)
        {
            throw new NotImplementedException();
        }

        internal Proprietario ObterPorId(int proprietarioAutenticado)
        {
            throw new NotImplementedException();
        }

        internal List<Estabelecimento> ConsultarEstabelecimentos(int proprietarioAutenticado)
        {
            return estabelecimentos.ConsultarEstabelecimentos(proprietarioAutenticado);
        }

        internal List<Alimento> ConsultarAlimentos(int idEstabelecimento)
        {
            return ConsultarAlimentos(idEstabelecimento);
        }

        internal bool RegistarAlimento(int idEstabelecimento, Alimento alim)
        {
            return estabelecimentos.RegistarAlimento(idEstabelecimento, alim);
        }

        internal bool EditarFotoAlimento(int idAlimento, byte[] foto)
        {
            return estabelecimentos.EditarFotoAlimento(idAlimento, foto);
        }

        internal bool AdicionarIngredientesAlimento(int idAlimento, List<string> designacaoIngredientes)
        {
            return estabelecimentos.AdicionarIngredientesAlimento(idAlimento, designacaoIngredientes);
        }

        internal bool RemoverIngredientesAlimento(int idAlimento, List<string> designacaoIngredientes)
        {
            return estabelecimentos.RemoverIngredientesAlimento(idAlimento, designacaoIngredientes);
        }
    }
}
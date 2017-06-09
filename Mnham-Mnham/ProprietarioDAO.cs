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
            throw new NotImplementedException();
        }

        internal void EditarDados(Proprietario proprietario)
        {
            throw new NotImplementedException();
        }

        internal Proprietario ObterPorId(int clienteAutenticado)
        {
            throw new NotImplementedException();
        }

        internal List<Estabelecimento> ConsultarEstabelecimentos(int clienteAutenticado)
        {
            throw new NotImplementedException();
        }

        internal List<Alimento> ConsultarAlimentos(int idEstabelecimento)
        {
            throw new NotImplementedException();
        }

        internal void RegistarAlimento(int idEstabelecimento, Alimento alim)
        {
            throw new NotImplementedException();
        }

        internal void EditarFotoAlimento(int idAlimento, byte[] foto)
        {
            throw new NotImplementedException();
        }

        internal void AdicionarIngredientesAlimento(int idAlimento, List<string> designacaoIngredientes)
        {
            throw new NotImplementedException();
        }

        internal void RemoverIngredientesAlimento(int idAlimento, List<string> designacaoIngredientes)
        {
            throw new NotImplementedException();
        }
    }
}
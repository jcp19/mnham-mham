using System;
using System.Collections.Generic;

namespace Mnham_Mnham
{
    internal class EstabelecimentoDAO : DAO
    {
        private AlimentoDAO alimentos;
        private ClassificacaoEstabelecimentoDAO classificacoes;

        internal EstabelecimentoDAO()
        {
            this.alimentos = new AlimentoDAO();
        }

        internal IEnumerable<int> ObterIdsEstabelecimento()
        {
            throw new NotImplementedException();
        }

        internal List<Alimento> ObterIngredientesAlimentos(int idEstabelecimento, string v)
        {
            return alimentos.ObterIngredientesAlimentos(idEstabelecimento, v);
        }

        internal Estabelecimento ObterEstabelecimento(int idEstabelecimento)
        {
            throw new NotImplementedException();
        }

        internal Alimento ObterAlimento(int idAlimento)
        {
            return alimentos.ObterAlimento(idAlimento);
        }

        internal void ClassificarAlimento(int idAlimento, Classificacao cla)
        {
            alimentos.ClassificarAlimento(idAlimento, cla);
        }

        internal void ClassificarEstabelecimento(int idEstabelecimento, Classificacao cla)
        {
            classificacoes.ClassificarEstabelecimento(idEstabelecimento, cla);
        }

        internal void RemoverClassificacaoEstabelecimento(int idEstabelecimento, int clienteAutenticado)
        {
            classificacoes.RemoverClassificacaoEstabelecimento(idEstabelecimento, clienteAutenticado);
        }

        internal void RemoverClassificacaoAlimento(int idAlimento, int clienteAutenticado)
        {
            alimentos.RemoverClassificacaoEstabelecimento(idAlimento, clienteAutenticado);
        }
    }
}
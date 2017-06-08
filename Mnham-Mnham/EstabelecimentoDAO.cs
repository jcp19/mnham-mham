using System;
using System.Collections.Generic;

namespace Mnham_Mnham
{
    internal class EstabelecimentoDAO : DAO
    {
        private AlimentoDAO alimentos;
        private ClassificacaoEstabelecimentoDAO classificacoes;

        internal Estabelecimento ObterEstabelecimento(int idEstabelecimento)
        {
            throw new NotImplementedException();
        }

        internal Alimento ObterAlimento(int idAlimento)
        {
            return alimentos.ObterAlimento(idAlimento);
        }

        internal bool ClassificarAlimento(int idAlimento, Classificacao cla)
        {
            return alimentos.ClassificarAlimento(idAlimento, cla);
        }

        internal bool ClassificarEstabelecimento(int idEstabelecimento, Classificacao cla)
        {
            return classificacoes.ClassificarEstabelecimento(idEstabelecimento, cla);
        }

        internal bool RemoverClassificacaoEstabelecimento(int idEstabelecimento, int clienteAutenticado)
        {
            return classificacoes.RemoverClassificacaoEstabelecimento(idEstabelecimento, clienteAutenticado);
        }

        internal bool RemoverClassificacaoAlimento(int idAlimento, int clienteAutenticado)
        {
            return alimentos.RemoverClassificacaoAlimento(idAlimento, clienteAutenticado);
        }

        internal IEnumerable<Alimento> ObterAlimentos(string nomeAlimento)
        {
            return alimentos.ObterAlimentos(nomeAlimento);
        }

        internal Estabelecimento ObterEstabelecimentoAlimento(int idAlimento)
        {
            return alimentos.ObterEstabelecimento(idAlimento);
        }
    }
}
using System;
using System.Collections.Generic;

namespace Mnham_Mnham
{
    internal class EstabelecimentoDAO : DAO
    {
        internal IEnumerable<int> ObterIdsEstabelecimento()
        {
            throw new NotImplementedException();
        }

        internal List<Alimento> ObterAlimentos(int idEstabelecimento, string v)
        {
            throw new NotImplementedException();
        }

        internal Estabelecimento ObterEstabelecimento(int idEstabelecimento)
        {
            throw new NotImplementedException();
        }

        internal void ClassificarAlimento(int idAlimento, int idEstabelecimento, Classificacao cla)
        {
            throw new NotImplementedException();
        }

        internal void ClassificarEstabelecimento(int idEstabelecimento, Classificacao cla)
        {
            throw new NotImplementedException();
        }
    }
}
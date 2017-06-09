using System;
using System.Collections.Generic;

namespace Mnham_Mnham
{
    class ClassificacaoEstabelecimentoDAO : DAO
    {
        public ClassificacaoEstabelecimentoDAO() : base()
        {

        }

        public ClassificacaoEstabelecimentoDAO(string connectionString) : base(connectionString)
        {
            
        }

        internal bool ClassificarEstabelecimento(int idEstabelecimento, Classificacao cla)
        {
            throw new NotImplementedException();
        }

        internal bool RemoverClassificacaoEstabelecimento(int idEstabelecimento, int clienteAutenticado)
        {
            throw new NotImplementedException();
        }

        internal List<Classificacao> ConsultarClassificacoesEstabelecimentos(int clienteAutenticado)
        {
            throw new NotImplementedException();
        }
    }
}
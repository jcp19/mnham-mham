using System;
using System.Collections.Generic;

namespace Mnham_Mnham
{
    class ClassificacaoAlimentoDAO : DAO
    {
        public ClassificacaoAlimentoDAO(string connectionString) : base(connectionString)
        {
        }

        public ClassificacaoAlimentoDAO() : base()
        {
        }

        internal bool ClassificarAlimento(int idAlimento, Classificacao cla)
        {
            throw new NotImplementedException();
        }

        internal bool RemoverClassificacaoAlimento(int idAlimento, int clienteAutenticado)
        {
            throw new NotImplementedException();
        }

        internal List<Classificacao> ConsultarClassificacoesAlimentos(int clienteAutenticado)
        {
            throw new NotImplementedException();
        }
    }
}
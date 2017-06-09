using System;

namespace Mnham_Mnham
{
    class NaoPreferenciaDAO : DAO
    {
        public NaoPreferenciaDAO() : base()
        {

        }
        public NaoPreferenciaDAO(string connectionString) : base(connectionString)
        {

        }

        internal bool AdicionarNaoPreferencia(int clienteAutenticado, Preferencia naoPreferencia)
        {
            throw new NotImplementedException();
        }

        internal bool RemoverNaoPreferencia(int clienteAutenticado, Preferencia naoPreferencia)
        {
            throw new NotImplementedException();
        }
    }
}
using System;

namespace Mnham_Mnham
{
    class PreferenciaDAO : DAO
    {
        public PreferenciaDAO() : base() { }
        public PreferenciaDAO(string connectionString) : base(connectionString) { }

        public bool AdicionarPreferencia(int clienteAutenticado, Preferencia preferencia)
        {
            throw new NotImplementedException();
        }

        internal bool RemoverPreferencia(int clienteAutenticado, Preferencia preferencia)
        {
            throw new NotImplementedException();
        }
    }
}
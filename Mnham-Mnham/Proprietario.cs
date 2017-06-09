using System;
using System.Collections.Generic;

namespace Mnham_Mnham
{
    public class Proprietario
    {
        private int id;
        private char genero;
        private string email;
        private string nome;
        private string palavraPasse;
        private string contactoTel;
        private IList<Estabelecimento> estabelecimentos;

        public int Id { get { return id; } }
        public char Genero { get { return genero; } }
        public string Email { get { return email; } set { email = value; } }
        public string Nome { get { return nome; } set { nome = value; } }
        public string PalavraPasse { get { return palavraPasse; } set { palavraPasse = value; } }
        public string ContactoTel { get { return contactoTel; } set { contactoTel = value; } }

        private Proprietario() { }

        public Proprietario(int id, char genero, string email, string nome, string palavraPasse) : this(id, genero, email, nome, palavraPasse, null)
        {

        }

        public Proprietario(int id, char genero, string email, string nome, string palavraPasse, string contactoTel)
        {
            this.id = id;
            this.genero = genero;
            this.email = email;
            this.nome = nome;
            this.palavraPasse = palavraPasse;
            this.contactoTel = contactoTel;
        }

        internal void DefinirId(int clienteAutenticado)
        {
            throw new NotImplementedException();
        }
    }
}
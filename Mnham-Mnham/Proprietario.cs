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

        public int Id { get; }
        public char Genero { get; }
        public string Email { get; set; }
        public string Nome { get; set; }
        public string ContactoTel { get; set; }

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
    }
}
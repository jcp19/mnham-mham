using System;
using System.Collections.Generic;

namespace Mnham_Mnham
{
    public class Cliente
    {
        private int id;
        private char genero;
        private string email;
        private string nome;
        private string palavraPasse;
        private ISet<Preferencia> preferencias;
        private ISet<Preferencia> naoPreferencias;

        public int Id { get { return id; } }
        public string Email { get { return email; } }
        public string PalavraPasse { get { return palavraPasse; } }
        public string Nome { get { return nome; } }
        public char Genero { get { return genero; } }

        public Cliente(int id, char genero, string email, string nome, string palavraPasse)
        {
            if (genero != 'M' && genero != 'F')
                throw new ArgumentException("O género tem de ser 'M' ou 'F'.");

            this.id = id;
            this.genero = genero;
            this.email = email;
            this.nome = nome;
            this.preferencias = new HashSet<Preferencia>();
            this.naoPreferencias = new HashSet<Preferencia>();
            this.palavraPasse = palavraPasse;
        }

        public Cliente(char genero, string email, string nome, string palavraPasse) : this(-1, genero, email, nome, palavraPasse)
        {

        }

        public List<string> ObterNaoPreferencias(string nomeAlimento)
        {
            List<string> naoPrefsAlimento = new List<string>();

            foreach (var naoPref in naoPreferencias)
            {
                if (nomeAlimento.Contains(naoPref.DesignacaoAlimento))
                {
                    naoPrefsAlimento.Add(naoPref.DesignacaoIngrediente);
                }
            }
            return naoPrefsAlimento;
        }

        public List<string> ObterPreferencias(string nomeAlimento)
        {
            List<string> prefsAlimento = new List<string>();

            foreach (var pref in preferencias)
            {
                if (nomeAlimento.Contains(pref.DesignacaoAlimento))
                {
                    prefsAlimento.Add(pref.DesignacaoIngrediente);
                }
            }
            return prefsAlimento;
        }

        public bool RegistarPreferenciaGeral(String designacaoIngrediente)
        {
            return preferencias.Add(new Preferencia(designacaoIngrediente));
        }

        public bool RegistarPreferenciaAlimento(string designacaoIngrediente, string designacaoAlimento)
        {
            return preferencias.Add(new Preferencia(designacaoIngrediente, designacaoAlimento));
        }

        public bool RegistarNaoPreferenciaGeral(String designacaoPreferencia)
        {
            return naoPreferencias.Add(new Preferencia(designacaoPreferencia));
        }

        public bool RegistarNaoPreferenciaAlimento(string designacaoPreferencia, string designacaoAlimento)
        {
            return naoPreferencias.Add(new Preferencia(designacaoPreferencia, designacaoAlimento));
        }

        public bool RemovePreferenciaGeral(string designacaoIngrediente)
        {
            // O método Remove recebe uma Preferencia, logo é preciso criar uma igual à que se pretende remover.
            return preferencias.Remove(new Preferencia(designacaoIngrediente));
        }

        public bool RemovePreferencia(string designacaoIngrediente, string designacaoAlimento)
        {
            return preferencias.Remove(new Preferencia(designacaoIngrediente, designacaoAlimento));
        }

        public bool RemoverNaoPreferenciaGeral(string designacaoIngrediente)
        {
            return naoPreferencias.Remove(new Preferencia(designacaoIngrediente));
        }

        public bool RemoveNaoPreferencia(string designacaoIngrediente, string designacaoAlimento)
        {
            return naoPreferencias.Remove(new Preferencia(designacaoIngrediente, designacaoAlimento));
        }

        internal void DefinirId(int id)
        {
            this.id = id;
        }
    }
}
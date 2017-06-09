using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace Mnham_Mnham
{
    class EstabelecimentoDAO
    {
        private AlimentoDAO alimentos;
        private ClassificacaoEstabelecimentoDAO classificacoes;

        public EstabelecimentoDAO()
        {
            alimentos = new AlimentoDAO();
            classificacoes = new ClassificacaoEstabelecimentoDAO();
        }

        public Estabelecimento ObterEstabelecimento(int idEstabelecimento)
        {
            using (SqlConnection sqlCon = new SqlConnection(DAO.CONECTION_STRING))
            {
                SqlCommand cmd = new SqlCommand("SELECT * FROM Estabelecimento WHERE id = @id AND ativo != 0", sqlCon);
                cmd.Parameters.Add("@id", SqlDbType.Int);
                cmd.Parameters["@id"].Value = idEstabelecimento;

                cmd.Connection.Open();
                var reader = cmd.ExecuteReader();

                Estabelecimento e = null;

                if (reader.Read())
                {
                    e = new Estabelecimento(Convert.ToInt32(reader["id"]), reader["nome"].ToString(), reader["contacto_tel"].ToString(), reader["coords"].ToString(), reader["horario"].ToString(), !Convert.ToBoolean(reader["ativo"]));
                }

                return e;
            }
        }

        internal bool RegistarAlimento(int idEstabelecimento, Alimento alim)
        {
            return alimentos.RegistarAlimento(idEstabelecimento, alim);
        }

        internal bool EditarFotoAlimento(int idAlimento, byte[] foto)
        {
            return alimentos.EditarFotoAlimento(idAlimento, foto);
        }

        internal List<Estabelecimento> ConsultarEstabelecimentos(int proprietarioAutenticado)
        {
            throw new NotImplementedException();
        }

        internal bool AdicionarIngredientesAlimento(int idAlimento, List<string> designacaoIngredientes)
        {
            return alimentos.AdicionarIngredientesAlimento(idAlimento, designacaoIngredientes);
        }

        internal bool RemoverIngredientesAlimento(int idAlimento, List<string> designacaoIngredientes)
        {
            return alimentos.RemoverIngredientesAlimento(idAlimento, designacaoIngredientes);
        }

        internal bool RemoverAlimento(int idAlimento)
        {
            return alimentos.RemoverAlimento(idAlimento);
        }

        public Alimento ObterAlimento(int idAlimento)
        {
            return alimentos.ObterAlimento(idAlimento);
        }

        public bool ClassificarAlimento(int idAlimento, Classificacao cla)
        {
            return alimentos.ClassificarAlimento(idAlimento, cla);
        }

        public bool ClassificarEstabelecimento(int idEstabelecimento, Classificacao cla)
        {
            return classificacoes.ClassificarEstabelecimento(idEstabelecimento, cla);
        }

        public void RemoverClassificacaoEstabelecimento(int idEstabelecimento, int clienteAutenticado)
        {
            classificacoes.RemoverClassificacaoEstabelecimento(idEstabelecimento, clienteAutenticado);
        }

        public void RemoverClassificacaoAlimento(int idAlimento, int clienteAutenticado)
        {
            alimentos.RemoverClassificacaoAlimento(idAlimento, clienteAutenticado);
        }

        public IList<Alimento> ObterAlimentos(string nomeAlimento)
        {
            return alimentos.ObterAlimentos(nomeAlimento);
        }

        public IList<Classificacao> ConsultarClassificacoesAlimentos(int clienteAutenticado)
        {
            return alimentos.ConsultarClassificacoesAlimentos(clienteAutenticado);
        }

        public IList<Classificacao> ConsultarClassificacoesEstabelecimentos(int clienteAutenticado)
        {
            return classificacoes.ConsultarClassificacoesEstabelecimentos(clienteAutenticado);
        }
    }
}
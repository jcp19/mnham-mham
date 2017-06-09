using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace Mnham_Mnham
{
    class EstabelecimentoDAO : DAO
    {
        private AlimentoDAO alimentos;
        private ClassificacaoEstabelecimentoDAO classificacoes;

        public EstabelecimentoDAO() : base()
        {
            alimentos = new AlimentoDAO();
            classificacoes = new ClassificacaoEstabelecimentoDAO();
        }

        public EstabelecimentoDAO(string connectionString) : base(connectionString)
        {
            alimentos = new AlimentoDAO(connectionString);
            classificacoes = new ClassificacaoEstabelecimentoDAO(connectionString);
        }

        public Estabelecimento ObterEstabelecimento(int idEstabelecimento)
        {
            SqlCommand cmd = new SqlCommand("SELECT * FROM Estabelecimento WHERE id = @id AND ativo != 0", base.sqlCon);
            cmd.Parameters.Add("@id", SqlDbType.Int);
            cmd.Parameters["@id"].Value = idEstabelecimento;

            var reader = cmd.ExecuteReader();

            Estabelecimento e = null;

            if (reader.Read())
            {
                e = new Estabelecimento( Convert.ToInt32(reader["id"]), reader["nome"].ToString(), reader["contacto_tel"].ToString(), reader["coords"].ToString(), reader["horario"].ToString(), !Convert.ToBoolean(reader["ativo"]));
            }

            return e;
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

        public bool RemoverClassificacaoEstabelecimento(int idEstabelecimento, int clienteAutenticado)
        {
            return classificacoes.RemoverClassificacaoEstabelecimento(idEstabelecimento, clienteAutenticado);
        }

        public bool RemoverClassificacaoAlimento(int idAlimento, int clienteAutenticado)
        {
            return alimentos.RemoverClassificacaoAlimento(idAlimento, clienteAutenticado);
        }

        public List<Alimento> ObterAlimentos(string nomeAlimento)
        {
            return alimentos.ObterAlimentos(nomeAlimento);
        }

        public List<Classificacao> ConsultarClassificacoesAlimentos(int clienteAutenticado)
        {
            return alimentos.ConsultarClassificacoesAlimentos(clienteAutenticado);
        }

        public List<Classificacao> ConsultarClassificacoesEstabelecimentos(int clienteAutenticado)
        {
            return classificacoes.ConsultarClassificacoesEstabelecimentos(clienteAutenticado);
        }
    }
}
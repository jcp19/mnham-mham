using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace Mnham_Mnham
{
    public class EstabelecimentoDAO
    {
        private readonly AlimentoDAO alimentos;
        private readonly ClassificacaoEstabelecimentoDAO classificacoes;

        public EstabelecimentoDAO()
        {
            alimentos = new AlimentoDAO();
            classificacoes = new ClassificacaoEstabelecimentoDAO();
        }

        public Estabelecimento ObterEstabelecimento(int idEstabelecimento)
        {
            Estabelecimento e = null;

            using (var sqlCon = new SqlConnection(DAO.CONECTION_STRING))
            {
                string txtCmd = "SELECT * FROM Estabelecimento WHERE id = @id AND ativo != 0";

                sqlCon.Open();
                using (var cmd = new SqlCommand(txtCmd, sqlCon))
                {
                    cmd.Parameters.Add("@id", SqlDbType.Int);
                    cmd.Parameters["@id"].Value = idEstabelecimento;

                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            char[] delim = {' '};
                            string[] coords = reader["coords"].ToString().Split(delim);
                            double lat = double.Parse(coords[0]);
                            double lon = double.Parse(coords[1]);

                            e = new Estabelecimento(Convert.ToInt32(reader["id"]), reader["nome"].ToString(),
                                reader["contacto_tel"].ToString(), lat, lon, reader["horario"].ToString(),
                                !Convert.ToBoolean(reader["ativo"]));
                        }
                    }
                }
                if (e != null)
                {
                    e.AdicionarClassificacoes(classificacoes.ClassificacoesEstabelecimento(idEstabelecimento, sqlCon));
                    e.ClassificacaoMedia = e.ObterAvaliacaoMedia();
                }
            }
            return e;
        }

        internal bool RegistarAlimento(int idEstabelecimento, Alimento alim)
        {
            return alimentos.RegistarAlimento(idEstabelecimento, alim);
        }

        internal bool EditarFotoAlimento(int idAlimento, byte[] foto)
        {
            return alimentos.EditarFotoAlimento(idAlimento, foto);
        }

        internal IList<Estabelecimento> ConsultarEstabelecimentos(int proprietarioAutenticado)
        {
            IList<Estabelecimento> res = new List<Estabelecimento>();

            using (var sqlCon = new SqlConnection(DAO.CONECTION_STRING))
            {
                string txtCmd = "SELECT * FROM Estabelecimento WHERE id_proprietario = @id_p";

                sqlCon.Open();
                using (var cmd = new SqlCommand(txtCmd, sqlCon))
                {
                    cmd.Parameters.Add("@id_p", SqlDbType.Int);
                    cmd.Parameters["@id_p"].Value = proprietarioAutenticado;

                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            int idEstabelecimento = Convert.ToInt32(reader["id"]);

                            char[] delim = {' '};
                            string[] coords = reader["coords"].ToString().Split(delim);
                            double lat = double.Parse(coords[0]);
                            double lon = double.Parse(coords[1]);
                            Estabelecimento e = new Estabelecimento(idEstabelecimento, reader["nome"].ToString(),
                                reader["contacto_tel"].ToString(), lat, lon, reader["horario"].ToString(),
                                !Convert.ToBoolean(reader["ativo"]));

                            res.Add(e);
                        }
                    }
                }
                foreach (Estabelecimento e in res)
                {
                    IList<Classificacao> classif = classificacoes.ClassificacoesEstabelecimento(e.Id, sqlCon);

                    e.AdicionarClassificacoes(classif);
                    e.ClassificacaoMedia = e.ObterAvaliacaoMedia();
                }
            }
            return res;
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

        public IList<Alimento> ConsultarAlimentos(int idEstabelecimento)
        {
            return alimentos.ObterAlimentos(idEstabelecimento);
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

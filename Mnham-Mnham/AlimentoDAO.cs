using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace Mnham_Mnham
{
    public class AlimentoDAO
    {
        private readonly IngredienteDAO ingredientes;
        private readonly ClassificacaoAlimentoDAO classificacoes;

        public AlimentoDAO()
        {
            classificacoes = new ClassificacaoAlimentoDAO();
            ingredientes = new IngredienteDAO();
        }

        public Alimento ObterAlimento(int idAlimento)
        {
            Alimento a = null;

            using (var sqlCon = new SqlConnection(DAO.CONECTION_STRING))
            {
                string txtCmd = "SELECT * FROM Alimento WHERE id = @id AND removido = 0";

                sqlCon.Open();
                using (var cmd = new SqlCommand(txtCmd, sqlCon))
                {
                    cmd.Parameters.Add("@id", SqlDbType.Int);
                    cmd.Parameters["@id"].Value = idAlimento;

                    using (var reader = cmd.ExecuteReader())
                    {
                        bool encontrado = reader.Read();

                        if (encontrado)
                        {
                            string designacao = reader["designacao"].ToString();
                            float? preco = (reader["preco"] == DBNull.Value) ? null : (float?)Convert.ToDouble(reader["preco"]);
                            byte[] foto = (reader["foto"] == DBNull.Value) ? null : (byte[])reader["foto"];
                            int idEstabelecimento = Convert.ToInt32(reader["id_estabelecimento"]);
                            
                            // fecha o reader associado à sqlCon para que IngredienteDAO possa associar-lhe outro reader.
                            reader.Close(); 
                            ISet<string> ings = ingredientes.ObterIngredientes(idAlimento, sqlCon);

                            a = new Alimento(idAlimento, designacao, ings, idEstabelecimento, preco, foto);
                            a.AdicionarClassificacoes(classificacoes.ClassificacaoAlimento(idAlimento, sqlCon));
                            a.ClassificacaoMedia = a.ObterAvaliacaoMedia();
                        }
                    }
                }
            }
            return a;
        }

        // Marca como removido o alimento com o id passado argumento.
        public bool RemoverAlimento(int idAlimento)
        {
            bool removido;

            using (var sqlConn = new SqlConnection(DAO.CONECTION_STRING))
            {
                string txtCmd = "UPDATE Alimento SET removido = 1 WHERE id = @id_a;";

                using (var cmd = new SqlCommand(txtCmd, sqlConn))
                {
                    cmd.Parameters.Add("@id_a", SqlDbType.Int);
                    cmd.Parameters["@id_a"].Value = idAlimento;

                    try
                    {
                        cmd.ExecuteNonQuery();
                        removido = true;
                    }
                    catch (SqlException)
                    {
                        removido = false;
                    }
                }
            }
            return removido;
        }

        public bool RemoverIngredientesAlimento(int idAlimento, IList<string> designacaoIngredientes)
        {
            using (var sqlCon = new SqlConnection(DAO.CONECTION_STRING))
            {
                sqlCon.Open();
                IList<int> idsIngrs = ingredientes.ObterIds(designacaoIngredientes, sqlCon);

                if (idsIngrs.Count == 0)
                    return false; // o alimento indicado não tem ingredientes para remover.

                string inicioCmd = "DELETE FROM IngredienteAlimento" +
                                   "WHERE id_alimento = @id_a AND id_ingrediente IN ";
                string strListaIds = '(' + string.Join(",", idsIngrs) + ')';

                using (var cmd = new SqlCommand(inicioCmd + strListaIds, sqlCon))
                {
                    cmd.ExecuteNonQuery();
                }
            }
            return true;
        }

        // CONFIRMAR SE ESTA BEM!!! (INCOMPLETO)
        public bool AdicionarIngredientesAlimento(int idAlimento, IList<string> designacoesIngredientes)
        {
            using (var sqlCon = new SqlConnection(DAO.CONECTION_STRING))
            {
                string txtCmd = "INSERT INTO IngredienteAlimento(id_alimento,id_ingrediente) VALUES (@id_a, @id_i);";

                sqlCon.Open();
                using (var cmd = new SqlCommand(txtCmd, sqlCon))
                {
                    cmd.Parameters.Add("@id_a", SqlDbType.Int);
                    cmd.Parameters.Add("@id_i", SqlDbType.Int);

                    SqlTransaction transaccao = sqlCon.BeginTransaction("AdicionarIngredientesAlimento");

                    cmd.Connection = sqlCon;
                    cmd.Transaction = transaccao;
                    try
                    {   // Adiciona cada um dos ingredientes.
                        foreach (string ingr in designacoesIngredientes)
                        {
                            int idIngrediente = ingredientes.AdicionarIngrediente(ingr, sqlCon);

                            cmd.ExecuteNonQuery();

                            cmd.Parameters["@id_a"].Value = idAlimento;
                            cmd.Parameters["@id_i"].Value = idIngrediente;
                        }
                        transaccao.Commit();
                    }
                    catch (Exception)
                    {
                        transaccao.Rollback();
                        throw;
                    }
                }
                return true;
            }
        }

        // POR FAZER!!!
        public bool EditarFotoAlimento(int idAlimento, byte[] foto)
        {
            throw new NotImplementedException();
        }

        // CONFIRMAR SE ESTA BEM!!! (INCOMPLETO)
        public bool RegistarAlimento(int idEstabelecimento, Alimento alim)
        {
            using (var sqlCon = new SqlConnection(DAO.CONECTION_STRING))
            {
                string txtCmd = @"INSERT INTO Alimento(designacao, preco, removido, id_estabelecimento, foto)
                                  VALUES(@desig, @preco, @removido, @id_estabelecimento, @foto);";

                sqlCon.Open();
                using (var cmd = new SqlCommand(txtCmd, sqlCon))
                {
                    cmd.Parameters.Add("@designacao", SqlDbType.NVarChar, 150);
                    cmd.Parameters["@designacao"].Value = alim.Designacao;

                    cmd.Parameters.Add("@preco", SqlDbType.Decimal, 10);
                    cmd.Parameters["@preco"].Precision = 10;
                    cmd.Parameters["@preco"].Scale = 2;
                    cmd.Parameters["@preco"].Value = (alim.Preco.HasValue) ? (object)alim.Preco.Value : DBNull.Value;

                    cmd.Parameters.Add("@removido", SqlDbType.TinyInt);
                    cmd.Parameters["@removido"].Value = 0;

                    cmd.Parameters.Add("@id_estabelecimento", SqlDbType.Int);
                    cmd.Parameters["@id_estabelecimento"].Value = idEstabelecimento;

                    cmd.Parameters.Add("@foto", SqlDbType.VarBinary, -1); // -1 corresponde a VarBinary(max)
                    cmd.Parameters["@foto"].Value = (object)alim.Foto ?? DBNull.Value;

                    cmd.ExecuteNonQuery();
                }
            }
            // Só chegamos aqui se o alimento foi inserido com sucesso.
            return true;
        }

        public bool ClassificarAlimento(int idAlimento, Classificacao cla)
        {
            return classificacoes.ClassificarAlimento(idAlimento, cla);
        }

        public void RemoverClassificacaoAlimento(int idAlimento, int clienteAutenticado)
        {
            classificacoes.RemoverClassificacaoAlimento(idAlimento, clienteAutenticado);
        }

        /** Obtém a lista de alimentos que contêm 'nomeAlimento' na sua designação. */
        public IList<Alimento> ObterAlimentos(string nomeAlimento)
        {
            using (var sqlCon = new SqlConnection(DAO.CONECTION_STRING))
            {
                IList<int> idsAlimentos = new List<int>();
                string txtCmd = "SELECT id from Alimento WHERE CHARINDEX(@v,designacao) > 0";

                sqlCon.Open();
                using (var cmd = new SqlCommand(txtCmd, sqlCon))
                {
                    cmd.Parameters.Add("@v", SqlDbType.NVarChar, 150);
                    cmd.Parameters["@v"].Value = nomeAlimento;

                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            idsAlimentos.Add(Convert.ToInt32(reader["id"]));
                        }
                    }

                    IList<Alimento> alimentos = new List<Alimento>();
                    foreach (var id in idsAlimentos)
                    {
                        ISet<string> ing = ingredientes.ObterIngredientes(id, sqlCon);

                        // Apenas se obtém o id e os ingredientes.
                        alimentos.Add(new Alimento(id, null, ing, -1));
                    }
                    return alimentos;
                }
            }
        }

        public IList<Alimento> ObterAlimentos(int idEstabelecimento)
        {
            IList<Alimento> alimsEstabelecimento = new List<Alimento>();

            using (var sqlCon = new SqlConnection(DAO.CONECTION_STRING))
            {
                var txtCmd = "SELECT * FROM alimento WHERE idEstabelecimento = @id_e AND removido = 0;";

                sqlCon.Open();
                using (var cmd = new SqlCommand(txtCmd, sqlCon))
                {
                    cmd.Parameters.Add("@id_e", SqlDbType.Int);
                    cmd.Parameters["@id_e"].Value = idEstabelecimento;

                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            int id = Convert.ToInt32(reader["id"]);
                            string designacao = reader["designacao"].ToString();
                            float? preco = (reader["preco"] == DBNull.Value) ? null : (float?)Convert.ToDouble(reader["preco"]);
                            byte[] foto = (reader["foto"] == DBNull.Value) ? null : (byte[])reader["foto"];

                            alimsEstabelecimento.Add(new Alimento(id, designacao, null, idEstabelecimento, preco, foto));
                        }
                    }
                }
                // Para obter os ingredientes de um alimento, aproveitando uma conexão aberta, o IngredienteDAO precisa
                // de poder associar exclusivamente um reader à conexão. Só neste ponto é que o reader inicial se encontra fechado.
                foreach (var alimento in alimsEstabelecimento)
                {
                    ISet<string> ings = ingredientes.ObterIngredientes(alimento.Id, sqlCon);
                    alimento.AdicionarIngredientes(ings);
                }
            }
            return alimsEstabelecimento;
        }

        public IList<Classificacao> ConsultarClassificacoesAlimentos(int clienteAutenticado)
        {
            return classificacoes.ConsultarClassificacoesAlimentos(clienteAutenticado);
        }
    }
}

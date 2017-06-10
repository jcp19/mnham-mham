using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace Mnham_Mnham
{
    class AlimentoDAO
    {
        private IngredienteDAO ingredientes;
        private ClassificacaoAlimentoDAO classificacoes;

        public AlimentoDAO()
        {
            classificacoes = new ClassificacaoAlimentoDAO();
            ingredientes = new IngredienteDAO();
        }

        public Alimento ObterAlimento(int idAlimento)
        {
            using (SqlConnection sqlCon = new SqlConnection(DAO.CONECTION_STRING))
            {
                Alimento a = null;
                SqlCommand cmd = new SqlCommand("SELECT * FROM Alimento WHERE id = @id AND removido = 0", sqlCon);

                cmd.Parameters.Add("@id", SqlDbType.Int);
                cmd.Parameters["@id"].Value = idAlimento;

                cmd.Connection.Open();

                SqlDataReader reader = cmd.ExecuteReader();
                bool encontrado = reader.Read();

                if (encontrado)
                {
                    string designacao = reader["designacao"].ToString();
                    float? preco = (reader["preco"] == null) ? null : (float?)Convert.ToDecimal(reader["preco"]);
                    byte[] foto = (reader["foto"] == null) ? null : (byte[])reader["foto"];

                    reader.Close();
                    ISet<string> ings = ingredientes.ObterIngredientes(idAlimento, sqlCon);

                    a = new Alimento(idAlimento, designacao, preco, ings, foto);
                    a.AdicionarClassificacoes(classificacoes.ClassificacaoAlimento(idAlimento, sqlCon));
                    a.ClassificacaoMedia = classificacoes.ObterClassificacaoMedia(idAlimento, sqlCon);
                }
                else
                    reader.Close();

                return a;
            }
        }

        // POR FAZER!!!
        public bool RemoverAlimento(int idAlimento)
        {
            throw new NotImplementedException();
        }

        // POR FAZER !!!!
        public bool RemoverIngredientesAlimento(int idAlimento, List<string> designacaoIngredientes)
        {
            throw new NotImplementedException();
        }


        // CONFIRMAR SE ESTA BEM!!! (INCOMPLETO)
        public bool AdicionarIngredientesAlimento(int idAlimento, List<string> designacoesIngredientes)
        {
            using (SqlConnection sqlCon = new SqlConnection(DAO.CONECTION_STRING))
            {
                bool adicionou = false;
                SqlCommand cmd = new SqlCommand("INSERT INTO IngredienteAlimento(id_alimento,id_ingrediente) VALUES (@id_a, @id_i);", sqlCon);
                SqlTransaction transaccao;

                cmd.Parameters.Add("@id_a", SqlDbType.Int);
                cmd.Parameters.Add("@id_i", SqlDbType.Int);

                sqlCon.Open();
                transaccao = sqlCon.BeginTransaction("AdicionarIngredientesAlimento");

                cmd.Connection = sqlCon;
                cmd.Transaction = transaccao;
                try
                {
                    foreach (string ingr in designacoesIngredientes)
                    {
                        int idIngrediente = ingredientes.AdicionarIngrediente(ingr, sqlCon);

                        cmd.ExecuteNonQuery();

                        cmd.Parameters["@id_a"].Value = idAlimento;
                        cmd.Parameters["@id_i"].Value = idIngrediente;
                    }
                    transaccao.Commit();
                    adicionou = true;
                }
                catch (Exception e)
                {
                    transaccao.Rollback();
                    throw e;
                }
                return adicionou;
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
            bool adicionou = false;

            using (SqlConnection sqlCon = new SqlConnection(DAO.CONECTION_STRING))
            {
                string txtCmd = @"INSERT INTO Alimento(designacao, preco, removido, id_estabelecimento, foto)
                                  VALUES(@desig, @preco, @removido, @id_estabelecimento, @foto);";

                SqlCommand cmd = new SqlCommand(txtCmd, sqlCon);
                cmd.Parameters.Add("@designacao", SqlDbType.NVarChar, 150);
                cmd.Parameters["@designacao"].Value = alim.Designacao;

                cmd.Parameters.Add("@preco", SqlDbType.Decimal, 10);
                cmd.Parameters["@preco"].Value = alim.Preco;
                cmd.Parameters["@preco"].Precision = 10;
                cmd.Parameters["@preco"].Scale = 2;

                cmd.Parameters.Add("@removido", SqlDbType.TinyInt);
                cmd.Parameters["@removido"].Value = 0;

                cmd.Parameters.Add("@id_estabelecimento", SqlDbType.Int);
                cmd.Parameters["@id_estabelecimento"].Value = idEstabelecimento;

                // POR O TIPO CERTO --> cmd.Parameters.Add("@foto", SqlDbType.Char, 19);
                cmd.Parameters["@foto"].Value = alim.Foto;

                cmd.Connection.Open();
                cmd.ExecuteNonQuery();
                // Só chegamos aqui se o alimento for inserido com sucesso.
                adicionou = true;
            }
            return adicionou;
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
            using (SqlConnection sqlCon = new SqlConnection(DAO.CONECTION_STRING))
            {
                IList<int> idsAlimentos = new List<int>();
                SqlCommand cmd = new SqlCommand("SELECT id from Alimento WHERE CHARINDEX(@v,designacao) > 0", sqlCon);

                cmd.Parameters.Add("@v", SqlDbType.NVarChar, 150);
                cmd.Parameters["@v"].Value = nomeAlimento;

                cmd.Connection.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    idsAlimentos.Add(Convert.ToInt32(reader["id"]));
                }
                reader.Close();

                IList<Alimento> alimentos = new List<Alimento>();
                ISet<string> ing = new HashSet<string>(); // Guarda os ingredientes de um alimento diferente em cada iteração.
                foreach (var id in idsAlimentos)
                {
                    ing = ingredientes.ObterIngredientes(id, sqlCon);

                    // Apenas se obtém o id e os ingredientes.
                    alimentos.Add(new Alimento(id, null, null, ing, null));
                    ing.Clear();
                }
                return alimentos;
            }
        }

        public IList<Classificacao> ConsultarClassificacoesAlimentos(int clienteAutenticado)
        {
            return classificacoes.ConsultarClassificacoesAlimentos(clienteAutenticado);
        }
    }
}
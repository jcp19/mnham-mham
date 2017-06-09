using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace Mnham_Mnham
{
    class AlimentoDAO
    {
        private ClassificacaoAlimentoDAO classificacoes;
        private IngredienteDAO ingredientes;

        public AlimentoDAO()
        {
            classificacoes = new ClassificacaoAlimentoDAO();
        }

        public Alimento ObterAlimento(int idAlimento)
        {
            using (SqlConnection sqlCon = new SqlConnection(DAO.CONECTION_STRING))
            {
                bool encontrado = false;
                Alimento a = null;
                SqlCommand cmd = new SqlCommand("SELECT * FROM Alimento WHERE id = @id AND removido = 0", sqlCon);

                cmd.Parameters.Add("@id", SqlDbType.Int);
                cmd.Parameters["@id"].Value = idAlimento;             

                cmd.Connection.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                 
                encontrado = reader.Read();

                if (encontrado)
                {
                    string designacao = reader["designacao"].ToString();
                    float preco = (float)Convert.ToDecimal(reader["preco"]);
                    byte[] foto = (byte[])reader["foto"];
                    reader.Close();

                    ISet<string> ings = ingredientes.ObterIngredientes(idAlimento, sqlCon);
                    
                    a = new Alimento(idAlimento, designacao, preco, ings, foto);
                    a.AdicionarClassificacoes(classificacoes.ClassificacaoAlimento(idAlimento,sqlCon));
                    a.ClassificacaoMedia = classificacoes.ObterClassificacaoMedia(idAlimento, sqlCon);
                }
                else
                {
                    reader.Close();
                }
                return a;
            }
        }

        // POR FAZER!!!
        internal bool RemoverAlimento(int idAlimento)
        {
            throw new NotImplementedException();
        }

        // POR FAZER !!!!
        internal bool RemoverIngredientesAlimento(int idAlimento, List<string> designacaoIngredientes)
        {
            throw new NotImplementedException();
        }


        // CONFIRMAR SE ESTA BEM!!! (INCOMPLETO)
        internal bool AdicionarIngredientesAlimento(int idAlimento, List<string> designacaoIngredientes)
        {
            using (SqlConnection sqlCon = new SqlConnection(DAO.CONECTION_STRING))
            {
                bool adicionou = true;

                foreach(string ingr in designacaoIngredientes)
                {
                    int idIngrediente = ingredientes.AdicionarIngrediente(ingr, sqlCon);

                    SqlCommand cmd = new SqlCommand("INSERT INTO IngredienteAlimento(id_alimento,id_ingrediente) VALUES (@id_a, @id_i);", sqlCon);
                    //cmd.Parameters.Add("@id_a", SqlDbType.Char, 1);
                    cmd.Parameters["@id_a"].Value = idAlimento;

                    //cmd.Parameters.Add("@id_i", SqlDbType.Char, 1);
                    cmd.Parameters["@id_i"].Value = idIngrediente;
                    
                    cmd.Connection.Open();

                    try
                    {
                        cmd.ExecuteNonQuery();
                    }
                    catch (SqlException)
                    {
                        // ha algum problema no acesso à BD
                        adicionou = false;
                    }
                    finally
                    {
                        cmd.Connection.Close();
                    }
                }

                return adicionou;
            }
        }

        // POR FAZER!!!
        internal bool EditarFotoAlimento(int idAlimento, byte[] foto)
        {
            throw new NotImplementedException();
        }

        // CONFIRMAR SE ESTA BEM!!! (INCOMPLETO)
        internal bool RegistarAlimento(int idEstabelecimento, Alimento alim)
        {
            using (SqlConnection sqlCon = new SqlConnection(DAO.CONECTION_STRING))
            {
                // ignora o id do cliente e cria um novo
                bool adicionou = true;

                SqlCommand cmd = new SqlCommand("INSERT INTO Alimento(designacao,preco,removido,id_estabelecimento,foto) VALUES (@desig, @preco, @rem, @id_e, @foto);", sqlCon);
                cmd.Parameters.Add("@designacao", SqlDbType.NVarChar, 150);
                cmd.Parameters["@designacao"].Value = alim.Designacao;

                //cmd.Parameters.Add("@preco", SqlDbType.NVarChar, 50);
                cmd.Parameters["@preco"].Value = alim.Preco;

                //cmd.Parameters.Add("@rem", SqlDbType.NVarChar, 75);
                cmd.Parameters["@rem"].Value = 0;

                //cmd.Parameters.Add("@id_e", SqlDbType.Char, 32);
                cmd.Parameters["@id_e"].Value = idEstabelecimento;

                //cmd.Parameters.Add("@foto", SqlDbType.Char, 19);
                cmd.Parameters["@foto"].Value = alim.Foto;

                cmd.Connection.Open();

                try
                {
                    cmd.ExecuteNonQuery();
                }
                catch (SqlException)
                {
                    // se ja existe um proprietario registado com o mesmo email ou 
                    // ha algum problema no acesso à BD
                    adicionou = false;
                }
                finally
                {
                    cmd.Connection.Close();
                }
                return adicionou;
            }
        }

        public bool ClassificarAlimento(int idAlimento, Classificacao cla)
        {
            return classificacoes.ClassificarAlimento(idAlimento, cla);
        }

        public void RemoverClassificacaoAlimento(int idAlimento, int clienteAutenticado)
        {
            classificacoes.RemoverClassificacaoAlimento(idAlimento, clienteAutenticado);
        }

        public IList<Alimento> ObterAlimentos(string nomeAlimento)
        {
            // construir lista de alimentos que na designação contenham nomeAlimento
            using (SqlConnection sqlCon = new SqlConnection(DAO.CONECTION_STRING))
            {
                IList<int> idsAlimentos = new List<int>();
                IList<Alimento> ret = new List<Alimento>();
                SqlCommand cmd = new SqlCommand("SELECT id from Alimento WHERE CHARINDEX(@v,designacao) > 0", sqlCon);

                cmd.Parameters.Add("@v", SqlDbType.NVarChar, 150);
                cmd.Parameters["@v"].Value = nomeAlimento;

                cmd.Connection.Open();
                var reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    idsAlimentos.Add(Convert.ToInt32(reader["id"]));
                }
                reader.Close();

                ISet<string> ing; // conjunto de ingredientes
                foreach (var i in idsAlimentos)
                {
                    ing = ingredientes.ObterIngredientes(i, sqlCon);
                    
                    // apenas se obtém id e ingredientes.
                    ret.Add(new Alimento(i, null, null, ing, null));
                    ing.Clear();
                }
                return ret;
            }
        }

        public IList<Classificacao> ConsultarClassificacoesAlimentos(int clienteAutenticado)
        {
            return classificacoes.ConsultarClassificacoesAlimentos(clienteAutenticado);
        }
    }
}
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace Mnham_Mnham
{
    class AlimentoDAO
    {
        ClassificacaoAlimentoDAO classificacoes;

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

                    SqlCommand cmd2 = new SqlCommand("SELECT Ingrediente.designacao FROM Ingrediente INNER JOIN IngredienteAlimento ON IngredienteAlimento.id_alimento = Ingrediente.id WHERE Ingrediente.id = @id", sqlCon);
                    cmd2.Parameters.Add("@id", SqlDbType.Int);
                    cmd2.Parameters["@id"].Value = idAlimento;

                    SqlDataReader reader2 = cmd2.ExecuteReader();
                    ISet<string> ingredientes = new HashSet<string>();

                    while (reader2.Read())
                    {
                        ingredientes.Add(reader2["designacao"].ToString());
                    }
                    reader2.Close();

                    a = new Alimento(idAlimento, designacao, preco, ingredientes, foto);
                    a.Classificacoes = classificacoes.ClassificacaoAlimento(idAlimento,sqlCon);
                    a.ClassificacaoMedia = classificacoes.ObterClassificacaoMedia(idAlimento, sqlCon);
                }
                else
                {
                    reader.Close();
                }
                return a;
            }
        }

        internal bool RemoverAlimento(int idAlimento)
        {
            throw new NotImplementedException();
        }

        internal bool RemoverIngredientesAlimento(int idAlimento, List<string> designacaoIngredientes)
        {
            throw new NotImplementedException();
        }

        internal bool AdicionarIngredientesAlimento(int idAlimento, List<string> designacaoIngredientes)
        {
            throw new NotImplementedException();
        }

        internal bool EditarFotoAlimento(int idAlimento, byte[] foto)
        {
            throw new NotImplementedException();
        }

        internal bool RegistarAlimento(int idEstabelecimento, Alimento alim)
        {
            throw new NotImplementedException();
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

                ISet<string> ing = new HashSet<string>(); // conjunto de ingredientes
                foreach (var i in idsAlimentos)
                {
                    SqlCommand cmd2 = new SqlCommand("SELECT Ingrediente.designacao FROM Ingrediente INNER JOIN IngredienteAlimento ON IngredienteAlimento.id_alimento = Ingrediente.id WHERE Ingrediente.id = @id", sqlCon);
                    cmd2.Parameters.Add("@id", SqlDbType.Int);
                    cmd2.Parameters["@id"].Value = i;

                    var reader2 = cmd2.ExecuteReader();

                    while (reader2.Read())
                    {
                        ing.Add(reader2["designacao"].ToString());
                    }
                    reader2.Close();

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
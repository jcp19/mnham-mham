using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System.Data;
using System.Data.SqlClient;

namespace Mnham_Mnham
{
    class AlimentoDAO
    {
        ClassificacaoAlimentoDAO classificacoes;
        /*
                public List<Alimento> ObterIngredientesAlimentos(int idEstabelecimento, string v)
                {
                    // construir lista de alimentos que na designação contenham v
                    SqlCommand cmd = new SqlCommand("Select id from Alimento WHERE id_estabelecimento = @id_est AND CHARINDEX(@v,designacao) > 0", base.sqlCon);
                    cmd.Parameters["@v"].Value = v;
                    cmd.Parameters["@id_est"].Value = idEstabelecimento;
                    var reader = cmd.ExecuteReader();
                    List<Alimento> ret = new List<Alimento>();

                    List<int> idsAlimentos = new List<int>();

                    while (reader.Read())
                    {
                        idsAlimentos.Add(Convert.ToInt32(reader["id"]));
                    }

                    foreach(var i in idsAlimentos)
                    {
                        SqlCommand cmd2 = new SqlCommand("select Ingrediente.designacao FROM Ingrediente INNER JOIN IngredienteAlimento ON IngredienteAlimento.id_alimento = Ingrediente.id WHERE Ingrediente.id = @id", base.sqlCon);
                        cmd2.Parameters["@id"].Value = i;
                        var reader2 = cmd2.ExecuteReader();
                        ISet<string> ing = new HashSet<string>();

                        while (reader2.Read())
                        {
                            ing.Add(reader2["designacao"].ToString());
                        }

                        // apenas se obtem id e ingredientes
                        ret.Add(new Alimento(i, null, null, ing, null));
                    }

                    return ret;
                }
                */

        public AlimentoDAO()
        {
            classificacoes = new ClassificacaoAlimentoDAO();
        }

        public Alimento ObterAlimento(int idAlimento)
        {
            using (SqlConnection sqlCon = new SqlConnection(DAO.CONECTION_STRING))
            {
                SqlCommand cmd = new SqlCommand("SELECT * FROM Alimento WHERE id = @id AND removido = 0", sqlCon);
                cmd.Parameters.Add("@id", SqlDbType.Int);
                cmd.Parameters["@id"].Value = idAlimento;             
                Alimento a = null;
                bool contains = false;

                cmd.Connection.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                 
                contains = reader.Read();

                if (contains)
                {
                    string designacao = reader["designacao"].ToString();
                    float preco = (float)Convert.ToDecimal(reader["preco"]);
                    byte[] foto = (byte[])reader["foto"];
                    reader.Close();

                    SqlCommand cmd2 = new SqlCommand("select Ingrediente.designacao FROM Ingrediente INNER JOIN IngredienteAlimento ON IngredienteAlimento.id_alimento = Ingrediente.id WHERE Ingrediente.id = @id", sqlCon);
                    cmd2.Parameters.Add("@id", SqlDbType.Int);
                    cmd2.Parameters["@id"].Value = idAlimento;

                    SqlDataReader reader2 = cmd2.ExecuteReader();
                    ISet<String> ingredientes = new HashSet<String>();

                    while (reader2.Read())
                    {
                        ingredientes.Add(reader2["designacao"].ToString());
                    }

                    reader2.Close();

                    a = new Alimento(idAlimento, designacao, preco, ingredientes, foto);
                }
                else
                {
                    reader.Close();
                }

                return a;
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

        public List<Alimento> ObterAlimentos(string nomeAlimento)
        {
            // construir lista de alimentos que na designação contenham nomeAlimento
            using (SqlConnection sqlCon = new SqlConnection(DAO.CONECTION_STRING))
            {
                SqlCommand cmd = new SqlCommand("Select id from Alimento WHERE CHARINDEX(@v,designacao) > 0", sqlCon);
                cmd.Parameters.Add("@v", SqlDbType.NVarChar, 150);
                cmd.Parameters["@v"].Value = nomeAlimento;

                cmd.Connection.Open();
                var reader = cmd.ExecuteReader();

                List<Alimento> ret = new List<Alimento>();

                List<int> idsAlimentos = new List<int>();

                while (reader.Read())
                {
                    idsAlimentos.Add(Convert.ToInt32(reader["id"]));
                }
                reader.Close();

                foreach (var i in idsAlimentos)
                {
                    SqlCommand cmd2 = new SqlCommand("select Ingrediente.designacao FROM Ingrediente INNER JOIN IngredienteAlimento ON IngredienteAlimento.id_alimento = Ingrediente.id WHERE Ingrediente.id = @id", sqlCon);
                    cmd2.Parameters.Add("@id", SqlDbType.Int);
                    cmd2.Parameters["@id"].Value = i;

                    var reader2 = cmd2.ExecuteReader();
                    ISet<string> ing = new HashSet<string>();

                    while (reader2.Read())
                    {
                        ing.Add(reader2["designacao"].ToString());
                    }
                    reader2.Close();

                    // apenas se obtem id e ingredientes
                    ret.Add(new Alimento(i, null, null, ing, null));
                }

                return ret;
            }
        }

        public List<Classificacao> ConsultarClassificacoesAlimentos(int clienteAutenticado)
        {
            return classificacoes.ConsultarClassificacoesAlimentos(clienteAutenticado);
        }
    }
}
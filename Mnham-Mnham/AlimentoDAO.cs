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
    class AlimentoDAO : DAO
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

        public AlimentoDAO() : base()
        {
            classificacoes = new ClassificacaoAlimentoDAO();
        }

        public AlimentoDAO(string connectionString) : base(connectionString)
        {
            classificacoes = new ClassificacaoAlimentoDAO(connectionString);
        }

        public Alimento ObterAlimento(int idAlimento)
        {
            SqlCommand cmd = new SqlCommand("SELECT * FROM Alimento WHERE id = @id AND removido = 0", base.sqlCon);
            cmd.Parameters.Add("@id", SqlDbType.Int);
            cmd.Parameters["@id"].Value = idAlimento;
            Alimento a = null;

            SqlDataReader reader = cmd.ExecuteReader();
            bool contains = false;

            try
            {
                contains = reader.Read();
            }
            finally
            {
                reader.Close();
            }

            if(contains)
            {
                SqlCommand cmd2 = new SqlCommand("select Ingrediente.designacao FROM Ingrediente INNER JOIN IngredienteAlimento ON IngredienteAlimento.id_alimento = Ingrediente.id WHERE Ingrediente.id = @id", base.sqlCon);
                cmd2.Parameters.Add("@id", SqlDbType.Int);
                cmd2.Parameters["@id"].Value = idAlimento;
                SqlDataReader reader2 = cmd2.ExecuteReader();
                ISet<String> ingredientes = new HashSet<String>();

                while (reader2.Read())
                {
                    ingredientes.Add(reader2["designacao"].ToString());
                }

                a = new Alimento(idAlimento, reader["designacao"].ToString(), (float) Convert.ToDecimal(reader["preco"]), ingredientes, (byte[]) reader["foto"]);
            }

            return a;
        }

        public bool ClassificarAlimento(int idAlimento, Classificacao cla)
        {
            return classificacoes.ClassificarAlimento(idAlimento, cla);
        }

        public bool RemoverClassificacaoAlimento(int idAlimento, int clienteAutenticado)
        {
            return classificacoes.RemoverClassificacaoAlimento(idAlimento, clienteAutenticado);
        }

        public List<Alimento> ObterAlimentos(string nomeAlimento)
        {
            // construir lista de alimentos que na designação contenham nomeAlimento
            SqlCommand cmd = new SqlCommand("Select id from Alimento WHERE CHARINDEX(@v,designacao) > 0", base.sqlCon);
            cmd.Parameters.Add("@v", SqlDbType.NVarChar, 150);
            cmd.Parameters["@v"].Value = nomeAlimento;

            var reader = cmd.ExecuteReader();

            List<Alimento> ret = new List<Alimento>();

            List<int> idsAlimentos = new List<int>();

            while (reader.Read())
            {
                idsAlimentos.Add(Convert.ToInt32(reader["id"]));
            }

            foreach (var i in idsAlimentos)
            {
                SqlCommand cmd2 = new SqlCommand("select Ingrediente.designacao FROM Ingrediente INNER JOIN IngredienteAlimento ON IngredienteAlimento.id_alimento = Ingrediente.id WHERE Ingrediente.id = @id", base.sqlCon);
                cmd2.Parameters.Add("@id", SqlDbType.Int);
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

        public List<Classificacao> ConsultarClassificacoesAlimentos(int clienteAutenticado)
        {
            return classificacoes.ConsultarClassificacoesAlimentos(clienteAutenticado);
        }
    }
}
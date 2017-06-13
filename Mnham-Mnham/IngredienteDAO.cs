using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace Mnham_Mnham
{
    public class IngredienteDAO
    {
        public ISet<string> ObterIngredientes(int idAlimento, SqlConnection sqlCon)
        {
            ISet<string> ingredientes = new HashSet<string>();
            string txtCmd = @"
                SELECT Ingrediente.designacao
                FROM Ingrediente INNER JOIN IngredienteAlimento ON IngredienteAlimento.id_alimento = Ingrediente.id
                WHERE Ingrediente.id = @id";

            using (var cmd = new SqlCommand(txtCmd, sqlCon))
            {
                cmd.Parameters.Add("@id", SqlDbType.Int);
                cmd.Parameters["@id"].Value = idAlimento;

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        ingredientes.Add(reader["designacao"].ToString());
                    }
                }
            }
            return ingredientes;
        }

        public int AdicionarIngrediente(string ingr, SqlConnection sqlCon)
        {
            string txtCmdVerifExist = "SELECT id FROM Ingrediente WHERE designacao = @desig";

            using (var cmdVerifExist = new SqlCommand(txtCmdVerifExist, sqlCon))
            {
                cmdVerifExist.Parameters.Add("@desig", SqlDbType.NVarChar, 75);
                cmdVerifExist.Parameters["@desig"].Value = ingr;

                using (var reader = cmdVerifExist.ExecuteReader())
                {
                    if (reader.Read()) // O ingrediente com a designação indicada já está registado.
                    {
                        return Convert.ToInt32(reader["id"]);
                    }
                }
            }
            string txtCmdIns = @"INSERT INTO Ingrediente(designacao) output INSERT.ID
                                 VALUES (@desig);";

            using (var cmdIns = new SqlCommand(txtCmdIns, sqlCon))
            {
                cmdIns.Parameters.Add("@desig", SqlDbType.NVarChar, 75);
                cmdIns.Parameters["@desig"].Value = ingr;

                return (int)cmdIns.ExecuteScalar();
            }
        }

        // Usa uma conexão já aberta e não a fecha.
        public IList<int> ObterIds(IList<string> designacoesIngrs, SqlConnection sqlCon)
        {
            IList<int> idsIngrs = new List<int>(designacoesIngrs.Count);

            using (var cmd = new SqlCommand())
            {
                string formatoCmd = "SELECT id FROM Ingrediente WHERE designacao IN ({0})";
                string[] parametros = new string[designacoesIngrs.Count];

                int i = 0;
                foreach (var desig in designacoesIngrs)
                {
                    string nomeParam = "@p" + i;
                    cmd.Parameters.Add(nomeParam, SqlDbType.NVarChar, 75);
                    cmd.Parameters[nomeParam].Value = desig;
                    parametros[i++] = nomeParam;
                }
                cmd.CommandText = string.Format(formatoCmd, string.Join(",", parametros));
                cmd.Connection = sqlCon;

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        idsIngrs.Add(Convert.ToInt32(reader["id"]));
                    }
                }
            }
            return idsIngrs;
        }
    }
}

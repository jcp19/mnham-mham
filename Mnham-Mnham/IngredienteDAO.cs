using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace Mnham_Mnham
{
    internal class IngredienteDAO
    {
        // CONFIRMAR SE ESTA BEM!!
        internal ISet<string> ObterIngredientes(int idAlimento, SqlConnection sqlCon)
        {
            ISet<string> ingredientes = new HashSet<string>();

            SqlCommand cmd2 = new SqlCommand("SELECT Ingrediente.designacao FROM Ingrediente INNER JOIN IngredienteAlimento ON IngredienteAlimento.id_alimento = Ingrediente.id WHERE Ingrediente.id = @id", sqlCon);
            cmd2.Parameters.Add("@id", SqlDbType.Int);
            cmd2.Parameters["@id"].Value = idAlimento;

            cmd2.Connection.Open();
            SqlDataReader reader2 = cmd2.ExecuteReader();

            while (reader2.Read())
            {
                ingredientes.Add(reader2["designacao"].ToString());
            }
            reader2.Close();

            return ingredientes;
        }


        // NAO ESTA COMPLETO!! 
        // deve retornar o id do ingrediente (se ja existir retorna o que existe, senao cria um novo)
        internal int AdicionarIngrediente(string ingr, SqlConnection sqlCon)
        {
            SqlCommand cmd2 = new SqlCommand("SELECT id FROM Ingrediente WHERE designacao == @desig", sqlCon);
            //cmd2.Parameters.Add("@desig", SqlDbType.Int);
            cmd2.Parameters["@desig"].Value = ingr;

            cmd2.Connection.Open();
            SqlDataReader reader = cmd2.ExecuteReader();

            if (reader.Read())
            {
                // COMPLETAR!!
                return 0;
            }

            SqlCommand cmd3 = new SqlCommand("INSERT INTO Ingrediente(designacao) VALUES (@desig);", sqlCon);
            //cmd3.Parameters.Add("@desig", SqlDbType.Int);
            cmd3.Parameters["@desig"].Value = ingr;

            cmd3.Connection.Open();


            return 0;
        }
    }
}
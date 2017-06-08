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

namespace Mnham_Mnham
{
    class AlimentoDAO : DAO
    {
        ClassificacaoAlimentoDAO classificacoes;

        internal List<Alimento> ObterIngredientesAlimentos(int idEstabelecimento, string v)
        {
            // construir lista de alimentos que na designação contenham v
            // apenas se obtem id e ingredientes
            throw new NotImplementedException();
        }

        internal Alimento ObterAlimento(int idAlimento)
        {
            throw new NotImplementedException();
        }

        internal void ClassificarAlimento(int idAlimento, Classificacao cla)
        {
            classificacoes.ClassificarAlimento(idAlimento, idEstabelecimento, cla);
        }

        internal void RemoverClassificacaoAlimento(int idAlimento, int clienteAutenticado)
        {
            classificacoes.RemoverClassificacaoAlimento(idAlimento, idEstabelecimento, clienteAutenticado);
        }


    }
}
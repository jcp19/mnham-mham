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

        internal bool ClassificarAlimento(int idAlimento, Classificacao cla)
        {
            return classificacoes.ClassificarAlimento(idAlimento, cla);
        }

        internal bool RemoverClassificacaoAlimento(int idAlimento, int clienteAutenticado)
        {
            return classificacoes.RemoverClassificacaoAlimento(idAlimento, clienteAutenticado);
        }

        internal IEnumerable<Alimento> ObterAlimentos(string nomeAlimento)
        {
            // cada alimento apenas tem o id e os seus ingredientes
            // obter alimentos que contenham 'nomeAlimento'
            throw new NotImplementedException();
        }

        internal List<Classificacao> ConsultarClassificacoesAlimentos(int clienteAutenticado)
        {
            return classificacoes.ConsultarClassificacoesAlimentos(clienteAutenticado);
        }
    }
}
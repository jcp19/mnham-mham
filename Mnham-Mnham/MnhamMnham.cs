using System;
using System.Collections.Generic;

namespace Mnham_Mnham
{
    public class MnhamMnham
    {
        private int clienteAutenticado;
        private bool utilizadorEProprietario;

        public MnhamMnham()
        {
            estabelecimentos = new EstabelecimentoDAO();
            clientes = new ClienteDAO();
            pedidos = new PedidoDAO();
        }

        public bool IniciarSessaoCliente(string email, string palavraPasse)
        {
            Cliente cliente = clientes.ObterPorEmail(email);
            if (cliente != null)
            {
                if (palavraPasse.Equals(cliente.PalavraPasse))
                {
                    this.clienteAutenticado = cliente.Id;

                    return true;
                }
            }
            return false;
        }

        public void IniciarSessaoProprietario(string email, string palavraPasse)
        {
            //
        }

        public bool RegistarCliente(Cliente cliente)
        {
            string email = cliente.Email;
            if (clientes.ContemEmail(email))
            {
                return false;
            }
            else
            {
                clientes.AdicionarCliente(cliente);
            }
            return true;
        }

        public void RegistarProprietario()
        {
            throw new System.Exception("Not implemented");
        }

        public List<AlimentoEstabelecimento> EfetuarPedido(ref string termo)
        {
            RegistaPedidoHistorico(ref termo);
            PedidoProcessado pedidoProcessado = new PedidoProcessado(termo);
            Cliente cliente;

            List<string> preferencias;
            List<string> naoPreferencias;

            if (clienteAutenticado != 0)
            {
                // cliente
                cliente = clientes.ObterPorId(clienteAutenticado);
                preferencias = cliente.ObterPreferencias(pedidoProcessado.ObterNomeAlimento());
                List<string> preferenciasPedido = pedidoProcessado.ObterPreferencias();
                preferencias.AddRange(preferenciasPedido);

                naoPreferencias = cliente.ObterNaoPreferencias(pedidoProcessado.ObterNomeAlimento());
                List<string> naoPreferenciasPedido = pedidoProcessado.ObterNaoPreferencias();
                naoPreferencias.AddRange(naoPreferenciasPedido);
            }
            else
            {
                // utilizador não autenticado
                preferencias = pedidoProcessado.ObterPreferencias();
                naoPreferencias = pedidoProcessado.ObterNaoPreferencias();
            }

            // Obter localização !!

            List<AlimentoEstabelecimento> listaAEs = new List<AlimentoEstabelecimento>();
            foreach (int idEstabelecimento in estabelecimentos.ObterIdsEstabelecimento())
            {
                List<Alimento> alimentos = estabelecimentos.ObterIngredientesAlimentos(idEstabelecimento, pedidoProcessado.ObterNomeAlimento());
                foreach (Alimento a in alimentos)
                {
                    if (a.ContemNaoPreferencias(naoPreferencias) == false)
                    {
                        int nPreferencias = a.QuantasPreferenciasContem(preferencias);
                        Alimento alim = estabelecimentos.ObterAlimento(a.Id);
                        Estabelecimento e = estabelecimentos.ObterEstabelecimento(idEstabelecimento);
                        AlimentoEstabelecimento ae = new AlimentoEstabelecimento(nPreferencias, e, alim);
                        listaAEs.Add(ae);
                    }
                }
            }

            listaAEs.Sort();

            return listaAEs;
        }

        private void RegistaPedidoHistorico(ref string termo)
        {
            if (clienteAutenticado != 0)
            {
                //cliente
                Pedido pedido = new Pedido(termo, clienteAutenticado);
                pedidos.AdicionarPedido(pedido);
            }
            else
            {
                // utilizador não autenticado
                // Como guardar??
            }
        }

        /*public void RegistarPreferenciaGeral(ref String designacaoPreferencia)
        {
            Preferencia preferencia = new Preferencia(designacaoPreferencia);
            clientes.AdicionarPreferencia(clienteAutenticado, preferencia);
        }

        public void RegistarPreferenciaAlimento(ref string designacaoPreferencia, ref string designacaoAlimento)
        {
            Preferencia preferencia = new Preferencia(designacaoPreferencia, designacaoAlimento);
            clientes.AdicionarPreferencia(clienteAutenticado, preferencia);
        }

        public void RegistarNaoPreferenciaGeral(ref String designacaoNaoPreferencia)
        {
            Preferencia naoPreferencia = new Preferencia(designacaoNaoPreferencia);
            clientes.AdicionarNaoPreferencia(clienteAutenticado, naoPreferencia);
        }

        public void RegistarNaoPreferenciaAlimento(ref string designacaoNaoPreferencia, ref string designacaoAlimento)
        {
            Preferencia naoPreferencia = new Preferencia(designacaoNaoPreferencia, designacaoAlimento);
            clientes.AdicionarNaoPreferencia(clienteAutenticado, naoPreferencia);
        }*/

        public void RegistarPreferencia(ref Preferencia pref)
        {
            clientes.AdicionarPreferencia(clienteAutenticado, pref);
        }

        public void RegistarNaoPreferencia(ref Preferencia pref)
        {
            clientes.AdicionarNaoPreferencia(clienteAutenticado, pref);
        }

        public AlimentoEstabelecimento ConsultarAlimento(ref int idAlimento)
        {
            throw new System.Exception("Not implemented");
        }
        public Estabelecimento ConsultarEstabelecimento(ref int idEstabelecimento)
        {
            throw new System.Exception("Not implemented");
        }

        public List<Pedido> ConsultarHistorico()
        {
            return pedidos.ObterPedidos(clienteAutenticado);
        }

        public void ClassificarAlimento(ref int idAlimento, ref int classificacao)
        {
            Classificacao cla = new Classificacao(classificacao, clienteAutenticado);
            estabelecimentos.ClassificarAlimento(idAlimento, cla);
        }

        public void ClassificarAlimento(ref int idAlimento, ref int classificacao, ref string comentario)
        {
            Classificacao cla = new Classificacao(classificacao, comentario, clienteAutenticado);
            estabelecimentos.ClassificarAlimento(idAlimento, cla);
        }

        public int RegistarAlimento(ref int idEstabelecimento, ref string nomeAlimento, ref float preco)
        {
            throw new System.Exception("Not implemented");
        }
        public void AssociarFotoAlimento(ref int idEstabelecimento, ref int idAlimento, ref byte[] photo)
        {
            throw new System.Exception("Not implemented");
        }
        public void AssociarIngredienteAlimento(ref int idEstabelecimento, ref int idAlimento, ref string designacaoIngrediente)
        {
            throw new System.Exception("Not implemented");
        }

        public void RemoverClassificacaoEstabelecimento(ref int idEstabelecimento)
        {
            estabelecimentos.RemoverClassificacaoAlimento(idEstabelecimento, clienteAutenticado);
        }
    
        public void RemoverAlimento(ref int idEstabelecimento, ref int idAlimento)
        {
            throw new System.Exception("Not implemented");
        }

        public void RemoverPreferencia(ref string designacaoPreferencia, ref string designacaoAlimento)
        {
            Preferencia preferencia = new Preferencia(designacaoPreferencia, designacaoAlimento);
            clientes.RemoverPreferencia(clienteAutenticado, preferencia);
        }

        public void RemoverNaoPreferencia(ref string designacaoNaoPreferencia, ref string designacaoAlimento)
        {
            Preferencia naoPreferencia = new Preferencia(designacaoNaoPreferencia, designacaoAlimento);
            clientes.RemoverNaoPreferencia(clienteAutenticado, naoPreferencia);
        }

        public List<String> ObterTendencias()
        {
            throw new System.Exception("Not implemented");
        }

        public void ClassificarEstabelecimento(ref int idEstabelecimento, ref int classificacao)
        {
            Classificacao cla = new Classificacao(classificacao, clienteAutenticado);
            estabelecimentos.ClassificarEstabelecimento(idEstabelecimento, cla);
        }

        public void ClassificarEstabelecimento(ref int idEstabelecimento, ref int classificacao, ref string comentario)
        {
            Classificacao cla = new Classificacao(classificacao, comentario, clienteAutenticado);
            estabelecimentos.ClassificarEstabelecimento(idEstabelecimento, cla);
        }

        public void RemoverClassificacaoAlimento(ref int idAlimento)
        {
            estabelecimentos.RemoverClassificacaoAlimento(idAlimento, clienteAutenticado);
        }

        /* mudar para DAOs */
        private Dictionary<int, Proprietario> proprietarios;
        //private Dictionary<int, Estabelecimento> estabelecimentos;
        //private Dictionary<int, List<Pedido>> pedidos;
        //private Dictionary<string, Cliente> clientes;

        private ClienteDAO clientes;
        private EstabelecimentoDAO estabelecimentos;
        private PedidoDAO pedidos;
    }
}
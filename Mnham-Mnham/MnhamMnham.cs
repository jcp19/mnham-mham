using Android.Locations;
using System;
using System.Collections.Generic;

namespace Mnham_Mnham
{
    public class MnhamMnham
    {
        // Dados do utilizador autenticado
        private int clienteAutenticado;
        private bool utilizadorEProprietario;
        // DAOs
        private ProprietarioDAO proprietarios;
        private ClienteDAO clientes;
        private EstabelecimentoDAO estabelecimentos;
        private PedidoDAO pedidos;

        public MnhamMnham()
        {
            estabelecimentos = new EstabelecimentoDAO();
            clientes = new ClienteDAO();
            pedidos = new PedidoDAO();
            proprietarios = new ProprietarioDAO();
        }

        //=====================================================================
        // REGISTO E AUTENTICAÇÃO
        //=====================================================================

        public int IniciarSessao(string email, string palavraPasse)
        {
            Cliente cliente = clientes.ObterPorEmail(email);

            if (cliente != null)
            {
                if (palavraPasse.Equals(cliente.PalavraPasse))
                {
                    this.clienteAutenticado = cliente.Id;
                    this.utilizadorEProprietario = false;

                    return 1;
                }
            }
            /*Proprietario proprietario = proprietarios.ObterPorEmail(email);
            if (proprietario != null)
            {
                if (palavraPasse.Equals(proprietario.PalavraPasse))
                {
                    this.clienteAutenticado = proprietario.Id;
                    this.utilizadorEProprietario = true;

                    return 2;
                }
            }*/
            return 0;
        }

        // Clientes
        public bool IniciarSessaoCliente(string email, string palavraPasse)
        {
            Cliente cliente = clientes.ObterPorEmail(email);

            if (cliente != null)
            {
                if (palavraPasse.Equals(cliente.PalavraPasse))
                {
                    this.clienteAutenticado = cliente.Id;
                    this.utilizadorEProprietario = false;

                    return true;
                }
            }
            return false;
        }

        public bool RegistarCliente(Cliente cliente)
        {
            // Retorna falso se email já existe.
            return clientes.AdicionarCliente(cliente);
        }

        // Proprietários
        public bool IniciarSessaoProprietario(string email, string palavraPasse)
        {
            Proprietario proprietario = proprietarios.ObterPorEmail(email);
            if (proprietario != null)
            {
                if (palavraPasse.Equals(proprietario.PalavraPasse))
                {
                    this.clienteAutenticado = proprietario.Id;
                    this.utilizadorEProprietario = true;

                    return true;
                }
            }
            return false;
        }

        public bool RegistarProprietario(Proprietario proprietario)
        {
            // Retorna falso se email já existe
            return proprietarios.AdicionarProprietario(proprietario);
        }

        //=====================================================================
        // GESTÃO DE CONTA
        //=====================================================================

        public void TerminarSessao()
        {
            this.clienteAutenticado = 0;
        }

        // Clientes

        public void EditarDados(Cliente cliente)
        {
            cliente.Id = this.clienteAutenticado;
            clientes.EditarDados(cliente);
        }

        public Cliente ConsultarDadosCliente()
        {
            return clientes.ObterPorId(clienteAutenticado);
        }

        // Proprietarios

        public void EditarDados(Proprietario proprietario)
        {
            proprietario.Id = this.clienteAutenticado;
            proprietarios.EditarDados(proprietario);
        }

        public Proprietario ConsultarDadosProprietario()
        {
            return proprietarios.ObterPorId(clienteAutenticado);
        }

        //=====================================================================
        // PREFERÊNCIAS E NÃO PREFERÊNCIAS
        //=====================================================================

        public void RegistarPreferencia(Preferencia pref)
        {
            clientes.AdicionarPreferencia(clienteAutenticado, pref);
        }

        public void RegistarNaoPreferencia(Preferencia pref)
        {
            clientes.AdicionarNaoPreferencia(clienteAutenticado, pref);
        }

        public IList<Preferencia> ConsultarPreferencias()
        {
            return clientes.ConsultarPreferencias(clienteAutenticado);
        }

        public IList<Preferencia> ConsultarNaoPreferencias()
        {
            return clientes.ConsultarNaoPreferencias(clienteAutenticado);
        }

        public void RemoverPreferencia(string designacaoPreferencia, string designacaoAlimento)
        {
            Preferencia preferencia = new Preferencia(designacaoPreferencia, designacaoAlimento);
            clientes.RemoverPreferencia(clienteAutenticado, preferencia);
        }

        public void RemoverNaoPreferencia(string designacaoNaoPreferencia, string designacaoAlimento)
        {
            Preferencia naoPreferencia = new Preferencia(designacaoNaoPreferencia, designacaoAlimento);
            clientes.RemoverNaoPreferencia(clienteAutenticado, naoPreferencia);
        }

        //=====================================================================
        // PEDIDOS E CONSULTAS
        //=====================================================================

        public List<AlimentoEstabelecimento> EfetuarPedido(string termo, Location localizacao)
        {
            RegistaPedidoHistorico(termo);

            PedidoProcessado pedidoProcessado = new PedidoProcessado(termo);
            ISet<string> preferencias = new HashSet<string>();
            ISet<string> naoPreferencias = new HashSet<string>();

            //-----------------------------------------------------
            // Obtenção das Preferências e Não Preferências
            //-----------------------------------------------------
            if (clienteAutenticado != 0)
            {
                // cliente
                Cliente cliente = clientes.ObterPorId(clienteAutenticado);

                preferencias = cliente.ObterPreferencias(pedidoProcessado.NomeAlimento);
                ISet<string> preferenciasPedido = pedidoProcessado.Preferencias;
                preferencias.UnionWith(preferenciasPedido);

                naoPreferencias = cliente.ObterNaoPreferencias(pedidoProcessado.NomeAlimento);
                ISet<string> naoPreferenciasPedido = pedidoProcessado.NaoPreferencias;
                naoPreferencias.UnionWith(naoPreferenciasPedido);
            }
            else
            {
                // utilizador não autenticado
                preferencias = pedidoProcessado.Preferencias;
                naoPreferencias = pedidoProcessado.NaoPreferencias;
            }

            //----------------------------------------------------------------------------------------------------
            // Obtenção dos Estabelecimentos e respetivos Alimentos que não contêm não preferências do utilizador
            //----------------------------------------------------------------------------------------------------
            List<AlimentoEstabelecimento> listaAEs = new List<AlimentoEstabelecimento>();
            IDictionary<int, Estabelecimento> estabsObtidos = new Dictionary<int, Estabelecimento>();
            foreach (Alimento a in estabelecimentos.ObterAlimentos(pedidoProcessado.NomeAlimento))
            {
                if (a.ContemNaoPreferencias(naoPreferencias) == false)
                {
                    Alimento alim = estabelecimentos.ObterAlimento(a.Id);
                    int nPreferencias = a.QuantasPreferenciasContem(preferencias);

                    int idEstab = alim.IdEstabelecimento;
                    Estabelecimento estab;

                    if (estabsObtidos.TryGetValue(idEstab, out estab) == false)
                    {
                        estab = estabelecimentos.ObterEstabelecimento(idEstab);
                        estabsObtidos.Add(idEstab, estab);
                    }
                    float distancia = localizacao.DistanceTo(estab.Coords);
                    AlimentoEstabelecimento ae = new AlimentoEstabelecimento(nPreferencias, distancia, estab, alim);
                    listaAEs.Add(ae);
                }
            }
            listaAEs.Sort();

            return listaAEs;
        }

        private void RegistaPedidoHistorico(string termo)
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

        public AlimentoEstabelecimento ConsultarAlimento(int idAlimento)
        {
            Alimento a = estabelecimentos.ObterAlimento(idAlimento);
            Estabelecimento e = estabelecimentos.ObterEstabelecimento(a.IdEstabelecimento);

            return new AlimentoEstabelecimento(e, a);
        }

        public Estabelecimento ConsultarEstabelecimento(int idEstabelecimento)
        {
            return estabelecimentos.ObterEstabelecimento(idEstabelecimento);
        }

        public IList<Pedido> ConsultarHistorico()
        {
            return pedidos.ObterPedidos(clienteAutenticado);
        }

        public SortedSet<Tendencia> ObterTendencias()
        {
            Dictionary<string, Tendencia> aux = new Dictionary<string, Tendencia>();
            foreach (string s in pedidos.ObterPedidosUltimaSemana())
            {
                Tendencia t;
                if (aux.TryGetValue(s, out t))
                {
                    t.inc();
                }
                else
                {
                    t = new Tendencia(s);
                    aux.Add(s, t);
                }
            }
            SortedSet<Tendencia> tendencias = new SortedSet<Tendencia>();
            foreach (Tendencia t in aux.Values)
            {
                tendencias.Add(t);
            }
            //Alterar para retornar as 5 mais frequentes!!
            return tendencias;
        }

        //=====================================================================
        // CLASSIFICAÇÕES E PARTILHA
        //=====================================================================

        public void ClassificarAlimento(int idAlimento, int classificacao)
        {
            Classificacao cla = new Classificacao(classificacao, clienteAutenticado);
            estabelecimentos.ClassificarAlimento(idAlimento, cla);
        }

        public void ClassificarAlimento(int idAlimento, int classificacao, string comentario)
        {
            Classificacao cla = new Classificacao(classificacao, comentario, clienteAutenticado);
            estabelecimentos.ClassificarAlimento(idAlimento, cla);
        }

        public void ClassificarEstabelecimento(int idEstabelecimento, int classificacao)
        {
            Classificacao cla = new Classificacao(classificacao, clienteAutenticado);
            estabelecimentos.ClassificarEstabelecimento(idEstabelecimento, cla);
        }

        public void ClassificarEstabelecimento(int idEstabelecimento, int classificacao, string comentario)
        {
            Classificacao cla = new Classificacao(classificacao, comentario, clienteAutenticado);
            estabelecimentos.ClassificarEstabelecimento(idEstabelecimento, cla);
        }

        public IList<Classificacao> ConsultarClassificacoesAlimentos()
        {
            return estabelecimentos.ConsultarClassificacoesAlimentos(clienteAutenticado);
        }

        public IList<Classificacao> ConsultarClassificacoesEstabelecimentos()
        {
            return estabelecimentos.ConsultarClassificacoesEstabelecimentos(clienteAutenticado);
        }

        public void RemoverClassificacaoAlimento(int idAlimento)
        {
            estabelecimentos.RemoverClassificacaoAlimento(idAlimento, clienteAutenticado);
        }

        public void RemoverClassificacaoEstabelecimento(int idEstabelecimento)
        {
            estabelecimentos.RemoverClassificacaoAlimento(idEstabelecimento, clienteAutenticado);
        }

        //=====================================================================
        // GESTÃO DE ESTABELECIMENTO
        //=====================================================================

        public void RegistarAlimento(int idEstabelecimento, Alimento alim)
        {
            proprietarios.RegistarAlimento(idEstabelecimento, alim);
        }

        public void EditarFotoAlimento(int idAlimento, byte[] foto)
        {
            proprietarios.EditarFotoAlimento(idAlimento, foto);
        }

        public void AdicionarIngredientesAlimento(int idAlimento, List<string> designacaoIngredientes)
        {
            proprietarios.AdicionarIngredientesAlimento(idAlimento, designacaoIngredientes);
        }

        public void RemoverIngredientesAlimento(int idAlimento, List<string> designacaoIngredientes)
        {
            proprietarios.RemoverIngredientesAlimento(idAlimento, designacaoIngredientes);
        }

        public void RemoverAlimento(int idAlimento)
        {
            proprietarios.RemoverAlimento(idAlimento);
        }

        public IList<Estabelecimento> ConsultarEstabelecimentos()
        {
            return proprietarios.ConsultarEstabelecimentos(clienteAutenticado);
        }

        public IList<Alimento> ConsultarAlimentos(int idEstabelecimento)
        {
            return proprietarios.ConsultarAlimentos(idEstabelecimento);
        }

        //=====================================================================
        // COMPARADORES, FILTROS E ORDENAÇÕES POR CRITÉRIO
        //=====================================================================

        public List<AlimentoEstabelecimento> OrdenarPorCusto(List<AlimentoEstabelecimento> listaAEs, bool crescentemente)
        {
            var comparadorCusto = new ComparadorCusto();

            if (crescentemente)
                listaAEs.Sort(comparadorCusto);
            else
                listaAEs.Sort((ae1, ae2) => comparadorCusto.Compare(ae2, ae1));

            return listaAEs;
        }

        public List<AlimentoEstabelecimento> OrdenarPorDistancia(List<AlimentoEstabelecimento> listaAEs, bool decrescente)
        {
            var comparadorDistancia = new ComparadorDistancia();

            if (decrescente)
                listaAEs.Sort((ae1, ae2) => comparadorDistancia.Compare(ae2, ae1));
            else
                listaAEs.Sort(comparadorDistancia);

            return listaAEs;
        }

        public List<AlimentoEstabelecimento> OrdenarPorClassificacaoAlimento(List<AlimentoEstabelecimento> listaAEs, bool decrescente)
        {
            var comparadorClassificacaoAlimento = new ComparadorClassificaoAlimento();

            if (decrescente)
                listaAEs.Sort((ae1, ae2) => comparadorClassificacaoAlimento.Compare(ae2, ae1));
            else
                listaAEs.Sort(comparadorClassificacaoAlimento);

            return listaAEs;
        }

        public List<AlimentoEstabelecimento> OrdenarClassificacaoEstabelecimento(List<AlimentoEstabelecimento> listaAEs, bool decrescente)
        {
            var comparadorClassificacaoEstabelecimento = new ComparadorClassificaoEstabelecimento();

            if (decrescente)
                listaAEs.Sort((ae1, ae2) => comparadorClassificacaoEstabelecimento.Compare(ae2, ae1));
            else
                listaAEs.Sort(comparadorClassificacaoEstabelecimento);

            return listaAEs;
        }

        public List<AlimentoEstabelecimento> OrdenarPorOmissao(List<AlimentoEstabelecimento> listaAEs)
        {
            listaAEs.Sort();
            return listaAEs;
        }

        public List<AlimentoEstabelecimento> FiltrarPorPreco(List<AlimentoEstabelecimento> listaAEs, float preco)
        {
            var listaFiltrada = new List<AlimentoEstabelecimento>();

            foreach (AlimentoEstabelecimento ae in listaAEs)
            {
                if (ae.Alimento.Preco <= preco)
                {
                    listaFiltrada.Add(ae);
                }
            }
            return listaFiltrada;
        }

        public List<AlimentoEstabelecimento> FiltrarPorDistancia(List<AlimentoEstabelecimento> listaAEs, float distancia)
        {
            var listaFiltrada = new List<AlimentoEstabelecimento>();

            foreach (AlimentoEstabelecimento ae in listaAEs)
            {
                if (ae.Distancia <= distancia)
                {
                    listaFiltrada.Add(ae);
                }
            }
            return listaFiltrada;
        }

        public List<AlimentoEstabelecimento> FiltrarPorClassificacaoAlimento(List<AlimentoEstabelecimento> listaAEs, int classificacao)
        {
            var listaFiltrada = new List<AlimentoEstabelecimento>();

            foreach (AlimentoEstabelecimento ae in listaAEs)
            {
                if (ae.Alimento.ClassificacaoMedia >= classificacao)
                {
                    listaFiltrada.Add(ae);
                }
            }
            return listaFiltrada;
        }

        public List<AlimentoEstabelecimento> FiltrarClassificacaoEstabelecimento(List<AlimentoEstabelecimento> listaAEs, int classificacao)
        {
            var listaFiltrada = new List<AlimentoEstabelecimento>();

            foreach (AlimentoEstabelecimento ae in listaAEs)
            {
                if (ae.Estabelecimento.ClassificacaoMedia >= classificacao)
                {
                    listaFiltrada.Add(ae);
                }
            }
            return listaFiltrada;
        }
    }

    internal class ComparadorClassificaoEstabelecimento : IComparer<AlimentoEstabelecimento>
    {
        public int Compare(AlimentoEstabelecimento ae1, AlimentoEstabelecimento ae2)
        {
            if (ae1 == null)
            {
                return (ae2 == null) ? 0 : -1; // quando (ae2 != null), ae2 aparece primeiro numa ordenação decrescente.
            }
            else // ae1 != null
            {
                if (ae2 == null)
                    return 1;
                else
                {
                    return ae1.Estabelecimento.ClassificacaoMedia
                                              .CompareTo(ae2.Estabelecimento.ClassificacaoMedia);
                }
            }
        }
    }

    internal class ComparadorClassificaoAlimento : IComparer<AlimentoEstabelecimento>
    {
        public int Compare(AlimentoEstabelecimento ae1, AlimentoEstabelecimento ae2)
        {
            if (ae1 == null)
            {
                return (ae2 == null) ? 0 : -1;
            }
            else // ae1 != null
            {
                if (ae2 == null)
                    return 1;
                else
                {
                    return ae1.Alimento.ClassificacaoMedia
                                       .CompareTo(ae2.Alimento.ClassificacaoMedia);
                }
            }
        }
    }

    internal class ComparadorDistancia : IComparer<AlimentoEstabelecimento>
    {
        public int Compare(AlimentoEstabelecimento ae1, AlimentoEstabelecimento ae2)
        {
            if (ae1 == null)
            {
                return (ae2 == null) ? 0 : -1;
            }
            else // ae1 != null
            {
                if (ae2 == null)
                    return 1;
                else
                    return ae1.Distancia.CompareTo(ae2.Distancia);
            }
        }
    }

    internal class ComparadorCusto : IComparer<AlimentoEstabelecimento>
    {
        public int Compare(AlimentoEstabelecimento ae1, AlimentoEstabelecimento ae2)
        {
            if (ae1 == null)
            {
                return (ae2 == null) ? 0 : -1;
            }
            else // ae1 != null
            {
                if (ae2 == null)
                    return 1;
                else
                    return Nullable.Compare<float>(ae1.Alimento.Preco, ae2.Alimento.Preco);
            }
        }
    }
}

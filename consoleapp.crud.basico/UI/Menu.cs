using Component.Grid;
using consoleapp.crud.basico.Entities;
using consoleapp.crud.basico.UseCases;
using System.Text;

namespace consoleapp.crud.basico.UI
{
    public class Menu
    {
        private enum OpcoesMenu
        {
            Sair = 0,
            ListarTodasPessoas = 1,
            ListarPessoasPorEstado = 2,
            AlterarDadosPessoais = 3,
            InserirNovaPessoa = 4,
            ApagarPessoa = 5
        }

        public void ExibirMenu()
        {
            var exibirMenu = true;

            do
            {
                Console.Clear();

                var textoMenu = MontaMenu();
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine(textoMenu);
                Console.ForegroundColor = ConsoleColor.White;

                var valorMenuEscolhido = Console.ReadLine()?.Trim();
                valorMenuEscolhido = valorMenuEscolhido == string.Empty | valorMenuEscolhido == null ? "0" : valorMenuEscolhido;

                var escolhaMenuUsuario = (OpcoesMenu)Enum.Parse(typeof(OpcoesMenu), valorMenuEscolhido);
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine($"O menu escolhido foi {escolhaMenuUsuario}");
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine();

                switch (escolhaMenuUsuario)
                {
                    case OpcoesMenu.ListarTodasPessoas:
                        ListarTodasPessoas();
                        break;

                    case OpcoesMenu.ListarPessoasPorEstado:
                        ListarPessoasPorEstado();
                        break;

                    case OpcoesMenu.AlterarDadosPessoais:
                        AlterarDadosPessoais();
                        break;

                    case OpcoesMenu.InserirNovaPessoa:
                        InserirNovaPessoa();
                        break;

                    case OpcoesMenu.ApagarPessoa:
                        ApagarPessoa();
                        break;

                    case OpcoesMenu.Sair:
                        exibirMenu = false;
                        break;

                    default:
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Essa opção não existe no menu.");
                        Console.ForegroundColor = ConsoleColor.White;
                        break;
                }
            } while (exibirMenu);
        }

        private string MontaMenu()
        {
            var menu = new StringBuilder();
            menu.AppendLine("**********************************************");
            menu.AppendLine("       Menu de opções da aplicação");
            menu.AppendLine("**********************************************");
            menu.AppendLine("0 - Sair");
            menu.AppendLine("1 - Listar Todas Pessoas");
            menu.AppendLine("2 - Listar Pessoas por Estado");
            menu.AppendLine("3 - Alterar Dados de uma Pessoa");
            menu.AppendLine("4 - Inserir uma Nova Pessoa");
            menu.AppendLine("5 - Excluir Pessoa");
            menu.AppendLine("Informe o número do menu da sua escolha:");

            return menu.ToString();
        }

        private void ListarTodasPessoas()
        {
            var pessoas = new PessoaUC()
                .ListarTodasPessoasDepartamento();

            var grid = new DataGrid<PessoaDepartamento>(pessoas);

            // Config do componente
            grid.Titulo = "Listar todas as pessoas";
            grid.PaginarItensGrid = true;
            grid.QuantidadeItensPagina = 12;
            grid.PaginaInicial = 1;

            grid.DataBinding();
        }

        private void ListarPessoasPorEstado()
        {
            var estados = new EstadoUC().ListarTodosEstados();

            var gridEstados = new DataGrid<Estado>(estados);
            gridEstados.ImprimirGrid += GridEstados_ImprimirGrid;
            gridEstados.SelecionarItem += GridEstados_SelecionarItem;

            gridEstados.Titulo = "Todos os Estados da federação";
            gridEstados.PaginarItensGrid = true;
            gridEstados.QuantidadeItensPagina = 5;
            gridEstados.DataBinding();
        }

        private void GridEstados_ImprimirGrid(object? sender, DataGridEventArgs<Estado> e)
        {
            Console.WriteLine("Selecione o um Estado da Federação");
        }

        private void GridEstados_SelecionarItem(object? sender, DataGridItemSelecionadoEventArgs<Estado> e)
        {
            var pessoasEstado = new PessoaUC().ListarPessoasPorEstado(e.Item.Id);

            if (pessoasEstado?.Count > 0)
            {
                var grid = new DataGrid<PessoaEstado>(pessoasEstado);
                grid.PaginarItensGrid = false;
                grid.Titulo = $"Pessoas pertencentes ao Estado {e.Item.Id} - {e.Item.Nome}";
                grid.DataBinding();
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"\n\rNão existem pessoas cadastradas para o Estado {e.Item.Id} - {e.Item.Nome}!");
                Console.ResetColor();
            }
        }

        private void ListarDepartamentos()
        {
            var departamentos = new DepartamentoUC().ListarTodosDepartamentos();

            var grid = new DataGrid<DepartamentoCidade>(departamentos);
            grid.DataBinding();
        }

        private void CabecalhoAlterarDadosPessoais()
        {
            Console.WriteLine("*****ALTERAR DADOS DE PESSOA*****\n");
        }

        private void AlterarDadosPessoais()
        {
            Console.Clear();
            CabecalhoAlterarDadosPessoais();
            ListarTodasPessoas();

            var idPessoaInput = int.MinValue;
            var nomePessoaInput = string.Empty;
            var idDepartamentoPessoaInput = int.MinValue;

            var listaPessoas = new PessoaUC().ListarTodasPessoas();

            var entradasValidas =
                int.TryParse(ObterIdPessoaAlterarcao(), out idPessoaInput)
                && NomePessoaValido(ObterNomePessoaAlterarcao(idPessoaInput, listaPessoas), out nomePessoaInput)
                && int.TryParse(ObterIdDepartamentoPessoaAlterarcao(), out idDepartamentoPessoaInput);

            if (entradasValidas)
            {
                var pessoa = new Pessoa();

                pessoa.Id = idPessoaInput;
                pessoa.Nome = nomePessoaInput;
                pessoa.IdDepartamento = idDepartamentoPessoaInput;

                var pessoaUc = new PessoaUC();
                pessoaUc.AlterarDadosPessoas(pessoa);

                Console.Clear();
                CabecalhoAlterarDadosPessoais();
                ListarTodasPessoas();

                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("\nDados pessoais alterados com sucesso!");
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine("Os dados informados não são válidos!");
                Console.ForegroundColor = ConsoleColor.White;
            }
        }

        private string ObterIdPessoaAlterarcao()
        {
            Console.WriteLine("\nInforme o Id da pessoa que será alterado:");
            var retorno = Console.ReadLine();

            return retorno;
        }

        private string ObterNomePessoaAlterarcao(int idPessoa, IList<PessoaDepartamento> pessoas)
        {
            Console.WriteLine("\nInforme o novo Nome da pessoa que será alterado:");

            var nome = pessoas
                .FirstOrDefault(pes => pes.Id == idPessoa)
                ?.NomePessoa;

            Console.WriteLine($"Nome atual: {nome}");
            var retorno = Console.ReadLine();

            return retorno;
        }

        private string ObterIdDepartamentoPessoaAlterarcao()
        {
            Console.WriteLine();
            ListarDepartamentos();

            Console.WriteLine("\n\nInforme o Id do departamento que será alterado:");
            var retorno = Console.ReadLine();

            return retorno;
        }

        private bool NomePessoaValido(string nomePessoa, out string nomePessoaValidado)
        {
            var retorno = true;
            retorno = !string.IsNullOrWhiteSpace(nomePessoa)
                && !int.TryParse(nomePessoa, out _);

            nomePessoaValidado = nomePessoa;

            return retorno;
        }

        public Pessoa Pessoa { get; set; } = new Pessoa();

        private void InserirNovaPessoa()
        {
            Console.WriteLine("Informe o nome da nova pessoa:");
            Pessoa.Nome = Console.ReadLine();

            var departamentos = new DepartamentoUC().ListarTodosDepartamentos();

            var gridDepartamento = new DataGrid<DepartamentoCidade>(departamentos);
            gridDepartamento.SelecionarItem += GridDepartamento_SelecionarItem;
            gridDepartamento.Titulo = "Selecione o departamento";
            gridDepartamento.QuantidadeItensPagina = 10;
            gridDepartamento.PaginarItensGrid = true;
            gridDepartamento.DataBinding();
        }

        private void GridDepartamento_SelecionarItem(object? sender, DataGridItemSelecionadoEventArgs<DepartamentoCidade> e)
        {
            Pessoa.IdDepartamento = e.Item.Id;

            Console.WriteLine("Você deseja inserir essa nova pessoa? S/N");
            var confirmado = Console.ReadLine() == "S" ? true : false;

            if (confirmado)
            {
                var pessoaUC = new PessoaUC();
                pessoaUC.InserirPessoa(Pessoa.IdDepartamento, Pessoa.Nome);
                Console.WriteLine("Pessoa inserida com sucesso!");


                var pessoasDeparamento = pessoaUC.ListarTodasPessoasDepartamento();
                var gridPessoasDepartamento = new DataGrid<PessoaDepartamento>(pessoasDeparamento);
                gridPessoasDepartamento.Titulo = "Inserir nova pessoa";
                gridPessoasDepartamento.PaginarItensGrid = true;
                gridPessoasDepartamento.QuantidadeItensPagina = 10;
                gridPessoasDepartamento.DataBinding();
            }
            else
            { 
                Console.WriteLine("Chegou"); 
            }
        }

        private void ApagarPessoa()
        {
            Console.Clear();
            Console.WriteLine("***** EXCLUIR PESSOA *****\n");
            ListarTodasPessoas();
            Console.WriteLine();

            Console.WriteLine("Escolha o Id da pessoa que deseja exluir: ");
            string inputIdPessoa = Console.ReadLine();

            var entradasValidas =
                int.TryParse(inputIdPessoa, out int IdPessoaInformado);

            if (entradasValidas)
            {
                var pessoaUC = new PessoaUC();
                var pessoa = pessoaUC
                    .ListarTodasPessoasDepartamento()
                    .FirstOrDefault(pes => pes.Id == IdPessoaInformado);

                var apagou = pessoaUC.ApagarPessoa(IdPessoaInformado);

                if (apagou)
                {
                    Console.Clear();
                    Console.WriteLine();
                    Console.WriteLine("***** EXCLUIR PESSOA *****\n");
                    Console.WriteLine($"A pessoa {pessoa.NomePessoa} - Id: {pessoa.Id}, do departamento {pessoa.NomeDepartamento}, foi excluída com sucesso!");
                    Console.WriteLine();
                    ListarTodasPessoas();
                }
                else
                {
                    Console.WriteLine($"Não existe uma pessoa cadastrada com o id: {IdPessoaInformado}.");
                }
            }
            else
            {
                Console.WriteLine("Entrada inválida!");
            }
        }
    }
}
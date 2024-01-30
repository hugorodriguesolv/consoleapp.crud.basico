using consoleapp.crud.basico.Entities;
using consoleapp.crud.basico.UseCases;
using System;
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

                Console.WriteLine();
                Console.WriteLine("Pressione qualquer tecla para continuar.");
                Console.ReadLine();
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
            var pessoaUC = new PessoaUC();
            var pessoas = pessoaUC.ListarTodasPessoasDepartamento();

            var grid = new DataGrid<PessoaDepartamento>(pessoas);
            grid.DataBinding();
        }

        private void DataGridAlterada(object? sender, DataGridEventArgs<PessoaDepartamento> e)
        {
            Console.Clear();

            Console.WriteLine("O evento ocorreu e altera");
            Console.WriteLine($"Linha: {e.Linha}");
            Console.WriteLine($"Pessoa: {e.ItemAlterado.NomePessoa}");
            Console.WriteLine($"Tipo Evento: {e.TipoEvento}");
        }

        private void ListarPessoasPorEstado()
        {
            var pessoaUC = new PessoaUC();

            Console.WriteLine("Informe um Id de um estado:");
            var IdEstadoInformado = int.Parse(Console.ReadLine());

            var pessoasEstado = pessoaUC.ListarPessoasPorEstado(IdEstadoInformado);


            var grid = new DataGrid<PessoaEstado>(pessoasEstado);
            grid.DataBinding();


            var fechamentoTabela = $"| {new string('¯', 25)} | {new string('¯', 25)} | {new string('¯', 15)} |";

            Console.WriteLine(fechamentoTabela);
            Console.WriteLine($"| {"Pessoa".PadRight(25)} | {"Departamento".PadRight(25)} | {"Estado".PadRight(15)} |");
            Console.WriteLine(fechamentoTabela);

            foreach (var pessoa in pessoasEstado)
            {
                Console.WriteLine($"| {pessoa.NomePessoa.PadRight(25)} | {pessoa.NomeDepartamento.PadRight(25)} | {pessoa.NomeEstado.PadRight(15)} |");
            }
        }

        private void ListarDepartamentos()
        {
            var departamentoUC = new DepartamentoUC();
            var departamentos = departamentoUC.ListarTodosDepartamentos();

            var fechamentoTabela = $"| {new string('¯', 8)} | {new string('¯', 25)} | {new string('¯', 25)} |";

            Console.WriteLine(fechamentoTabela);
            Console.WriteLine($"| {"Id".PadRight(8)} | {"Departamento".PadRight(25)} | {"Cidade".PadRight(25)} |");
            Console.WriteLine(fechamentoTabela);

            foreach (var departamento in departamentos)
            {
                Console.WriteLine($"| {departamento.Id.ToString().PadRight(8)} | {departamento.NomeDepartamento.PadRight(25)} | {departamento.NomeCidade.PadRight(25)} |");
            }
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

        private void InserirNovaPessoa()
        {
            Console.Clear();
            Console.WriteLine("***** INSERIR NOVA PESSOA *****\n");

            ListarTodasPessoas();

            Console.WriteLine("\nInforme o nome da nova pessoa: ");
            var nomeNovaPessoa = Console.ReadLine();

            var pessoaUC = new PessoaUC();

            var pessoaExiste = pessoaUC
                .ListarTodasPessoas()
                .Any(pes => pes.NomePessoa == nomeNovaPessoa);

            if (!pessoaExiste)
            {
                Console.Clear();
                Console.WriteLine("***** INSERIR NOVA PESSOA *****\n");

                ListarDepartamentos();

                Console.WriteLine($"\nNome da pessoa informado foi: {nomeNovaPessoa}");
                Console.WriteLine("\nInforme o Id do departamento dessa pessoa: ");
                string infoIdDepNovaPessoa = Console.ReadLine();

                var entradasValidas =
                    int.TryParse(infoIdDepNovaPessoa, out int idDepartamentoNovaPessoa)
                    && !string.IsNullOrWhiteSpace(nomeNovaPessoa);

                if (entradasValidas)
                {
                    var departamentoUC = new DepartamentoUC();

                    var departamentoExiste = departamentoUC
                        .ListarTodosDepartamentos()
                        .Any(dep => dep.Id == idDepartamentoNovaPessoa);

                    if (departamentoExiste)
                    {
                        pessoaUC.InserirPessoa(idDepartamentoNovaPessoa, nomeNovaPessoa);

                        Console.Clear();
                        Console.WriteLine($"{nomeNovaPessoa} foi inserido com sucesso! \n");
                        ListarTodasPessoas();
                    }
                    else
                    {
                        Console.WriteLine("\nEsse departamento não existe!!");
                    }
                }
                else
                {
                    Console.WriteLine("\nEntrada inválida!! Certifique-se de inserir um número inteiro.");
                }
            }
            else
            {
                Console.WriteLine("\n\nEssa pessoa já existe!!");
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
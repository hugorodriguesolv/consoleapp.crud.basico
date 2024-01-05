﻿using consoleapp.crud.basico.Entities;
using consoleapp.crud.basico.UseCases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace consoleapp.crud.basico.UI
{
    public class Menu
    {
        private enum OpcoesMenu
        {
            Sair = 0,
            ListarTodasPessoas = 1,
            ListarPessoasPorEstado = 2,
            AlterarDadosPessoa = 3,
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

                    case OpcoesMenu.AlterarDadosPessoa:
                        AlterarNomePessoais();
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
            var pessoas = pessoaUC.ListarTodasPessoas();
            
            var fechamentoTabela = $"| {new string('¯', 8)} | {new string('¯', 25)} | {new string('¯', 25)} |";

            Console.WriteLine(fechamentoTabela);
            Console.WriteLine($"| {"Id".PadRight(8)} | {"Pessoa".PadRight(25)} | {"Departamento".PadRight(25)} |");
            Console.WriteLine(fechamentoTabela);

            foreach (var pessoa in pessoas)
            {
                Console.WriteLine($"| {pessoa.Id.ToString().PadRight(8)} | {pessoa.NomePessoa.PadRight(25)} | {pessoa.NomeDepartamento.PadRight(25)} |");
            }
        }

        private void ListarPessoasPorEstado()
        {
            var pessoaUC = new PessoaUC();

            Console.WriteLine("Informe um Id de um estado:");
            var IdEstadoInformado = int.Parse(Console.ReadLine());

            var pessoasEstado = pessoaUC.ListarPessoasPorEstado(IdEstadoInformado);

            var fechamentoTabela = $"| {new string('¯', 25)} | {new string('¯', 25)} | {new string('¯', 15)} |";

            Console.WriteLine(fechamentoTabela);
            Console.WriteLine($"| {"Pessoa".PadRight(25)} | {"Departamento".PadRight(25)} | {"Estado".PadRight(15)} |");
            Console.WriteLine(fechamentoTabela);

            foreach (var pessoa in pessoasEstado)
            {
                Console.WriteLine($"| {pessoa.NomePessoa.PadRight(25)} | {pessoa.NomeDepartamento.PadRight(25)} | {pessoa.NomeEstado.PadRight(15)} |");
            }
        }

        private void AlterarNomePessoais()
        {
            private enum OpcoesAlterarDados
            {
                Voltar = 0,
                AlterarNomePessoa = 1,
                AlterarDepartamentoPessoa = 2,
                AlterarCidadePessoa = 3,
                AlterarEstadoPessoa = 4
            }

            public void ExibirSubMenuAlterarDadosPessoa()
            {
                var exibirMenu = true;

                do
                {
                    Console.Clear();

                    var textoMenu = MontaSubMenuAlterarDadosPessoa();
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine(textoMenu);
                    Console.ForegroundColor = ConsoleColor.White;

                    var valorMenuEscolhido = Console.ReadLine()?.Trim();
                    valorMenuEscolhido = valorMenuEscolhido == string.Empty | valorMenuEscolhido == null ? "0" : valorMenuEscolhido;

                    var escolhaMenuUsuario = (OpcoesAlterarDados)Enum.Parse(typeof(OpcoesAlterarDados), valorMenuEscolhido);
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine($"O menu escolhido foi {escolhaMenuUsuario}");
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.WriteLine();

                    switch (escolhaMenuUsuario)
                    {
                        case OpcoesAlterarDados.AlterarNomePessoa:
                            AlterarNomePessoa();
                            break;
                        case OpcoesAlterarDados.AlterarDepartamentoPessoa:
                            AlterarDepartamentoPessoa();
                            break;

                        case OpcoesAlterarDados.AlterarCidadePessoa:
                            AlterarCidadePessoa();
                            break;
                        case OpcoesAlterarDados.AlterarEstadoPessoa:
                            AlterarEstadoPessoa();
                            break;
                        case OpcoesAlterarDados.Voltar:
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


        private string MontaSubMenuAlterarDadosPessoa()
            {
                var menu = new StringBuilder();
                menu.AppendLine("**********************************************");
                menu.AppendLine("       Menu Alterar Dados Pessoais");
                menu.AppendLine("**********************************************");
                menu.AppendLine("0 - Voltar");
                menu.AppendLine("1 - Alterar Nome de uma Pessoa");
                menu.AppendLine("2 - Alterar Departamento de uma Pessoa");
                menu.AppendLine("3 - Alterar Cidade de uma Pessoa");
                menu.AppendLine("4 - Alterar Estado de uma Pessoa");
                menu.AppendLine("\n ");
                menu.AppendLine("Informe o número do menu da sua escolha:");

                return menu.ToString();
            }

        private void AlterarNomePessoa()
        {

            Console.Write("Informe o nome da Pessoa: ");
            string nomePessoa = Console.ReadLine();

            Console.Write("Infome novo departamento: ");
            string novoDepartamento = Console.ReadLine();

            var pessoa = new Pessoa();
            pessoa.Nome = nomePessoa;
            pessoa.IdDepartamento = int.Parse(novoDepartamento);

            var pessoaUc = new PessoaUC();
            pessoaUc.AlterarDadosPessoais(idPessoa);
        }
        
        }

        private void InserirNovaPessoa()
        {
            throw new NotImplementedException();
        }

        private void ApagarPessoa()
        {
            Console.WriteLine("Escolha o Id da pessoa que deseja exluir: ");
            var IdPessoaInformado = int.Parse(Console.ReadLine());

            var pessoaUC = new PessoaUC();
            var apagou = pessoaUC.ApagarPessoa(IdPessoaInformado);

            if (apagou)
            {
                Console.WriteLine($"A pessoa com o id: {IdPessoaInformado} foi excluída com sucesso!");
            } 
            else
            {
                Console.WriteLine($"Não foi possível excluir a pessoa com o id: {IdPessoaInformado}.");
            }
        }
    }
}
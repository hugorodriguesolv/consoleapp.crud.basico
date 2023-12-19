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
            ListarTodasPessoas = 1,
            ListarPessoasPorEstado = 2,
            AlterarDadosPessoa = 3,
            InserirNovaPessoa = 4,
            ApagarPessoa = 5
        }

        public static void ExibirMenu()
        {
            do
            {
                var textoMenu = MontaMenu();
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine(textoMenu);
                Console.ForegroundColor = ConsoleColor.White;

                var escolhaMenuUsuario = (OpcoesMenu)Enum.Parse(typeof(OpcoesMenu), Console.ReadLine());
                Console.WriteLine($"O menu escolhido foi {escolhaMenuUsuario}");

                switch (escolhaMenuUsuario)
                {
                    case OpcoesMenu.ListarTodasPessoas:
                        break;

                    case OpcoesMenu.ListarPessoasPorEstado:
                        break;

                    case OpcoesMenu.AlterarDadosPessoa:
                        break;

                    case OpcoesMenu.InserirNovaPessoa:
                        break;

                    case OpcoesMenu.ApagarPessoa:
                        break;

                    default:
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Essa opção não existe no menu.");
                        Console.ForegroundColor = ConsoleColor.White;
                        break;
                }


            } while (true);
        }

        private static string MontaMenu()
        {
            var menu = new StringBuilder();
            menu.AppendLine("**********************************************");
            menu.AppendLine("       Menu de opções da aplicação");
            menu.AppendLine("**********************************************");
            menu.AppendLine("1 - Listar Todas Pessoas");
            menu.AppendLine("2 - Listar Pessoas por Estado");
            menu.AppendLine("3 - Alterar Dados de uma Pessoa");
            menu.AppendLine("4 - Inserir uma Nova Pessoa");
            menu.AppendLine("5 - Excluir Pessoa");
            menu.AppendLine("Informe o número do menu da sua escolha:");

            return menu.ToString();
        }
    }
}
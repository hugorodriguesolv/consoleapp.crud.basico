using consoleapp.crud.basico.Entities;
using consoleapp.crud.basico.UseCases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace consoleapp.crud.basico.UI
{
    public class SubMenuAlterarDados
    {
        public enum OpcoesAlterarDados
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
            throw new NotImplementedException();
        }
        private void AlterarDepartamentoPessoa()
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

}
}

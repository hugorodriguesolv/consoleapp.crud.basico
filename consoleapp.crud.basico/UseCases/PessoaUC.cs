using consoleapp.crud.basico.Entities;
using consoleapp.crud.basico.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace consoleapp.crud.basico.UseCases
{
    public class PessoaUC
    {
        public IList<Pessoa>  ListarTodasPessoas()
        { 
            var pessoaRepository = new PessoaRepository();
            var pessoas = pessoaRepository.ObterTodasPessoas();

            return pessoas;
        }
    }
}

using consoleapp.crud.basico.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace consoleapp.crud.basico.Interfaces
{
    public interface IPessoaRepository
    {
        IList<PessoaDepartamento> ObterTodasPessoas();

        IList<PessoaEstado> ObterPessoasPorEstado(int IdEstado);
    }
}

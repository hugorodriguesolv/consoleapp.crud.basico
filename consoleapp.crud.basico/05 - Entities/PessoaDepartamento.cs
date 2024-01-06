using consoleapp.crud.basico.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace consoleapp.crud.basico.Entities
{
    public class PessoaDepartamento : Entity
    {
        public string? NomePessoa { get; set; }

        public string? NomeDepartamento { get; set; }
    }
}

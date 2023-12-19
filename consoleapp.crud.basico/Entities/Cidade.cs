using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace consoleapp.crud.basico.Entities
{
    public class Cidade : Entity
    {
        public string? Nome { get; set; }

        public int Populacao { get; set; }
    }
}

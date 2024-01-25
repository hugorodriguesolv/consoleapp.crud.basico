using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace consoleapp.crud.basico.UI
{
    public class DataGrid<T> where T : class
    {
        private IList<T> _dadosGrid;

        public DataGrid()
        { }

        public DataGrid(IList<T> dadosGrid) => _dadosGrid = dadosGrid;

        public void CarregarDados(IList<T> dadosGrid) => _dadosGrid = dadosGrid;

        public void AdicionarLinha(T item)
        {
        }
        public void RemoverLinha(int numeroLinha)
        {
        }

        public void MontarCabecalho(T item)
        {
        }

        public void Ordenar(int[] args)
        { 
            
        }

        public void DataBinding()
        { 
        
        }
    }
}
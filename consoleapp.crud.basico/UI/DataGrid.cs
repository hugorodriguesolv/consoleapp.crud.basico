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

        public event EventHandler<DataGridEventArgs<T>> DataGridChanged;

        public DataGrid()
        { }

        public DataGrid(IList<T> dadosGrid) => _dadosGrid = dadosGrid;

        public void CarregarDados(IList<T> dadosGrid) => _dadosGrid = dadosGrid;

        public void AdicionarLinha(T item)
        {
            _dadosGrid?.Add(item);
            OnDataGridAlterado(DataGridTipoEvento.AdicaoItem, _dadosGrid.Count(), item);
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

        protected virtual void OnDataGridAlterado(DataGridTipoEvento tipoEvento, int linha, T item)
        {
            DataGridChanged?.Invoke(this, new DataGridEventArgs<T>(tipoEvento, linha, item));
        }
    }

    public class DataGridEventArgs<T> : EventArgs
    {
        public DataGridTipoEvento TipoEvento { get; }

        public int Linha { get; }

        public T ItemAlterado { get; }

        public DataGridEventArgs(DataGridTipoEvento tipoEvento, int linha, T itemAlterado)
        {
            TipoEvento = tipoEvento;
            Linha = linha;
            ItemAlterado = itemAlterado;
        }
    }

    public enum DataGridTipoEvento
    {
        CargaDados,
        AdicaoItem,
        ExclusaoItem,
        OrdenacaoItens
    }
}
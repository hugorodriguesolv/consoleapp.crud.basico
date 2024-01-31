using System.Linq;
using System.Collections.Generic;
using System.Collections;
using System.Text;

namespace consoleapp.crud.basico.UI
{
    public class DataGrid<T> where T : class
    {
        private IList<T> _dadosGrid;
        private string[] _cabecalho;
        private string[,] _corpoGrid;

        public event EventHandler<DataGridEventArgs<T>> DataGridAlterada;

        public event EventHandler<DataGridEventArgs<T>> ItemExcluido;

        public event EventHandler<DataGridEventArgs<T>> ItemAdicionado;

        public event EventHandler<DataGridEventArgs<T>> GridPaginada;

        public DataGrid()
        { }

        public DataGrid(IList<T> dadosGrid) => _dadosGrid = dadosGrid;

        public void CarregarDados(IList<T> dadosGrid) => _dadosGrid = dadosGrid;

        public void DefinirCabecalho(string[] args)
        {
            _cabecalho = args;
        }

        public void AdicionarLinha(T item)
        {
            _dadosGrid?.Add(item);
            OnDataGridAlterado(DataGridTipoEvento.AdicaoItem, _dadosGrid.Count(), item);
        }

        public void RemoverLinha(int numeroLinha)
        {
            _dadosGrid.RemoveAt(numeroLinha);
        }

        public void DataBinding()
        {
            MontarDadosGrid();
            MontarLayoutGrid();
        }

        private void MontarDadosGrid()
        {
            var quantidadeLinhas = _dadosGrid.Count() + 1;
            var quantidadeColunas = _dadosGrid[0].GetType().GetProperties().Count();
            var propriedades = _dadosGrid[0].GetType().GetProperties();

            _corpoGrid = new string[quantidadeLinhas, quantidadeColunas];

            for (int col = 0; col < quantidadeColunas; col++)
            {
                _corpoGrid[0, col] = propriedades[col].Name;
            }

            var lin = 1;

            foreach (var item in _dadosGrid)
            {
                var tipoItem = typeof(T).GetType();
                var col = 0;

                foreach (var propriedade in propriedades)
                {
                    _corpoGrid[lin, col] = propriedade.GetValue(item).ToString();
                    col++;
                }

                lin++;
            }
        }

        public void MontarLayoutGrid()
        {

            var linhaGrid = new StringBuilder();
            var gridMontada = string.Empty;


            for (int linha = 1; linha < _corpoGrid.Length; linha++)
            {
                var maxColuna = 0;

                for (int coluna = 0; coluna <= _corpoGrid.Rank; coluna++)
                {
                    var linhaAux = $"|  {_corpoGrid.GetValue(linha, coluna)}  ";
                    maxColuna = maxColuna > linhaAux.Length ? maxColuna : linhaAux.Length;

                    linhaGrid.Append(linhaAux);
                }

                linhaGrid.AppendLine("|");

            }


            Console.ReadKey();

        }

        protected virtual void OnDataGridAlterado(DataGridTipoEvento tipoEvento, int linha, T item)
        {
            DataGridAlterada?.Invoke(this, new DataGridEventArgs<T>(tipoEvento, linha, item));
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
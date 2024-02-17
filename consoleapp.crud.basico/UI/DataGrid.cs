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
        private int[] _maxColunas;

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

            _maxColunas = new int[quantidadeColunas];
            _corpoGrid = new string[quantidadeLinhas, quantidadeColunas];

            for (int col = 0; col < quantidadeColunas; col++)
            {
                var nomePropriedade = propriedades[col].Name;
                _corpoGrid[0, col] = nomePropriedade;
                _maxColunas[col] = _maxColunas[col] > nomePropriedade.Length ? _maxColunas[col] : nomePropriedade.Length;
            }

            var lin = 1;

            foreach (var item in _dadosGrid)
            {
                var tipoItem = typeof(T).GetType();
                var col = 0;

                foreach (var propriedade in propriedades)
                {
                    var valorPropriedade = propriedade?.GetValue(item)?.ToString();
                    _corpoGrid[lin, col] = valorPropriedade;
                    _maxColunas[col] = _maxColunas[col] > valorPropriedade.Length ? _maxColunas[col] : valorPropriedade.Length;

                    col++;
                }

                lin++;
            }
        }

        public void MontarLayoutGrid()
        {
            var linhaGrid = new StringBuilder();
            var qtdLinhas = _corpoGrid.GetLength(0);
            var qtdColunas = _corpoGrid.GetLength(1);

            for (int lin = 0; lin < qtdLinhas; lin++)
            {
                for (int col = 0; col < qtdColunas; col++)
                {
                    var valorPropriedade = _corpoGrid?.GetValue(lin, col)?.ToString();
                    var linhaAux = string.Empty;
                    var tamanho = 0;

                    if (lin > 0)
                    {
                        tamanho = _maxColunas[col] % 2 != 0 ? _maxColunas[col] + 1 : _maxColunas[col];
                        linhaAux = $"|{valorPropriedade?.PadRight(tamanho)}";
                    }
                    else
                    {
                        tamanho = _maxColunas[col] - valorPropriedade.Length;
                        var numEspacosVazios = tamanho % 2 == 0 ? tamanho / 2 : (tamanho + 1) / 2;
                        linhaAux = $"|{new string(' ', numEspacosVazios)}{valorPropriedade}{new string(' ', numEspacosVazios)}";
                    }

                    linhaGrid.Append(linhaAux);
                }

                linhaGrid.AppendLine("|");
            }

            Console.WriteLine(linhaGrid.ToString());
        }

        protected virtual void OnDataGridAlterado(DataGridTipoEvento tipoEvento, int linha, T item)
        {
            DataGridAlterada?.Invoke(this, new DataGridEventArgs<T>(tipoEvento, linha, item));
        }
        
        public virtual void RemoveLine(int line)
        {
            var item =_dadosGrid.ElementAt<T>(line);
            _dadosGrid.RemoveAt(line);
            ItemExcluido?.Invoke(this, new DataGridEventArgs<T>(DataGridTipoEvento.ExclusaoItem, line, item));
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
using System.Reflection;

namespace consoleapp.crud.basico.UI
{
    /// <summary>
    ///
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class DataGrid<T> where T : class
    {
        private IList<T> _dadosGrid;
        private string[] _cabecalho;
        private string[,] _corpoGrid;
        private int[] _maxColunas;

        private int _quantidadeItensPagina = 5;
        private int _paginaInicial = 1;

        private bool _paginar = false;

        /// <summary>
        /// Quantidade de itens por página
        /// </summary>
        public int QuantidadeItensPagina
        {
            get { return _quantidadeItensPagina; }
            set { _quantidadeItensPagina = value; }
        }

        /// <summary>
        /// Página que a grid irá exibir primeiro
        /// </summary>
        public int PaginaInicial
        {
            get { return _paginaInicial; }
            set { _paginaInicial = value; }
        }

        /// <summary>
        /// Opção para que de forma automática a grid seja paginada
        /// </summary>
        public bool Paginar
        {
            get { return _paginar; }
            set { _paginar = value; }
        }

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
            if (_paginar)
                PaginarGrid(_quantidadeItensPagina, _paginaInicial);
            else
                MontarLayoutGrid(_dadosGrid);
        }

        private void PaginarGrid(int tamanhoPagina, int paginaAtual)
        {
            while (true)
            {
                var startIndex = --paginaAtual * tamanhoPagina;
                IList<T> pagina = _dadosGrid.Skip(startIndex).Take(tamanhoPagina).ToList();
                var totalPaginas = Math.Ceiling((double)_dadosGrid.Count / tamanhoPagina);

                MontarLayoutGrid(pagina);

                Console.WriteLine($"Página de {++paginaAtual} até {totalPaginas}");

                var tecla = Console.ReadKey(true);

                switch (tecla.Key)
                {
                    case ConsoleKey.LeftArrow:
                        if (paginaAtual > 1)
                            --paginaAtual;
                        break;

                    case ConsoleKey.RightArrow:
                        if (paginaAtual <= totalPaginas)
                            ++paginaAtual;
                        break;

                    case ConsoleKey.Escape:
                        return;
                }
            }
        }

        private void MontarLayoutGrid(IList<T> pagina)
        {
            Console.Clear();

            var propriedadesTamanho = GetMaxPropertyLengths(pagina);

            foreach (var prop in propriedadesTamanho)
            {
                var tamanho = prop.Value - prop.Key.Length;
                var numEspacosVazios = tamanho % 2 == 0 ? tamanho / 2 : (tamanho + 1) / 2;

                Console.Write($"|{new string(' ', numEspacosVazios)}{prop.Key}{new string(' ', numEspacosVazios)}");
            }

            Console.Write("| \n");

            foreach (var itemGrid in pagina)
            {
                var propriedades = typeof(T).GetProperties();

                foreach (var prop in propriedades)
                {
                    var maxLen = propriedadesTamanho[prop.Name];

                    var tamanho = maxLen % 2 != 0 ? maxLen + 1 : maxLen;
                    Console.Write($"|{prop.GetValue(itemGrid).ToString()?.PadRight(tamanho)}");
                }

                Console.Write("| \n");
            }
        }

        private static Dictionary<string, int> GetMaxPropertyLengths(IEnumerable<T> items)
        {
            Dictionary<string, int> maxPropertyLengths = new Dictionary<string, int>();

            PropertyInfo[] properties = typeof(T).GetProperties();

            foreach (var property in properties)
            {
                var cabecalho = items
                    .Select(cab => property.Name.ToString().Length);

                var maxLength = items
                    .Select(item => property.GetValue(item)?.ToString()?.Length ?? 0)
                    .Union(cabecalho)
                    .Max();

                maxPropertyLengths.Add(property.Name, maxLength);
            }

            return maxPropertyLengths;
        }

        protected virtual void OnDataGridAlterado(DataGridTipoEvento tipoEvento, int linha, T item)
        {
            DataGridAlterada?.Invoke(this, new DataGridEventArgs<T>(tipoEvento, linha, item));
            ItemAdicionado?.Invoke(this, new DataGridEventArgs<T>(tipoEvento, linha, item));
        }

        public virtual void RemoveLine(int line)
        {
            var item = _dadosGrid.ElementAt<T>(line);
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
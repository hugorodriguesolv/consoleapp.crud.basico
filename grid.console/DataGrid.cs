using System.Reflection;

namespace consoleapp.crud.basico.UI
{
    /// <summary>
    ///
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class DataGrid<T> where T : class
    {
        private IList<T> _dados = new List<T>();
        private IList<T> _dadosGrid = new List<T>();

        private string[] _cabecalho;
        private string[,] _corpoGrid;
        private double _totalPaginas;
        private int _paginaAtual;

        public string Titulo { get; set; }

        /// <summary>
        /// Quantidade de itens por página
        /// </summary>
        public int QuantidadeItensPagina { get; set; } = 5;

        /// <summary>
        /// Página que a grid irá exibir primeiro
        /// </summary>
        public int PaginaInicial { get; set; } = 1;

        /// <summary>
        /// Opção para que de forma automática a grid seja paginada
        /// </summary>
        public bool Paginar { get; set; } = false;

        public event EventHandler<DataGridEventArgs<T>> DataGridAlterada;

        public event EventHandler<DataGridEventArgs<T>> ItemExcluido;

        public event EventHandler<DataGridEventArgs<T>> ItemAdicionado;

        public event EventHandler<DataGridEventArgs<T>> GridPaginada;

        public DataGrid()
        { }

        public DataGrid(IList<T> dadosGrid) => _dados = dadosGrid;

        public void CarregarDados(IList<T> dadosGrid) => _dados = dadosGrid;

        public void DefinirCabecalho(string[] args)
        {
            _cabecalho = args;
        }

        public void AdicionarLinha(T item)
        {
            _dados?.Add(item);
            OnDataGridAlterado(DataGridTipoEvento.AdicaoItem, _dados.Count(), item);
        }

        private void PaginarGrid(int tamanhoPagina, int paginaAtual)
        {
            var startIndex = --paginaAtual * tamanhoPagina;
            _dadosGrid = _dados.Skip(startIndex).Take(tamanhoPagina).ToList();
            _totalPaginas = Math.Ceiling((double)_dados.Count / tamanhoPagina);
            MontarLayoutGrid(_dadosGrid);
            _paginaAtual = ++paginaAtual;
        }

        public void OrdenarCampos<Tkey>(Func<T, Tkey> expressao)
        {
            _dadosGrid = _dados
                .OrderBy(expressao)
                .ToList();
        }

        private void OrdenarCampos(int cursorIndiceCabecalho = 0, TipoOrdem tipoOrdem = TipoOrdem.Crescente)
        {
            var propriedade = typeof(T).GetProperties()[cursorIndiceCabecalho];
            Func<T, object> expressao = x => propriedade.GetValue(x);

            switch (tipoOrdem)
            {
                case TipoOrdem.Crescente:

                    _dadosGrid = _dadosGrid
                        .OrderBy(expressao)
                        .ToList();

                    break;

                case TipoOrdem.Decrecente:
                    _dadosGrid = _dadosGrid
                        .OrderByDescending(expressao)
                        .ToList();
                    break;
            }

            MontarLayoutGrid(_dadosGrid, cursorIndiceCabecalho);
        }

        public void DataBinding()
        {
            _paginaAtual = PaginaInicial;
            var colunaCabecalho = 0;
            var linhaGrid = 0;
            var qtdColunas = typeof(T).GetProperties().Length - 1;

            if (Paginar)
                PaginarGrid(QuantidadeItensPagina, _paginaAtual);
            else
                MontarLayoutGrid(_dados);

            while (true)
            {
                var tecla = Console.ReadKey(true);

                switch (tecla.Key)
                {
                    case ConsoleKey.PageDown:

                        if (_paginaAtual > 1)
                        {
                            colunaCabecalho = 0;
                            --_paginaAtual;
                            PaginarGrid(QuantidadeItensPagina, _paginaAtual);
                        }

                        break;

                    case ConsoleKey.PageUp:
                        if (_paginaAtual < _totalPaginas)
                        {
                            ++_paginaAtual;
                            PaginarGrid(QuantidadeItensPagina, _paginaAtual);
                        }
                        break;

                    case ConsoleKey.LeftArrow:
                        if (colunaCabecalho > 0)
                        {
                            --colunaCabecalho;
                            MontarLayoutGrid(_dadosGrid, colunaCabecalho);
                        }
                        break;

                    case ConsoleKey.RightArrow:
                        if (colunaCabecalho < qtdColunas)
                        {
                            ++colunaCabecalho;
                            MontarLayoutGrid(_dadosGrid, colunaCabecalho);
                        }
                        break;

                    case ConsoleKey.DownArrow:
                        OrdenarCampos(colunaCabecalho, TipoOrdem.Decrecente);
                        break;

                    case ConsoleKey.UpArrow:
                        OrdenarCampos(colunaCabecalho, TipoOrdem.Crescente);
                        break;

                    case ConsoleKey.Enter:

                        var continuar = true;
                        linhaGrid = 1;

                        MontarLayoutGrid(_dadosGrid, -1, linhaGrid);

                        while (continuar)
                        {
                            tecla = Console.ReadKey(true);

                            switch (tecla.Key)
                            {
                                case ConsoleKey.DownArrow:
                                    
                                    if (linhaGrid < QuantidadeItensPagina)
                                    {
                                        ++linhaGrid;
                                        MontarLayoutGrid(_dadosGrid, -1, linhaGrid);
                                    }
                                    break;

                                case ConsoleKey.UpArrow:

                                    if (linhaGrid != 1)
                                    {
                                        --linhaGrid;
                                        MontarLayoutGrid(_dadosGrid, -1, linhaGrid);
                                    }
                                    break;

                                case ConsoleKey.Escape:
                                    continuar = false;
                                    PaginarGrid(QuantidadeItensPagina, _paginaAtual);
                                    break;
                            }
                        }

                        break;

                    case ConsoleKey.Escape:
                        return;
                }
            }
        }

        private void MontarLayoutGrid(IList<T> pagina, int colunaSelecionada = 0, int linhaSelecionada = 0)
        {
            Console.Clear();
            Console.WriteLine($"{Titulo}\n\r");

            var propriedadesTamanho = GetMaxPropertyLengths(pagina);
            var coluna = 0;

            foreach (var prop in propriedadesTamanho)
            {
                var tamanho = prop.Value - prop.Key.Length;
                var numEspacosVazios = tamanho % 2 == 0 ? tamanho / 2 : (tamanho + 1) / 2;

                if (coluna == colunaSelecionada)
                {
                    Console.BackgroundColor = ConsoleColor.Yellow;
                    Console.ForegroundColor = ConsoleColor.Black;
                }
                else
                {
                    Console.BackgroundColor = ConsoleColor.DarkYellow;
                    Console.ForegroundColor = ConsoleColor.White;
                }

                Console.Write($"|{new string(' ', numEspacosVazios)}{prop.Key}{new string(' ', numEspacosVazios)}");

                coluna++;
            }

            Console.Write("| \n");
            Console.ResetColor();

            var linha = 1;

            foreach (var itemGrid in pagina)
            {
                var propriedades = typeof(T).GetProperties();

                foreach (var prop in propriedades)
                {
                    if (linha % 2 == 0)
                    {
                        Console.BackgroundColor = ConsoleColor.DarkGray;
                        Console.ForegroundColor = ConsoleColor.White;
                    }

                    if (linha == linhaSelecionada)
                    {
                        Console.ResetColor();
                        Console.BackgroundColor = ConsoleColor.DarkGreen;
                        Console.ForegroundColor = ConsoleColor.White;
                    }

                    var maxLen = propriedadesTamanho[prop.Name];

                    var tamanho = maxLen % 2 != 0 ? maxLen + 1 : maxLen;
                    Console.Write($"|{prop.GetValue(itemGrid).ToString()?.PadRight(tamanho)}");
                }

                Console.Write("| \n");
                Console.ResetColor();

                ++linha;
            }

            Console.WriteLine($"\n\rPágina de {_paginaAtual} até {_totalPaginas}");
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
            var item = _dados.ElementAt<T>(line);
            _dados.RemoveAt(line);
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

    public enum TipoOrdem
    {
        Crescente,
        Decrecente
    }

    public enum DataGridTipoEvento
    {
        CargaDados,
        AdicaoItem,
        ExclusaoItem,
        OrdenacaoItens
    }
}
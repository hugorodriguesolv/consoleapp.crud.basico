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
        private int[] _maxColunas;

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
            var totalPaginas = Math.Ceiling((double)_dados.Count / tamanhoPagina);
            MontarLayoutGrid(_dadosGrid);
            Console.WriteLine($"Página de {++paginaAtual} até {totalPaginas}");
        }

        public void OrdenarCampos<Tkey>(Func<T, Tkey> expressao)
        {
            _dadosGrid = _dados
                .OrderBy(expressao)
                .ToList();
        }

        private void OrdenarCampos(string nomeCampo, int cursorIndiceCabecalho = 1)
        {
            var propriedade = typeof(T).GetProperty(nomeCampo);
            Func<T, object> expressao = x => propriedade.GetValue(x);

            var dadosOrdenados = _dadosGrid
                .OrderBy(expressao)
                .ToList();

            MontarLayoutGrid(dadosOrdenados, cursorIndiceCabecalho);
        }

        public void DataBinding()
        {
            var paginaAtual = PaginaInicial;
            var indexCursorCabcalho = 0;

            if (Paginar)
                PaginarGrid(QuantidadeItensPagina, paginaAtual);
            else
                MontarLayoutGrid(_dados);

            while (true)
            {
                var tecla = Console.ReadKey(true);

                switch (tecla.Key)
                {
                    case ConsoleKey.PageDown:

                        if (paginaAtual > 1)
                        {
                            --paginaAtual;
                            PaginarGrid(QuantidadeItensPagina, paginaAtual);
                        }
                        break;

                    case ConsoleKey.PageUp:
                        ++paginaAtual;
                        PaginarGrid(QuantidadeItensPagina, paginaAtual);
                        break;

                    case ConsoleKey.LeftArrow:
                        --indexCursorCabcalho;
                        OrdenarCampos("NomeDepartamento", indexCursorCabcalho);
                        break;

                    case ConsoleKey.RightArrow:
                        ++indexCursorCabcalho;
                        OrdenarCampos("NomePessoa", indexCursorCabcalho);
                        break;

                    case ConsoleKey.Escape:
                        return;
                }
            }

            //if (Paginar)
            //    PaginarGrid(_dadosGrid, QuantidadeItensPagina, PaginaInicial);
            //else
            //    MontarLayoutGrid(_dadosGrid);
        }

        private void MontarLayoutGrid(IList<T> pagina, int cursorIndiceCabecalho = 1)
        {
            Console.Clear();

            var propriedadesTamanho = GetMaxPropertyLengths(pagina);
            var indexAux = 1;

            foreach (var prop in propriedadesTamanho)
            {
                var tamanho = prop.Value - prop.Key.Length;
                var numEspacosVazios = tamanho % 2 == 0 ? tamanho / 2 : (tamanho + 1) / 2;

                if (cursorIndiceCabecalho == indexAux)
                {
                    Console.BackgroundColor = ConsoleColor.Blue;
                    Console.ForegroundColor = ConsoleColor.White;
                }
                else
                {
                    Console.ResetColor();
                }

                Console.Write($"|{new string(' ', numEspacosVazios)}{prop.Key}{new string(' ', numEspacosVazios)}");

                indexAux++;
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

    public enum DataGridTipoEvento
    {
        CargaDados,
        AdicaoItem,
        ExclusaoItem,
        OrdenacaoItens
    }
}
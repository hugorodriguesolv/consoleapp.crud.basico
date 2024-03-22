using System.Reflection;

namespace Component.Grid
{
    /// <summary>
    /// Componente Grid para telas de console
    /// </summary>
    /// <typeparam name="T">Tipo da entidade que será utilizada como </typeparam>
    public class DataGrid<T> where T : class
    {
        private IList<T> _dados = new List<T>();
        private IList<T> _dadosGrid = new List<T>();

        private string[] _cabecalho;
        private string[,] _corpoGrid;
        private double _totalPaginas;
        private int _paginaAtual;

        /// <summary>
        /// Título da Grid
        /// </summary>
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
        public bool PaginarItensGrid { get; set; } = false;

        /// <summary>
        /// O evento ocorre quando a coluna da gird é ordenada
        /// </summary>
        public event EventHandler<DataGridOrdernacaoEventArgs<T>> Ordenar;

        /// <summary>
        /// O evento ocorre quando um item da grid é excluído
        /// </summary>
        public event EventHandler<DataGridItemExcluidoEventArgs<T>> ExcluirItem;

        /// <summary>
        /// O evento ocorre quando um item é adicionado a grid
        /// </summary>
        public event EventHandler<DataGridItemAdicionadoEventArgs<T>> AdicionarItem;

        /// <summary>
        /// O evento ocorre quando a grid é paginada
        /// </summary>
        public event EventHandler<DataGridPagincaoEventArgs<T>> Paginar;

        /// <summary>
        /// O evento ocorre quando um item da grid é slecionado
        /// </summary>
        public event EventHandler<DataGridItemSelecionadoEventArgs<T>> SelecionarItem;

        /// <summary>
        /// Construtor
        /// </summary>
        public DataGrid()
        { }

        /// <summary>
        /// Construtor
        /// </summary>
        /// <param name="dadosGrid">Dados que dão a carga na grid</param>
        public DataGrid(IList<T> dadosGrid) => _dados = dadosGrid;

        /// <summary>
        /// Carrega os dados da grid
        /// </summary>
        /// <param name="dadosGrid">Dados que dão carga na grid</param>
        public void CarregarDados(IList<T> dadosGrid) => _dados = dadosGrid;

        /// <summary>
        /// Método para definir o cabeçalho
        /// </summary>
        /// <param name="args">Array de itens que serão utilizados como cabeçalho</param>
        public void DefinirCabecalho(string[] args)
        {
            _cabecalho = args;
        }

        /// <summary>
        /// Adiciona um item aos itens da grid
        /// </summary>
        /// <param name="item">Item que será adicionada aos itens existentes da grid</param>
        public void AdicionarLinha(T item)
        {
            _dados?.Add(item);

            AdicionarItem?.Invoke(this, new DataGridItemAdicionadoEventArgs<T>(item, _dados?.Count));
        }

        /// <summary>
        /// Exclui a linha da grid
        /// </summary>
        /// <param name="linha">Linha que será excluida</param>
        public void ExcluirLinha(int linha)
        {
            var item = _dados[linha];
            _dados.RemoveAt(linha);

            ExcluirItem?.Invoke(this, new DataGridItemExcluidoEventArgs<T>(item, linha));
        }

        /// <summary>
        /// Obtém o item da grid
        /// </summary>
        /// <param name="linha">Linha selecionada</param>
        private void ObterItem(int linha)
        {
            var index = linha - 1;
            var item = _dadosGrid[index];

            SelecionarItem?.Invoke(this, new DataGridItemSelecionadoEventArgs<T>(item, linha));
        }

        /// <summary>
        /// Faz a paginação da grid
        /// </summary>
        /// <param name="tamanhoPagina">Tamanho da página da grid</param>
        /// <param name="paginaAtual">Página atual da grid</param>
        private void PaginarGrid(int tamanhoPagina, int paginaAtual)
        {
            var startIndex = --paginaAtual * tamanhoPagina;
            _dadosGrid = _dados.Skip(startIndex).Take(tamanhoPagina).ToList();
            _totalPaginas = Math.Ceiling((double)_dados.Count / tamanhoPagina);
            MontarLayoutGrid(_dadosGrid);
            _paginaAtual = ++paginaAtual;

            Paginar?.Invoke(this, new DataGridPagincaoEventArgs<T>((int)_totalPaginas, paginaAtual, (paginaAtual - 1), tamanhoPagina));
        }

        /// <summary>
        /// Ordena a coluna da grid conforme a expressão
        /// </summary>
        /// <typeparam name="Tkey">Tipo dos dados</typeparam>
        /// <param name="expressao">Expressão do campo em que será ordenado</param>
        public void OrdenarCampos<Tkey>(Func<T, Tkey> expressao)
        {
            _dadosGrid = _dados
                .OrderBy(expressao)
                .ToList();
        }

        /// <summary>
        /// Ordena a coluna da grid conforme o índice do cabeçalho
        /// </summary>
        /// <param name="indiceCabecalho">Posição do índice do cabelaho</param>
        /// <param name="tipoOrdem">Tipo de ordenação</param>
        private void OrdenarCampos(int indiceCabecalho = 0, TipoOrdem tipoOrdem = TipoOrdem.Crescente)
        {
            var propriedade = typeof(T).GetProperties()[indiceCabecalho];
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

            MontarLayoutGrid(_dadosGrid, indiceCabecalho);

            Ordenar?.Invoke(this, new DataGridOrdernacaoEventArgs<T>(indiceCabecalho, tipoOrdem, _dadosGrid));
        }

        /// <summary>
        /// Exibe a grid na tela
        /// </summary>
        public void DataBinding()
        {
            _paginaAtual = PaginaInicial;
            var colunaCabecalho = 0;
            var linhaGrid = 0;
            var qtdColunas = typeof(T).GetProperties().Length - 1;

            if (PaginarItensGrid)
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

                                case ConsoleKey.Enter:
                                    ObterItem(linhaGrid);
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

        private void MontarTituloGrid()
        {
            var tituloGrid = "***************************************\n\r" +
                             $"{Titulo}\n\r" +
                             "***************************************" +
                             "\n\rPressione 'Enter' para escolher um item da grid:" +
                             "\n\rPressione 'Seta para baixo' ou 'Seta para baixo' para ordenar a grid:" +
                             "\n\rPressione 'Page Down' ou 'Page Up' para mudar a página da grid:\n\r";

            Console.WriteLine(tituloGrid);
        }

        /// <summary>
        /// Monta o layout da grid e imprime a grid na tela
        /// </summary>
        /// <param name="pagina">Página em que a grid será exibida</param>
        /// <param name="colunaSelecionada">Coluna da grid que ficará selecionada</param>
        /// <param name="linhaSelecionada">Linha da grid que ficará selecionada</param>
        private void MontarLayoutGrid(IList<T> pagina, int colunaSelecionada = 0, int linhaSelecionada = 0)
        {
            Console.Clear();
            MontarTituloGrid();

            var propriedadesTamanho = ObterTamanhoMaximoPropriedade(pagina);
            var coluna = 0;

            // Cabeçalho da grid
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

            // Itens da grid
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

            Console.WriteLine($"\n\rPágina de {_paginaAtual} até {_totalPaginas}\n\r");
        }

        /// <summary>
        /// Obtém o tamanho máximo dos valores da propriedade para ser usado no espaço para a montagem da grid
        /// </summary>
        /// <param name="items">Coleção de itens que serão analisados</param>
        /// <returns>Dicionário com todos os tamanhos máximos por coluna da grid</returns>
        private static Dictionary<string, int> ObterTamanhoMaximoPropriedade(IEnumerable<T> items)
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
    }

    /// <summary>
    /// Tipo de ordenação
    /// </summary>
    public enum TipoOrdem
    {
        Crescente,
        Decrecente
    }
}
namespace Component.Grid
{
    public class DataGridEventArgs<T> : EventArgs
    {
        public DataGridTipoEvento TipoEvento { get; }

        public DataGridEventArgs(DataGridTipoEvento tipoEvento)
        {
            TipoEvento = tipoEvento;
        }
    }

    public class DataGridOrdernacaoEventArgs<T> : DataGridEventArgs<T>
    {
        public int Coluna { get; }

        public TipoOrdem TipoOrdenacao { get; }

        public IList<T> itensOrdenados { get; }

        public DataGridOrdernacaoEventArgs(int coluna, TipoOrdem tipoOrdenacao, IList<T> itensOrdenados)
            : base(DataGridTipoEvento.OrdenacaoItens)
        {
            Coluna = coluna;
            TipoOrdenacao = tipoOrdenacao;
            this.itensOrdenados = itensOrdenados;
        }
    }

    public class DataGridPagincaoEventArgs<T> : DataGridEventArgs<T>
    {
        public int QuantidadePaginas { get; }

        public int PaginaAtual { get; }

        public int PaginaAnterior { get; }

        public int QuantidadeItensPagina { get; }

        public DataGridPagincaoEventArgs(int quantidadePaginas, int paginaAtual, int paginaAnterior, int quantidadeItensPagina)
            : base(DataGridTipoEvento.Paginacao)
        {
            QuantidadePaginas = quantidadePaginas;
            PaginaAtual = paginaAtual;
            PaginaAnterior = paginaAnterior;
            QuantidadeItensPagina = quantidadeItensPagina;
        }
    }

    public class DataGridItemAdicionadoEventArgs<T> : DataGridEventArgs<T>
    {
        public T Item { get; }

        public int? Linha { get; }

        public DataGridItemAdicionadoEventArgs(T item, int? linha)
            : base(DataGridTipoEvento.AdicaoItem)
        {
            Item = item;
            Linha = linha;
        }
    }

    public class DataGridItemExcluidoEventArgs<T> : DataGridEventArgs<T>
    {
        public T Item { get; }

        public DataGridItemExcluidoEventArgs(T item, int linha)
            : base(DataGridTipoEvento.ExclusaoItem)
        {
            Item = item;
        }
    }

    public class DataGridItemSelecionadoEventArgs<T> : DataGridEventArgs<T>
    {
        public T Item { get; }

        public int Linha { get; }

        public DataGridItemSelecionadoEventArgs(T item, int linha)
            : base(DataGridTipoEvento.SelecaoItem)
        {
            Item = item;
            Linha = linha;
        }
    }
}
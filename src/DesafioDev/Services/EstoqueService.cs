using DesafioDev.Models;

namespace DesafioDev.Services;

public class EstoqueService
{
    private readonly List<Produto> _produtos;
    private readonly List<Movimentacao> _movimentacoes = [];

    public EstoqueService(IEnumerable<Produto> produtos)
    {
        ArgumentNullException.ThrowIfNull(produtos);

        _produtos = produtos
            .Select(p => new Produto
            {
                CodigoProduto = p.CodigoProduto,
                DescricaoProduto = p.DescricaoProduto,
                Estoque = p.Estoque
            })
            .ToList();
    }

    public IReadOnlyList<Produto> ListarProdutos() => _produtos.AsReadOnly();

    public IReadOnlyList<Movimentacao> ListarMovimentacoes() => _movimentacoes.AsReadOnly();

    public Movimentacao Movimentar(
        int codigoProduto,
        TipoMovimentacao tipo,
        int quantidade,
        string descricao)
    {
        if (quantidade <= 0)
            throw new ArgumentOutOfRangeException(nameof(quantidade), "A quantidade deve ser maior que zero.");

        if (string.IsNullOrWhiteSpace(descricao))
            throw new ArgumentException("A descrição da movimentação é obrigatória.", nameof(descricao));

        var produto = _produtos.FirstOrDefault(p => p.CodigoProduto == codigoProduto)
            ?? throw new KeyNotFoundException($"Produto {codigoProduto} não encontrado.");

        switch (tipo)
        {
            case TipoMovimentacao.Entrada:
                produto.Estoque += quantidade;
                break;

            case TipoMovimentacao.Saida:
                if (quantidade > produto.Estoque)
                    throw new InvalidOperationException("Estoque insuficiente para realizar a saída.");

                produto.Estoque -= quantidade;
                break;

            default:
                throw new ArgumentOutOfRangeException(nameof(tipo), "Tipo de movimentação inválido.");
        }

        var movimentacao = new Movimentacao
        {
            CodigoProduto = codigoProduto,
            Tipo = tipo,
            Quantidade = quantidade,
            Descricao = descricao.Trim(),
            EstoqueFinal = produto.Estoque
        };

        _movimentacoes.Add(movimentacao);
        return movimentacao;
    }
}

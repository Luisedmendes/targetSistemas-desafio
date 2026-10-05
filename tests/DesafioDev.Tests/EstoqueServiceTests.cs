using DesafioDev.Models;
using DesafioDev.Services;

namespace DesafioDev.Tests;

public class EstoqueServiceTests
{
    private static EstoqueService CriarService() => new([
        new Produto
        {
            CodigoProduto = 101,
            DescricaoProduto = "Caneta Azul",
            Estoque = 10
        }
    ]);

    [Fact]
    public void EntradaDeveSomarAoEstoque()
    {
        var service = CriarService();

        var movimento = service.Movimentar(
            101,
            TipoMovimentacao.Entrada,
            5,
            "Reposição");

        Assert.Equal(15, movimento.EstoqueFinal);
        Assert.NotEqual(Guid.Empty, movimento.Id);
    }

    [Fact]
    public void SaidaDeveSubtrairDoEstoque()
    {
        var service = CriarService();

        var movimento = service.Movimentar(
            101,
            TipoMovimentacao.Saida,
            4,
            "Venda");

        Assert.Equal(6, movimento.EstoqueFinal);
    }

    [Fact]
    public void NaoDevePermitirEstoqueNegativo()
    {
        var service = CriarService();

        Assert.Throws<InvalidOperationException>(() =>
            service.Movimentar(101, TipoMovimentacao.Saida, 11, "Venda"));
    }
}

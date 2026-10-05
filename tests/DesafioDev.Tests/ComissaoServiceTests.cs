using DesafioDev.Models;
using DesafioDev.Services;

namespace DesafioDev.Tests;

public class ComissaoServiceTests
{
    private readonly ComissaoService _service = new();

    [Theory]
    [InlineData(50, 0)]
    [InlineData(99.99, 0)]
    [InlineData(100, 1)]
    [InlineData(250, 2.5)]
    [InlineData(499.99, 4.9999)]
    [InlineData(500, 25)]
    [InlineData(1000, 50)]
    public void DeveCalcularComissaoConformeFaixa(decimal valor, decimal esperado)
    {
        var resultado = _service.CalcularComissaoDaVenda(valor);
        Assert.Equal(esperado, resultado);
    }

    [Fact]
    public void DeveAgruparComissaoPorVendedor()
    {
        var vendas = new List<Venda>
        {
            new() { Vendedor = "Ana", Valor = 100m },
            new() { Vendedor = "Ana", Valor = 500m },
            new() { Vendedor = "Bruno", Valor = 50m }
        };

        var resultado = _service.CalcularPorVendedor(vendas);

        var ana = Assert.Single(resultado, r => r.Vendedor == "Ana");
        Assert.Equal(600m, ana.TotalVendas);
        Assert.Equal(26m, ana.TotalComissao);
    }
}

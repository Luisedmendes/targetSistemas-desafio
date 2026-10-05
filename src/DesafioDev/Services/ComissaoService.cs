using DesafioDev.Models;

namespace DesafioDev.Services;

public class ComissaoService
{
    public decimal CalcularComissaoDaVenda(decimal valor)
    {
        if (valor < 0)
            throw new ArgumentOutOfRangeException(nameof(valor), "O valor da venda não pode ser negativo.");

        if (valor < 100m)
            return 0m;

        if (valor < 500m)
            return valor * 0.01m;

        return valor * 0.05m;
    }

    public IReadOnlyList<ComissaoVendedor> CalcularPorVendedor(IEnumerable<Venda> vendas)
    {
        ArgumentNullException.ThrowIfNull(vendas);

        return vendas
            .GroupBy(v => v.Vendedor)
            .Select(grupo => new ComissaoVendedor(
                grupo.Key,
                grupo.Sum(v => v.Valor),
                grupo.Sum(v => CalcularComissaoDaVenda(v.Valor))))
            .OrderBy(r => r.Vendedor)
            .ToList();
    }
}

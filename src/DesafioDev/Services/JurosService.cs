using DesafioDev.Models;

namespace DesafioDev.Services;

public class JurosService
{
    public const decimal TaxaDiaria = 0.025m;

    public ResultadoJuros Calcular(decimal valor, DateTime vencimento, DateTime dataCalculo)
    {
        if (valor < 0)
            throw new ArgumentOutOfRangeException(nameof(valor), "O valor não pode ser negativo.");

        var diasAtraso = Math.Max(0, (dataCalculo.Date - vencimento.Date).Days);
        var juros = valor * TaxaDiaria * diasAtraso;
        var valorFinal = valor + juros;

        return new ResultadoJuros(
            valor,
            vencimento.Date,
            dataCalculo.Date,
            diasAtraso,
            TaxaDiaria,
            juros,
            valorFinal);
    }
}

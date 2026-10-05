namespace DesafioDev.Models;

public record ResultadoJuros(
    decimal ValorOriginal,
    DateTime DataVencimento,
    DateTime DataCalculo,
    int DiasAtraso,
    decimal TaxaDiaria,
    decimal Juros,
    decimal ValorFinal);

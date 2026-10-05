using DesafioDev.Services;

namespace DesafioDev.Tests;

public class JurosServiceTests
{
    private readonly JurosService _service = new();

    [Fact]
    public void DeveCalcularJurosSimplesDeDoisEMeioPorCentoAoDia()
    {
        var resultado = _service.Calcular(
            1000m,
            new DateTime(2026, 10, 1),
            new DateTime(2026, 10, 5));

        Assert.Equal(4, resultado.DiasAtraso);
        Assert.Equal(100m, resultado.Juros);
        Assert.Equal(1100m, resultado.ValorFinal);
    }

    [Fact]
    public void NaoDeveCobrarJurosAntesDoVencimento()
    {
        var resultado = _service.Calcular(
            1000m,
            new DateTime(2026, 10, 10),
            new DateTime(2026, 10, 5));

        Assert.Equal(0, resultado.DiasAtraso);
        Assert.Equal(0m, resultado.Juros);
        Assert.Equal(1000m, resultado.ValorFinal);
    }
}

using System.Text.Json;

namespace DesafioDev.Infrastructure;

public static class JsonDataLoader
{
    private static readonly JsonSerializerOptions Options = new()
    {
        PropertyNameCaseInsensitive = true
    };

    public static T Carregar<T>(string caminho)
    {
        if (!File.Exists(caminho))
            throw new FileNotFoundException($"Arquivo JSON não encontrado: {caminho}");

        var json = File.ReadAllText(caminho);

        return JsonSerializer.Deserialize<T>(json, Options)
            ?? throw new InvalidOperationException($"Não foi possível desserializar o arquivo: {caminho}");
    }
}

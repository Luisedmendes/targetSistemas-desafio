using System.Globalization;
using DesafioDev.Infrastructure;
using DesafioDev.Models;
using DesafioDev.Services;

var dataDirectory = Path.Combine(AppContext.BaseDirectory, "Data");
var vendasPath = Path.Combine(dataDirectory, "vendas.json");
var estoquePath = Path.Combine(dataDirectory, "estoque.json");

var dadosVendas = JsonDataLoader.Carregar<DadosVendas>(vendasPath);
var dadosEstoque = JsonDataLoader.Carregar<DadosEstoque>(estoquePath);

var comissaoService = new ComissaoService();
var estoqueService = new EstoqueService(dadosEstoque.Estoque);
var jurosService = new JurosService();

while (true)
{
    Console.Clear();
    Console.WriteLine("=== DESAFIO DEV ===");
    Console.WriteLine("1 - Calcular comissões");
    Console.WriteLine("2 - Movimentar estoque");
    Console.WriteLine("3 - Calcular juros por atraso");
    Console.WriteLine("0 - Sair");
    Console.Write("\nEscolha uma opção: ");

    switch (Console.ReadLine())
    {
        case "1":
            ExibirComissoes();
            break;
        case "2":
            MovimentarEstoque();
            break;
        case "3":
            CalcularJuros();
            break;
        case "0":
            return;
        default:
            Console.WriteLine("Opção inválida.");
            break;
    }

    Console.WriteLine("\nPressione ENTER para continuar...");
    Console.ReadLine();
}

void ExibirComissoes()
{
    Console.WriteLine("\n=== COMISSÕES POR VENDEDOR ===\n");

    var resultados = comissaoService.CalcularPorVendedor(dadosVendas.Vendas);

    foreach (var resultado in resultados)
    {
        Console.WriteLine($"Vendedor: {resultado.Vendedor}");
        Console.WriteLine($"Total vendido: {resultado.TotalVendas:C2}");
        Console.WriteLine($"Comissão: {resultado.TotalComissao:C2}");
        Console.WriteLine(new string('-', 35));
    }
}

void MovimentarEstoque()
{
    Console.WriteLine("\n=== ESTOQUE ===\n");

    foreach (var produto in estoqueService.ListarProdutos())
        Console.WriteLine($"{produto.CodigoProduto} - {produto.DescricaoProduto} | Estoque: {produto.Estoque}");

    Console.Write("\nCódigo do produto: ");
    if (!int.TryParse(Console.ReadLine(), out var codigoProduto))
    {
        Console.WriteLine("Código inválido.");
        return;
    }

    Console.WriteLine("1 - Entrada");
    Console.WriteLine("2 - Saída");
    Console.Write("Tipo: ");

    if (!int.TryParse(Console.ReadLine(), out var tipoNumero) ||
        !Enum.IsDefined(typeof(TipoMovimentacao), tipoNumero))
    {
        Console.WriteLine("Tipo de movimentação inválido.");
        return;
    }

    Console.Write("Quantidade: ");
    if (!int.TryParse(Console.ReadLine(), out var quantidade))
    {
        Console.WriteLine("Quantidade inválida.");
        return;
    }

    Console.Write("Descrição: ");
    var descricao = Console.ReadLine() ?? string.Empty;

    try
    {
        var movimentacao = estoqueService.Movimentar(
            codigoProduto,
            (TipoMovimentacao)tipoNumero,
            quantidade,
            descricao);

        Console.WriteLine("\nMovimentação realizada com sucesso.");
        Console.WriteLine($"ID: {movimentacao.Id}");
        Console.WriteLine($"Tipo: {movimentacao.Tipo}");
        Console.WriteLine($"Quantidade: {movimentacao.Quantidade}");
        Console.WriteLine($"Estoque final: {movimentacao.EstoqueFinal}");
    }
    catch (Exception ex) when (ex is ArgumentException or InvalidOperationException or KeyNotFoundException)
    {
        Console.WriteLine($"Erro: {ex.Message}");
    }
}

void CalcularJuros()
{
    Console.WriteLine("\n=== CÁLCULO DE JUROS ===\n");

    Console.Write("Valor: R$ ");
    if (!decimal.TryParse(Console.ReadLine(), NumberStyles.Number, CultureInfo.CurrentCulture, out var valor))
    {
        Console.WriteLine("Valor inválido.");
        return;
    }

    Console.Write("Data de vencimento (dd/MM/yyyy): ");
    if (!DateTime.TryParseExact(
            Console.ReadLine(),
            "dd/MM/yyyy",
            CultureInfo.InvariantCulture,
            DateTimeStyles.None,
            out var vencimento))
    {
        Console.WriteLine("Data inválida.");
        return;
    }

    try
    {
        var resultado = jurosService.Calcular(valor, vencimento, DateTime.Today);

        Console.WriteLine($"\nDias em atraso: {resultado.DiasAtraso}");
        Console.WriteLine($"Taxa diária: {resultado.TaxaDiaria:P1}");
        Console.WriteLine($"Juros: {resultado.Juros:C2}");
        Console.WriteLine($"Valor final: {resultado.ValorFinal:C2}");
    }
    catch (ArgumentOutOfRangeException ex)
    {
        Console.WriteLine($"Erro: {ex.Message}");
    }
}

# Desafio Dev - C# / .NET 8

Solução dos três exercícios propostos no desafio:

1. cálculo de comissão por vendedor;
2. entrada e saída de estoque;
3. cálculo de juros de 2,5% ao dia sobre valores vencidos.

## Estrutura

```text
DesafioDev/
├── src/
│   └── DesafioDev/
│       ├── Data/
│       │   ├── vendas.json
│       │   └── estoque.json
│       ├── Infrastructure/
│       │   └── JsonDataLoader.cs
│       ├── Models/
│       ├── Services/
│       │   ├── ComissaoService.cs
│       │   ├── EstoqueService.cs
│       │   └── JurosService.cs
│       └── Program.cs
├── tests/
│   └── DesafioDev.Tests/
└── DesafioDev.sln
```

## Requisitos

- .NET SDK 8.0 ou superior.

## Executar

Na raiz do projeto:

```bash
dotnet restore
dotnet run --project src/DesafioDev
```

## Testes

```bash
dotnet test
```

## Decisões de implementação

### Comissão

A comissão é calculada individualmente para cada venda:

- abaixo de R$ 100,00: sem comissão;
- de R$ 100,00 até abaixo de R$ 500,00: 1%;
- a partir de R$ 500,00: 5%.

Depois, os valores são agrupados por vendedor.

### Estoque

Cada movimentação possui:

- identificador único (`Guid`);
- produto;
- tipo (`Entrada` ou `Saida`);
- quantidade;
- descrição;
- data;
- estoque final.

A aplicação impede saída maior que o estoque disponível.

### Juros

Foi adotado juros simples de 2,5% ao dia:

```text
juros = valor × 0,025 × diasEmAtraso
```

Se a data de vencimento ainda não passou, os juros são zero.

using Grpc.Net.Client;
using VotingSystem.Voting;

var server = Environment.GetEnvironmentVariable("GRPC_SERVER")
             ?? "http://localhost:9091";

using var channel = GrpcChannel.ForAddress(server);
var client = new VotingService.VotingServiceClient(channel);

Console.WriteLine("=== Client.Apuramento (AA) ===");
Console.WriteLine($"Servidor: {server}");

try
{
    var response = client.GetResults(new GetResultsRequest());

    Console.WriteLine("\n--- Resultados ---");

    if (response.Results.Count == 0)
    {
        Console.WriteLine("Sem resultados disponíveis.");
    }
    else
    {
        foreach (var r in response.Results)
        {
            Console.WriteLine($"{r.Id} - {r.Name} : {r.Votes} voto(s)");
        }
    }
}
catch (Exception ex)
{
    Console.WriteLine("Erro ao obter resultados:");
    Console.WriteLine(ex.Message);
}

Console.WriteLine("\nPrima ENTER para sair...");
Console.ReadLine();

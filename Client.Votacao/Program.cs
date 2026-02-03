using Grpc.Core;
using Grpc.Net.Client;
using VotingSystem.Voting;

static string Ask(string label)
{
    Console.Write(label);
    return Console.ReadLine()?.Trim() ?? "";
}

static int AskInt(string label)
{
    while (true)
    {
        var s = Ask(label);
        if (int.TryParse(s, out var n)) return n;
        Console.WriteLine("Valor inválido. Tenta novamente.");
    }
}

var server = Environment.GetEnvironmentVariable("GRPC_SERVER") ?? "http://localhost:9091";

using var channel = GrpcChannel.ForAddress(server);
var client = new VotingService.VotingServiceClient(channel);

Console.WriteLine($"[Client.Votacao] Server: {server}");

while (true)
{
    Console.WriteLine("\n1) GetCandidates  2) Vote  0) Exit");
    var opt = Ask("Option: ");

    if (opt == "0") break;

    try
    {
        if (opt == "1")
        {
            var resp = client.GetCandidates(new GetCandidatesRequest());

            Console.WriteLine("\n--- Candidates ---");
            if (resp?.Candidates == null || resp.Candidates.Count == 0)
            {
                Console.WriteLine("(sem candidatos)");
            }
            else
            {
                foreach (var c in resp.Candidates)
                {
                    Console.WriteLine($"{c.Id} - {c.Name}");
                }
            }
        }
        else if (opt == "2")
        {
            var credential = Ask("Voting credential: ");
            var candidateId = AskInt("Candidate id: ");

            var resp = client.Vote(new VoteRequest
            {
                VotingCredential = credential,
                CandidateId = candidateId
            });

            Console.WriteLine($"\nSuccess: {resp.Success}");
            Console.WriteLine($"Message: {resp.Message}");
        }
        else if (opt == "3")
        {
            var resp = client.GetResults(new GetResultsRequest());

            Console.WriteLine("\n--- Results ---");
            if (resp?.Results == null || resp.Results.Count == 0)
            {
                Console.WriteLine("(sem resultados)");
            }
            else
            {
                foreach (var r in resp.Results)
                {
                    Console.WriteLine($"{r.Id} - {r.Name} : {r.Votes} vote(s)");
                }
            }
        }
        else
        {
            Console.WriteLine("Opção inválida.");
        }
    }
    catch (RpcException ex)
    {
        Console.WriteLine($"\n[gRPC ERROR] {ex.StatusCode} - {ex.Status.Detail}");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"\n[ERROR] {ex.Message}");
    }
}


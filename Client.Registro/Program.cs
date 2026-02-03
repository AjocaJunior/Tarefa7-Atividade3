using Grpc.Core;
using Grpc.Net.Client;
using VotingSystem.Registration;

static string Ask(string label)
{
    Console.Write(label);
    return Console.ReadLine()?.Trim() ?? "";
}

var server = Environment.GetEnvironmentVariable("GRPC_SERVER") ?? "http://localhost:9093";
using var channel = GrpcChannel.ForAddress(server);

var client = new VoterRegistrationService.VoterRegistrationServiceClient(channel);

Console.WriteLine($"[Client.Registro] Server: {server}");

while (true)
{
    Console.WriteLine("\n1) IssueVotingCredential  0) Exit");
    var opt = Ask("Option: ");

    if (opt == "0") break;

    try
    {
        if (opt == "1")
        {
            var cc = Ask("Citizen Card Number: ");

            var reply = await client.IssueVotingCredentialAsync(new VoterRequest
            {
                CitizenCardNumber = cc
            });

            Console.WriteLine($"isEligible={reply.IsEligible}");
            Console.WriteLine($"credential={reply.VotingCredential}");
        }
        else
        {
            Console.WriteLine("Invalid option.");
        }
    }
    catch (RpcException ex)
    {
        Console.WriteLine($"RPC Error: {ex.Status.StatusCode} - {ex.Status.Detail}");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error: {ex.Message}");
    }
}

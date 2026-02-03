# Integração de Sistemas – Tarefa 7 – Atividade III


Este repositório contém:
- *VotingSystem.RegistrationService* (serviço gRPC de Registo) — `http://localhost:9093`
- *VotingSystem.VotingSevice* (serviço gRPC de Votação) — `http://localhost:9091`
- *Client.Registro* (Console App .NET) — testa `IssueVotingCredential`
- *Client.Votacao* (Console App .NET) — testa `GetCandidates`, `Vote`
- *Client.Apuramento* (Console App .NET) — testa `GetResults`

- Conforme o enunciado da Tarefa 7 - Atividade III, os serviços gRPC são executados a partir do repositório VotingSystem.
Este repositório contém apenas os *clientes* e os ficheiros *.proto* necessários.

---

# Estrutura do Repositório

|- Protos
| |- voter.proto
| |- voting.proto
|- Client.Registro
|- Client.Votacao
|- Client.Apuramento

# Tecnologias Utilizadas
- ASP.NET Core gRPC
- Protocol Buffers (.proto)
- grpcurl
- .NET 10 (Console Applications)
- Visual Studio Community 2026 (Opcional)

# Pré-requisitos
- .NET SDK 8 ou superior
- grpcurl (https://github.com/fullstorydev/grpcurl)
- Windows (CMD ou PowerShell)


# Como Executar

1. Fazer download/clonar o repositório:
   - `https://github.com/arsenioreis/VotingSystem.git`

2. Executar *AR – RegistrationService* na porta **9093** (num terminal):
   ```cmd
   cd "C:\Users\[USER]\VSProjects\VotingSystem\VotingSystem.RegistrationService>"
   dotnet run --urls http://localhost:9093```


3. Executar *AV – VotingSevice* na porta **9091** (noutro terminal):
   ```cmd
   cd "C:\Users\[USER]\VSProjects\VotingSystem\VotingSystem.VotingService"
   dotnet run --urls http://localhost:9091```

# Testar serviços com grpcurl, assumindo que os ficheiros estão todos em .\Protos\

1. GetCandidates (AV – 9091)
```cmd
grpcurl -plaintext -import-path ".\Protos" -proto "voting.proto" localhost:9091 voting.VotingService/GetCandidates```

2. IssueVotingCredential (AR – 9093)
```cmd
grpcurl -plaintext -import-path ".\Protos" -proto "voter.proto" -d "{ \"citizen_card_number\": \"123456789\" }" localhost:9093 voting.VoterRegistrationService/IssueVotingCredential```

3. Vote (AV – 9091) - CRED-GHI-789 - pode ser substituída por uma credencial válida obtida no passado 2
```cmd
grpcurl -plaintext -import-path ".\Protos" -proto "voting.proto" -d "{ \"voting_credential\": \"CRED-GHI-789\", \"candidate_id\": 1 }" localhost:9091 voting.VotingService/Vote```

4. GetResults (AA – 9091)
```cmd
grpcurl -plaintext -import-path ".\Protos" -proto "voting.proto" localhost:9091 voting.VotingService/GetResults```

# Executar os clientes (.NET Console)
```cmd
cd Client.Registro
set GRPC_SERVER=http://localhost:9093
dotnet run
```

```cmd
cd Client.Votacao
set GRPC_SERVER=http://localhost:9091
dotnet run
```

```cmd
cd Client.Apuramento
set GRPC_SERVER=http://localhost:9091
dotnet run
```

# Testes da UI

- Executar: VotingClient.GUI.csproj no VISUAL STUDIO 2026


Os clientes ligam-se aos endpoints locais:

AR: http://localhost:9093

AV e AA: http://localhost:9091

@author Assis Caetano

# dotnet10-benchmarkdotnet-githubactions-testcontainers-postgres
Workflow do GitHub Actions utilizando BenchmarkDotNet para avaliar a performance na inclusão de registros + Aplicação de testes que faz uso de .NET 10, Testcontainers, PostgreSQL, Dapper, ADO e Entity Framework Core.

---

Inicialização do container:

![Inicialização do container](img/testcontainers-01.png)

Um dos possíveis resultados (15 registros de contatos por empresa):

![Resultado 1](img/testcontainers-02.png)

Outro dos possíveis resultados (2 registros de contatos por empresa):

![Resultado 2](img/testcontainers-03.png)

Alguns dados fake gerados como resultados dos testes:

![Dados fake](img/testcontainers-04.png)

Um possível resultado da execução do workflow do GitHub Actions:

![Workflow do GitHub Actions](img/testcontainers-05.png)
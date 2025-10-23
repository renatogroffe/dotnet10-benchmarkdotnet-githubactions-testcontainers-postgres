using BenchmarkDotNet.Running;
using BenchmarkingDapperEFCoreCRM;
using BenchmarkingDapperEFCoreCRM.Tests;
using BenchmarkingDapperEFCoreCRM.Utils;
using DotNet.Testcontainers.Builders;
using Testcontainers.PostgreSql;


Console.WriteLine("Numero de contatos por empresa/companhia definido para os testes: " +
    Environment.GetEnvironmentVariable("NumeroContatosPorCompanhia"));
Console.WriteLine();

CommandLineHelper.Execute("docker images",
    "Imagens antes da execucao do Testcontainers...");
CommandLineHelper.Execute("docker container ls",
    "Containers antes da execucao do Testcontainers...");

Console.WriteLine("Criando container para uso do PostgreSQL...");
var postgresContainer = new PostgreSqlBuilder()
    .WithImage("postgres:17.6")
    .WithResourceMapping(
        DBFileAsByteArray.GetContent("basecrmado.sql"),
        "/docker-entrypoint-initdb.d/01-basecrmado.sql")
    .WithResourceMapping(
        DBFileAsByteArray.GetContent("basecrmdapper.sql"),
        "/docker-entrypoint-initdb.d/02-basecrmdapper.sql")
    .WithResourceMapping(
        DBFileAsByteArray.GetContent("basecrmef.sql"),
        "/docker-entrypoint-initdb.d/03-basecrmef.sql")
  .WithWaitStrategy(Wait.ForUnixContainer().UntilCommandIsCompleted("pg_isready"))
  .Build();
await postgresContainer.StartAsync();

CommandLineHelper.Execute("docker images",
    "Imagens apos execucao do Testcontainers...");
CommandLineHelper.Execute("docker container ls",
    "Containers apos execucao do Testcontainers...");

var connectionString = postgresContainer.GetConnectionString();
Console.WriteLine($"Connection String da base de dados postgres: {connectionString}");
Configurations.Load(connectionString);

Console.WriteLine($"Versao utilizada do Postgres:");
var resultSelectPostgresVersion = await postgresContainer.ExecScriptAsync(
    "SELECT version();");
Console.WriteLine(resultSelectPostgresVersion.Stdout);

new BenchmarkSwitcher([typeof(CRMTests)]).Run(args);

string[] databases = [ "basecrmef", "basecrmdapper", "basecrmado"];
foreach (string database in databases)
{
    Console.WriteLine();
    Console.WriteLine($"# Amostragem de registros criados na base {database}");

    Console.WriteLine();
    Console.WriteLine("*** Empresas ***");
    var resultSelectEmpresas = await postgresContainer.ExecAsync(new[]
    {
        "psql",
        "-d", database,
        "-U", "postgres",
        "-c", "SELECT * FROM \"Empresas\" LIMIT 10;"
    });
    Console.WriteLine(resultSelectEmpresas.Stdout);

    Console.WriteLine();
    Console.WriteLine("*** Contatos ***");
    var resultSelect = await postgresContainer.ExecAsync(new[]
    {
        "psql",
        "-d", database,
        "-U", "postgres",
        "-c", "SELECT * FROM \"Contatos\" LIMIT 30;"
    });
    Console.WriteLine(resultSelect.Stdout);
}

if (Environment.GetEnvironmentVariable("ExecucaoManual") == "true")
{

    Console.WriteLine();
    Console.WriteLine($"Connection String da base de dados postgres: {connectionString}");

    Console.WriteLine();
    Console.WriteLine("Pressione ENTER para interromper a execucao do container...");
    Console.ReadLine();

    await postgresContainer.StopAsync();
    Console.WriteLine("Pressione ENTER para encerrar a aplicacao...");
    Console.ReadLine();
}

CommandLineHelper.Execute("docker ps -a",
    "Containers ao finalizar a aplicacao...");

return Environment.ExitCode;
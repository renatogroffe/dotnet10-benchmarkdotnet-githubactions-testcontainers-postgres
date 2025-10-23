$env:NumeroContatosPorCompanhia = "2"
$env:ExecucaoManual = "true"
dotnet run --filter BenchmarkingDapperEFCoreCRM.Tests.* -c Release
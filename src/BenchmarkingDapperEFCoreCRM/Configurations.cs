namespace BenchmarkingDapperEFCoreCRM;

public static class Configurations
{
    public static string BasePostgres => Environment.GetEnvironmentVariable("BasePostgresConnectionString")!;
    public static string BaseEFCore => Environment.GetEnvironmentVariable("BaseEFCoreConnectionString")!;
    public static string BaseDapper => Environment.GetEnvironmentVariable("BaseDapperConnectionString")!;
    public static string BaseADO => Environment.GetEnvironmentVariable("BaseADOConnectionString")!;

    public static void Load(string connectionStringBasePostgres)
    {
        Environment.SetEnvironmentVariable("BasePostgresConnectionString", connectionStringBasePostgres);
        Environment.SetEnvironmentVariable("BaseEFCoreConnectionString",
            connectionStringBasePostgres.Replace(";Database=postgres;", ";Database=basecrmef;"));
        Environment.SetEnvironmentVariable("BaseDapperConnectionString",
            connectionStringBasePostgres.Replace(";Database=postgres;", ";Database=basecrmdapper;"));
        Environment.SetEnvironmentVariable("BaseADOConnectionString",
            connectionStringBasePostgres.Replace(";Database=postgres;", ";Database=basecrmado;"));
    }
}
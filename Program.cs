using MCPServer;
using System.Reflection;
using MSSQLMCP;
using System;

if (args.Length > 0 && args[0] == "--test-connection")
{
    string connectionString = "Data Source=localhost;Integrated Security=True;Persist Security Info=False;Pooling=False;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=True;Application Name=\"SQL Server Management Studio\";Command Timeout=0";
    Console.WriteLine("Testing MSSQL Connection...");
    try 
    {
        var tables = MSSQLTools.ListTables(connectionString);
        Console.WriteLine("Tables fetched successfully:");
        Console.WriteLine(tables);
    } 
    catch (Exception ex) 
    {
        Console.WriteLine("Error fetching tables: " + ex.Message);
    }
    return;
}

var options = new McpServerOptions 
{ 
    ServerName = "MSSQL MCP Server", 
    ServerVersion = "1.0.0" 
};

var host = new McpServerHost(Assembly.GetExecutingAssembly(), options);
host.Run();

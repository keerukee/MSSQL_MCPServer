# MSSQL_MCPServer

A Model Context Protocol (MCP) Server for Microsoft SQL Server that enables AI assistants to interact with MSSQL databases through standardized tools.

## Overview

This MCP server provides a set of tools that allow AI assistants (like GitHub Copilot, Claude, etc.) to execute SQL queries, manage data, and explore database schemas in Microsoft SQL Server databases.

## Features

The server exposes the following MCP tools:

| Tool | Description |
|------|-------------|
| `MSSQLExecuteQuery` | Executes a SELECT query and returns results as JSON |
| `MSSQLExecuteNonQuery` | Executes INSERT, UPDATE, or DELETE commands and returns affected row count |
| `MSSQLListTables` | Lists all user tables in the database |
| `MSSQLGetTableSchema` | Describes the structure of a specific table (columns, data types, nullability) |

## Requirements

- .NET 10.0 SDK
- Microsoft SQL Server (any supported version)
- Valid SQL Server connection string

## Dependencies

- `Keerukee.MCPServer.Stdio` (v1.0.0) - MCP Server framework for stdio communication
- `Microsoft.Data.SqlClient` (v6.0.1) - SQL Server data provider

## Installation

1. Clone the repository:
   ```bash
   git clone https://github.com/keerukee/MSSQL_MCPServer.git
   cd MSSQL_MCPServer
   ```

2. Build the project:
   ```bash
   dotnet build
   ```

3. Publish the application:
   ```bash
   dotnet publish -c Release
   ```

## Usage

### Running as MCP Server

The server communicates via stdio and can be configured in your MCP client (e.g., VS Code, Claude Desktop):

```json
{
  "mcpServers": {
    "mssql": {
      "command": "dotnet",
      "args": ["run", "--project", "path/to/MSSQLMCP.csproj"]
    }
  }
}
```

### Testing Connection

You can test your SQL Server connection using the built-in test mode:

```bash
dotnet run -- --test-connection
```

Note: Update the connection string in `Program.cs` for the test connection, or modify it as needed.

## Tool Parameters

All tools require a `connectionString` parameter. Example connection string:

```
Data Source=localhost;Initial Catalog=MyDatabase;Integrated Security=True;TrustServerCertificate=True
```

### MSSQLExecuteQuery
- `connectionString` (required): The MSSQL connection string
- `query` (required): The SQL SELECT query to execute

### MSSQLExecuteNonQuery
- `connectionString` (required): The MSSQL connection string
- `command` (required): The SQL command to execute (INSERT/UPDATE/DELETE)

### MSSQLListTables
- `connectionString` (required): The MSSQL connection string

### MSSQLGetTableSchema
- `connectionString` (required): The MSSQL connection string
- `tableName` (required): The name of the table to describe

## License

MIT License

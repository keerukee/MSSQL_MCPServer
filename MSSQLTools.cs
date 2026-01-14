using MCPServer.Attributes;
using Microsoft.Data.SqlClient;
using System.Text.Json;
using System.Threading.Tasks;
using System.Collections.Generic;
using System;

namespace MSSQLMCP;

public class MSSQLTools
{
    [McpTool("MSSQLExecuteQuery", "Executes a SELECT query on the MSSQL database and returns results as JSON.")]
    public static string ExecuteQuery(
        [McpParameter("The MSSQL connection string", true)] string connectionString,
        [McpParameter("The SQL SELECT query to execute", true)] string query)
    {
        try
        {
            using var connection = new SqlConnection(connectionString);
            connection.Open();

            using var command = new SqlCommand(query, connection);
            using var reader = command.ExecuteReader();
            
            var results = new List<Dictionary<string, object>>();
            
            while (reader.Read())
            {
                var row = new Dictionary<string, object>();
                for (int i = 0; i < reader.FieldCount; i++)
                {
                    row[reader.GetName(i)] = reader.GetValue(i);
                }
                results.Add(row);
            }

            return JsonSerializer.Serialize(results, new JsonSerializerOptions { WriteIndented = true });
        }
        catch (Exception ex)
        {
            return $"Error: {ex.Message}";
        }
    }

    [McpTool("MSSQLExecuteNonQuery", "Executes an INSERT, UPDATE, or DELETE command. Returns the number of affected rows.")]
    public static string ExecuteNonQuery(
        [McpParameter("The MSSQL connection string", true)] string connectionString,
        [McpParameter("The SQL command to execute", true)] string command)
    {
        try
        {
            using var connection = new SqlConnection(connectionString);
            connection.Open();

            using var sqlCommand = new SqlCommand(command, connection);
            int rowsAffected = sqlCommand.ExecuteNonQuery();
            
            return $"Rows affected: {rowsAffected}";
        }
        catch (Exception ex)
        {
            return $"Error: {ex.Message}";
        }
    }

    [McpTool("MSSQLListTables", "Lists all user tables in the database.")]
    public static string ListTables(
        [McpParameter("The MSSQL connection string", true)] string connectionString)
    {
        try
        {
            using var connection = new SqlConnection(connectionString);
            connection.Open();

            var tables = new List<string>();
            // Query to list tables in MSSQL
            string query = "SELECT TABLE_NAME FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_TYPE = 'BASE TABLE'";
            
            using var command = new SqlCommand(query, connection);
            using var reader = command.ExecuteReader();
            
            while (reader.Read())
            {
                if (!reader.IsDBNull(0))
                {
                    tables.Add(reader.GetString(0));
                }
            }

            return JsonSerializer.Serialize(tables, new JsonSerializerOptions { WriteIndented = true });
        }
        catch (Exception ex)
        {
            return $"Error: {ex.Message}";
        }
    }

    [McpTool("MSSQLGetTableSchema", "Describes the structure of a specific table.")]
    public static string GetTableSchema(
        [McpParameter("The MSSQL connection string", true)] string connectionString,
        [McpParameter("The name of the table to describe", true)] string tableName)
    {
        try
        {
            using var connection = new SqlConnection(connectionString);
            connection.Open();

            var schema = new List<Dictionary<string, object>>();
            // Query to get column info in MSSQL
            string query = "SELECT COLUMN_NAME, DATA_TYPE, IS_NULLABLE FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = @tableName";

            using var command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@tableName", tableName);
            using var reader = command.ExecuteReader();
            
            while (reader.Read())
            {
                var row = new Dictionary<string, object>();
                for (int i = 0; i < reader.FieldCount; i++)
                {
                    row[reader.GetName(i)] = reader.GetValue(i);
                }
                schema.Add(row);
            }

            return JsonSerializer.Serialize(schema, new JsonSerializerOptions { WriteIndented = true });
        }
        catch (Exception ex)
        {
            return $"Error: {ex.Message}";
        }
    }
}

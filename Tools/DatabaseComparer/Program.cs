using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DatabaseSchemaComparer
{
    /// <summary>
    /// Compares SQLite database schemas and data content
    /// </summary>
    internal enum DiffKind { Removed, Added, Modified }

    internal record SchemaDiffEntry(
        DiffKind Kind,
        bool IsConstraint,
        string Name,
        string? DefinitionDb1,
        string? DefinitionDb2);

    internal class Program
    {
        // Configure which tables to compare data content
        private static readonly List<string> TablesToCompareData = new()
        {
            "CategoriesOfFood",
            "DevicesModels",
            "Manufacturers",
            "Foods",
            "InsulinDrugs",
            "Manufacturers",
            "Parameters",
            "PositionsOfReferences",
            "UnitsOfFood",
            // Add or remove table names as needed
        };

        static void Main(string[] args)
        {
            string db1 = @"C:\Users\gabri\OneDrive\Salute\Diabete\Dati\GlucoMan\2026-08-30 Cartella_Glucoman_da telefono\GlucoManData.Sqlite";
            string db2 = @"C:\Users\gabri\Documents\GlucoMan\Data\GlucoManData.Sqlite";

            Console.WriteLine("╔════════════════════════════════════════════════════════╗");
            Console.WriteLine("║   SQLite Database Schema & Content Comparer            ║");
            Console.WriteLine("╚════════════════════════════════════════════════════════╝\n");

            try
            {
                CompareSchemas(db1, db2);
                Console.WriteLine("\n" + new string('═', 60) + "\n");
                CompareTableData(db1, db2);
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Error: {ex.Message}");
                Console.ResetColor();
                Environment.Exit(1);
            }

            Console.WriteLine("\n✓ Comparison completed.");
        }

        private static void CompareSchemas(string db1Path, string db2Path)
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("▶ SCHEMA COMPARISON\n");
            Console.ResetColor();

            var schema1 = GetDatabaseSchema(db1Path);
            var schema2 = GetDatabaseSchema(db2Path);

            var onlyInDb1 = schema1.Keys.Except(schema2.Keys).ToList();
            if (onlyInDb1.Any())
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("Tables ONLY in DB1:");
                Console.ResetColor();
                foreach (var table in onlyInDb1)
                {
                    Console.WriteLine($"  ✗ {table}");
                }
                Console.WriteLine();
            }

            var onlyInDb2 = schema2.Keys.Except(schema1.Keys).ToList();
            if (onlyInDb2.Any())
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("Tables ONLY in DB2:");
                Console.ResetColor();
                foreach (var table in onlyInDb2)
                {
                    Console.WriteLine($"  ✗ {table}");
                }
                Console.WriteLine();
            }

            var commonTables = schema1.Keys.Intersect(schema2.Keys).ToList();
            var differentTables = new List<string>();

            foreach (var table in commonTables)
            {
                if (NormalizeSqlForComparison(schema1[table]) != NormalizeSqlForComparison(schema2[table]))
                {
                    differentTables.Add(table);
                }
            }

            if (differentTables.Any())
            {
                Console.ForegroundColor = ConsoleColor.Magenta;
                Console.WriteLine($"Tables with DIFFERENT structure ({differentTables.Count}):");
                Console.ResetColor();

                foreach (var table in differentTables)
                {
                    Console.WriteLine($"\n  ▼ {table}");

                    var diff = GetSchemaDiff(schema1[table], schema2[table]);
                    if (diff.Count == 0)
                    {
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                        Console.WriteLine("    Only formatting differs (whitespace/line breaks): structure is identical.");
                        Console.ResetColor();
                        continue;
                    }

                    foreach (var entry in diff)
                    {
                        string kind = entry.Kind switch
                        {
                            DiffKind.Removed  => "-",
                            DiffKind.Added    => "+",
                            DiffKind.Modified => "~",
                            _                 => "?"
                        };
                        string category = entry.IsConstraint ? "Constraint" : "Column";

                        switch (entry.Kind)
                        {
                            case DiffKind.Removed:
                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.WriteLine($"    {kind} {category} removed:  {entry.Name}");
                                Console.WriteLine($"         DB1: {entry.DefinitionDb1}");
                                Console.ResetColor();
                                break;

                            case DiffKind.Added:
                                Console.ForegroundColor = ConsoleColor.Green;
                                Console.WriteLine($"    {kind} {category} added:    {entry.Name}");
                                Console.WriteLine($"         DB2: {entry.DefinitionDb2}");
                                Console.ResetColor();
                                break;

                            case DiffKind.Modified:
                                Console.ForegroundColor = ConsoleColor.Yellow;
                                Console.WriteLine($"    {kind} {category} modified: {entry.Name}");
                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.WriteLine($"         DB1: {entry.DefinitionDb1}");
                                Console.ForegroundColor = ConsoleColor.Green;
                                Console.WriteLine($"         DB2: {entry.DefinitionDb2}");
                                Console.ResetColor();
                                break;
                        }
                    }
                }
            }

            if (!onlyInDb1.Any() && !onlyInDb2.Any() && !differentTables.Any())
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("✓ All common tables have identical schemas.");
                Console.ResetColor();
            }
        }

        private static string NormalizeSqlForComparison(string sql)
        {
            if (string.IsNullOrWhiteSpace(sql))
                return string.Empty;

            var cleaned = sql.Replace("\r", " ").Replace("\n", " ");
            cleaned = System.Text.RegularExpressions.Regex.Replace(cleaned, @"\s+", " ");
            cleaned = cleaned.Replace("`", string.Empty);
            return cleaned.Trim();
        }

        private static List<SchemaDiffEntry> GetSchemaDiff(string sql1, string sql2)
        {
            var parts1 = SplitCreateTableParts(sql1);
            var parts2 = SplitCreateTableParts(sql2);

            var dict1 = parts1
                .Select(p => new { Key = ExtractDefinitionName(p), Value = NormalizeSqlForComparison(p) })
                .Where(x => !string.IsNullOrWhiteSpace(x.Key))
                .ToDictionary(x => x.Key, x => x.Value, StringComparer.OrdinalIgnoreCase);

            var dict2 = parts2
                .Select(p => new { Key = ExtractDefinitionName(p), Value = NormalizeSqlForComparison(p) })
                .Where(x => !string.IsNullOrWhiteSpace(x.Key))
                .ToDictionary(x => x.Key, x => x.Value, StringComparer.OrdinalIgnoreCase);

            var result = new List<SchemaDiffEntry>();
            var allKeys = dict1.Keys.Union(dict2.Keys, StringComparer.OrdinalIgnoreCase)
                               .OrderBy(x => x, StringComparer.OrdinalIgnoreCase).ToList();

            foreach (var key in allKeys)
            {
                var isConstraint = key.Contains("KEY", StringComparison.OrdinalIgnoreCase)
                                || key.Contains("CONSTRAINT", StringComparison.OrdinalIgnoreCase)
                                || key.Equals("UNIQUE", StringComparison.OrdinalIgnoreCase);

                bool inDb1 = dict1.TryGetValue(key, out var def1);
                bool inDb2 = dict2.TryGetValue(key, out var def2);

                if (!inDb1)
                    result.Add(new SchemaDiffEntry(DiffKind.Added, isConstraint, key, null, def2));
                else if (!inDb2)
                    result.Add(new SchemaDiffEntry(DiffKind.Removed, isConstraint, key, def1, null));
                else if (!string.Equals(def1, def2, StringComparison.OrdinalIgnoreCase))
                    result.Add(new SchemaDiffEntry(DiffKind.Modified, isConstraint, key, def1, def2));
            }

            // Sort: removed first, then added, then modified; constraints last
            return result
                .OrderBy(e => e.IsConstraint ? 1 : 0)
                .ThenBy(e => e.Kind)
                .ToList();
        }

        private static List<string> SplitCreateTableParts(string sql)
        {
            if (string.IsNullOrWhiteSpace(sql))
                return new List<string>();

            int start = sql.IndexOf('(');
            if (start < 0)
                return new List<string> { NormalizeSqlForComparison(sql) };

            int end = FindMatchingParenthesis(sql, start);
            if (end <= start)
                return new List<string> { NormalizeSqlForComparison(sql) };

            var body = sql.Substring(start + 1, end - start - 1);
            var parts = new List<string>();
            var current = new System.Text.StringBuilder();
            int depth = 0;

            foreach (var ch in body)
            {
                if (ch == '(')
                    depth++;
                else if (ch == ')')
                    depth--;

                if (ch == ',' && depth == 0)
                {
                    var part = current.ToString().Trim();
                    if (!string.IsNullOrWhiteSpace(part))
                        parts.Add(part);
                    current.Clear();
                    continue;
                }

                current.Append(ch);
            }

            var leftover = current.ToString().Trim();
            if (!string.IsNullOrWhiteSpace(leftover))
                parts.Add(leftover);

            return parts;
        }

        private static int FindMatchingParenthesis(string text, int openIndex)
        {
            int depth = 0;
            for (int i = openIndex; i < text.Length; i++)
            {
                if (text[i] == '(')
                    depth++;
                else if (text[i] == ')')
                {
                    depth--;
                    if (depth == 0)
                        return i;
                }
            }

            return -1;
        }

        private static string ExtractDefinitionName(string part)
        {
            if (string.IsNullOrWhiteSpace(part))
                return string.Empty;

            var trimmed = part.Trim();
            var tokens = trimmed.Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
            if (tokens.Length == 0)
                return string.Empty;

            var first = tokens[0].Trim('"', '`', '[', ']');
            if (string.Equals(first, "PRIMARY", StringComparison.OrdinalIgnoreCase))
                return "PRIMARY KEY";
            if (string.Equals(first, "FOREIGN", StringComparison.OrdinalIgnoreCase))
                return "FOREIGN KEY";
            if (string.Equals(first, "UNIQUE", StringComparison.OrdinalIgnoreCase))
                return "UNIQUE";
            if (string.Equals(first, "CONSTRAINT", StringComparison.OrdinalIgnoreCase) && tokens.Length > 1)
                return tokens[1].Trim('"', '`', '[', ']');

            return first;
        }

        private static void CompareTableData(string db1Path, string db2Path)
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("▶ TABLE DATA COMPARISON\n");
            Console.ResetColor();

            Console.WriteLine($"Comparing data in these tables:");
            foreach (var table in TablesToCompareData)
            {
                Console.WriteLine($"  • {table}");
            }
            Console.WriteLine();

            foreach (var tableName in TablesToCompareData)
            {
                try
                {
                    long count1 = GetTableRowCount(db1Path, tableName);
                    long count2 = GetTableRowCount(db2Path, tableName);

                    Console.WriteLine($"\n  Table: {tableName}");
                    Console.Write($"    DB1: {count1} rows  |  ");
                    Console.Write($"DB2: {count2} rows  |  ");

                    if (count1 == count2)
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine("✓");
                        Console.ResetColor();
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine($"⚠ Difference: {Math.Abs(count1 - count2)} rows");
                        Console.ResetColor();

                        // Show first 5 rows from each
                        Console.WriteLine("    First 5 rows DB1:");
                        DisplayTableSample(db1Path, tableName);

                        Console.WriteLine("    First 5 rows DB2:");
                        DisplayTableSample(db2Path, tableName);
                    }
                }
                catch (Exception ex)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"    ✗ Error: {ex.Message}");
                    Console.ResetColor();
                }
            }
        }

        private static Dictionary<string, string> GetDatabaseSchema(string dbPath)
        {
            if (!File.Exists(dbPath))
                throw new FileNotFoundException($"Database not found: {dbPath}");

            var schema = new Dictionary<string, string>();
            string connectionString = $"Data Source=\"{dbPath}\"; Cache = Shared; Mode = ReadOnly";

            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();

                // Get all table names
                var command = connection.CreateCommand();
                command.CommandText = "SELECT name FROM sqlite_master WHERE type='table' ORDER BY name";

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string tableName = reader.GetString(0);
                        if (tableName.StartsWith("sqlite_")) continue; // Skip system tables

                        // Get CREATE TABLE statement
                        var createCmd = connection.CreateCommand();
                        createCmd.CommandText = $"SELECT sql FROM sqlite_master WHERE type='table' AND name='{tableName}'";
                        var sql = createCmd.ExecuteScalar()?.ToString() ?? "";

                        schema[tableName] = sql;
                    }
                }
            }

            return schema;
        }

        private static long GetTableRowCount(string dbPath, string tableName)
        {
            string connectionString = $"Data Source=\"{dbPath}\"; Cache = Shared; Mode = ReadOnly";

            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = $"SELECT COUNT(*) FROM \"{tableName}\"";
                var result = command.ExecuteScalar();
                return result is long count ? count : 0;
            }
        }

        private static void DisplayTableSample(string dbPath, string tableName)
        {
            string connectionString = $"Data Source=\"{dbPath}\"; Cache = Shared; Mode = ReadOnly";

            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = $"SELECT * FROM \"{tableName}\" LIMIT 5";

                using (var reader = command.ExecuteReader())
                {
                    if (!reader.HasRows)
                    {
                        Console.WriteLine("      (no data)");
                        return;
                    }

                    // Get column names
                    var columns = Enumerable.Range(0, reader.FieldCount)
                        .Select(i => reader.GetName(i))
                        .ToList();

                    int rowNum = 0;
                    while (reader.Read())
                    {
                        rowNum++;
                        var values = Enumerable.Range(0, reader.FieldCount)
                            .Select(i => reader.GetValue(i)?.ToString() ?? "(null)")
                            .ToList();

                        Console.Write($"      Row {rowNum}: ");
                        Console.WriteLine(string.Join(" | ", values.Take(3)));
                    }
                }
            }
        }
    }
}

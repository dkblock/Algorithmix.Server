using Microsoft.EntityFrameworkCore;
using System;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Algorithmix.Database
{
    public static class DataInitializer
    {
        private const string ScriptName = "Algorithmix.Database.Scripts.init_db.sql";

        public static void Initialize(ApplicationContext context)
        {
            if (IsInitialized(context))
                return;

            var assembly = Assembly.GetExecutingAssembly();
            var reader = new StreamReader(assembly.GetManifestResourceStream(ScriptName));
            var content = reader.ReadToEnd();
            var commands = content.Split(new string[] { "\r\nGO\r\n" }, StringSplitOptions.RemoveEmptyEntries);

            var conn = context.Database.GetDbConnection();
            var connState = conn.State;

            try
            {
                if (connState != ConnectionState.Open)
                    conn.Open();

                using var sqlCmd = conn.CreateCommand();
                foreach (var cmd in commands)
                {
                    sqlCmd.CommandText = cmd;
                    sqlCmd.ExecuteNonQuery();
                }
            }
            finally
            {
                if (connState != ConnectionState.Open)
                    conn.Close();
            }
        }

        private static bool IsInitialized(ApplicationContext context)
        {
            return context.Algorithms.Any() ||
                context.AlgorithmTimeComplexities.Any() ||
                context.Groups.Any() ||
                context.PublishedTestAnswers.Any() ||
                context.PublishedTestQuestions.Any() ||
                context.PublishedTests.Any() ||
                context.TestAlgorithms.Any() ||
                context.TestAnswers.Any() ||
                context.TestQuestions.Any() ||
                context.Tests.Any() ||
                context.UserAnswers.Any() ||
                context.UserTestResults.Any() ||
                context.Users.Any() ||
                context.Roles.Any();
        }
    }
}

﻿using System;
using System.CodeDom;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Newtonsoft.Json.Linq;

namespace DynamoGraphMigrationAssistant
{
    public class Utilities
    {
        /// <summary>
        /// Returns true if both paths exist
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static bool IsValidPath(string path)
        {
            return Directory.Exists(path);
        }
        /// <summary>
        /// Returns a list of files of given path and extension
        /// </summary>
        /// <param name="path"></param>
        /// <param name="extension"></param>
        /// <returns></returns>
        public static IEnumerable<string> GetAllFilesOfExtension(string path, string extension = ".dyn")
        {
            var files = Directory.EnumerateFiles(path, "*.*", SearchOption.AllDirectories)
                .Where(x => extension.Equals(Path.GetExtension(x).ToLowerInvariant()));

            return files;
        }
        /// <summary>
        /// Returns the calculated version for the given graph path.
        /// </summary>
        /// <param name="graphPath">The path of the dyn</param>
        /// <returns></returns>
        public static string GetDynamoVersionForGraph(string graphPath)
        {
            try
            {
                var jsonText = File.ReadAllText(graphPath);
                JObject o = JObject.Parse(jsonText);
                string versionString = (string)o.SelectToken("View.Dynamo.Version");
                Version version = Version.Parse(versionString);

                return $"{version.Major}.{version.Minor}";
            }
            catch (Exception e)
            {
                return "2.0";
            }
           
        }
        private static string mColumnLetters = "zabcdefghijklmnopqrstuvwxyz";

        // Convert Column name to 0 based index
        public static int ColumnIndexByName(string ColumnName)
        {
            string CurrentLetter;
            int ColumnIndex, LetterValue, ColumnNameLength;
            ColumnIndex = -1; // A is the first column, but for calculation it's number is 1 and not 0. however, Index is alsways zero-based.
            ColumnNameLength = ColumnName.Length;
            for (int i = 0; i < ColumnNameLength; i++)
            {
                CurrentLetter = ColumnName.Substring(i, 1).ToLower();
                LetterValue = mColumnLetters.IndexOf(CurrentLetter);
                ColumnIndex += LetterValue * (int)Math.Pow(26, (ColumnNameLength - (i + 1)));
            }
            return ColumnIndex;
        }

        // Convert 0 based index to Column name
        public static string ColumnNameByIndex(int ColumnIndex)
        {
            int ModOf26, Subtract;
            StringBuilder NumberInLetters = new StringBuilder();
            ColumnIndex += 1; // A is the first column, but for calculation it's number is 1 and not 0. however, Index is alsways zero-based.
            while (ColumnIndex > 0)
            {
                if (ColumnIndex <= 26)
                {
                    ModOf26 = ColumnIndex;
                    NumberInLetters.Insert(0, mColumnLetters.Substring(ModOf26, 1));
                    ColumnIndex = 0;
                }
                else
                {
                    ModOf26 = ColumnIndex % 26;
                    Subtract = (ModOf26 == 0) ? 26 : ModOf26;
                    ColumnIndex = (ColumnIndex - Subtract) / 26;
                    NumberInLetters.Insert(0, mColumnLetters.Substring(ModOf26, 1));
                }
            }
            return NumberInLetters.ToString().ToUpper();
        }
    }
}

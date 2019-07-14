using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace StructureMapRepositoryPattern.Utility
{
    public static class ConsoleTablePrinter
    {
        private static int tableWidth = 77;

        public static void PrintLine()
        {
            Console.WriteLine(new string('-', tableWidth));
        }

        private static void PrintHeader(params string[] headers)
        {
            int width = (tableWidth - headers.Length) / headers.Length;
            string row = "|";

            foreach (string header in headers)
            {
                row += AlignCentre(header, width) + "|";
            }

            Console.BackgroundColor = ConsoleColor.Green;
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine(row);
            Console.ResetColor();
        }

        public static void PrintHeader<T>(PropertyInfo[] propertyInfos = null)
        {
            if (propertyInfos == null)
            {
                propertyInfos = typeof(T).GetProperties();
            }
            PrintHeader(propertyInfos.Select(i => i.Name).ToArray());
        }

        public static void PrintObject<T>(T obj)
        {
            var props = typeof(T).GetProperties();
            PrintHeader<T>(props);
            PrintRow(props.Select(i => i.GetValue(obj).ToString()).ToArray());
            PrintLine();
        }

        public static void PrintRow<T>(T obj, PropertyInfo[] propertyInfos = null)
        {
            if (propertyInfos == null)
            {
                propertyInfos = typeof(T).GetProperties();
            }

            PrintRow(propertyInfos.Select(p => p.GetValue(obj).ToString()).ToArray());
            PrintLine();
        }

        public static void PrintAllDataSet<T>(List<T> obj)
        {
            var props = typeof(T).GetProperties();
            PrintHeader<T>(props);
            foreach (T row in obj)
            {
                PrintRow(row, props);
            }
        }

        private static void PrintRow(params string[] columns)
        {
            int width = (tableWidth - columns.Length) / columns.Length;
            string row = "|";

            foreach (string column in columns)
            {
                row += AlignCentre(column, width) + "|";
            }

            Console.WriteLine(row);
        }

        private static string AlignCentre(string text, int width)
        {
            text = text.Length > width ? text.Substring(0, width - 3) + "..." : text;

            if (string.IsNullOrEmpty(text))
            {
                return new string(' ', width);
            }
            else
            {
                return text.PadRight(width - (width - text.Length) / 2).PadLeft(width);
            }
        }
    }
}

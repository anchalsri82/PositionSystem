using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using PositionSystem.Interfaces;

namespace PositionSystem.Implementations
{
    public class CsvWriter : ICsvWriter
    {
        public void Write<T>(IEnumerable<T> data, string fileName)
        {
            var properties = typeof(T).GetProperties();
            using (var fileStream = new StreamWriter(fileName))
            {
                fileStream.WriteLine(GetColumnNames(properties));
            }
        }

        private string GetColumnNames(PropertyInfo[] properties)
        {
            var columns = new StringBuilder();
            columns.Append(properties[0].Name);
            for (var i = 1; i < properties.Length; i++)
            {
                columns.Append("," + properties[i].Name);
            }
            return columns.ToString();
        }
    }
}

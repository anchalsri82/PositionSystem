using System;
using System.Collections.Generic;
using System.IO;
using Autofac;
using PositionSystem.Interfaces;

namespace PositionSystem.Implementations
{
    public class CsvReader : ICsvReader
    {
        private readonly ILifetimeScope _lifetimeScope;

        public CsvReader(ILifetimeScope lifetimeScope)
        {
            _lifetimeScope = lifetimeScope;
        }

        public IEnumerable<T> Read<T>(string source, string delimiter, bool hasHeaders)
        {
            using (var sr = new StreamReader(source))
            {
                return GetObjects<T>(sr, delimiter, hasHeaders);
            }
        }

        internal IDictionary<string, int> GetColumnIndices(string[] propertyValues, string delimiter="/t")
        {
            var dictionary = new Dictionary<string, int>();
            for (var i = 0; i < propertyValues.Length; i++)
            {
                dictionary.Add(propertyValues[i], i);
            }
            return dictionary;
        }

        internal IEnumerable<T> GetObjects<T>(TextReader sr, string delimiter, bool hasHeaders)
        {
            var objects = new List<T>();
            var headersRead = !hasHeaders;
            string line;
            IDictionary<string, int> columnIndices = null;
            do
            {
                line = sr.ReadLine();
                string[] propertyValues = null;
                if (line != null)
                {
                    propertyValues = line.Split(delimiter.ToCharArray());
                }

                if (propertyValues != null && headersRead)
                {
                    var obj = GetObject<T>(propertyValues, columnIndices);
                    objects.Add(obj);
                }
                else if (propertyValues != null)
                {
                    columnIndices = GetColumnIndices(propertyValues, delimiter);
                }

                if (!headersRead)
                {
                    headersRead = true;
                }
            } while (line != null);

            return objects;
        }

        internal T GetObject<T>(string[] propertyValues, IDictionary<string, int> columnIndices)
        {
            var obj = _lifetimeScope.IsRegistered<T>() ? _lifetimeScope.Resolve<T>() : Activator.CreateInstance<T>();
            foreach (var columnIndex in columnIndices)
            {
                var propertyInfo = typeof(T).GetProperty(columnIndex.Key);
                if (propertyInfo != null)
                {
                    var type = propertyInfo.PropertyType.Name;
                    switch (type)
                    {
                        // Limiting it to int, double, string for now
                        case "Int32":
                            propertyInfo.SetValue(obj, int.Parse(propertyValues[columnIndex.Value]));
                            break;
                        case "Double":
                            propertyInfo.SetValue(obj, double.Parse(propertyValues[columnIndex.Value]));
                            break;
                        default:
                            propertyInfo.SetValue(obj, propertyValues[columnIndex.Value]);
                            break;
                    }
                }
            }
            return obj;
        }
    }
}

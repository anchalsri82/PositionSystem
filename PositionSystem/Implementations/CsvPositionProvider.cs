using System.Collections.Generic;
using PositionSystem.Interfaces;

namespace PositionSystem.Implementations
{
    internal class CsvPositionProvider<T> : IPositionProvider<T>
    {
        private readonly ICsvReader _csvReader;
        private readonly ICsvWriter _csvWriter;
        private string _source;
        private string _delimiter;
        public CsvPositionProvider(ICsvReader csvReader, ICsvWriter csvWriter)
        {
            _csvReader = csvReader;
            _csvWriter = csvWriter;
        }

        public IEnumerable<T> GetPositions(string sourceConfig)
        {
            var config = sourceConfig.Split("\n".ToCharArray());
            _source = config[0];
            _delimiter = config[1];
            bool.TryParse(config[2], out var hasHeaders);
            return _csvReader.Read<T>(_source, _delimiter, hasHeaders);
        }
        public void SavePositions(IEnumerable<T> positions)
        {
            _csvWriter.Write<T>(positions, _source);
        }
    }
}

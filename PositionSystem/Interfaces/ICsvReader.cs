using System.Collections.Generic;

namespace PositionSystem.Interfaces
{
    public interface ICsvReader
    {
        IEnumerable<T> Read<T>(string source, string delimiter="\t", bool hasHeaders=true);
    }
}

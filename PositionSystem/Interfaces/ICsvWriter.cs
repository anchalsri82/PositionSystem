using System.Collections.Generic;
namespace PositionSystem.Interfaces
{
    public interface ICsvWriter
    {
        void Write<T>(IEnumerable<T> data, string fileName);
    }
}

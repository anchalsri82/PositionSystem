using System.Collections.Generic;
namespace PositionSystem.Interfaces
{
    public interface IPositionProvider<T>
    {
        IEnumerable<T> GetPositions(string sourceConfig);
        void SavePositions(IEnumerable<T> positions);
    }
}

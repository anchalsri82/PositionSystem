using System.Collections.Generic;

namespace PositionSystem.Interfaces
{
    public interface IReceiver<TInput, TAction, TResult>
    {
        void SetAction(TAction action);
        IEnumerable<TResult> GetResult(IEnumerable<TInput> input);
        void SaveResult(IEnumerable<TInput> input);
    }
}

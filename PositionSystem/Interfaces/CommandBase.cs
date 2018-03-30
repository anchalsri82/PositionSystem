using System.Collections.Generic;

namespace PositionSystem.Interfaces
{
    internal abstract class CommandBase<TInput,TAction, TResult>
    {
        protected IReceiver<TInput, TAction, TResult> Receiver;

        protected CommandBase(IReceiver<TInput, TAction, TResult> receiver)
        {
            Receiver = receiver;
        }

        public abstract IEnumerable<TResult> Execute(IEnumerable<TInput> input);
    }
}

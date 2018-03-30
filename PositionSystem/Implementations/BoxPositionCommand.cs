using System.Collections.Generic;
using PositionSystem.Interfaces;
namespace PositionSystem.Implementations
{
    internal class BoxPositionCommand<TInput, TAction, TResult> : CommandBase<TInput, TAction, TResult>
    {
        public BoxPositionCommand(IReceiver<TInput, TAction, TResult> receiver) : base(receiver)
        {
        }

        public override IEnumerable<TResult> Execute(IEnumerable<TInput> input)
        {
            if (typeof(TAction) == typeof(TradingPositionAction))
            {
                Receiver.SetAction((TAction) (object) TradingPositionAction.NetPosition);
            }

            return Receiver.GetResult(input);
        }
    }
}

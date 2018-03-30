using System.Collections.Generic;
using Autofac;
using PositionSystem.Interfaces;

namespace PositionSystem.Implementations
{
    internal class NetPositionCommand<TInput,TAction, TResult> : CommandBase<TInput,TAction, TResult>
    {
        private readonly ILifetimeScope _lifetimeScope;
        public NetPositionCommand(IReceiver<TInput,TAction, TResult> receiver, ILifetimeScope lifetimeScope) : base(receiver)
        {
            _lifetimeScope = lifetimeScope;
        }
        public override IEnumerable<TResult> Execute(IEnumerable<TInput> input)
        {
            Receiver.SetAction((TAction)(object)TradingPositionAction.NetPosition);
            return Receiver.GetResult(input);
        }
    }
}

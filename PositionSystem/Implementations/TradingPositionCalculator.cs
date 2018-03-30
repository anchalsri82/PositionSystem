using System;
using System.Collections.Generic;
using System.Linq;
using PositionSystem.Interfaces;

namespace PositionSystem.Implementations
{
    public enum TradingPositionAction
    {
        NetPosition,
        BoxedPosition
    }
    internal class TradingPositionCalculator<TInput, TAction, TResult> : IReceiver<TInput, TAction, TResult>
    {
        TradingPositionAction _currentAction;
        private readonly ICsvWriter _csvWriter;
        public TradingPositionCalculator(ICsvWriter csvWriter)
        {
            _csvWriter = csvWriter;
        }

        public void SetAction(TAction action)
        {
            if (action is TradingPositionAction)
            {
                _currentAction = (TradingPositionAction) (object) action;
            }

            throw new System.NotImplementedException();
        }

        public IEnumerable<TResult> GetResult(IEnumerable<TInput> tradingPositions)
        {
            if (_currentAction == TradingPositionAction.NetPosition)
            {
                return GetNetPositions(tradingPositions);
            }

            if (_currentAction == TradingPositionAction.BoxedPosition)
            {
                return GetBoxPositions(tradingPositions);
            }

            throw new System.NotImplementedException();
        }
        public void SaveResult(IEnumerable<TInput> tradingPositions)
        {
            string path = @"c:\temp";
            if (_currentAction == TradingPositionAction.NetPosition)
            {
                _csvWriter.Write(GetNetPositions(tradingPositions), path);
            }
            else
            if (_currentAction == TradingPositionAction.BoxedPosition)
            {
                _csvWriter.Write(GetBoxPositions(tradingPositions), path);
            }
        }

        internal IEnumerable<TResult> GetBoxPositions(IEnumerable<TInput> tradingPositions)
        {
            List<ITradingPosition> positions = new List<ITradingPosition>();
            foreach (var tradingPosition in tradingPositions)
            {
                positions.Add((ITradingPosition)(object)tradingPosition);
            }
            if (typeof(TResult) == typeof(ITradingPositionResult) && typeof(TInput) == typeof(ITradingPosition))
            {
                //var result = new List<TResult>();
                var result = from longList in
                    (from longPositions in positions
                     where longPositions.QUANTITY > 0
                        group longPositions by (longPositions.TRADER + ";" + longPositions.SYMBOL)
                        into longs
                        select new TradingPositionResult
                        {
                            QUANTITY = longs.Sum(c => c.QUANTITY),
                            TRADER = longs.Key.Split(";".ToCharArray())[0],
                            SYMBOL = longs.Key.Split(";".ToCharArray())[1]
                        }
                    )
                    join shortList in (from shortPositions in positions
                                       where shortPositions.QUANTITY < 0
                            group shortPositions by (shortPositions.TRADER + ";" + shortPositions.SYMBOL)
                            into shorts
                            select new TradingPositionResult
                            {
                                QUANTITY = shorts.Sum(c => c.QUANTITY),
                                TRADER = shorts.Key.Split(";".ToCharArray())[0],
                                SYMBOL = shorts.Key.Split(";".ToCharArray())[1]
                            }
                        ) on longList.TRADER + ";" + longList.SYMBOL equals shortList.TRADER + ";" + shortList.SYMBOL
                    select (TResult)(object)new TradingPositionResult
                    {
                        QUANTITY = Math.Min(longList.QUANTITY,Math.Abs(shortList.QUANTITY)),
                        TRADER = longList.TRADER,
                        SYMBOL = longList.SYMBOL
                    };
                return result;
            }
            throw new System.NotImplementedException();
        }

        internal IEnumerable<TResult> GetNetPositions(IEnumerable<TInput> tradingPositions)
        {
            List<ITradingPosition> positions = new List<ITradingPosition>();
            foreach (var tradingPosition in tradingPositions)
            {
                positions.Add((ITradingPosition)(object)tradingPosition);
            }

            if (typeof(TResult) == typeof(ITradingPositionResult))
            {
                var result = from tradingPosition in positions
                             group tradingPosition by (tradingPosition.TRADER + ";" + tradingPosition.SYMBOL)
                    into netTradingPositions
                    select (TResult)(object)new TradingPositionResult
                    {
                        QUANTITY = netTradingPositions.Sum(c => c.QUANTITY),
                        TRADER = netTradingPositions.Key.Split(";".ToCharArray())[0],
                        SYMBOL = netTradingPositions.Key.Split(";".ToCharArray())[0]
                    };
                return result;
            }
            throw new System.NotImplementedException();
        }
    }
}

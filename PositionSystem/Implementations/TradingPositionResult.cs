using PositionSystem.Interfaces;

namespace PositionSystem.Implementations
{
    internal class TradingPositionResult : ITradingPositionResult
    {
        public string TRADER { get; set; }
        public string SYMBOL { get; set; }
        public double QUANTITY { get; set; }
    }
}

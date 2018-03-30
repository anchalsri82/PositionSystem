using PositionSystem.Interfaces;

namespace PositionSystem.Implementations
{
    internal class TradingPosition : ITradingPosition
    {
        public string TRADER { get; set; }
        public string BROKER { get; set; }
        public string SYMBOL { get; set; }
        public double QUANTITY { get; set; }
        public double PRICE { get; set; }
        public TradingPosition()
        {
        }

        public TradingPosition(string trader, string broker, string symbol, double quantity, double price)
        {
            TRADER = trader;
            BROKER = broker;
            SYMBOL = symbol;
            QUANTITY = quantity;
            PRICE = price;
        }
    }
}

namespace PositionSystem.Interfaces
{
    public interface ITradingPosition
    {
        string TRADER {get; set;}
        string BROKER { get; set; }
        string SYMBOL { get; set; }
        double QUANTITY { get; set; }
        double PRICE { get; set; }
    }
}

namespace PositionSystem.Interfaces
{
    public interface ITradingPositionResult
    {
        string TRADER { get; set; }
        string SYMBOL { get; set; }
        double QUANTITY { get; set; }
    }
}
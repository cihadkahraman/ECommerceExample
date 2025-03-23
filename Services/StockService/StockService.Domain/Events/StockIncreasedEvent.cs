using StockService.Domain.Common;


namespace StockService.Domain.Events
{
    public class StockIncreasedEvent : DomainEvent
    {
        public int ProductId { get; }
        public int NewQuantity { get; }

        public StockIncreasedEvent(int productId, int newQuantity)
        {
            ProductId = productId;
            NewQuantity = newQuantity;
        }
    }
}

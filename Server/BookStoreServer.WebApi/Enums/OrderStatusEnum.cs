namespace BookStoreServer.WebApi.Enums
{
    public enum OrderStatusEnum
    {
        AwaitingApproval = 0,
        BeingPrepared = 1,
        InTransit = 2,
        Delivered = 3,
        Rejected = 4,
        Returned = 5
    }
}

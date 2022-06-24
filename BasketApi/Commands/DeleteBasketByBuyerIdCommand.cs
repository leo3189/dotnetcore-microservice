namespace BasketApi.Commands
{
    public class DeleteBasketByBuyerIdCommand : IRequest<bool>
    {
        public string BuyerId { get; set; }

        public class DeleteBasketByBuyerIdCommandHandler : IRequestHandler<DeleteBasketByBuyerIdCommand, bool>
        {
            private readonly IDatabase _database;

            public DeleteBasketByBuyerIdCommandHandler(ConnectionMultiplexer redis)
            { 
                _database = redis.GetDatabase();
            }

            public async Task<bool> Handle(DeleteBasketByBuyerIdCommand command, CancellationToken cancellationToken)
            {
                return await _database.KeyDeleteAsync(command.BuyerId);
            }
        }
    }
}

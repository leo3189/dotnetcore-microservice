namespace BasketApi.Commands
{
    public class UpdateBasketCommand : IRequest<CustomerBasket?>
    {
        public CustomerBasket Basket { get; set; }

        public class UpdateBasketCommandHandler : IRequestHandler<UpdateBasketCommand, CustomerBasket?>
        {
            private readonly IMediator _mediator;
            private readonly IDatabase _database;

            public UpdateBasketCommandHandler(IMediator mediator, ConnectionMultiplexer redis)
            { 
                _mediator = mediator;
                _database = redis.GetDatabase();
            }

            public async Task<CustomerBasket?> Handle(UpdateBasketCommand command, CancellationToken cancellationToken)
            {
                var created = await _database.StringSetAsync(command.Basket.BuyerId, JsonSerializer.Serialize(command.Basket));
                if (!created) return null;
                return await _mediator.Send(new GetBasketByBuyerIdQuery { BuyerId = command.Basket.BuyerId }, cancellationToken);
            }
        }
    }
}

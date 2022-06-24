namespace BasketApi.Queries
{
    public class GetBasketByBuyerIdQuery : IRequest<CustomerBasket?>
    {
        public string BuyerId { get; set; }

        public class GetBasketByBuyerIdQueryHandler : IRequestHandler<GetBasketByBuyerIdQuery, CustomerBasket?>
        {
            private readonly IDatabase _database;

            public GetBasketByBuyerIdQueryHandler(ConnectionMultiplexer redis)
            { 
                _database = redis.GetDatabase();
            }

            public async Task<CustomerBasket?> Handle(GetBasketByBuyerIdQuery request, CancellationToken cancellationToken)
            {
                var data = await _database.StringGetAsync(request.BuyerId);
                if (data.IsNullOrEmpty) return null;
                return JsonSerializer.Deserialize<CustomerBasket>(data, new JsonSerializerOptions
                { 
                    PropertyNameCaseInsensitive = true,
                });
            }
        }
    }
}

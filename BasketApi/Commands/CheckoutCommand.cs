namespace BasketApi.Commands
{
    public class CheckoutCommand : IRequest<bool>
    {
        public BasketCheckout BasketCheckout { get; set; }

        public string Id { get; set; }

        public class CheckoutCommandHandle : IRequestHandler<CheckoutCommand, bool>
        {
            private readonly IEventBus _eventBus;
            private readonly IMediator _mediator;
            private readonly ILogger<CheckoutCommandHandle> _logger;

            public CheckoutCommandHandle(IEventBus eventBus, IMediator mediator, ILogger<CheckoutCommandHandle> logger)
            { 
                _eventBus = eventBus;
                _mediator = mediator;
                _logger = logger;
            }

            public async Task<bool> Handle(CheckoutCommand request, CancellationToken cancellationToken)
            {
                request.BasketCheckout.RequestId = (Guid.TryParse(request.Id, out Guid guid) && guid != Guid.Empty) ?
                    guid : request.BasketCheckout.RequestId;

                var basket = await _mediator.Send(new GetBasketByBuyerIdQuery { BuyerId = "123" }, cancellationToken);

                if (basket is null) return false;

                var eventMsg = new UserCheckoutAcceptedIntegrationEvent("123", "123", request.BasketCheckout.City, request.BasketCheckout.Street,
                    request.BasketCheckout.State, request.BasketCheckout.Country, request.BasketCheckout.ZipCode, request.BasketCheckout.CardNumber,
                    request.BasketCheckout.CardHolderName, request.BasketCheckout.CardExpiration, request.BasketCheckout.CardSecurityNumber,
                    request.BasketCheckout.CardTypeId, request.BasketCheckout.Buyer, request.BasketCheckout.RequestId, basket);

                try
                {
                    _eventBus.Publish(eventMsg);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "ERROR Publishing integration event: {IntegrationEventId} from {AppName}", eventMsg.Id, "Basket API");
                    throw;                
                }

                return true;
            }
        }
    }
}

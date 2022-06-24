

namespace OrderingApi.IntegrationEvents.EventHandling
{
    public class UserCheckoutAcceptedIntegrationEventHandler : IIntegrationEventHandler<UserCheckoutAcceptedIntegrationEvent>
    {
        private readonly IMediator _mediator;
        private readonly ILogger<UserCheckoutAcceptedIntegrationEventHandler> _logger;

        public UserCheckoutAcceptedIntegrationEventHandler(IMediator mediator, ILogger<UserCheckoutAcceptedIntegrationEventHandler> logger)
        { 
            _logger = logger;
            _mediator = mediator;   
        }

        public async Task Handle(UserCheckoutAcceptedIntegrationEvent @event)
        {
            using (LogContext.PushProperty("IntegrationEventContext", $"{@event.Id}-Ordering"))
            {
                _logger.LogInformation("----- Handling integration event: {IntegrationEventId} at Ordering - ({@IntegrationEvent})", @event.Id, @event);

                var result = false;

                if (@event.RequestId != Guid.Empty)
                {
                    using (LogContext.PushProperty("IdentifiedCommandId", @event.RequestId))
                    {
                        var createOrderCommand = new CreateOrderCommand(@event.Basket.Items, @event.UserId, @event.UserName, @event.City, @event.Street,
                            @event.State, @event.Country, @event.ZipCode,
                            @event.CardNumber, @event.CardHolderName, @event.CardExpiration,
                            @event.CardSecurityNumber, @event.CardTypeId);

                        var requestCreateOrder = new IdentifiedCommand<CreateOrderCommand, bool>(createOrderCommand, @event.RequestId);

                        _logger.LogInformation(
                            "----- Sending command: {CommandName} - {IdProperty}: {CommandId} ({@Command})",
                            requestCreateOrder.GetGenericTypeName(),
                            nameof(requestCreateOrder.Id),
                            requestCreateOrder.Id,
                            requestCreateOrder);

                        result = await _mediator.Send(requestCreateOrder);

                        if (result)
                        {
                            _logger.LogInformation("----- CreateOrderCommand suceeded - RequestId: {RequestId}", @event.RequestId);
                        }
                        else
                        {
                            _logger.LogWarning("CreateOrderCommand failed - RequestId: {RequestId}", @event.RequestId);
                        }
                    }
                }
                else
                {
                    _logger.LogWarning("Invalid IntegrationEvent - RequestId is missing - {@IntegrationEvent}", @event);
                }
            }
        }
    }
}

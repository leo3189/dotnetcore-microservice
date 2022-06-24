namespace OrderingApi.Commands
{
    public class IdentifiedCommandHandler<T, R> : IRequestHandler<IdentifiedCommand<T, R>, R>
        where T : IRequest<R>
    {
        private readonly IMediator _mediator;

        public IdentifiedCommandHandler(IMediator mediator)
        { 
            _mediator = mediator;
        }

        public async Task<R> Handle(IdentifiedCommand<T, R> request, CancellationToken cancellationToken)
        {
            var command = request.Command;
            return await _mediator.Send(command, cancellationToken);
        }
    }
}

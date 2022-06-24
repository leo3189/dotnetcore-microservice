namespace CatalogApi.Commands
{
    public class DeleteCatalogGroupByIdCommand : IRequest<int>
    {
        public int Id { get; set; }

        public class DeleteCatalogGroupByIdCommandHandler : IRequestHandler<DeleteCatalogGroupByIdCommand, int>
        { 
            private readonly CatalogContext _context;

            public DeleteCatalogGroupByIdCommandHandler(CatalogContext context)
            { 
                _context = context;
            }

            public async Task<int> Handle(DeleteCatalogGroupByIdCommand command, CancellationToken cancellationToken)
            {
                var group = await _context.CatalogGroups
                    .AsNoTracking()
                    .SingleOrDefaultAsync(s => s.Id.Equals(command.Id), cancellationToken);
                if (group == null) return default;

                _context.CatalogGroups.Remove(group);
                await _context.SaveChangesAsync(cancellationToken);
                return command.Id;
            }
        }
    }
}

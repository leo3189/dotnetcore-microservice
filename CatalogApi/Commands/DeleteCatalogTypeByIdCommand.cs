using CatalogApi.Context;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CatalogApi.Commands
{
    public class DeleteCatalogTypeByIdCommand : IRequest<int>
    {
        public int Id { get; set; }

        public class DeleteCatalogTypeByIdCommandHandler : IRequestHandler<DeleteCatalogTypeByIdCommand, int>
        {
            private readonly CatalogContext _context;

            public DeleteCatalogTypeByIdCommandHandler(CatalogContext context)
            { 
                _context = context; 
            }

            public async Task<int> Handle(DeleteCatalogTypeByIdCommand command, CancellationToken cancellationToken)
            {
                var type = await _context.CatalogTypes
                    .AsNoTracking()
                    .SingleOrDefaultAsync(s => s.Id.Equals(command.Id), cancellationToken);
                if (type == null) return default;

                _context.CatalogTypes.Remove(type);
                await _context.SaveChangesAsync(cancellationToken);
                return type.Id;
            }
        }
    }
}

using CatalogApi.Context;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CatalogApi.Commands
{
    public class DeleteCatalogByIdCommand : IRequest<int>
    {
        public int Id { get; set; }

        public class DeleteCatalogByIdCommandHandler : IRequestHandler<DeleteCatalogByIdCommand, int>
        { 
            private readonly CatalogContext _context;

            public DeleteCatalogByIdCommandHandler(CatalogContext context)
            { 
                _context = context;
            }

            public async Task<int> Handle(DeleteCatalogByIdCommand command, CancellationToken cancellationToken)
            {
                var catalog = await _context.CatalogItems.Where(w => w.Id == command.Id).FirstOrDefaultAsync(cancellationToken);
                if (catalog == null) return default;
                _context.CatalogItems.Remove(catalog);
                await _context.SaveChangesAsync(cancellationToken);
                return command.Id;
            }
        }
    }
}

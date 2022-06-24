using CatalogApi.Context;
using CatalogApi.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CatalogApi.Commands
{
    public class UpdateCatalogByIdCommand : IRequest<int>
    {
        public CatalogItem Item { get; set; }

        public class UpdateCatalogByIdCommandHandler : IRequestHandler<UpdateCatalogByIdCommand, int>
        {
            private readonly CatalogContext _context;

            public UpdateCatalogByIdCommandHandler(CatalogContext context)
            { 
                _context = context;
            }

            public async Task<int> Handle(UpdateCatalogByIdCommand command, CancellationToken cancellationToken)
            {
                var catalog = await _context.CatalogItems
                    .AsNoTracking()
                    .SingleOrDefaultAsync(s => s.Id == command.Item.Id, cancellationToken);
                if (catalog == null) return default;

                _context.CatalogItems.Update(command.Item);
                await _context.SaveChangesAsync(cancellationToken); 
                return command.Item.Id;
            }
        }
    }
}

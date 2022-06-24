using CatalogApi.Context;
using CatalogApi.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CatalogApi.Commands
{
    public class UpdateCatalogTypeCommand : IRequest<int>
    {
        public CatalogType Type { get; set; }

        public class UpdateCatalogTypeCommandHandler : IRequestHandler<UpdateCatalogTypeCommand, int>
        {
            private readonly CatalogContext _context;

            public UpdateCatalogTypeCommandHandler(CatalogContext context)
            { 
                _context = context;
            }

            public async Task<int> Handle(UpdateCatalogTypeCommand command, CancellationToken cancellationToken)
            {
                var type = await _context.CatalogTypes
                    .AsNoTracking()
                    .SingleOrDefaultAsync(s => s.Id == command.Type.Id, cancellationToken);
                if (type == null) return default;

                _context.CatalogTypes.Update(command.Type);
                await _context.SaveChangesAsync(cancellationToken);
                return command.Type.Id;
            }
        }
    }
}

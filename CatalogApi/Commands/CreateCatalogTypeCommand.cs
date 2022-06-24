using CatalogApi.Context;
using CatalogApi.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CatalogApi.Commands
{
    public class CreateCatalogTypeCommand : IRequest<CatalogType>
    {
        public string Type { get; set; }

        public class CreateCatalogItemCommandHandler : IRequestHandler<CreateCatalogTypeCommand, CatalogType>
        { 
            private readonly CatalogContext _context;

            public CreateCatalogItemCommandHandler(CatalogContext context)
            { 
                _context = context; 
            }

            public async Task<CatalogType> Handle(CreateCatalogTypeCommand command, CancellationToken cancellationToken)
            {
                var type = new CatalogType { Type = command.Type };
                _context.CatalogTypes.Add(type);
                await _context.SaveChangesAsync(cancellationToken);
                return type;
            }
        }
    }
}

using AutoMapper;
using CatalogApi.Context;
using CatalogApi.Models;
using MediatR;

namespace CatalogApi.Commands
{
    public class CreateCatalogItemCommand : IRequest<CatalogItem>
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public decimal Price { get; set; }

        public string? PictureUri { get; set; }

        public int CatalogTypeId { get; set; }

        public int CatalogGroupId { get; set; }

        public class CreateCatalogItemCommandHandler : IRequestHandler<CreateCatalogItemCommand, CatalogItem>
        {
            private readonly CatalogContext _context;
            private readonly IMapper _mapper;

            public CreateCatalogItemCommandHandler(CatalogContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }

            public async Task<CatalogItem> Handle(CreateCatalogItemCommand command, CancellationToken cancellationToken)
            {
                var catalogItem = _mapper.Map<CatalogItem>(command);
                _context.CatalogItems.Add(catalogItem);
                await _context.SaveChangesAsync(cancellationToken);
                return catalogItem;
            }
        }
    }
}

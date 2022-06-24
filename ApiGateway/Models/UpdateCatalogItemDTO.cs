namespace ApiGateway.Models
{
    public class UpdateCatalogItemDTO
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public decimal Price { get; set; }

        public string PictureUri { get; set; }

        public int CatalogTypeId { get; set; }

        public int CatalogGroupId { get; set; }
    }
}

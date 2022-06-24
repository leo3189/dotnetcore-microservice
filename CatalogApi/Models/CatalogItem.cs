﻿using System.ComponentModel.DataAnnotations.Schema;

namespace CatalogApi.Models
{
    public class CatalogItem
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public decimal Price { get; set; }
        
        public string? PictureUri { get; set; }

        [ForeignKey(nameof(CatalogType))]
        public int CatalogTypeId { get; set; }

        public virtual CatalogType CatalogType { get; set; }

        public int CatalogGroupId { get; set; }

        public virtual CatalogGroup CatalogGroup { get; set; }
    }
}

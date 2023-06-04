using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalProject.Application.Features.Products.Queries.GetAll
{
    public class ProductGetAllDto
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("picture")]
        public string Picture { get; set; }

        [JsonProperty("isOnSale")]
        public bool IsOnSale { get; set; }

        [JsonProperty("price")]
        public decimal Price { get; set; }

        [JsonProperty("salePrice")]
        public decimal SalePrice { get; set; }
    }

}

using MediatR;
using ProductCatalog.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductCatalog.Application.Queries
{
    public class GetCatalogItemQuery:IRequest<CatalogItem>
    {
        public int Id { get; set; }
    }
}

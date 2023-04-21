using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using ProductCatalog.Domain.Entities;

namespace ProductCatalog.Application.Commands
{
    public class UpdateCatalogItemCommand:IRequest<Unit>
    {
        public CatalogItem ProductToUpdate { get; set; }
    }
}

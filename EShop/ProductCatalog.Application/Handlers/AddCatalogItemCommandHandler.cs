﻿using MediatR;
using Microsoft.Extensions.Logging;
using ProductCatalog.Application.Commands;
using ProductCatalog.Application.Contracts.Persistence;
using ProductCatalog.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductCatalog.Application.Handlers
{
    public class AddCatalogItemCommandHandler : IRequestHandler<AddCatalogItemCommand,CatalogItem>
    {

        private readonly ICatalogItemRepository _repo;
        private ILogger<AddCatalogItemCommandHandler> _logger;

     

        public AddCatalogItemCommandHandler(ICatalogItemRepository repo, ILogger<AddCatalogItemCommandHandler> logger)
        {
             _repo = repo;
            _logger = logger;
        }

        public async Task<CatalogItem> Handle(AddCatalogItemCommand request, CancellationToken cancellationToken)
        {
            CatalogItem item = new CatalogItem(request.Name, request.Description,request.Price,request.PictureFileName, request.PictureUrl, request.CatalogTypeId, request.CatalogBrandId);
            item.Validate();
           
            return await _repo.Add(item);
        }
    }
  
}

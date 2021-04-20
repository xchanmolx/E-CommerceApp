using System.Collections.Generic;
using System.Threading.Tasks;
using API.Dtos;
using API.Errors;
using API.Helpers;
using AutoMapper;
using Core.Entities;
using Core.Interfaces;
using Core.Specifications;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class ProductsController : BaseApiController
    {
        private readonly IGenericRepository<Product> _productRepo;
        private readonly IGenericRepository<ProductBrand> _productBrandRepo;
        private readonly IGenericRepository<ProductType> _productTypeRepo;
        private readonly IMapper _mapper;
        public ProductsController(IGenericRepository<Product> productRepo, IGenericRepository<ProductBrand> productBrandRepo,
        IGenericRepository<ProductType> productTypeRepo, IMapper mapper)
        {
            _mapper = mapper;
            _productTypeRepo = productTypeRepo;
            _productBrandRepo = productBrandRepo;
            _productRepo = productRepo;
        }

    [HttpGet]
    public async Task<ActionResult<Pagination<ProductToReturnDto>>> GetProducts(
        [FromQuery]ProductSpecParams productParams)
    {
        var spec = new ProductsWithTypesAndBrandsSpecification(productParams);

        var countSpec = new ProductWithFiltersForCountSpecification(productParams);

        var totalItems = await _productRepo.CountAsync(countSpec);

        var products = await _productRepo.ListAsync(spec);

        var data = _mapper
            .Map<IReadOnlyList<Product>, IReadOnlyList<ProductToReturnDto>>(products); 

        return Ok(new Pagination<ProductToReturnDto>(productParams.PageIndex,
            productParams.PageSize, totalItems, data));
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ProductToReturnDto>> GetProduct(int id)
    {
        var spec = new ProductsWithTypesAndBrandsSpecification(id);

        var product = await _productRepo.GetEntityWithSpec(spec);

        if (product == null) return NotFound(new ApiResponse(404));

        return _mapper.Map<Product, ProductToReturnDto>(product);
    }

    [HttpGet("brands")]
    public async Task<ActionResult<IReadOnlyList<ProductBrand>>> GetProductBrands()
    {
        return Ok(await _productBrandRepo.ListAllAsync());
    }

    [HttpGet("types")]
    public async Task<ActionResult<IReadOnlyList<ProductType>>> GetProductTypes()
    {
        return Ok(await _productTypeRepo.ListAllAsync());
    }
}
}
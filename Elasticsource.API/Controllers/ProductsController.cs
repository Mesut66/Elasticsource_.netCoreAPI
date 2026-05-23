using Elasticsource.API.DTO;
using Elasticsource.API.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Immutable;

namespace Elasticsource.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {

        private readonly ProductService _productService;//readonly dememizin sebebi sadece okusun değiştiremesin newleyemesin diye

        public ProductsController(ProductService productService)
        {
            _productService = productService;
        }

        [HttpPost]
        [Route("create")]
        public async Task<ResponseDto<ProductDto>> Save(ProductCreateDto request)
        {
            return await _productService.SaveAsync(request);
        }

        [HttpGet]
        public async Task<ImmutableList<ProductDto>> GetAll()
        {
            return await _productService.GetAllAsync();
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<ResponseDto<ProductDto>> GetById(string id)
        {
            return await _productService.GeByIdAsync(id);
        }

        [HttpPut]
        [Route("{id}")]
        public async Task<ResponseDto<ProductDto>> Update(string id, ProductUpdateDto request)
        {
            request = request with { Id = id };
            return await _productService.UpdateAsync(request);
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<ResponseDto<bool>> Delete(string id)
        {
            return await _productService.DeleteAsync(id);
        }

    }
}

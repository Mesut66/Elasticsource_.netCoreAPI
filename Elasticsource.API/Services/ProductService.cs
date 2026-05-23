using Elastic.Clients.Elasticsearch;
using Elasticsource.API.DTO;
using Elasticsource.API.Models;
using Elasticsource.API.Repositories;
using Microsoft.OpenApi;
using System.Collections.Immutable;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Reflection;

namespace Elasticsource.API.Services
{
    public class ProductService
    {
        private readonly ProductRepository _productRepository;
        private readonly ILogger<ProductService> _logger;

        public ProductService(ProductRepository productRepository, ILogger<ProductService> logger)
        {
            _productRepository = productRepository;
            _logger = logger;
        }

        private string? GetColorDisplayName(EColor? color)
        {
            if (color == null)
                return null;

            var field = color.Value.GetType().GetField(color.Value.ToString());
            var attribute = field?.GetCustomAttribute<System.ComponentModel.DataAnnotations.DisplayAttribute>();
            return attribute?.Name ?? color.Value.ToString();
        }

        public async Task<ResponseDto<ProductDto>> SaveAsync(ProductCreateDto request)
        {
            try
            {
                var response = await _productRepository.SaveAsync(request.CreateProduct());

                if (response == null)
                    return ResponseDto<ProductDto>.Fail(new List<string> { "Kayıt esnasında hata meydana geldi" }, HttpStatusCode.InternalServerError);

                return ResponseDto<ProductDto>.Success(response.CreateDto(), HttpStatusCode.Created);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "SaveAsync hatası");
                return ResponseDto<ProductDto>.Fail(new List<string> { ex.Message }, HttpStatusCode.InternalServerError);
            }
        }
        public async Task<ImmutableList<ProductDto>> GetAllAsync()
        {
            var products = await _productRepository.GetAllAsync();

            return products.Select(i => new ProductDto(i.Id, i.Name, i.Price, i.Stock, new ProductFeatureDto(i.Feature?.Width, i.Feature?.Height, GetColorDisplayName(i.Feature?.Color))
            )).ToImmutableList();

        }

        public async Task<ResponseDto<ProductDto>> GeByIdAsync(string id)
        {
            var product = await _productRepository.GeByIdAsync(id);
            if (product == null)
                return ResponseDto<ProductDto>.Fail(new List<string> { "Ürün bulunamadı" }, HttpStatusCode.NotFound);

            return ResponseDto<ProductDto>.Success(product.CreateDto(), HttpStatusCode.OK);
        }

        public async Task<ResponseDto<ProductDto>> UpdateAsync(ProductUpdateDto request)
        {
            var existingProduct = await _productRepository.GeByIdAsync(request.Id);
            if (existingProduct == null)
                return ResponseDto<ProductDto>.Fail(new List<string> { "Ürün bulunamadı" }, HttpStatusCode.NotFound);
            var updateResult = await _productRepository.UpdateAsync(request);
            if (!updateResult)
                return ResponseDto<ProductDto>.Fail(new List<string> { "Güncelleme esnasında hata meydana geldi" }, HttpStatusCode.InternalServerError);
            var updatedProduct = await _productRepository.GeByIdAsync(request.Id);
            return ResponseDto<ProductDto>.Success(updatedProduct.CreateDto(), HttpStatusCode.OK);

        }

        public async Task<ResponseDto<bool>> DeleteAsync(string id)
        {
            var deleteResponse = await _productRepository.DeleteAsync(id);


            if (!deleteResponse.IsValidResponse && deleteResponse.Result == Result.NotFound)
            {
                return ResponseDto<bool>.Fail(new List<string> { "Silmeye çalıştığınız Ürün bulunamadı" }, HttpStatusCode.NotFound);
            }

            //hataları loglayacağız es burada bize yardımcı oluyor. Yukarda Ilogger'ı inject edip loglama yapabiliriz.
            if (!deleteResponse.IsValidResponse)
            {
                deleteResponse.TryGetOriginalException(out Exception? ex);
                _logger.LogError(ex, deleteResponse.ElasticsearchServerError.Error.ToString());
            }

            if (!deleteResponse.IsSuccess())
                return ResponseDto<bool>.Fail(new List<string> { "Silme esnasında bir hata oluştu" }, HttpStatusCode.InternalServerError);


            return ResponseDto<bool>.Success(true, HttpStatusCode.NoContent);
        }
    }
}

using Elastic.Clients.Elasticsearch;
using Elasticsource.API.DTO;
using Elasticsource.API.Models;
using System.Collections.Immutable;

namespace Elasticsource.API.Repositories
{
    public class ProductRepository
    {
        private readonly ElasticsearchClient _elasticClient;
        private readonly ILogger<ProductRepository> _logger;
        private const string indexName = "products";

        public ProductRepository(ElasticsearchClient elasticClient, ILogger<ProductRepository> logger)
        {
            _elasticClient = elasticClient;
            _logger = logger;
        }
        public async Task<Product> SaveAsync(Product product)
        {
            product.Created = DateTime.Now;

            // Elasticsearch için color'ı int olarak gönder (enum value)
            var docToIndex = new
            {
                product.Id,
                product.Name,
                product.Price,
                product.Stock,
                product.Created,
                product.Updated,
                Feature = product.Feature != null ? new
                {
                    product.Feature.Width,
                    product.Feature.Height,
                    Color = (int)product.Feature.Color
                } : (object)null
            };

            var response = await _elasticClient.IndexAsync(docToIndex, x => x.Index(indexName).Id(product.Id ?? Guid.NewGuid().ToString()));

            if (!response.IsSuccess())
            {
                _logger.LogError($"Elasticsearch Index hatası: {response.ApiCallDetails?.DebugInformation ?? "Bilinmeyen hata"}");
                return null;
            }

            product.Id = response.Id;
            return product;
        }

        public async Task<ImmutableList<Product>> GetAllAsync()
        {
            var result = await _elasticClient.SearchAsync<Product>(i => i.Index(indexName).Query(i => i.MatchAll()));

            foreach(var hit in result.Hits) hit.Source.Id = hit.Id;//es de id alanı otomatik olarak oluşturulur ve döner ama bizim modelimizde id alanı var ise onu doldurmamız gerekir.Bu yüzden her hit için source un id sini hit in id sine eşitliyoruz.
            return result.Documents.ToImmutableList();//ImmutableList te kimse değişiklik yapamaz
        }

        public async Task<Product> GeByIdAsync(string id)
        {
            var response = await _elasticClient.GetAsync<Product>(id, i => i.Index(indexName));

            if (!response.IsSuccess()) return null;

            response.Source.Id = response.Id;

            return response.Source;        
        }

        public async Task<bool> UpdateAsync(ProductUpdateDto updateProduct)
        {
            var response = await _elasticClient.UpdateAsync<Product, ProductUpdateDto>(indexName,updateProduct.Id, u => u.Doc(updateProduct));
            return response.IsSuccess();

        }

        ///<summary>
        /// Hata yönetimi burada yapılabilir.Örneğin silme işlemi sırasında ürün bulunamazsa veya es de bir hata meydana gelirse bunları yönetebiliriz.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>

        public async Task<DeleteResponse> DeleteAsync(string id)
        {
            var response = await _elasticClient.DeleteAsync<Product>(id, i => i.Index(indexName));

            return response;

        }
    }
}

using Elasticsource.API.DTO;
using Elasticsource.API.Models;
using Nest;
using System.Collections.Immutable;

namespace Elasticsource.API.Repositories
{
    public class ProductRepository
    {
        private readonly ElasticClient _elasticClient;
        private const string indexName = "products";

        public ProductRepository(ElasticClient elasticClient)
        {
            _elasticClient = elasticClient;
        }
        public async Task<Product> SaveAsync(Product product)
        {

            product.Created = DateTime.Now;
            var response = await _elasticClient.IndexAsync(product, x=> x.Index(indexName));//es de save yoktur Index vardır.Index save demektir.İndexleme işlemi yapar ve kaydeder.
            if (!response.IsValid) return null;//fast fail demek bu yazım şekli.İf bloğu tek satır ise süslü parantez kullanmaya gerek yoktur.

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

            if (!response.IsValid) return null;

            response.Source.Id = response.Id;

            return response.Source;        
        }

        public async Task<bool> UpdateAsync(ProductUpdateDto updateProduct)
        {
            var response = await _elasticClient.UpdateAsync<Product, ProductUpdateDto>(updateProduct.Id, u => u.Index(indexName).Doc(updateProduct));
            return response.IsValid;

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

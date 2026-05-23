

using Elastic.Clients.Elasticsearch;
using Elastic.Transport;

namespace Elasticsource.API.Extention
{
    public static class Elasticsource
    {

        public static void AddElasticsource(this IServiceCollection services,IConfiguration configuration)
        {

            //Elastic.Client kütüphanesi için gerekli yapılandırmayı yapın

            var userName = configuration.GetSection("Elasticsearch")["UserName"];
            var password = configuration.GetSection("Elasticsearch")["Password"];

            var settings = new ElasticsearchClientSettings(new Uri(configuration.GetSection("Elasticsearch")["Url"]!)).Authentication(new BasicAuthentication(userName!, password!));

            var client = new ElasticsearchClient(settings);

            services.AddSingleton(client);
        }
        
    }
}

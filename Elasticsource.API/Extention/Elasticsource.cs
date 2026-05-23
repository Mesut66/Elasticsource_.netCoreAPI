using Elasticsearch.Net;
using Nest;

namespace Elasticsource.API.Extention
{
    public static class Elasticsource
    {

        public static void AddElasticsource(this IServiceCollection services,IConfiguration configuration)
        {
            var uri = configuration.GetSection("Elasticsearch")["Url"];
            if (string.IsNullOrEmpty(uri))
                throw new InvalidOperationException("Elasticsearch Uri is not configured in appsettings.json");

            var pool = new SingleNodeConnectionPool(new Uri(uri));
            var settings = new ConnectionSettings(pool);
            var client = new ElasticClient(settings);
            services.AddSingleton(client);
        }
        
    }
}

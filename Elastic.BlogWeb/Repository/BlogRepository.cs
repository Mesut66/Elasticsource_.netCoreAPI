using Elastic.BlogWeb.Models;
using Elastic.Clients.Elasticsearch;

namespace Elastic.BlogWeb.Repository
{
    public class BlogRepository
    {
        private readonly ElasticsearchClient _client;
        private const string indexName = "blog";

        public BlogRepository(ElasticsearchClient client)
        {
            _client = client;
        }

        public async Task<Blog?> SaveAsync(Blog blog)
        {
            blog.Created = DateTime.Now;

            var response = await _client.IndexAsync(blog, x => x.Index(indexName));

            if (!response.IsValidResponse) return null;
   

            blog.Id = response.Id;
            return blog;
        }

        public async Task<List<Blog>> SearchAsync(string text)
        {
            //title ve content int göre arama yapılacak
            var result = await _client.SearchAsync<Blog>(s => s
                .Index(indexName)
                .Size(1000)
                .Query(q => q.Bool(b => b.Should(
                    s => s.Match(m => m.Field(f => f.Content).Query(text)),
                    s => s.MatchBoolPrefix(m => m.Field(f => f.Title).Query(text))
                ))));

            foreach (var hit in result.Hits) 
                hit.Source.Id = hit.Id;

            return result.Documents.ToList();
        }
    }
}


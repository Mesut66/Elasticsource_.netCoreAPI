using Elastic.BlogWeb.Models;
using Elastic.Clients.Elasticsearch;
using Elastic.Clients.Elasticsearch.QueryDsl;

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

            //Burası Elasticsearch in .net taki kodlamasında kullanılan bir yapıdır. 
            List<Action<QueryDescriptor<Blog>>> ListQuery = new();


            Action<QueryDescriptor<Blog>> matchAll = (q) => q.MatchAll();//Elasticsearch’teki tüm dökümanları getirir.


            Action<QueryDescriptor<Blog>> matchContent = (q) => q.Match(m => m.Field(f => f.Content).Query(text));//Match Full-text arama yapar


            Action<QueryDescriptor<Blog>> titleMatchBoolPrefix = (q) => q.MatchBoolPrefix(m => m.Field(f => f.Content).Query(text));//MatchBoolPrefixPrefix/auto complete araması yapar

            //Field Hangi alan (property/column) üzerinde arama yapacağını söyler.

            //Tag üzerinde arama
            Action<QueryDescriptor<Blog>> tagTerm = (q) => q.Term(t => t.Field(f => f.Tags).Value(text));


            //text boşsa tüm data gelsin
            if (string.IsNullOrEmpty(text))
            {
                ListQuery.Add(matchAll);
            }
            else
            {
                ListQuery.Add(matchContent);
                ListQuery.Add(titleMatchBoolPrefix);
                ListQuery.Add(tagTerm);
            }

            //title ve content int göre arama yapılacak
            var result = await _client.SearchAsync<Blog>(s => s.Index(indexName)
                                                            .Size(1000)
                                                            .Query(q => q.Bool(b => b.Should(ListQuery.ToArray()))));
                                                            //Should Elasticsearch’teki bool query içindeki OR mantığını temsil eder.

            foreach (var hit in result.Hits)
                hit.Source.Id = hit.Id;

            return result.Documents.ToList();
        }
    }
}


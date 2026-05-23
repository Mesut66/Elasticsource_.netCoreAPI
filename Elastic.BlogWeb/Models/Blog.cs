using System.Text.Json.Serialization;

namespace Elastic.BlogWeb.Models
{
    public class Blog
    {
        //Buradaki isimler ES de yaptığımız mapping ile aynı olmalıdır.Json propery ile aynı yaptım
        [JsonPropertyName("_id")]
        public string Id { get; set; } = null!;
        [JsonPropertyName("title")]
        public string Title { get; set; } = null!;
        [JsonPropertyName("content")]
        public string Content { get; set; } = null!;
        [JsonPropertyName("tags")]
        public string[] Tags { get; set; } = null!;
        [JsonPropertyName("user_id")]
        public Guid UserId { get; set; }
        [JsonPropertyName("created")]
        public DateTime Created { get; set; }
    }
}

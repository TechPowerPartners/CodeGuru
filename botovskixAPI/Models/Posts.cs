using System.Text.Json.Serialization;

namespace botovskixAPI.Models
{
    public class Posts
    {
        [JsonIgnore]
        public Guid Id { get; set; } = Guid.NewGuid();
        public string PostContent { get; set; }

        
    }
}

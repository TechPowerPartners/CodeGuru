using System.Text.Json.Serialization;

namespace botovskixAPI.Models
{
    public class Users
    {
        [JsonIgnore]
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Name { get; set; }

        public string Password { get; set; }
    }
}

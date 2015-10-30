using Newtonsoft.Json;

namespace RestApiDemoRepository
{
    public class Task
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("done")]
        public bool Done { get; set; }
    }
}

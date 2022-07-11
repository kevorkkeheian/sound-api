using Newtonsoft.Json;

namespace Application.Models;

public class RocksetRequest
{
    [JsonProperty("collections")]
    public string[] Collections { get; set; }

    [JsonProperty("aliases")]
    public string[] Aliases { get; set; }

    [JsonProperty("results")]
    public List<Sound> Results { get; set; }

}
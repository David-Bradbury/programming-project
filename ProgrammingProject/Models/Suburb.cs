using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ProgrammingProject.Models
{
    public class Suburb
    {
        [JsonProperty("postcode")]
        [Required, Key]
        public string Postcode { get; set; }
        [JsonProperty("suburb")]
        [Required, Key]
        public string SuburbName { get; set; }
        [JsonProperty("state")]
        [Required, Key]
        public string State { get; set; }
        [JsonProperty("lat")]
        public string Lat { get; set; }
        [JsonProperty("lon")]
        public string Lon { get; set; }

        public virtual List<Walks> Walks { get; set; }
        public virtual List<Walker> Walkers { get; set; } 
        public virtual List<Owner> Owners { get; set; }
    }
}

using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ProgrammingProject.Models
{
    public class Suburb
    {
        /* Mattieeec3 and Marius (1966) Newtonsoft deserialize doesn't convert string to datetime, Stack Overflow. 
         * Available at: https://stackoverflow.com/questions/59057738/newtonsoft-deserialize-doesnt-convert-string-to-datetime 
         * (Accessed: April 23, 2023). 
         */

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

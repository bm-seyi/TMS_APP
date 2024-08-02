using System.Text.Json.Serialization;

namespace TMS_APP.Utilities.API.Schema
{
    public class TomTomRoot
    {
        public required List<Incidents> Incidents {get; set;}
    }

    public class Incidents
    {
        [JsonPropertyName("type")]
        public string? Type { get; set; }
        
        [JsonPropertyName("properties")]
        public required Properties Properties { get; set; }

        [JsonPropertyName("geometry")]
        public required Geometry Geometry {get; set;}
    }

    public class Properties
    {
        [JsonPropertyName("id")]
        public string? id { get; set; }

        [JsonPropertyName("iconCategory")]
        public int iconCategory { get; set; }

        [JsonPropertyName("magnitudeOfDelay")]
        public int magnitudeOfDelay { get; set; }

        [JsonPropertyName("startTime")]
        public DateTime? startTime { get; set; }

        [JsonPropertyName("endTime")]
        public DateTime? endTime { get; set; }

        [JsonPropertyName("from")]
        public required string from { get; set; }

        [JsonPropertyName("to")]
        public required string to { get; set; }

        [JsonPropertyName("length")]
        public double length { get; set; }

        [JsonPropertyName("delay")]
        public int? delay { get; set; }

        [JsonPropertyName("roadNumbers")]
        public required List<string?> roadNumbers { get; set; }

        [JsonPropertyName("lastReportTime")]
        public DateTime? lastReportTime { get; set; }

        [JsonPropertyName("events")]
        public required List<Events> events { get; set;}

    }

    public class Events 
    {
        [JsonPropertyName("code")]
        public int code { get; set; }

        [JsonPropertyName("description")]
        public string? description { get; set; }

        [JsonPropertyName("iconCategory")]
        public int iconCategory {get; set; }
    }

    public class Geometry 
    {
        [JsonPropertyName("type")]
        public string? Type { get; set; }
        [JsonPropertyName("coordinates")]
        public required List<List<double>> Coordinates { get; set; }

    }
}
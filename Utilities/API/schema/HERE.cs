using System.Numerics;
using System.Text.Json.Serialization;

namespace TMS_APP.Utilities.API.Schema
{
    public class Point
    {
        [JsonPropertyName("lat")]
        public double Lat { get; set; }
        [JsonPropertyName("lng")]
        
        public double Lng { get; set; }
    }

    public class Link
    {
        [JsonPropertyName("points")]
        public required List<Point> points {get; set;}
        [JsonPropertyName("length")]
        public required object Length { get; set;}
    }

    public class Shape 
    {
        [JsonPropertyName("links")]
        public required List<Link> links {get; set;}
    }

    public class Location
    {
        [JsonPropertyName("length")]
        public required object length { get; set; }

        public required Shape shape { get; set; }
    }

    public class Description
    {
        [JsonPropertyName("value")]
        public required string Value { get; set; }
        [JsonPropertyName("language")]
        public string? Language { get; set; }
    }

    public class Summary
    {
        [JsonPropertyName("value")]
        public required string Value {get; set; }
        [JsonPropertyName("language")]
        public string? Language {get; set; }
    }

    public class IncidentDetails 
    {
        [JsonPropertyName("id")]
        public required string Id { get; set; }

        [JsonPropertyName("hrn")]
        public required string Hrn { get; set; }

        [JsonPropertyName("originalId")]
        public required string OriginalId { get; set; }

        [JsonPropertyName("originalHrn")]
        public required string OriginalHrn { get; set; }

        [JsonPropertyName("startTime")]
        public DateTime StartTime { get; set; }

        [JsonPropertyName("endTime")]
        public DateTime EndTime { get; set; }

        [JsonPropertyName("entryTime")]
        public DateTime EntryTime { get; set; }

        [JsonPropertyName("roadClosed")]
        public bool RoadClosed { get; set; }

        [JsonPropertyName("criticality")]
        public required string Criticality { get; set; }

        [JsonPropertyName("type")]
        public required string Type { get; set; }

        [JsonPropertyName("codes")]
        public List<int>? Codes { get; set; }

        [JsonPropertyName("description")]
        public required Description description{ get; set; }

        [JsonPropertyName("summary")]
        public required Summary summary{ get; set; }

        [JsonPropertyName("comment")]
        public string? Comment { get; set; }
    } 

    public class Results
    {
        [JsonPropertyName("location")]
        public required Location location { get; set; }

        [JsonPropertyName("incidentDetails")] 
        public required IncidentDetails incidentDetails { get; set; }
    }

    public class HERERoot 
    {
        [JsonPropertyName("sourceUpdated")]
        public DateTime SourceUpdated { get; set; }

        [JsonPropertyName("results")]
        public required List<Results> results { get; set; }
    }
}

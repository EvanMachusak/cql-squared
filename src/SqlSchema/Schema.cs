namespace CqlSquared.SqlSchema
{
    using System;
    using System.Collections.Generic;

    using System.Text.Json;
    using System.Text.Json.Serialization;
    using System.Globalization;
    using System.Diagnostics;
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

    [DebuggerDisplay("{View} (for resource {Resource})")]
    public partial class Schema
    {
        [JsonPropertyName("view")]
        public string? View { get; set; }

        [JsonPropertyName("resource")]
        public string? Resource { get; set; }

        [JsonPropertyName("filters")]
        public string?[] Filters { get; set; }

        [JsonPropertyName("vars")]
        public IDictionary<string, string>? Vars { get; set; }

        [JsonPropertyName("fhirVersion")]
        public string?[] FhirVersion { get; set; }

        [JsonPropertyName("columns")]
        public IDictionary<string,string>? Columns { get; set; }

        [JsonPropertyName("foreign")]
        public IDictionary<string,string>? Foreign { get; set; }

    }

    public class Relationship
    {
        [JsonPropertyName("view")]

        public string? View { get; set; }
        
        [JsonPropertyName("column")]
        public string? Column { get; set; }

    }

#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

}

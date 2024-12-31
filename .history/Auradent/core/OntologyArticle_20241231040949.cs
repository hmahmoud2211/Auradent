using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Auradent.core
{
    public class OntologyArticle
    {
        public OntologyArticle()
        {
            Meta = new Dictionary<string, List<MetaValue>>();
        }

        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("lbl")]
        public string Label { get; set; }

        [JsonPropertyName("meta")]
        public Dictionary<string, List<MetaValue>> Meta { get; set; }
    }

    public class MetaValue
    {
        [JsonPropertyName("val")]
        public string Value { get; set; }

        [JsonPropertyName("xrefs")]
        public List<string> Xrefs { get; set; }

        [JsonPropertyName("pred")]
        public string Predicate { get; set; }
    }
}
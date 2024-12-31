using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Auradent.core
{
    public class OntologyArticle
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("lbl")]
        public string Label { get; set; }

        [JsonPropertyName("meta")]
        public OntologyMeta Meta { get; set; }
    }

    public class OntologyMeta
    {
        [JsonPropertyName("definition")]
        public OntologyDefinition Definition { get; set; }

        [JsonPropertyName("synonyms")]
        public List<OntologySynonym> Synonyms { get; set; }

        [JsonPropertyName("xrefs")]
        public List<string> Xrefs { get; set; }
    }

    public class OntologyDefinition
    {
        [JsonPropertyName("val")]
        public string Value { get; set; }

        [JsonPropertyName("xrefs")]
        public List<string> Xrefs { get; set; }
    }

    public class OntologySynonym
    {
        [JsonPropertyName("val")]
        public string Value { get; set; }

        [JsonPropertyName("pred")]
        public string Predicate { get; set; }

        [JsonPropertyName("xrefs")]
        public List<string> Xrefs { get; set; }
    }
}
using System;
using System.Collections.Generic;

namespace Auradent.core
{
    public class OntologyArticle
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Definition { get; set; }
        public List<string> Synonyms { get; set; }
        public List<string> Xrefs { get; set; }
        public List<string> SubClassOf { get; set; }
        public DateTime LastUpdated { get; set; }
    }
} 
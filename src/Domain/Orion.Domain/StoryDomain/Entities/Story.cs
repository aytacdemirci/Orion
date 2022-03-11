using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orion.Domain.StoryDomain.Entities
{
    [JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy))]
    public class Story
    {
        public Guid Id { get; set; }
        public string Text { get; set; }

        public string[] Images { get; set; }
    }
}

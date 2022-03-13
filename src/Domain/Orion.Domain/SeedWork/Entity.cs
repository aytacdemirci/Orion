using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orion.Domain.SeedWork
{
    public abstract class Entity
    {
        [JsonProperty("id")]
        public Guid Id { get; protected set; } 
        [JsonProperty("createdAt")]
        public DateTimeOffset CreatedAt { get; protected set; } 
    }
}
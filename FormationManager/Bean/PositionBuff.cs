using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FormationManager.Bean
{
    public class PositionBuff
    {
        [JsonConverter(typeof(StringEnumConverter))]
        public CharacterPropertyType BuffType { get; set; }
        public string Value { get; set; }

        public PositionBuff(CharacterPropertyType type, string v)
        {
            this.BuffType = type;
            this.Value = v;
        }
    }
}

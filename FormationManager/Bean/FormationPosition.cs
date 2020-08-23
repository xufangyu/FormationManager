using FormationManager.Bean;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Collections.Generic;

namespace FormationManager
{
    public class FormationPosition
    {
        public List<PositionBuff> Buffs { get; set; }
    }
}
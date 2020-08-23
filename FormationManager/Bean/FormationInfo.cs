using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using UnityEngine;

namespace FormationManager
{
    public class FormationInfo
    {
        public string Name { get; set; }
        public string NameCN { get; set; }
        public List<FormationPosition> Positions { get; set; }
    }

}

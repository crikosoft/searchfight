using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace searchfight.SearchRunners
{
    [XmlInclude(typeof(WebClientSearchRunner))]
    public abstract class SerializableSearchRunner : ISearchRunner
    {

        [XmlAttribute]
        public string Name { get; set; }

        [XmlAttribute]
        [DefaultValue(false)]
        public bool Disabled { get; set; }

        public abstract Task<long> Run(string query);

    }
}

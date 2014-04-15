using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Adage.VSGrunt.Config
{
    public class Trigger
    {
        public string Pattern;
        public string GruntTask = "default";

        public bool Matches(string path)
        {
            return Regex.Match(path, this.Pattern).Success;
        }

        public Trigger(XElement element)
        {
            this.Pattern = element.Attribute("pattern").Value;
            
            var gruntTask = element.Attribute("gruntTask").Value;
            if (gruntTask != null)
            {
                this.GruntTask = gruntTask;
            }

        }

    }
}

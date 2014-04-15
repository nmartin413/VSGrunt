using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Adage.VSGrunt.Config
{
    public class GruntConfig
    {
        private XDocument document;

        public IEnumerable<Trigger> GetTriggersForPath(string path)
        {
            return this.GetTriggers().Where(t => t.Matches(path));
        }

        public IEnumerable<Trigger> GetTriggers()
        {
            IEnumerable<XElement> triggerElements = this.document.Descendants("VSGrunt").Descendants("triggers").Descendants("trigger");
            return triggerElements.Select(delegate(XElement element)
            {
                return new Trigger(element);
            });
        }

        public GruntConfig(string path)
        {
            this.document = XDocument.Load(path);
        }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Shell;
using System.ComponentModel;

namespace Adage.VSGrunt
{
    class ProjectProperties
    {
        public bool IsDirty { get; set; }
        private string myConfigProp;

        [Category("My Category")]
        [DisplayName("My Config Property")]
        [Description("My Description")]
        public string MyConfigProp
        {
            get { return myConfigProp; }
            set { myConfigProp = value; IsDirty = true; }
        }
    }
}

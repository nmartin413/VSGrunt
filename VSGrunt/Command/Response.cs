using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adage.VSGrunt
{
    public class NodeResponse
    {
        public Process Process;

        public string FullText = String.Empty;
        public string Message = String.Empty;

        public bool IsError = false;
        public bool IsFinal = false;
    }
}

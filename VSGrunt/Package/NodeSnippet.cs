using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adage.VSGrunt
{
    public class NodeSnippet
    {
        string contents;

        public NodeSnippet(string snippet)
        {
            contents = snippet
                .Replace("\n", " ")
                .Replace("\r", "")
                .Replace("\t", "");
        }

        public void Execute(string path, NodeDelegate nodeDelegate)
        {
            new NodeCommand
            {
                Module = "node",
                IsNPM = false,
                Arguments = String.Format("\"-e\" \"{0}\"", contents),
                Path = path,
                Delegate = nodeDelegate
            }.Start();
        }

    }
}

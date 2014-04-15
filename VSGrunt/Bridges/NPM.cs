using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adage.VSGrunt
{
    static partial class NPM
    {
        private static NodeDelegate defaultDelegate = delegate(NodeResponse response) { };

        public static void Install()
        {
            string projectPath = Utility.GetSelectedPath();
            Install(projectPath, defaultDelegate);
        }

        public static void Install(string projectPath)
        {
            Install(projectPath, defaultDelegate);
        }

        public static void Install(string projectPath, NodeDelegate nodeDelegate)
        {
            try
            {
                UserInterface.OutputPane.Show();
                var cmd = new NodeCommand
                {
                    Module = "npm",
                    Arguments = "install",
                    Path = projectPath,
                    Delegate = delegate(NodeResponse response)
                    {
                        UserInterface.Log(response.Message);
                        nodeDelegate.Invoke(response);
                    }
                };
                cmd.Start();
            }
            catch (Exception ex)
            {
                UserInterface.ShowError(ex.ToString());
            }
        }

    }
}

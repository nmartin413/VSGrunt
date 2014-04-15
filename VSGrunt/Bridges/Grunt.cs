using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace Adage.VSGrunt
{
    static partial class Grunt
    {
        public static List<GruntTask> Tasks = new List<GruntTask>();

        public static readonly string CommandModuleString = "grunt";

        private static NodeDelegate defaultDelegate = delegate(NodeResponse response) { };

        public static void Default(string targetPath)
        {
            Default(targetPath, defaultDelegate);
        }

        public static void Default(string targetPath, NodeDelegate nodeDelegate)
        {
            UserInterface.OutputPane.Show();
            try
            {
                Run(targetPath, "default", nodeDelegate);
            }
            catch (Exception ex)
            {
                UserInterface.ShowError(ex.ToString());
            }
        }

        public static void Run(string targetPath, string taskName)
        {
            Run(targetPath, taskName, defaultDelegate);
        }

        public static void Run(string targetPath, string taskName, NodeDelegate nodeDelegate)
        {
            var cmd = new NodeCommand
            {
                Module = CommandModuleString,
                Arguments = String.Format("--no-color {0}", taskName),
                Path = targetPath,
                Delegate = delegate(NodeResponse response)
                {
                    UserInterface.Log(response.Message);
                    nodeDelegate.Invoke(response);
                }
            };
            cmd.Start();
        }

        public static void LoadTasks(string path)
        {
            LoadTasks(path, defaultDelegate);
        }

        public static void LoadTasks(string path, NodeDelegate nodeDelegate)
        {
            var snip = new NodeSnippet(Resources.NodeSnippets.GetGruntTasks);
            snip.Execute(path, delegate(NodeResponse nodeResponse)
            {
                if (nodeResponse.Process.HasExited)
                {
                    ParseTasks(nodeResponse.FullText);
                }
                nodeDelegate.Invoke(nodeResponse);
            });
        }

        public static void ParseTasks(string json)
        {
            if (!String.IsNullOrEmpty(json))
            {
                Tasks.Clear();
                try
                {
                    Tasks.AddRange(JsonConvert.DeserializeObject<List<GruntTask>>(json));
                }
                catch (Exception ex)
                {
                    UserInterface.Log("Error Loading Grunt tasks for menu....");
                    UserInterface.Log(ex.Message);
                }
            }
        }

    }
}

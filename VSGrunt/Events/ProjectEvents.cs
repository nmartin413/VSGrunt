using Adage.VSGrunt.Config;
using EnvDTE;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adage.VSGrunt
{
    class ProjectEvents
    {
        public static DocumentEvents DocumentEvents;

        public static void Initialize()
        {
            DocumentEvents.DocumentSaved += DocumentEvents_DocumentSaved;
        }

        static void DocumentEvents_DocumentSaved(Document document)
        {
            Project proj = document.ProjectItem.ContainingProject;
            if (proj.GetIsVSGruntProject())
            {
                if (document.FullName.Contains("package.json"))
                {
                    NPM.Install(proj.GetProjectPath());
                }
                else
                {
                    GruntConfig config = proj.GetGruntConfig();
                    var triggers = config.GetTriggersForPath(document.Path + document.FullName);

                    foreach (Trigger trigger in triggers)
                    {
                        UserInterface.OutputPane.Pane.Clear();
                        UserInterface.OutputPane.Show();
                        UserInterface.Log("File save detected.");

                        Grunt.Run(proj.GetProjectPath(), trigger.GruntTask);
                    }

                }

            }

        }

    }
}

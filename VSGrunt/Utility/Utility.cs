using EnvDTE;
using Microsoft.VisualStudio.Shell;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Resources;
using System.Text;
using System.Threading.Tasks;


namespace Adage.VSGrunt
{
    class Utility
    {
        public static Package Package;
        public static DTE _dte;

        public static DTE dte
        {
            get
            {
                if (null != _dte) return _dte;
                else throw new Exception("Application reference not set.");
            }
            set { _dte = value; }
        }

        public static bool SolutionIsLoaded
        {
            get
            {
                return !String.IsNullOrEmpty(dte.Solution.FullName);
            }
        }

        public static string GetSolutionDirectory()
        {
            if (!SolutionIsLoaded) throw new Exception("No solution found.");
            return System.IO.Path.GetDirectoryName(dte.Solution.FullName);
        }

        public static IEnumerable<string> GetProjectNames()
        {
            var count = dte.Solution.Projects.Count;
            if (count == 0) throw new Exception("No Projects loaded.");

            var list = new List<string>();
            foreach (Project project in dte.Solution.Projects)
            {
                list.Add(project.Name);
            }
            return list;
        }

        public static IEnumerable<string> ProjectPaths
        {
            get
            {
                var root = GetSolutionDirectory();
                return GetProjectNames().Select(name => String.Format("{0}\\{1}", root, name));
            }
        }

        public static Project GetSelectedProject()
        {
            var _applicationObject = Package.GetGlobalService(typeof(DTE)) as EnvDTE80.DTE2;
            UIHierarchy uih = _applicationObject.ToolWindows.SolutionExplorer;
            Array selectedItems = (Array)uih.SelectedItems;

            if (null != selectedItems)
            {
                foreach (UIHierarchyItem selItem in selectedItems)
                {
                    return selItem.Object as Project;
                }
            }

            return null;
        }

        public static GruntProject GetSelectedGruntProject()
        {
            var proj = GetSelectedProject();
            if (proj != null)
            {
                return new GruntProject(proj);
            }
            else
            {
                return new GruntProject();
            }
        }

        public static string GetSelectedPath()
        {
            return GetSelectedGruntProject().GetProjectPath();
        }

    }
}

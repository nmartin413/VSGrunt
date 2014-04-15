using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EnvDTE;
using System.IO;
using Adage.VSGrunt.Config;

namespace Adage.VSGrunt
{
    public static class ProjectExtension
    {
        public static string GetProjectPath(this Project project)
        {
            string projectFile = project.FullName;
            return projectFile.Remove(projectFile.LastIndexOf("\\"));
        }

        public static string GetGruntConfigPath(this Project project)
        {
            return String.Concat(project.GetProjectPath(), "\\VSGrunt.xml");
        }

        public static bool GetIsVSGruntProject(this Project project)
        {
            return File.Exists(project.GetGruntConfigPath());
        }

        public static GruntConfig GetGruntConfig(this Project project)
        {
            if (project.GetIsVSGruntProject())
            {
                return new GruntConfig(project.GetGruntConfigPath());
            }
            else
            {
                return null;
            }
        }

    }


    public class GruntProject
    {
        public Project Project;

        public bool HasProject
        {
            get
            {
                return Project != null;
            }
        }

        public GruntProject(Project project = null)
        {
            if (project != null)
            {
                Project = project;
            }
        }

        public string GetGruntfile()
        {
            var path = String.Empty;

            var baseGruntfilePath = String.Concat(GetProjectPath(), "\\gruntfile.js");

            if (File.Exists(baseGruntfilePath))
            {
                path = baseGruntfilePath;
            }

            return path;
        }

        public string GetProjectPath()
        {
            string projectFile = Project.FullName;
            return projectFile.Remove(projectFile.LastIndexOf("\\"));
        }
    }
}

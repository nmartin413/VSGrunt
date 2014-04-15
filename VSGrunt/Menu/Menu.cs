using EnvDTE;
using Microsoft.VisualStudio.CommandBars;
using Microsoft.VisualStudio.Shell;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Adage.VSGrunt
{
    class Menu
    {
        public static Package Package;
        public static OleMenuCommandService CommandService;

        private static List<GruntTask> tasks = new List<GruntTask>();
        private static List<OleMenuCommand> taskCommands = new List<OleMenuCommand>();
        private static int baseTaskId = (int)PkgCmdIDList.cmdidGruntTask;

        private static Guid guid = GuidList.guidParfait_ToolMenuCmdSet;

        private static bool IsRefreshingTasks = false;

        public static void Initialize()
        {
            AddCommand(PkgCmdIDList.cmdidRunGrunt, RunGruntCallback);
            AddCommand(PkgCmdIDList.cmdidNPMInstall, NPMInstallCallback);

            InitTaskMenu();
            InitTaskLoading();
            BuildTaskMenu();
        }

        private static void OnCommandQueryStatus(object sender, EventArgs e)
        {
            var mc = sender as OleMenuCommand;
            //var project = Utility.GetSelectedGruntProject();
            //var gruntPath = project.GetGruntfile();
            //mc.Visible = !String.IsNullOrEmpty(gruntPath);
            Project proj = Utility.GetSelectedProject();
            mc.Visible = proj.GetIsVSGruntProject();
        }

        #region Task Menu Items

        private static void BuildTaskMenu()
        {
            ClearTaskCommands();

            for (int i = 0; i < tasks.Count; i++)
            {
                AddTaskCommand(tasks[i], i);
            }
        }

        private static void RefreshTasks()
        {
            if (IsRefreshingTasks == false)
            {
                IsRefreshingTasks = true;
                UserInterface.Group("Refreshing Grunt tasks...");
                Grunt.LoadTasks(Utility.GetSelectedPath(), delegate(NodeResponse nodeResponse)
                {
                    if (nodeResponse.Process.HasExited)
                    {
                        tasks = Grunt.Tasks.ToList();
                        BuildTaskMenu();

                        UserInterface.Log("Finished!");
                        UserInterface.GroupEnd();
                        IsRefreshingTasks = false;
                    }
                });
            }
        }

        private static void RefreshTasks(object sender, EventArgs e)
        {
            RefreshTasks();
        }

        private static void InitTaskMenu()
        {
            var cmdId = new CommandID(guid, (int)PkgCmdIDList.cmdidGruntTaskMenu);
            var mc = new OleMenuCommand(OnTaskLoadingExecute, cmdId);
            mc.BeforeQueryStatus += OnCommandQueryStatus;
            CommandService.AddCommand(mc);
        }

        private static void InitTaskLoading()
        {
            var cmdId = new CommandID(guid, (int)PkgCmdIDList.cmdidGruntTaskLoading);
            var mc = new OleMenuCommand(OnTaskLoadingExecute, cmdId);
            mc.BeforeQueryStatus += OnTaskLoadingQueryStatus;
            CommandService.AddCommand(mc);
        }

        private static void ClearTaskCommands()
        {
            foreach (OleMenuCommand menuCommand in taskCommands)
            {
                CommandService.RemoveCommand(menuCommand);
            }
            taskCommands.Clear();
        }

        private static void AddTaskCommand(GruntTask task, int index)
        {
            var cmdId = new CommandID(guid, baseTaskId + index);
            var mc = new OleMenuCommand(OnTaskExecute, cmdId);
            mc.Visible = true;
            mc.BeforeQueryStatus += OnTaskQueryStatus;
            mc.BeforeQueryStatus += OnCommandQueryStatus;
            CommandService.AddCommand(mc);
            taskCommands.Add(mc);
        }

        private static void OnTaskLoadingQueryStatus(object sender, EventArgs e)
        {
            var menuCommand = sender as OleMenuCommand;
            var hasTasks = tasks.Count != 0;
            if (hasTasks)
            {
                menuCommand.Text = "Reload Tasks";
                menuCommand.Enabled = true;
            }
            else
            {
                menuCommand.Text = "Loading Tasks...";
                menuCommand.Enabled = false;
                RefreshTasks();
            }
        }

        private static void OnTaskLoadingExecute(object sender, EventArgs e)
        {
            RefreshTasks();
        }

        private static void OnTaskQueryStatus(object sender, EventArgs e)
        {
            var menuCommand = sender as OleMenuCommand;
            var task = TaskForMenuCommand(menuCommand);
            menuCommand.Text = String.Format("Grunt > {0}", task.Name);
            menuCommand.Visible = true;
        }

        private static void OnTaskExecute(object sender, EventArgs e)
        {
            var menuCommand = sender as OleMenuCommand;
            var task = TaskForMenuCommand(menuCommand);

            UserInterface.OutputPane.Pane.Clear();
            UserInterface.OutputPane.Show();
            Grunt.Run(Utility.GetSelectedPath(), task.Name, delegate (NodeResponse response)
            {
                UserInterface.Log(response.Message);
            });
        }

        private static GruntTask TaskForMenuCommand(OleMenuCommand menuCommand)
        {
            int TaskItemIndex = menuCommand.CommandID.ID - baseTaskId;
            return tasks[TaskItemIndex];
        }

        #endregion

        private static MenuCommand GetCommand(uint cmdid)
        {
            return CommandService.FindCommand(new CommandID(guid, (int)cmdid));
        }

        private static void AddCommand(uint cmdid, EventHandler handler)
        {
            var commandId = new CommandID(guid, (int)cmdid);
            var menuCommand = new OleMenuCommand(handler, commandId);
            menuCommand.CommandChanged += menuCommand_CommandChanged;
            menuCommand.BeforeQueryStatus += OnCommandQueryStatus;
            CommandService.AddCommand(menuCommand);
        }

        private static void menuCommand_CommandChanged(object sender, EventArgs e)
        {
            var command = sender as MenuCommand;
            command.Enabled = Utility.SolutionIsLoaded;
        }

        private static void RunGruntCallback(object sender, EventArgs e)
        {
            UserInterface.OutputPane.Pane.Clear();
            UserInterface.Group("Running Grunt...");
            string selectedPath = Utility.GetSelectedPath();
            Grunt.Default(selectedPath, delegate(NodeResponse nodeResponse)
            {
                UserInterface.GroupEnd();
            });
        }

        private static void NPMInstallCallback(object sender, EventArgs e)
        {
            UserInterface.Log("Running NPM Install...");
            NPM.Install();
        }



    }
}

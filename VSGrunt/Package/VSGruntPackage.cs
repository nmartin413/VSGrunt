using System;
using System.Diagnostics;
using System.Globalization;
using System.Runtime.InteropServices;
using System.ComponentModel.Design;
using System.Linq.Expressions;
using Microsoft.Win32;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.VisualStudio.OLE.Interop;
using Microsoft.VisualStudio.Shell;
using EnvDTE;


namespace Adage.VSGrunt
{
    [PackageRegistration(UseManagedResourcesOnly = true)]
    [InstalledProductRegistration("#110", "#112", "1.0", IconResourceID = 400)]
    [ProvideMenuResource("Menus.ctmenu", 1)]
    //[ProvideToolWindow(typeof(UI.ToolWindow))]
    //[ProvideToolWindowVisibility(typeof(UI.ToolWindow), GuidList.UICONTEXT_SolutionExistsString)]
    [Guid(GuidList.guidParfait_ToolMenuPkgString)]
    [ProvideAutoLoad(GuidList.UICONTEXT_SolutionExistsString)]
    public sealed class Parfait_ToolMenuPackage : Package
    {

        public Parfait_ToolMenuPackage()
        {
            Debug.WriteLine(String.Format("Starting {0} extension", Resources.Strings.AppName));
        }

        protected override void Initialize()
        {
            Utility.Package = this;
            Utility.dte = GetService(typeof(DTE)) as DTE;
            base.Initialize();

            InitUserInterface();
            InitMenus();
            InitDocumentEvents();

            UserInterface.Log(String.Format("{0} ready.", Resources.Strings.AppName));
        }

        private void InitUserInterface()
        {
            UserInterface.OutputWindow = GetService(typeof(SVsOutputWindow)) as IVsOutputWindow;
            UserInterface.UIShell = GetService(typeof(SVsUIShell)) as IVsUIShell;
            UserInterface.Initialize();

            UserInterface.Log("UI Initialized");
        }

        private void InitDocumentEvents()
        {
            var dte2 = GetService(typeof(SDTE)) as EnvDTE80.DTE2;
            ProjectEvents.DocumentEvents = dte2.Events.DocumentEvents;
            ProjectEvents.Initialize();

            UserInterface.Log("Document Events Initialized");
        }

        private void InitMenus()
        {
            Menu.Package = this;
            Menu.CommandService = GetService(typeof(IMenuCommandService)) as OleMenuCommandService;
            Menu.Initialize();

            UserInterface.Log("Menus Initialized");
        }

    }
}

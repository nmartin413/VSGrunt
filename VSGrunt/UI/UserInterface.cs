using Microsoft.VisualStudio.Shell.Interop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adage.VSGrunt
{
    class UserInterface
    {
        public static IVsOutputWindow OutputWindow;
        public static IVsUIShell UIShell;
        public static OutputPane OutputPane;

        private static int indentationLevel;

        public static void Initialize()
        {
            OutputPane = new OutputPane(OutputWindow);
        }

        public static void Log(string message)
        {
            OutputPane.Show();
            string line = String.Concat(new String('\t', indentationLevel), message);
            OutputPane.WriteLine(line);
        }

        public static void Group(string groupTitle)
        {
            Log(groupTitle);
            indentationLevel++;
        }

        public static void GroupEnd(string message = "Done")
        {
            if (indentationLevel != 0)
            {
                indentationLevel--;
            }
            OutputPane.WriteLine(message);
        }

        public static void ClearOutput()
        {
            OutputPane.Pane.Clear();
        }

        public static void ShowError(string message)
        {
            Guid clsid = Guid.Empty;
            int result;
            Microsoft.VisualStudio.ErrorHandler.ThrowOnFailure(
                UIShell.ShowMessageBox(0, ref clsid, Resources.Strings.ErrorDialogTitle, message, string.Empty, 0,
                    OLEMSGBUTTON.OLEMSGBUTTON_OK, OLEMSGDEFBUTTON.OLEMSGDEFBUTTON_FIRST, OLEMSGICON.OLEMSGICON_INFO, 0,
                    out result)
            );
        }

    }
}

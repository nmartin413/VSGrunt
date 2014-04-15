using Microsoft.VisualStudio.Shell.Interop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adage.VSGrunt
{
    class OutputPane
    {
        string Title = Resources.Strings.AppName;
        bool Visible = true;
        bool ClearWithSoluton = true;
        Guid Guid = GuidList.guidParfait_ToolMenuOutputPane;

        IVsOutputWindow Window;

        IVsOutputWindowPane _pane;
        public IVsOutputWindowPane Pane
        {
            get
            {
                if (_pane == null)
                {
                    Window.CreatePane(ref Guid, Title, Convert.ToInt32(Visible), Convert.ToInt32(ClearWithSoluton));
                    Window.GetPane(ref Guid, out _pane);
                }
                 return _pane;
            }
        }


        public void WriteLine(string contents)
        {
            Pane.OutputString(String.Format("{0}\n", contents));
        }

        public void Show()
        {
            Pane.Activate();
        }

        public void Hide()
        {
            Pane.Hide();
        }

        public OutputPane(IVsOutputWindow outputWindow)
        {
            Window = outputWindow;

        }

    }
}

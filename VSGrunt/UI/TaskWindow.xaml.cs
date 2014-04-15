using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Adage.VSGrunt.UI
{

    public partial class TaskWindow : Window
    {

        public List<string> Lines = new List<string>();

        public TaskWindow()
        {
            OutputTextBox = new TextBox();
            OutputTextBox.Text = " ";
            InitializeComponent();
        }

        public void WriteLine(string message)
        {
            Lines.Add(message);
            OutputTextBox.AppendText("\r\n" + message);
            OutputTextBox.ScrollToEnd();
        }

    }
}

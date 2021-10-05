using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BallanceSkyboxViewer {
    public class DialogUtils {

        public static string OpenFileDialog() {
            Microsoft.Win32.OpenFileDialog op = new Microsoft.Win32.OpenFileDialog();
            op.RestoreDirectory = true;
            op.Multiselect = false;
            op.Filter = "Sky files(*.bmp)|*.bmp|All files(*.*)|*.*";
            if (!(bool)op.ShowDialog()) return "";
            return op.FileName;
        }

    }
}

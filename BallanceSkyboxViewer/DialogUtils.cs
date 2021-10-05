using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BallanceSkyboxViewer {
    public class DialogUtils {

        public static System.Array OpenFileDialog() {
            Microsoft.Win32.OpenFileDialog op = new Microsoft.Win32.OpenFileDialog();
            op.RestoreDirectory = true;
            op.Multiselect = true;
            op.Filter = "Sky files(*.bmp)|*.bmp|All files(*.*)|*.*";
            if (!(bool)op.ShowDialog()) return null;
            return op.FileNames;
        }

    }
}

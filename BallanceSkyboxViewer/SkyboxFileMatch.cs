using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BallanceSkyboxViewer {
    public class SkyboxFileMatch {

        public static bool IsLegal5Files(System.Array arr) {
            bool[] bl = new bool[5];

            for(int i = 0; i < 5; i++) {
                var str = System.IO.Path.GetFileName(arr.GetValue(i).ToString()).ToLower();

                if (str.Contains("back")) {
                    if (bl[0]) return false;
                    else bl[0] = true;
                } else if (str.Contains("down")) {
                    if (bl[1]) return false;
                    else bl[1] = true;
                } else if (str.Contains("front")) {
                    if (bl[2]) return false;
                    else bl[2] = true;
                } else if (str.Contains("left")) {
                    if (bl[3]) return false;
                    else bl[3] = true;
                } else if (str.Contains("right")) {
                    if (bl[4]) return false;
                    else bl[4] = true;
                } else return false; // only 5 item, if not matched, failed immediately
            }

            return true;
        }

        public static string[] Get5Files(System.Array arr) {
            string[] filename = new string[5];

            for (int i = 0; i < 5; i++) {
                var origin = arr.GetValue(i).ToString();
                var str = System.IO.Path.GetFileName(origin).ToLower();

                if (str.Contains("back")) {
                    if (filename[0] == "") throw new InvalidOperationException();
                    else filename[0] = origin;
                } else if (str.Contains("down")) {
                    if (filename[1] == "") throw new InvalidOperationException();
                    else filename[1] = origin;
                } else if (str.Contains("front")) {
                    if (filename[2] == "") throw new InvalidOperationException();
                    else filename[2] = origin;
                } else if (str.Contains("left")) {
                    if (filename[3] == "") throw new InvalidOperationException();
                    else filename[3] = origin;
                } else if (str.Contains("right")) {
                    if (filename[4] == "") throw new InvalidOperationException();
                    else filename[4] = origin;
                } else throw new InvalidOperationException();
            }

            return filename;
        }

    }
}

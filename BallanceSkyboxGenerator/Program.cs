using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BallanceSkyboxGenerator {
    class Program {
        static void Main(string[] args) {

            var folder = Environment.CurrentDirectory;
            if (folder[folder.Length - 1] != '\\') folder += '\\';

            try {
                if (args.Length != 6) throw new Exception("Invalid arguments!");
                Kernel.ConvertKernel(new BitmapDataDeliver() {
                    origin = args[0],
                    back = args[1],
                    front = args[2],
                    left = args[3],
                    right = args[4],
                    down = args[5]
                });
            } catch (Exception e) {
                Console.WriteLine(e.Message);
            }

            Console.WriteLine("Done");
            Console.ReadKey();
        }
    }
}

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
                Kernel.ConvertKernel(folder + "origin.bmp", folder);
            } catch (Exception e) {
                Console.WriteLine(e.Message);
            }

            Console.WriteLine("Done");
            Console.ReadKey();
        }
    }
}

/*  ---------------------------------------------------------------------------------------------------------------------------------------
 *  (C) 2019, Dr Warren Creemers.
 *  This file is subject to the terms and conditions defined in the included file 'LICENSE.txt'
 *  ---------------------------------------------------------------------------------------------------------------------------------------
 */
using System;
using System.IO;
using WD_toolbox;
using System.Linq;

namespace FindUsage
{
    /// <summary>
    /// Used to see where (In my dev folder) I have used a certian import.
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {
            foreach (var dir in Directory.GetDirectories(@"X:\Programming\Projects\WDLib"))
            {
                Console.WriteLine(dir);
            }

            Console.WriteLine();

            var package = "WDMedia"; //just wd for everything
            var where = @"X:\Programming\";
            Console.WriteLine($"Finding usages of {package} in {where}");

            string[] files = Directory.GetFiles(where, "*.cs", SearchOption.AllDirectories);

            int count = 0;
            var lastPath = "";
            foreach (var file in files.Where(F => !F.Contains("WDLib")))
            {
                //Console.WriteLine(file);
                var text = File.ReadAllText(file);
                if (!text.ToLower().Contains($"using {package.ToLower()}"))
                    continue;
                
                var path = Path.GetDirectoryName(file);
                if (path != lastPath)
                {
                    lastPath = path;
                    Console.WriteLine();
                    Console.WriteLine("---------------------------------------------------------------------------------------");
                    Console.WriteLine(path);
                    Console.WriteLine("---------------------------------------------------------------------------------------");
                }

                count++;
                Console.WriteLine(file);
                if (count > 100)
                {
                    //break;
                }
            }

        }
    }
}

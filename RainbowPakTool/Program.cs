using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RainbowPakTool
{
    class Program
    {
        static void Main(string[] args)
        {
            Program Tool = new Program();

            Tool.Title();

            if (args.Length == 0)
            {
                Tool.Usage();
            }
            else
            {
                if (File.Exists(args[0]))
                {
                    if (args[0].Contains(".pak"))
                    {
                        new ReadArchive(args[0]);
                    }
                    else
                    {
                        Console.WriteLine("  [ERROR]: Only .pak files are accepted. Enter to close!");
                    }
                }
                else
                {
                    new WriteArchive(args[0]);
                }
            }

            Console.ReadLine();

        }

        public void Title()
        {
            Console.Title = "Rainbow Studios PAK Tool (1.0)";

            Console.WriteLine();
            Console.WriteLine("  Rainbow Studios PAK Tool - v1.0 by Meth0d");
            Console.WriteLine("  Compatible with Cars (2006) and MX vs ATV Unleashed (2005).");
            Console.WriteLine("  Software provided without any warranty, always backup your files.");
            Console.WriteLine();

        }

        public void Usage()
        {
            Console.WriteLine("  [ USAGE ]");
            Console.WriteLine("  * Unpack: drag and drop a pak file to the executable.");
            Console.WriteLine("  * Repack: drag and drop a folder to the executable.");
            Console.WriteLine();
            Console.WriteLine("    Enter to close...");
        }
    }
}

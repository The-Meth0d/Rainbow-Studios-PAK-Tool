using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RainbowPakTool
{
    public class WriteArchive
    {
        public string[] AllEntries;
        public List<Item> EntriesList = new List<Item>();

        public FileStream fs;
        public BinaryWriter bw;

        public WriteArchive(string dirPath)
        {
            string FolderName = String.Empty;

            Console.WriteLine("  [INFO]: Starting the rebuild.");

            if (Directory.Exists(dirPath))
            {
                FolderName = Path.GetFileName(dirPath);

                AllEntries = Directory.GetFiles(dirPath, "*.*", SearchOption.AllDirectories).OrderBy(f => f).ToArray();

                if (AllEntries.Length == 0)
                {
                    Console.WriteLine("  [ERROR]: Empty folder, enter to close!");
                }
                else
                {
                    string WriteToFile = FolderName + ".pak";

                    if (File.Exists(WriteToFile))
                    {
                        File.Move(WriteToFile, WriteToFile + ".bak");
                    }

                    fs = new FileStream(WriteToFile, FileMode.Create);
                    bw = new BinaryWriter(fs);

                    uint EntryOffset = 0;

                    foreach (var Entry in AllEntries)
                    {
                        FileInfo EntryInfo = new FileInfo(Entry);

                        string EntryPath = Entry.Replace(dirPath+ "\\", "").ToString();
                        uint EntrySize = (uint) EntryInfo.Length;

                        EntriesList.Add(new Item(EntryPath, EntryOffset, EntrySize));

                        EntryOffset += EntrySize;
                    }

                    bw.Write((uint) AllEntries.Length);
                    WriteEntriesInfo();
                    WriteEntriesData();

                    Console.WriteLine();
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("  [COMPLETE]: " + AllEntries.Length + " files were repacked to " + FolderName + ".pak");

                    bw.Close();
                    fs.Close();
                }
            }
            else
            {
                Console.WriteLine("  [ERROR]: Folder not found, enter to close!");
            }
        }

        public void WriteEntriesInfo()
        {
            Console.WriteLine("  [INFO]: Writing game file entries.");
            foreach (var Entry in EntriesList)
            {
                bw.Write(Encoding.UTF8.GetBytes(Entry.PathName.PadRight(100, '\0')));
                bw.Write(Entry.DataOffset);
                bw.Write(Entry.DataSize);
            }
        }

        public void WriteEntriesData()
        {
            Console.WriteLine("  [INFO]: Writing file data to archive, wait, this may take a while...");
            foreach (var Entry in AllEntries)
            {
                bw.Write(File.ReadAllBytes(Entry));
            }
        }
    }
}

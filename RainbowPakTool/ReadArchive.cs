using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RainbowPakTool
{
    public class ReadArchive
    {
        public string Filename;
        public uint TotalEntries;
        public long EntryDataBlockOffset;

        public FileStream fs;
        public BinaryReader br;

        List<Item> EntriesList = new List<Item>();

        public ReadArchive(string filePath)
        {
            Filename = Path.GetFileNameWithoutExtension(filePath);

            fs = new FileStream(filePath, FileMode.Open);
            br = new BinaryReader(fs);

            TotalEntries = br.ReadUInt32();
            Console.WriteLine("  [INFO]: " + TotalEntries + " game files.");

            ReadEntries();
            ExtractEntries();

            br.Close();
            fs.Close();

            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("  [COMPLETE]: " + TotalEntries + " were extracted to folder: " + Filename);
        }

        public void ReadEntries()
        {
            Console.WriteLine("  [INFO]: Reading files information block.");

            for (int i = 0; i < TotalEntries; i++)
            {
                EntriesList.Add(new Item(FixPath(br.ReadBytes(100)), br.ReadUInt32(), br.ReadUInt32()));
            }

            EntryDataBlockOffset = br.BaseStream.Position;
        }

        public void ExtractEntries()
        {
            Console.WriteLine("  [INFO]: Extracting files... Wait, this may take a while!");

            Directory.CreateDirectory(Filename);

            foreach (var Entry in EntriesList)
            {
                br.BaseStream.Position = (EntryDataBlockOffset + Entry.DataOffset);
                
                Directory.CreateDirectory(Filename + "\\" + Path.GetDirectoryName(Entry.PathName));
                
                File.WriteAllBytes(Filename + "\\" + Entry.PathName, br.ReadBytes((int) Entry.DataSize));
            }
        }

        public string FixPath(byte[] path)
        {
            string Path = Encoding.UTF8.GetString(path);
            return Path.Substring(0, Path.IndexOf('\0'));
        }
    }
}

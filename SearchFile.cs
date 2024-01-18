using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Search_and_Zip
{
    internal class SearchFile
    {
        public static List<string> R;
        public SearchFile(string s)
        {
            string FileName = s;

            List<string> Result = new List<string>();
            DriveInfo[] Disks = DriveInfo.GetDrives();
            for (int i = 0; i < Disks.Length; i++)
            {
                Search(Result, Convert.ToString(Disks[i])!, FileName);
            }
            R = Result;
            Console.WriteLine("Resoult: ");
            foreach (string f in Result)
                Console.WriteLine(f);
        }
        public void Search(List<string> notFound, string Path, string FilesName)
        {
            string[] Files = Directory.GetFiles(Path);
            foreach (string f in Files)
            {
                string[] filenomi = f.Split('\\');
                if (filenomi[filenomi.Length - 1] == FilesName)
                    notFound.Add(f);
            }


            string[] folders = Directory.GetDirectories(Path);
            foreach (string f in folders)
            {

                try
                {
                    Search(notFound, f, FilesName);
                }
                catch { }
            }

        }
        public List<string> ResultReturn()
        {
            return R;
        }
    }
}

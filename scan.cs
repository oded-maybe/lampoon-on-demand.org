using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace diipnet_business_logic
{
    public static class scan
    {
        private static folder __diipFolder;
        private static bool __initializedDiipScan = false;
        private static List<flatFolder> __flatFolders;

        public static void initDiipScan(string root)
        {
            if (__initializedDiipScan)
                return;

            __initializedDiipScan = true;

            if (System.IO.Directory.Exists(root))
            {
                __flatFolders = new List<flatFolder>();
                __diipFolder = diipScan(root);
            }
            else
            {
                __diipFolder = new folder(root + " was not found!");
            }

            __initializedDiipScan = false;
        }

        private static folder diipScan(string root) 
        {
            folder diipFolder = new folder(root.Split("\\".ToCharArray()).Last());

            string[] subfolders = System.IO.Directory.GetDirectories(root);
            string[] files = System.IO.Directory.GetFiles(root);

            foreach (string file in files) 
            {
                string fileName = file.Split("\\".ToCharArray()).Last();

                if (fileName.ToLower().Equals("description.txt"))
                    diipFolder.folderInfo.description = System.IO.File.ReadAllText(file);
                else if (fileName.ToLower().Equals("header.txt"))
                    diipFolder.folderInfo.header = System.IO.File.ReadAllText(file);
                else if (fileName.ToLower().Equals("hyperlinks.txt"))
                    diipFolder.folderInfo.InitHyperlinks(System.IO.File.ReadAllText(file));
                else if (fileName.ToLower().Equals("meta.txt"))
                    diipFolder.folderInfo.InitMetadata(System.IO.File.ReadAllText(file));

                diipFolder.files.Add(fileName);
            }

            foreach (string subfolder in subfolders) { diipFolder.subfolders.Add(diipScan(subfolder)); }

            __flatFolders.Add(new flatFolder(diipFolder));

            return diipFolder;
        }

        public static Dictionary<folder, int> diipSearch(Dictionary<string, string> query) 
        {
            Dictionary<folder, int> results = new Dictionary<folder, int>();
            Dictionary<string, string> lowerQuery = new Dictionary<string, string>();

            foreach (string key in query.Keys) 
            {
                lowerQuery.Add(key.ToLower(), query[key].ToLower());
            }

            foreach (flatFolder flatFolder in __flatFolders)
            {
                int matches = flatFolder.match(lowerQuery);

                if (matches > 0)
                {
                    results.Add(flatFolder.folder, matches);
                }
            }

            return (from r in results
                    orderby r.Value descending
                    select r).ToDictionary(t => t.Key, t => t.Value);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace diipnet_business_logic
{
    public class folder
    {
        public string folderName;
        public List<folder> subfolders;
        public List<string> files;
        public folderInfo folderInfo;

        public folder(string folderName)
        {
            this.folderName = folderName;
            subfolders = new List<folder>();
            files = new List<string>();
            folderInfo = new folderInfo();
        }

        public Dictionary<string, string> getFlatMetadata()
        {
            return this.folderInfo.getFlatMetadata();
        }

        public int match(Dictionary<string, string> query)
        {
            return this.folderInfo.match(query);
        }
    }
}

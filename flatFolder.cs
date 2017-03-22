using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace diipnet_business_logic
{
    public class flatFolder
    {
        public folder folder;
        public Dictionary<string, string> flatMetadata;
        public flatFolder(folder folder)
        {
            this.folder = folder;
            this.flatMetadata = folder.getFlatMetadata();
        }

        public int match(Dictionary<string, string> query)
        {
            return this.folder.match(query);
        }
    }
}

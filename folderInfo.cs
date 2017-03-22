using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace diipnet_business_logic
{
    public class folderInfo
    {
        public string fullName;
        public string header;
        public string description;
        public Dictionary<string, string> hyperlinks;
        public Dictionary<string, List<string>> metadata;

        public folderInfo()
        {
            hyperlinks = new Dictionary<string, string>();
            metadata = new Dictionary<string, List<string>>();
        }

        public void InitHyperlinks(string fileContents)
        {
            foreach (string line in fileContents.Split("\n".ToCharArray()))
            {
                string[] href = line.Split(":".ToCharArray());
                if (href.Length > 1)
                    hyperlinks.Add(href[0], href[1]);
                else
                    hyperlinks.Add(href[0], href[0]);
            }
        }

        public void InitMetadata(string fileContents)
        {
            foreach (string line in fileContents.Split("\n".ToCharArray()))
            {
                string[] metadata = line.Split(":".ToCharArray());
                if (metadata.Length > 0)
                {
                    this.metadata.Add(metadata[0], metadata[1].Replace("[", "").Replace("]", "").Replace("\r", "").Split(",".ToCharArray()).ToList());
                }
            }
        }

        public Dictionary<string, string> getFlatMetadata()
        {
            Dictionary<string, string> flatMetadata = new Dictionary<string, string>();

            foreach (var key in this.metadata)
            {
                string valuesList = "";
                foreach (string value in key.Value)
                {
                    if (valuesList != "")
                        valuesList += ",";

                    valuesList = value.ToLower().Trim();
                    //flatMetadata.Add(key.Key.ToLower(), value.ToLower());
                }

                flatMetadata.Add(key.Key.ToLower(), valuesList);
            }

            return flatMetadata;
        }

        public int match(Dictionary<string, string> query)
        {
            int matchesFound = 0;

            foreach (var key in this.metadata)
            {
                if (query.Keys.Contains(key.Key))
                {
                    foreach (string value in key.Value)
                    {
                        string[] options = query[key.Key].Split(",".ToCharArray());

                        foreach (string option in options)
                        {
                            if (value.ToLower().Contains(option))
                            {
                                matchesFound++;
                            }
                        }
                    }
                }
            }

            return matchesFound;
        }
    }
}

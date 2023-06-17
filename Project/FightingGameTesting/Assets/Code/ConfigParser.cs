using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using System.Linq;

public static class ConfigParser
{

    public static HashSet<KeyValuePair<string, string>> GetSectionData(string path, string header)
    {

        FileStream stream = new FileStream(path, FileMode.Open, FileAccess.Read);
        StreamReader reader = new StreamReader(stream);

        HashSet<KeyValuePair<string, string>> valuePairs = new HashSet<KeyValuePair<string, string>>();
        
        string line;

        while ((line = reader.ReadLine()) != null)
        {
            if (line.Equals('[' + header + ']'))
            {
                while ((line = reader.ReadLine()) != null)
                {
                    if (line.Contains("="))
                    {
                        string keyWithWhiteSpace = line.Substring(0, line.IndexOf("="));
                        string key = string.Concat(keyWithWhiteSpace.Where(c => !char.IsWhiteSpace(c)));

                        string valueWithWhiteSpace = line.Substring(line.IndexOf("=") + 1);
                        string value = string.Concat(valueWithWhiteSpace.Where(c => !char.IsWhiteSpace(c)));

                        valuePairs.Add(new KeyValuePair<string, string>(key, value));
                    }
                    else if (line.ToCharArray().Length > 0 && line.ToCharArray()[0] == '[')
                        break;
                }
            }

        }

        reader.Close();
        stream.Close();

        var count = string.Join("    ", valuePairs);
        Debug.Log("Section: [" + header + "] --> " + count);

        return valuePairs;
    }

}
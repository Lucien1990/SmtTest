// See https://aka.ms/new-console-template for more information
using Newtonsoft.Json.Linq;
using SmtTest;
using System.Collections;
using System.Data;
using System.Text.RegularExpressions;

Console.WriteLine("开始采集...");


//args 入参
//var perfix = args[0];
//var fileName = args[1];
Regex regex = new Regex("\\[{1}(.+)]");
//var file = new FileStream(@"D:\\smt\20221020022344260-01-1-2-4-12862165+12862219-00-TOP-V3_0-1031.u03", FileMode.Open, FileAccess.Read);
string myStr = string.Empty;
string path = Directory.GetCurrentDirectory() + "\\files";
var allPath = Directory.GetFiles(path, "*.*", SearchOption.TopDirectoryOnly).Where(f => f.EndsWith(".u03"));
if (allPath != null && allPath.Count() > 0)
{
    foreach (var filePath in allPath)
    {
        //using (FileStream fsRead = file.Open(FileMode.Open))
        using (FileStream fsRead = new(filePath, FileMode.Open))
        {
            int fsLen = (int)fsRead.Length;
            byte[] heByte = new byte[fsLen];
            int r = fsRead.Read(heByte, 0, heByte.Length);
            myStr = System.Text.Encoding.UTF8.GetString(heByte);
           // Console.WriteLine(myStr);
            var result = regex.Split(myStr);
            //Console.WriteLine(JObject.FromObject(result));
            if (result.Length > 0)
            {
                for (int i = 1; i < result.Length; i += 2)
                {
                    string? item = result[i];
                    if (string.IsNullOrEmpty(item))
                        continue;
                    if (item == "Index")
                    {
                        var sp = result[i + 1].Split("\r\n");
                        if (sp.Length > 0)
                        {
                            var ht = FormatTag(sp);
                            var ret = new
                            {
                                Index = ht
                            };
                            Console.WriteLine("Tag of [Index]");
                            Console.WriteLine(JObject.FromObject(ret));
                        }
                    }
                    if (item == "Information")
                    {
                        var sp = result[i + 1].Split("\r\n");
                        if (sp.Length > 0)
                        {
                            var ht = FormatTag(sp);
                            var ret = new
                            {
                                Information = ht
                            };
                            Console.WriteLine("Tag of [Information]");
                            Console.WriteLine(JObject.FromObject(ret));
                        }
                    }
                    if (item == "MountLatestReel")
                    {
                        var tags = result[i + 1].Split("\r\n");
                        var dt = FormatTableTag(tags);
                        var ret = new
                        {
                            MountLatestReel = dt
                        };
                        Console.WriteLine("Tag of [MountLatestReel]");
                        Console.WriteLine(JObject.FromObject(ret));
                    }
                }                 
                //TODO:to db or do something  
            }
        }
    }
}

static Hashtable FormatTag(string[] tags)
{
    Hashtable hs = new Hashtable();
    for (int j = 0; j < tags.Length; j++)
    {
        if (!string.IsNullOrEmpty(tags[j]))
        {
            var temp = tags[j].Split('=');
            if (temp.Length > 0)
                hs.Add(temp[0], temp[1]);
        }
    }

    return hs;
}

dynamic FormatTableTag(string[] tags)
{
    DataTable dt = new();
    if (tags != null && tags.Length >= 0)
    {
        bool IsFirst = true;
        int columnCount = 0;
        for (int i = 0; i < tags.Length; i++)
        {
            if (!string.IsNullOrEmpty(tags[i]))
            {
                //Console.WriteLine(tags[i]);
                string[] splits = tags[i].Split(' ');
                if (splits.Length > 0)
                {
                    if (IsFirst)
                    {
                        IsFirst = false;
                        columnCount = splits.Length;
                        foreach (string column in splits)
                        {
                            DataColumn dc = new DataColumn(column);
                            dt.Columns.Add(dc);
                        }

                    }
                    else
                    {
                        DataRow dr = dt.NewRow();
                        for (int j = 0; j < columnCount; j++)
                        {
                            dr[j] = splits[j];
                        }
                        dt.Rows.Add(dr);
                    }
                }

            }
        }
    }
    return dt;
}






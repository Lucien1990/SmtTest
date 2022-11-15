// See https://aka.ms/new-console-template for more information
using Newtonsoft.Json.Linq;
using SmtTest;
using System.Collections;
using System.Text.RegularExpressions;

Console.WriteLine("开始采集...");


//args 入参
//var perfix = args[0];
//var fileName = args[1];
Regex regex = new Regex("\\[{1}(.+)]");
//var file = new FileStream(@"D:\\smt\20221020022344260-01-1-2-4-12862165+12862219-00-TOP-V3_0-1031.u03", FileMode.Open, FileAccess.Read);
string myStr = string.Empty;
string path = @"X:\XXX\XX";
DirectoryInfo root = new DirectoryInfo(path);
FileInfo[] files = root.GetFiles();
if (files.Length > 0)
{
    foreach (FileInfo file in files)
    {
        // using (FileStream fsRead = new FileStream(@"D:\\smt\20221020022344260-01-1-2-4-12862165+12862219-00-TOP-V3_0-1031.u03", FileMode.Open))
        using (FileStream fsRead = file.Open(FileMode.Open))
        {
            int fsLen = (int)fsRead.Length;
            byte[] heByte = new byte[fsLen];
            int r = fsRead.Read(heByte, 0, heByte.Length);
            myStr = System.Text.Encoding.UTF8.GetString(heByte);
            Console.WriteLine(myStr);
            // Console.ReadKey();


            //var str = file.ReadAsync(file.Length)
            Console.WriteLine(myStr);
            var result = regex.Split(myStr);
            //Console.WriteLine(JObject.FromObject(result));
            if (result.Length > 0)
            {
                Hashtable hs = new Hashtable();
                for (int i = 2; i < 5; i += 2)
                {
                    string? item = result[i];
                    if (string.IsNullOrEmpty(item))
                        continue;
                    var sp = item.Split('\n');
                    if (sp.Length > 0)
                    {
                        for (int j = 0; j < sp.Length; j++)
                        {
                            if (!string.IsNullOrEmpty(sp[j]))
                            {
                                var temp = sp[j].Split('=');
                                if (temp.Length > 0)
                                    hs.Add(temp[0], temp[1]);
                            }
                        }
                    }
                }
                Console.WriteLine(JObject.FromObject(hs));


                //TODO:写db
                //  return hs;
            }
        }
    }
}





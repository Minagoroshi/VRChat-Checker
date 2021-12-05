using System;
using System.IO;
using System.Threading;

namespace TopLib
{
  public class data
  {
    public static readonly string day = DateTime.Now.ToString("hh.mm.ss - MMM dd, yyyy");
    public static string fileResult = "./Results/" + data.day + "/";

    public void Results(string text, string file)
    {
      while (true)
      {
        try
        {
          Directory.CreateDirectory(data.fileResult);
          using (StreamWriter streamWriter = new StreamWriter(data.fileResult + "/" + file + ".txt", true))
          {
            streamWriter.WriteLine(text);
            break;
          }
        }
        catch
        {
          Thread.Sleep(100);
        }
      }
    }
  }
}

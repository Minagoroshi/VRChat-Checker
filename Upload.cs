using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace TopLib
{
  public class Upload
  {
    public static List<string> combolist;

    public List<string> Combo()
    {
      while (true)
      {
        try
        {
          OpenFileDialog openFileDialog1 = new OpenFileDialog();
          openFileDialog1.Title = "Choose your comboList";
          openFileDialog1.Filter = "Text File | *.txt";
          OpenFileDialog openFileDialog2 = openFileDialog1;
          if (openFileDialog2.ShowDialog() == DialogResult.OK)
          {
            Upload.combolist = ((IEnumerable<string>) File.ReadAllLines(openFileDialog2.FileName)).ToList<string>();
            return Upload.combolist;
          }
        }
        catch (Exception ex)
        {
          Console.WriteLine(ex.Message);
        }
      }
    }

    public List<string> Proxys()
    {
      while (true)
      {
        try
        {
          OpenFileDialog openFileDialog1 = new OpenFileDialog();
          openFileDialog1.Title = "Choose your proxyList";
          openFileDialog1.Filter = "Text File | *.txt";
          OpenFileDialog openFileDialog2 = openFileDialog1;
          if (openFileDialog2.ShowDialog() == DialogResult.OK)
            return ((IEnumerable<string>) File.ReadAllLines(openFileDialog2.FileName)).ToList<string>();
        }
        catch (Exception ex)
        {
          Console.WriteLine(ex.Message);
        }
      }
    }

    public int Threads()
    {
      Console.ForegroundColor = ConsoleColor.Cyan;
      Console.Write(" Threads?: ");
      Console.ForegroundColor = ConsoleColor.White;
      try
      {
        Variables.Threads = int.Parse(Console.ReadLine());
      }
      catch
      {
        Variables.Threads = 50;
      }
      return Variables.Threads;
    }

    public string proxyType()
    {
      Console.ForegroundColor = ConsoleColor.Cyan;
      Console.Write(" Proxy Type?: ");
      Console.ForegroundColor = ConsoleColor.White;
      return Console.ReadLine();
    }
  }
}

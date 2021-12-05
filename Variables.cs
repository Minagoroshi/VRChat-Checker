using System.Collections.Generic;
using xNet;

namespace TopLib
{
  public class Variables
  {
    public static List<string> ComboList;

    public static int Progress { get; set; }

    public List<string> ProxyList { get; set; }
    
    public ProxyType proxyType { get; set; }

    public static int Threads { get; set; }

    public static int ComboLenght { get; set; }

    public static int Hits { get; set; }

    public static int Fails { get; set; }

    public static int Fac2 { get; set; }

    public static int Retries { get; set; }

    public static int Index { get; set; }
  }
}

using System;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using xNet;

namespace TopLib
{
  public class Checker
  {
    public static Stopwatch st = new Stopwatch();
    public static Upload upload = new Upload();
    public static Variables var = new Variables();
    public static int CPM;
    public static int Increment_CPM;

    public static void Init()
    {
      Console.Title = configuration.nameChecker + " " + configuration.versionChecker + " | Eternity Dev Team";
      Variables.ComboList = Checker.upload.Combo();
      Variables.ComboLenght = Variables.ComboList.Count<string>();
      if (configuration.Useproxy)
      {
        Checker.var.ProxyList = Checker.upload.Proxys();
        switch (Checker.upload.proxyType().ToLower())
        {
          case "http":
            Checker.var.proxyType = ProxyType.Http;
            break;
          case "socks4":
            Checker.var.proxyType = ProxyType.Socks4;
            break;
          case "socks4a":
            Checker.var.proxyType = ProxyType.Socks4a;
            break;
          case "socks5":
            Checker.var.proxyType = ProxyType.Socks5;
            break;
          case null:
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(" You entered the wrong type of proxy!");
            Console.ReadLine();
            Environment.Exit(0);
            break;
        }
      }
      Variables.Threads = Checker.upload.Threads();
      Console.ForegroundColor = ConsoleColor.White;
      Console.WriteLine("\n Running (" + Variables.Threads.ToString() + "/" + Variables.Threads.ToString() + ") Threads.\n");
      Task.Factory.StartNew((Action) (() =>
      {
        while (true)
          Checker.goTitle();
      }));
    }

    public static void goTitle()
    {
      Checker.CPM = Checker.Increment_CPM;
      Checker.Increment_CPM = 0;
      Console.Title = configuration.nameChecker + " " + configuration.versionChecker + " by " + configuration.Coder + " | (" + Variables.Progress.ToString() + "/" + Variables.ComboLenght.ToString() + ") | Hits: " + Variables.Hits.ToString() + " - Fails: " + Variables.Fails.ToString() + " - 2FA: " + Variables.Fac2.ToString() + " - Retries: " + Variables.Retries.ToString() + " - CPM: " + (Checker.CPM * 60).ToString() + " - Elapsed: " + Checker.GetElapsed();
      Thread.Sleep(1000);
    }

    public static string post(
      string url,
      ProxyType proxyType,
      bool Post = false,
      string contentType = null,
      string dataPost = null,
      string userAgent = null)
    {
      while (true)
      {
        try
        {
          using (HttpRequest httpRequest = new HttpRequest())
          {
            if (configuration.Useproxy)
            {
              string[] strArray = Checker.var.ProxyList[new Random().Next(Checker.var.ProxyList.Count)].Split(':');
              httpRequest.Proxy = ProxyClient.Parse(proxyType, strArray[0] + ":" + strArray[1]);
              if (strArray.Length == 4)
              {
                httpRequest.Proxy.Username = strArray[2];
                httpRequest.Proxy.Password = strArray[3];
              }
              httpRequest.ConnectTimeout = 3000;
            }
            httpRequest.IgnoreProtocolErrors = true;
            httpRequest.AllowAutoRedirect = true;
            if (userAgent != null)
              httpRequest.UserAgent = userAgent;
            return Post ? httpRequest.Post(new Uri(url, UriKind.Absolute), dataPost == null ? new byte[0] : Encoding.UTF8.GetBytes(dataPost), contentType).ToString() : httpRequest.Get(url).ToString();
          }
        }
        catch
        {
        }
      }
    }

    public static string Get(
      string url,
      ProxyType proxyType,
      bool? Headers = null,
      string header = null,
      string value = null,
      string userAgent = null)
    {
      while (true)
      {
        try
        {
          using (HttpRequest httpRequest = new HttpRequest())
          {
            if (configuration.Useproxy)
            {
              string[] strArray = Checker.var.ProxyList[new Random().Next(Checker.var.ProxyList.Count)].Split(':');
              httpRequest.Proxy = ProxyClient.Parse(proxyType, strArray[0] + ":" + strArray[1]);
              if (strArray.Length == 4)
              {
                httpRequest.Proxy.Username = strArray[2];
                httpRequest.Proxy.Password = strArray[3];
              }
              httpRequest.ConnectTimeout = 3000;
            }
            bool? nullable = Headers;
            bool flag = true;
            if (nullable.GetValueOrDefault() == flag & nullable.HasValue)
              httpRequest.AddHeader(header, value);
            httpRequest.IgnoreProtocolErrors = true;
            httpRequest.AllowAutoRedirect = true;
            if (userAgent != null)
              httpRequest.UserAgent = userAgent;
            return httpRequest.Get(url).ToString();
          }
        }
        catch
        {
        }
      }
    }

    public static void Start(MethodInvoker method)
    {
      Checker.st.Start();
      for (int index = 1; index <= Variables.Threads; ++index)
        new Thread(new ThreadStart(method.Invoke)).Start();
    }

    public static string GetElapsed() => Checker.st.Elapsed.ToString("dd\\:hh\\:mm\\:ss");
  }
}

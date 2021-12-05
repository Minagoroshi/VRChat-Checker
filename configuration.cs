namespace TopLib
{
  public class configuration
  {
    public static bool Useproxy { get; set; }

    public configuration(bool useProxy, string namechecker, string coder, string versionchecker = null)
    {
      configuration.nameChecker = namechecker;
      configuration.versionChecker = versionchecker;
      configuration.Coder = coder;
      configuration.Useproxy = useProxy;
    }

    public static string nameChecker { get; set; }

    public static string Coder { get; set; }

    public static string versionChecker { get; set; }
  }
}

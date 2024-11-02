using System.Runtime.InteropServices;

namespace CorpusExplorer.v3.Sdk.Helper
{
  public static class OSPlatformHelper
  {
    public static string GetOSPlatform()
    {
      if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        return "win";
      if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
        return "linux";
      if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
        return "osx";
      throw new PlatformNotSupportedException();
    }

    public static string GetArchitecture()
    {
      return Environment.Is64BitProcess ? "x64" : "x86";
    }

    public static string GetOSPlatformArchitecture()
    {
      return $"{GetOSPlatform()}_{GetArchitecture()}";
    }
  }
}

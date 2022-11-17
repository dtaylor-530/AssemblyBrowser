using AssemblyBrowserLib;
using System.Reflection;

namespace AssemblyBrowserGUI.Model
{
   public class Model {
      public static AssemblyDO LoadAssembly(string path) {
         AssemblyBrowserLib.AssemblyBrowser browser = new AssemblyBrowserLib.AssemblyBrowser();
         Assembly asm = Assembly.LoadFrom(path);
         return browser.BrowseAssembly(asm);
      }
   }
}

﻿using AssemblyBrowserLib;
using System.Reflection;

namespace AssemblyBrowserGUI.Model
{
   public class Model {
      public static AssemblyDO LoadAssembly(string path) {
         AssemblyBrowser browser = new AssemblyBrowser();
         Assembly asm = Assembly.LoadFrom(path);
         return browser.BrowseAssembly(asm);
      }
   }
}

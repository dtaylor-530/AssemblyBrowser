using System.Reflection;
using System.Linq;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace AssemblyBrowserLib
{
    public class AssemblyBrowser
    {
        //public AssemblyDO Assembly { get; private set; }

        public AssemblyDO BrowseAssembly(Assembly assembly)
        {
            AssemblyDO res = new AssemblyDO(assembly.GetName().Name);
            var extensionMethods = SelectExtensionMethods(assembly.GetTypes(), res);
            Add(extensionMethods, res);

            return res;
        }
        
        private static IEnumerable<MethodInfo> SelectExtensionMethods(IEnumerable<Type> definedTypes, AssemblyDO res)
        {
           foreach (Type t in definedTypes)
           {
              if (!IsCompilerGenerated(t))
              {
                 if (res.Namespaces.All(ns => ns.Name != t.Namespace))
                    res.Namespaces.Add(new NamespaceDO(t.Namespace));

                 const BindingFlags flags = BindingFlags.Static | BindingFlags.Instance | BindingFlags.Public |
                                            BindingFlags.NonPublic;
                 TypeDO typeDO = new TypeDO(t);
                 typeDO.Properties.AddRange(t.GetProperties(flags).Where(pi => !IsCompilerGenerated(pi)));
                 typeDO.Fields.AddRange(t.GetFields(flags).Where(fi => !IsCompilerGenerated(fi)));

                 if (t.IsAbstract && t.IsSealed) //Check for extension methods
                    foreach (MethodInfo mi in t.GetMethods(flags).Where(mi => !IsCompilerGenerated(mi)))
                       if (mi.IsDefined(typeof(ExtensionAttribute)))
                          yield return mi;
                       else typeDO.Methods.Add(mi);
                 else typeDO.Methods.AddRange(t.GetMethods(flags).Where(mi => !IsCompilerGenerated(mi)));

                 res.Namespaces.Find(ns => ns.Name == t.Namespace).Types.Add(typeDO);
              }
           }
        }

        private static void Add(IEnumerable<MethodInfo> extensionMethods, AssemblyDO res)
        {
           foreach(var memberInfo in extensionMethods)
           {
              Type extendedType = memberInfo.GetParameters()[0].ParameterType;
              TypeDO typeDO = FindTypeDO(res, extendedType);
              if (typeDO != null)
                 typeDO.Methods.Add(memberInfo);
              else
              {
                 TypeDO extendedTypeDO = new TypeDO(extendedType);
                 extendedTypeDO.Methods.Add(memberInfo);
                 NamespaceDO nsDO = res.Namespaces.Find(ns => ns.Name == extendedType.Namespace);
                 if (nsDO != null)
                    nsDO.Types.Add(extendedTypeDO);
                 else
                 {
                    NamespaceDO extendedNSDO = new NamespaceDO(extendedType.Namespace);
                    extendedNSDO.Types.Add(extendedTypeDO);
                    res.Namespaces.Add(extendedNSDO);
                 }
              }
           }
        }

        private static bool IsCompilerGenerated(MemberInfo type)
        {
            return type.GetCustomAttribute<System.Runtime.CompilerServices.CompilerGeneratedAttribute>() != null;
        }

        private static TypeDO FindTypeDO(AssemblyDO asm, Type type)
        {
            NamespaceDO ns = asm.Namespaces.Find(nDO => nDO.Name == type.Namespace);
            TypeDO typeDO = ns?.Types.Find(t => t.Type == type);
            return typeDO;
        }
    }
}

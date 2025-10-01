namespace RunDog.Resources
{
    using System;
    using System.Reflection;

    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "17.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class Resources
    {

        private static global::System.Resources.ResourceManager resourceMan;

        private static global::System.Globalization.CultureInfo resourceCulture;

        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Resources()
        {
        }

        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager
        {
            get
            {
                if (object.ReferenceEquals(resourceMan, null))
                {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("RunDog.Resources.Resources", typeof(Resources).GetTypeInfo().Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }

        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Globalization.CultureInfo Culture
        {
            get
            {
                return resourceCulture;
            }
            set
            {
                resourceCulture = value;
            }
        }

        internal static string CpuTitle
        {
            get
            {
                return ResourceManager.GetString("CpuTitle", resourceCulture);
            }
        }

        internal static string RamTitle
        {
            get
            {
                return ResourceManager.GetString("RamTitle", resourceCulture);
            }
        }

        internal static string StorageTitle
        {
            get
            {
                return ResourceManager.GetString("StorageTitle", resourceCulture);
            }
        }

        internal static string NetworkTitle
        {
            get
            {
                return ResourceManager.GetString("NetworkTitle", resourceCulture);
            }
        }

        internal static string Used
        {
            get
            {
                return ResourceManager.GetString("Used", resourceCulture);
            }
        }

        internal static string LocalIp
        {
            get
            {
                return ResourceManager.GetString("LocalIp", resourceCulture);
            }
        }

        internal static string Copied
        {
            get
            {
                return ResourceManager.GetString("Copied", resourceCulture);
            }
        }

        internal static string ErrorPerfCounters
        {
            get
            {
                return ResourceManager.GetString("ErrorPerfCounters", resourceCulture);
            }
        }

        internal static string ErrorOpeningExplorer
        {
            get
            {
                return ResourceManager.GetString("ErrorOpeningExplorer", resourceCulture);
            }
        }

        internal static string PauseMenuItem
        {
            get
            {
                return ResourceManager.GetString("PauseMenuItem", resourceCulture);
            }
        }

        internal static string ResumeMenuItem
        {
            get
            {
                return ResourceManager.GetString("ResumeMenuItem", resourceCulture);
            }
        }

        internal static string ChangeAnimalMenuItem
        {
            get
            {
                return ResourceManager.GetString("ChangeAnimalMenuItem", resourceCulture);
            }
        }

        internal static string ExitMenuItem
        {
            get
            {
                return ResourceManager.GetString("ExitMenuItem", resourceCulture);
            }
        }

        internal static string DogAnimalName
        {
            get
            {
                return ResourceManager.GetString("DogAnimalName", resourceCulture);
            }
        }

        internal static string CatAnimalName
        {
            get
            {
                return ResourceManager.GetString("CatAnimalName", resourceCulture);
            }
        }
    }
}
﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace AHashPassGen.Properties {
    using System;
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
    [System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    public class I18n {
        
        private static System.Resources.ResourceManager resourceMan;
        
        private static System.Globalization.CultureInfo resourceCulture;
        
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal I18n() {
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        public static System.Resources.ResourceManager ResourceManager {
            get {
                if (object.Equals(null, resourceMan)) {
                    System.Resources.ResourceManager temp = new System.Resources.ResourceManager("AHashPassGen.Properties.I18n", typeof(I18n).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        public static System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        public static string Version {
            get {
                return ResourceManager.GetString("Version", resourceCulture);
            }
        }
        
        public static string Closing {
            get {
                return ResourceManager.GetString("Closing", resourceCulture);
            }
        }
        
        public static string CloseApplication {
            get {
                return ResourceManager.GetString("CloseApplication", resourceCulture);
            }
        }
        
        public static string About {
            get {
                return ResourceManager.GetString("About", resourceCulture);
            }
        }
        
        public static string AHashPassGen {
            get {
                return ResourceManager.GetString("AHashPassGen", resourceCulture);
            }
        }
        
        public static string OK {
            get {
                return ResourceManager.GetString("OK", resourceCulture);
            }
        }
    }
}

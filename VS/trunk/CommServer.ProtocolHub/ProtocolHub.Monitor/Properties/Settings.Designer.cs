﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace CAS.CommServerConsole.Properties {
    
    
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator", "14.0.0.0")]
    internal sealed partial class Settings : global::System.Configuration.ApplicationSettingsBase {
        
        private static Settings defaultInstance = ((Settings)(global::System.Configuration.ApplicationSettingsBase.Synchronized(new Settings())));
        
        public static Settings Default {
            get {
                return defaultInstance;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("localhost")]
        public string CommServer_Host_Primary {
            get {
                return ((string)(this["CommServer_Host_Primary"]));
            }
            set {
                this["CommServer_Host_Primary"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("5757")]
        public int CommServer_ListenPort_Primary {
            get {
                return ((int)(this["CommServer_ListenPort_Primary"]));
            }
            set {
                this["CommServer_ListenPort_Primary"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("localhost")]
        public string CommServer_Host_AlternativeConfiguration {
            get {
                return ((string)(this["CommServer_Host_AlternativeConfiguration"]));
            }
            set {
                this["CommServer_Host_AlternativeConfiguration"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("5758")]
        public int CommServer_ListenPort_AlternativeConfiguration {
            get {
                return ((int)(this["CommServer_ListenPort_AlternativeConfiguration"]));
            }
            set {
                this["CommServer_ListenPort_AlternativeConfiguration"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("False")]
        public bool UseAlternativeConfiguration {
            get {
                return ((bool)(this["UseAlternativeConfiguration"]));
            }
            set {
                this["UseAlternativeConfiguration"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("True")]
        public bool DisplayConfigurationQuestionAtStartup {
            get {
                return ((bool)(this["DisplayConfigurationQuestionAtStartup"]));
            }
            set {
                this["DisplayConfigurationQuestionAtStartup"] = value;
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("tcp://{0}:{1}/CommServerConsole")]
        public string CommServer_Connection_Template {
            get {
                return ((string)(this["CommServer_Connection_Template"]));
            }
        }
    }
}
﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace VolumeControl.Audio {
    
    
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator", "17.2.0.0")]
    internal sealed partial class AudioAPISettings : global::System.Configuration.ApplicationSettingsBase {
        
        private static AudioAPISettings defaultInstance = ((AudioAPISettings)(global::System.Configuration.ApplicationSettingsBase.Synchronized(new AudioAPISettings())));
        
        public static AudioAPISettings Default {
            get {
                return defaultInstance;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("False")]
        public bool LockTargetSession {
            get {
                return ((bool)(this["LockTargetSession"]));
            }
            set {
                this["LockTargetSession"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("")]
        public string TargetSession {
            get {
                return ((string)(this["TargetSession"]));
            }
            set {
                this["TargetSession"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("2")]
        public int VolumeStepSize {
            get {
                return ((int)(this["VolumeStepSize"]));
            }
            set {
                this["VolumeStepSize"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("<?xml version=\"1.0\" encoding=\"utf-16\"?>\r\n<ArrayOfString xmlns:xsd=\"http://www.w3." +
            "org/2001/XMLSchema\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\">\r\n  <s" +
            "tring />\r\n</ArrayOfString>")]
        public global::System.Collections.Specialized.StringCollection EnabledDevices {
            get {
                return ((global::System.Collections.Specialized.StringCollection)(this["EnabledDevices"]));
            }
            set {
                this["EnabledDevices"] = value;
            }
        }
    }
}

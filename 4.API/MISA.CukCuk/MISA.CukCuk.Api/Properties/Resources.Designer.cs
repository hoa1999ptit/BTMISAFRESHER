//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace MISA.CukCuk.Api.Properties {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "16.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class Resources {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Resources() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("MISA.CukCuk.Api.Properties.Resources", typeof(Resources).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Overrides the current thread's CurrentUICulture property for all
        ///   resource lookups using this strongly typed resource class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Mã không hợp lệ..
        /// </summary>
        internal static string Exception_CodeInvalid {
            get {
                return ResourceManager.GetString("Exception_CodeInvalid", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Email không hợp lệ..
        /// </summary>
        internal static string Exception_EmailInvalid {
            get {
                return ResourceManager.GetString("Exception_EmailInvalid", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Id không chính xác!.
        /// </summary>
        internal static string Exception_ErrId {
            get {
                return ResourceManager.GetString("Exception_ErrId", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Có lỗi xảy ra! vui lòng liên hệ với MISA..
        /// </summary>
        internal static string Exception_ErrMsg {
            get {
                return ResourceManager.GetString("Exception_ErrMsg", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to https://openapi.misa.com.vn/errorcode/misa-001.
        /// </summary>
        internal static string Exception_MoreInfo {
            get {
                return ResourceManager.GetString("Exception_MoreInfo", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Không có dữ liệu..
        /// </summary>
        internal static string Exception_NoData {
            get {
                return ResourceManager.GetString("Exception_NoData", resourceCulture);
            }
        }
    }
}

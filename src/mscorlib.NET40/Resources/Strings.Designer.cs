﻿//------------------------------------------------------------------------------
// <auto-generated>
//     此代码由工具生成。
//     运行时版本:4.0.30319.42000
//
//     对此文件的更改可能会导致不正确的行为，并且如果
//     重新生成代码，这些更改将会丢失。
// </auto-generated>
//------------------------------------------------------------------------------

namespace System.Resources {
    using System;
    
    
    /// <summary>
    ///   一个强类型的资源类，用于查找本地化的字符串等。
    /// </summary>
    // 此类是由 StronglyTypedResourceBuilder
    // 类通过类似于 ResGen 或 Visual Studio 的工具自动生成的。
    // 若要添加或移除成员，请编辑 .ResX 文件，然后重新运行 ResGen
    // (以 /str 作为命令选项)，或重新生成 VS 项目。
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "17.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class Strings {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Strings() {
        }
        
        /// <summary>
        ///   返回此类使用的缓存的 ResourceManager 实例。
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("Resources.Strings", typeof(Strings).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   重写当前线程的 CurrentUICulture 属性，对
        ///   使用此强类型资源类的所有资源查找执行重写。
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
        ///   查找类似 Illegal enum value: {0}. 的本地化字符串。
        /// </summary>
        internal static string Arg_EnumIllegalVal {
            get {
                return ResourceManager.GetString("Arg_EnumIllegalVal", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 Value cannot be empty. 的本地化字符串。
        /// </summary>
        internal static string Argument_EmptyValue {
            get {
                return ResourceManager.GetString("Argument_EmptyValue", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 Value cannot be null. 的本地化字符串。
        /// </summary>
        internal static string ArgumentNull_Generic {
            get {
                return ResourceManager.GetString("ArgumentNull_Generic", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 Number must be either non-negative and less than or equal to Int32.MaxValue or -1. 的本地化字符串。
        /// </summary>
        internal static string ArgumentOutOfRange_NeedNonNegOrNegative1 {
            get {
                return ResourceManager.GetString("ArgumentOutOfRange_NeedNonNegOrNegative1", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 Cannot create &apos;{0}&apos; because a file or directory with the same name already exists. 的本地化字符串。
        /// </summary>
        internal static string IO_AlreadyExists_Name {
            get {
                return ResourceManager.GetString("IO_AlreadyExists_Name", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 The file &apos;{0}&apos; already exists. 的本地化字符串。
        /// </summary>
        internal static string IO_FileExists_Name {
            get {
                return ResourceManager.GetString("IO_FileExists_Name", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 Unable to find the specified file. 的本地化字符串。
        /// </summary>
        internal static string IO_FileNotFound {
            get {
                return ResourceManager.GetString("IO_FileNotFound", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 Could not find file &apos;{0}&apos;. 的本地化字符串。
        /// </summary>
        internal static string IO_FileNotFound_FileName {
            get {
                return ResourceManager.GetString("IO_FileNotFound_FileName", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 Could not find a part of the path. 的本地化字符串。
        /// </summary>
        internal static string IO_PathNotFound_NoPathName {
            get {
                return ResourceManager.GetString("IO_PathNotFound_NoPathName", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 Could not find a part of the path &apos;{0}&apos;. 的本地化字符串。
        /// </summary>
        internal static string IO_PathNotFound_Path {
            get {
                return ResourceManager.GetString("IO_PathNotFound_Path", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 The specified file name or path is too long, or a component of the specified path is too long. 的本地化字符串。
        /// </summary>
        internal static string IO_PathTooLong {
            get {
                return ResourceManager.GetString("IO_PathTooLong", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 The path &apos;{0}&apos; is too long, or a component of the specified path is too long. 的本地化字符串。
        /// </summary>
        internal static string IO_PathTooLong_Path {
            get {
                return ResourceManager.GetString("IO_PathTooLong_Path", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 The process cannot access the file &apos;{0}&apos; because it is being used by another process. 的本地化字符串。
        /// </summary>
        internal static string IO_SharingViolation_File {
            get {
                return ResourceManager.GetString("IO_SharingViolation_File", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 The process cannot access the file because it is being used by another process. 的本地化字符串。
        /// </summary>
        internal static string IO_SharingViolation_NoFileName {
            get {
                return ResourceManager.GetString("IO_SharingViolation_NoFileName", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 The specified value is not valid. 的本地化字符串。
        /// </summary>
        internal static string net_sockets_invalid_optionValue_all {
            get {
                return ResourceManager.GetString("net_sockets_invalid_optionValue_all", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 The condition argument is null. 的本地化字符串。
        /// </summary>
        internal static string SpinWait_SpinUntil_ArgumentNull {
            get {
                return ResourceManager.GetString("SpinWait_SpinUntil_ArgumentNull", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 The timeout must represent a value between -1 and Int32.MaxValue, inclusive. 的本地化字符串。
        /// </summary>
        internal static string SpinWait_SpinUntil_TimeoutWrong {
            get {
                return ResourceManager.GetString("SpinWait_SpinUntil_TimeoutWrong", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 Access to the path is denied. 的本地化字符串。
        /// </summary>
        internal static string UnauthorizedAccess_IODenied_NoPathName {
            get {
                return ResourceManager.GetString("UnauthorizedAccess_IODenied_NoPathName", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 Access to the path &apos;{0}&apos; is denied. 的本地化字符串。
        /// </summary>
        internal static string UnauthorizedAccess_IODenied_Path {
            get {
                return ResourceManager.GetString("UnauthorizedAccess_IODenied_Path", resourceCulture);
            }
        }
    }
}

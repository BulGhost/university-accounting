﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace UniversityAccounting.WEB.Resources.Controllers {
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
    public class StudentsController {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal StudentsController() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        public static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("UniversityAccounting.WEB.Resources.Controllers.StudentsController", typeof(StudentsController).Assembly);
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
        public static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Courses.
        /// </summary>
        public static string Courses {
            get {
                return ResourceManager.GetString("Courses", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Delete student.
        /// </summary>
        public static string DeleteStudent {
            get {
                return ResourceManager.GetString("DeleteStudent", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Edit student.
        /// </summary>
        public static string EditStudent {
            get {
                return ResourceManager.GetString("EditStudent", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to {0} group.
        /// </summary>
        public static string GroupName {
            get {
                return ResourceManager.GetString("GroupName", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to New student.
        /// </summary>
        public static string NewStudent {
            get {
                return ResourceManager.GetString("NewStudent", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to {0} student(s) removed.
        /// </summary>
        public static string SeveralStudentsDeleted {
            get {
                return ResourceManager.GetString("SeveralStudentsDeleted", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Student {0} {1} added.
        /// </summary>
        public static string StudentAdded {
            get {
                return ResourceManager.GetString("StudentAdded", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Student {0} {1} deleted.
        /// </summary>
        public static string StudentDeleted {
            get {
                return ResourceManager.GetString("StudentDeleted", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Such a student already exists..
        /// </summary>
        public static string StudentExists {
            get {
                return ResourceManager.GetString("StudentExists", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Student {0} {1} updated.
        /// </summary>
        public static string StudentUpdated {
            get {
                return ResourceManager.GetString("StudentUpdated", resourceCulture);
            }
        }
    }
}

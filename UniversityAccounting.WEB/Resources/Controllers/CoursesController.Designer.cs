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
    public class CoursesController {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal CoursesController() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        public static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("UniversityAccounting.WEB.Resources.Controllers.CoursesController", typeof(CoursesController).Assembly);
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
        ///   Looks up a localized string similar to &quot;{0}&quot; course added.
        /// </summary>
        public static string CourseAdded {
            get {
                return ResourceManager.GetString("CourseAdded", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to &quot;{0}&quot; course deleted.
        /// </summary>
        public static string CourseDeleted {
            get {
                return ResourceManager.GetString("CourseDeleted", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The course name {0} is already exists..
        /// </summary>
        public static string CourseExistsErrorMessage {
            get {
                return ResourceManager.GetString("CourseExistsErrorMessage", resourceCulture);
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
        ///   Looks up a localized string similar to &quot;{0}&quot; course updated.
        /// </summary>
        public static string CourseUpdated {
            get {
                return ResourceManager.GetString("CourseUpdated", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Create new.
        /// </summary>
        public static string CreateNew {
            get {
                return ResourceManager.GetString("CreateNew", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Delete Course.
        /// </summary>
        public static string DeleteCourse {
            get {
                return ResourceManager.GetString("DeleteCourse", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Unable to delete course that contains groups.
        /// </summary>
        public static string DeleteErrorMessage {
            get {
                return ResourceManager.GetString("DeleteErrorMessage", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Edit Course.
        /// </summary>
        public static string EditCourse {
            get {
                return ResourceManager.GetString("EditCourse", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to {0} course(s) removed.
        /// </summary>
        public static string SeveralCoursesDeleted {
            get {
                return ResourceManager.GetString("SeveralCoursesDeleted", resourceCulture);
            }
        }
    }
}

﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace MMKiwi.CBindingSG.SourceGenerator {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "17.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class SourceResources {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal SourceResources() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("MMKiwi.CBindingSG.SourceGenerator.SourceResources", typeof(SourceResources).Assembly);
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
        ///   Looks up a localized string similar to // This Source Code Form is subject to the terms of the Mozilla Public
        ///// License, v. 2.0. If a copy of the MPL was not distributed with this
        ///// file, You can obtain one at https://mozilla.org/MPL/2.0/.
        ///
        ///#if !CBSG_OMITINTERNAL
        ///
        ///using System.Reflection;
        ///
        ///namespace MMKiwi.CBindingSG;
        ///
        ///internal static class CbsgConstructionHelper
        ///{
        ///#if NET7_0_OR_GREATER
        ///    public static TRes Construct&lt;TRes, THandle&gt;(THandle handle)
        ///        where TRes : class, IConstructableWrapper&lt;TRes, THandle&gt;
        ///        where T [rest of string was truncated]&quot;;.
        /// </summary>
        internal static string CbsgConstructionHelper {
            get {
                return ResourceManager.GetString("CbsgConstructionHelper", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to // This Source Code Form is subject to the terms of the Mozilla Public
        ///// License, v. 2.0. If a copy of the MPL was not distributed with this
        ///// file, You can obtain one at https://mozilla.org/MPL/2.0/.
        ///
        ///#if !CBSG_OMITINTERNAL
        ///using System.Diagnostics.CodeAnalysis;
        ///
        ///namespace MMKiwi.CBindingSG;
        ///
        ///#if !NET7_0_OR_GREATER
        ///[SuppressMessage(&quot;ReSharper&quot;, &quot;UnusedTypeParameter&quot;, Justification = &quot;Type arguments needed for static methods&quot;)]
        ///#endif
        ///internal interface IConstructableHandle&lt;out THandle&gt; where  [rest of string was truncated]&quot;;.
        /// </summary>
        internal static string IConstructableHandle {
            get {
                return ResourceManager.GetString("IConstructableHandle", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to // This Source Code Form is subject to the terms of the Mozilla Public
        ///// License, v. 2.0. If a copy of the MPL was not distributed with this
        ///// file, You can obtain one at https://mozilla.org/MPL/2.0/.
        ///
        ///using System.Diagnostics.CodeAnalysis;
        ///
        ///namespace MMKiwi.CBindingSG;
        ///
        ///#if !CBSG_OMITINTERNAL
        ///
        ///#if !NET7_0_OR_GREATER
        ///[SuppressMessage(&quot;ReSharper&quot;, &quot;UnusedTypeParameter&quot;, Justification = &quot;Type arguments needed for static methods&quot;)]
        ///#endif
        ///internal interface IConstructableWrapper&lt;out TRes, in THa [rest of string was truncated]&quot;;.
        /// </summary>
        internal static string IConstructableWrapper {
            get {
                return ResourceManager.GetString("IConstructableWrapper", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to // This Source Code Form is subject to the terms of the Mozilla Public
        ///// License, v. 2.0. If a copy of the MPL was not distributed with this
        ///// file, You can obtain one at https://mozilla.org/MPL/2.0/.
        ///
        ///#if !CBSG_OMITINTERNAL
        ///namespace MMKiwi.CBindingSG;
        ///
        ///internal interface IHasHandle&lt;out THandle&gt; where THandle : SafeHandle
        ///{
        ///    public THandle Handle { get; }
        ///}
        ///#endif.
        /// </summary>
        internal static string IHasHandle {
            get {
                return ResourceManager.GetString("IHasHandle", resourceCulture);
            }
        }
    }
}

// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at https://mozilla.org/MPL/2.0/.

using System.Runtime.InteropServices;

using Microsoft.CodeAnalysis;

namespace MMKiwi.CBindingSG.SourceGenerator;

internal static class Constants
{
    public const string SgAttribute = """
                                      [global::System.CodeDom.Compiler.GeneratedCodeAttribute("MMKiwi.CBindingSG.SourceGenerator", "0.0.1.100")]
                                      """;

    public const string Namespace = $"MMKiwi.CBindingSG";
    
    public const string WrapperMarkerClass = "CbsgWrapperMethodAttribute";
    public const string ConstructHelperClass = "CbsgConstructionHelper";
    public const string WrapperMarkerFullName = $"{Namespace}.{WrapperMarkerClass}";
    
    public const string HandleMarkerClass = "CbsgGenerateHandleAttribute";
    public const string HandleMarkerFullName = $"{Namespace}.{HandleMarkerClass}";

    public const string ErrorMarkerClass = "CbsgErrorMethodAttribute";
    public const string ErrorMarkerFullName = $"{Namespace}.{ErrorMarkerClass}";
    
    public const string GenWrapperMarkerClass = "CbsgGenerateWrapperAttribute";
    public const string GenWrapperMarkerFullName = $"{Namespace}.{GenWrapperMarkerClass}";
    
    public const string NeverOwnsMarkerClass = "CbsgNeverOwnsAttribute";
    public const string NeverOwnsMarkerFullName = $"{Namespace}.{NeverOwnsMarkerClass}";

    public const string IHasHandle = "IHasHandle";
    public const string IHasHandleFullName = $"{Namespace}.{IHasHandle}";

    public const string IConstructableWrapper = "IConstructableWrapper";
    public const string IConstructableWrapperFullName = $"{Namespace}.{IConstructableWrapper}";
    
    public const string IConstructableHandle = "IConstructableHandle";
    public const string IConstructableHandleFullName = $"{Namespace}.{IConstructableHandle}";

    /// <summary>
    /// Could not generate wrapper method for {0} because the parent class could not be found
    /// </summary>
    public static DiagnosticDescriptor Diag01ParentNotFound
        => new("CBSG0001",
            "Could not generate method",
            "Could not generate wrapper method for {0} because the parent class could not be found",
            "CBindingSourceGenerator",
            DiagnosticSeverity.Warning,
            true);

    /// <summary>
    /// "{1} must be partial {0}"
    /// </summary>
    public static DiagnosticDescriptor Diag02IsNotPartial
        => new("CBSG0002",
            "Method must be partial",
            "{1} must be partial {0}",
            "CBindingSourceGenerator",
            DiagnosticSeverity.Warning,
            true);

    /// <summary>
    /// Could not generate wrapper method for {0}.
    /// </summary>
    public static DiagnosticDescriptor Diag03CouldNotGenerateWrapper
        => new("CBSG0003",
            "Could not generate wrapper method",
            "Could not generate wrapper method for {0}.",
            "CBindingSourceGenerator",
            DiagnosticSeverity.Warning,
            true);

    /// <summary>
    /// Skipping match for method {0}. Method {1} does not have the same number of parameters.
    /// </summary>
    public static DiagnosticDescriptor Diag04SkipParameters
        => new("CBSG0004",
            "Partial match",
            "Skipping match for method {0}. Method {1} does not have the same number of parameters.",
            "CBindingSourceGenerator",
            DiagnosticSeverity.Warning,
            true);

    /// <summary>
    /// Skipping match for method {0}. Method {1} is missing the LibraryImport attribute.
    /// </summary>
    public static DiagnosticDescriptor Diag05SkipLibraryImport
        => new("CBSG0005",
            "Partial match",
            "Skipping match for method {0}. Method {1} is missing the DllImport or LibraryImport attribute.",
            "CBindingSourceGenerator",
            DiagnosticSeverity.Warning,
            true);

    /// <summary>
    /// Skipping match for method {0}. Parameter {1} of method {2} cannot be matched.
    /// </summary>
    public static DiagnosticDescriptor Diag06SkipParameter
        => new("CBSG0006",
            "Partial match",
            "Skipping match for method {0}. Parameter {1} of method {2} cannot be matched.",
            "CBindingSourceGenerator",
            DiagnosticSeverity.Warning,
            true);

    /// <summary>
    /// Skipping match for method {0}. Method {1} has an incompatible return type.
    /// </summary>
    public static DiagnosticDescriptor Diag07SkipMatchReturn
        => new("CBSG0007",
            "Partial match",
            "Skipping match for method {0}. Method {1} has an incompatible return type.",
            "CBindingSourceGenerator",
            DiagnosticSeverity.Warning,
            true);

    /// <summary>
    /// Class {0} implements IHasHandle, but does not implement IDisposable.
    /// </summary>
    public static DiagnosticDescriptor Diag08IDisposable
        => new("CBSG0008",
            "Missing IDisposable",
            "Class {0} implements IHasHandle, but does not implement IDisposable.",
            "CBindingSourceGenerator",
            DiagnosticSeverity.Warning,
            true);

    /// <summary>
    /// Class {0} must implement IConstructableHandle or IHasHandle to generate the wrapper type.
    /// </summary>
    public static DiagnosticDescriptor Diag09NoImplement
        => new("CBSG00009",
            "Class does not implement IConstructableHandle or IHasHandle",
            "Class {0} must implement IConstructableHandle or IHasHandle to generate the wrapper type.",
            "CBindingSourceGenerator",
            DiagnosticSeverity.Warning,
            true);

    /// <summary>
    /// Class {0} should be sealed or abstract.
    /// </summary>
    public static DiagnosticDescriptor Diag10SealAbstract
        => new("CBSG00010",
            "Handle should be sealed or abstract",
            "Class {0} should be sealed or abstract.",
            "CBindingSourceGenerator",
            DiagnosticSeverity.Warning,
            true);


    /// <summary>
    /// Class {0} must subclass SafeHandle. Parent classes: {1}
    /// </summary>
    public static DiagnosticDescriptor Diag11ImplementHandle
        => new("CBSG00011",
            "Class must be handle",
            "Class {0} must subclass SafeHandle. Parent classes: {1}",
            "CBindingSourceGenerator",
            DiagnosticSeverity.Warning,
            true);

}
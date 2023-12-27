// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at https://mozilla.org/MPL/2.0/.

using System.Collections.Immutable;

using Microsoft.CodeAnalysis;

namespace MMKiwi.CBindingSG.SourceGeneratorTests;

/// <summary>
/// Represents the results of a single <see cref="ISourceGenerator"/> generation pass.
/// </summary>
public readonly struct FilteredRunResult
{
    internal FilteredRunResult(
        GeneratorRunResult result,
        IEnumerable<string> ignoredOutputs)
    {
        this.Generator = result.Generator;
        this.GeneratedSources = result.GeneratedSources.Where(gs => !ignoredOutputs.Contains(gs.HintName));
        this.Diagnostics = result.Diagnostics;
        this.TrackedSteps = result.TrackedSteps;
        this.TrackedOutputSteps = result.TrackedOutputSteps;
        this.Exception = result.Exception;
    }

    /// <summary>
    /// The <see cref="ISourceGenerator"/> that produced this result.
    /// </summary>
    public ISourceGenerator Generator { get; }

    /// <summary>
    /// The sources that were added by <see cref="Generator"/> during the generation pass this result represents.
    /// </summary>
    public IEnumerable<GeneratedSourceResult> GeneratedSources { get; }

    /// <summary>
    /// A collection of <see cref="Diagnostic"/>s reported by <see cref="Generator"/> 
    /// </summary>
    /// <remarks>
    /// When generation fails due to an <see cref="Exception"/> being thrown, a single diagnostic is added
    /// to represent the failure. Any generator reported diagnostics up to the failure point are not included.
    /// </remarks>
    public ImmutableArray<Diagnostic> Diagnostics { get; }

    /// <summary>
    /// An <see cref="System.Exception"/> instance that was thrown by the generator, or <c>null</c> if the generator completed without error.
    /// </summary>
    /// <remarks>
    /// When this property has a value, <see cref="GeneratedSources"/> property is guaranteed to be empty, and the <see cref="Diagnostics"/>
    /// collection will contain a single diagnostic indicating that the generator failed.
    /// </remarks>
    public Exception? Exception { get; }

    /// <summary>
    /// A collection of the named incremental steps (both intermediate and final output ones)
    /// executed during the generator pass this result represents.
    /// </summary>
    /// <remarks>
    /// Steps can be named by extension method WithTrackingName.
    /// </remarks>
    public ImmutableDictionary<string, ImmutableArray<IncrementalGeneratorRunStep>> TrackedSteps { get; }

    /// <summary>
    /// A collection of the named output steps executed during the generator pass this result represents.
    /// </summary>
    /// <remarks>
    /// Steps can be named by extension method WithTrackingName.
    /// </remarks>
    public ImmutableDictionary<string, ImmutableArray<IncrementalGeneratorRunStep>> TrackedOutputSteps { get; }


    
}

public static class VerifySourceGeneratorsFilter
{
    static VerifySourceGeneratorsFilter()
    {
        Initialize();
    }
    public static FilteredRunResult Filter(this GeneratorRunResult result, IEnumerable<string> results)
        => new FilteredRunResult(result, results);
    public static bool Initialized { get; private set; }

    public static void Initialize()
    {
        if (!Initialized)
        {
            Initialized = true;

            InnerVerifier.ThrowIfVerifyHasBeenRun();
            VerifierSettings.RegisterFileConverter<FilteredRunResult>(Convert);
        }
    }

    static ConversionResult Convert(FilteredRunResult target, IReadOnlyDictionary<string, object> context)
    {
        var targets = new List<Target>();
            
        if (target.Exception != null)
        {
            throw target.Exception;
        }

        var collection = target.GeneratedSources
            .OrderBy(_ => _.HintName)
            .Select(SourceToTarget);
        targets.AddRange(collection);
            
        if (target.Diagnostics.Any())
        {
            var info = new
            {
                target.Diagnostics
            };
            return new(info, targets);
        }

        return new(null, targets);
    }

    static Target SourceToTarget(GeneratedSourceResult source)
    {
        var data = $"""
                    //HintName: {source.HintName}
                    {source.SourceText}
                    """;
        return new("cs", data, Path.GetFileNameWithoutExtension(source.HintName));
    }
}
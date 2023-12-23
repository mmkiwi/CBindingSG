// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at https://mozilla.org/MPL/2.0/.

using System.Collections.Immutable;

using NetEscapades.EnumGenerators;

namespace MMKiwi.CBindingSG.SourceGeneratorTests;

[EnumExtensions]
public enum TargetFramework
{
    Net80,
    NetStandard20

}

public static partial class TargetFrameworkExtensions
{
    public static ImmutableArray<string> ToPreprocessor(this TargetFramework f)
        => f switch
        {
            TargetFramework.Net80 => ["NET7_0_OR_GREATER"],
            TargetFramework.NetStandard20 => [],
            _ => throw new ArgumentOutOfRangeException(nameof(f), f, null)
        };
}
// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at https://mozilla.org/MPL/2.0/.

namespace MMKiwi.CBindingSG.SourceGeneratorTests;

public class MemberVisibilities : TheoryData<TargetFramework, MemberVisibility>
{
    public MemberVisibilities()
    {
        foreach (TargetFramework f in TargetFrameworkExtensions.GetValues())
        foreach (var enumValue in MemberVisibilityExtensions.GetValues())
            Add(f, enumValue);
    }
}

public class TargetFrameworks : TheoryData<TargetFramework>
{
    public TargetFrameworks()
    {
        foreach (TargetFramework f in TargetFrameworkExtensions.GetValues())
            Add(f);
    }
}
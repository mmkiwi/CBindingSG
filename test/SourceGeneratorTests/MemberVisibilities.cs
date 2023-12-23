// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at https://mozilla.org/MPL/2.0/.

namespace MMKiwi.CBindingSG.SourceGeneratorTests;

[UsesVerify]
public class MemberVisibilities : TheoryData<MemberVisibility>
{
    public MemberVisibilities()
    {
        foreach (var enumValue in MemberVisibilityExtensions.GetValues())
            Add(enumValue);
    }
}
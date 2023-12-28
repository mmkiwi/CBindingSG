// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at https://mozilla.org/MPL/2.0/.
#if !CBSG_OMITATTRIBUTE && !CBSG_OMITALL
namespace MMKiwi.CBindingSG;

[AttributeUsage(AttributeTargets.Class)]
public class CbsgGenerateHandleAttribute : Attribute
{
    public MemberVisibility ConstructorVisibility { get; set; } = MemberVisibility.Protected;
    public bool GenerateOwns { get; set; } = true;
    public bool GenerateDoesntOwn { get; set; } = true;
}
#endif
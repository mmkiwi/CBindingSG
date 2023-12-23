// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at https://mozilla.org/MPL/2.0/.

namespace MMKiwi.CBindingSG;

[AttributeUsage(AttributeTargets.Assembly | AttributeTargets.Class | AttributeTargets.Method)]
public class CbsgErrorMethodAttribute : Attribute
{
    public CbsgErrorMethodAttribute(Type parentType, string methodName)
    {
        ParentType = parentType;
        MethodName = methodName;
    }
    public Type ParentType { get; }
    public string MethodName { get; }
}
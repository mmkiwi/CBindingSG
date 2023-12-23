// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at https://mozilla.org/MPL/2.0/.

namespace SampleProject.FullyGenerated;

public class TestWrapper: IConstructableWrapper<TestWrapper, TestHandle>, IHasHandle<TestHandle>
{
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("MMKiwi.CBindingSG.SourceGenerator", "0.0.1.000")]
#if NET7_0_OR_GREATER
    static TestWrapper IConstructableWrapper<TestWrapper, TestHandle>.Construct(TestHandle handle) => new(handle);
#else
    public static TestWrapper Construct(TestHandle handle) => new(handle);
#endif

    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("MMKiwi.CBindingSG.SourceGenerator", "0.0.1.000")]
    private TestWrapper(TestHandle handle) => Handle = handle;

    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("MMKiwi.CBindingSG.SourceGenerator", "0.0.1.000")]
    internal TestHandle Handle { get; }

    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("MMKiwi.CBindingSG.SourceGenerator", "0.0.1.000")]
    TestHandle IHasHandle<TestHandle>.Handle => Handle;
}
// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at https://mozilla.org/MPL/2.0/.

namespace SampleProject.ValidExamples;

[CbsgGenerateHandle]
public abstract partial class HandleParentHasConstructParent: FullyGenerated.TestHandleBase, IConstructableHandle<HandleParentHasConstructParent>
{
}

[CbsgGenerateHandle]
public abstract partial class HandleParentHasConstruct(bool ownsHandle) : HandleParentHasConstructParent(ownsHandle), IConstructableHandle<HandleParentHasConstruct>
{
    
}
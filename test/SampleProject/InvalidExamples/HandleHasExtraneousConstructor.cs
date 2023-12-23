// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at https://mozilla.org/MPL/2.0/.

namespace SampleProject.ValidExamples;

//This unit test also tests embedded classes
public abstract partial class HandleHasExtraneousConstructorBase: SafeHandle, IConstructableHandle<HandleHasConstructor>
{
    public HandleHasConstructor(int weird) : base(-1, true) { }
    
    [CbsgGenerateHandle]
    public abstract partial class HandleHasExtraneousConstructor: HandleHasExtraneousConstructorBase, IConstructableHandle<HandleHasConstructor>
    {
        public HandleHasConstructor(int weird) : base(15) { }
    }

}
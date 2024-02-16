// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at https://mozilla.org/MPL/2.0/.

using System.Text;

namespace MMKiwi.CBindingSG.SourceGenerator;

public static class HandleGenerationHelper
{
    internal static string GenerateExtensionClass(HandleGenerator.GenerationInfo.Ok genInfo)
    {
        StringBuilder resFile = new();
        
        
        string className = genInfo.Identifier;

        foreach (string use in genInfo.Usings)
            resFile.AppendLine(use);

        resFile.AppendLine("#nullable enable");

        foreach (string ns in genInfo.ParentNamespaces)
        {
            resFile.AppendLine($$"""namespace {{ns}} {""");
        }

        foreach (var cls in genInfo.ParentClasses)
        {
            resFile.AppendLine($$"""

                                 {{cls.Modifiers}} {{cls.Keyword}} {{cls.Identifier}}
                                 {
                                 """);
        }

        if (genInfo.GenerateConstruct)
        {
            switch (genInfo)
            {
                case { BaseHandleType: HandleGenerator.BaseType.Parameterless, StaticVirtual: true }:
                    resFile.AppendLine($$"""
                                         
                                            {{Constants.SgAttribute}}
                                             static {{className}} {{Constants.IConstructableHandle}}<{{className}}>.Construct(bool ownsHandle) => new();
                                         """);
                    break;
                case { BaseHandleType: HandleGenerator.BaseType.Parameterless, StaticVirtual: false }:
                    resFile.AppendLine($$"""
                                         
                                            {{Constants.SgAttribute}}
                                             public static {{className}} Construct(bool ownsHandle) => new();
                                         """);
                    break;
                case { BaseHandleType: HandleGenerator.BaseType.Bool, StaticVirtual: true }:
                    resFile.AppendLine($$"""
                                         
                                             {{Constants.SgAttribute}}
                                             static {{className}} {{Constants.IConstructableHandle}}<{{className}}>.Construct(bool ownsHandle) => ownsHandle ? new Owns() : new DoesntOwn();
                                         """);
                    break;
                case { BaseHandleType: HandleGenerator.BaseType.Bool, StaticVirtual: false }:
                    resFile.AppendLine($$"""
                                         
                                             {{Constants.SgAttribute}}
                                             public static {{className}} Construct(bool ownsHandle) => ownsHandle ? new Owns() : new DoesntOwn();
                                         """);
                    break;
            }
        }

        switch (genInfo.GenerateOwns)
        {
            case true when genInfo.StaticVirtual:
                resFile.AppendLine($$"""
                                         
                                         {{Constants.SgAttribute}}
                                         public class Owns() : {{className}}(true), {{Constants.IConstructableHandleFullName}}<Owns>
                                         {
                                             static Owns {{Constants.IConstructableHandleFullName}}<Owns>.Construct(bool ownsHandle)
                                             {
                                                if(!ownsHandle)
                                                    throw new InvalidOperationException("Cannot construct Owns that does not own handle");
                                                return new();
                                             }
                                         }
                                     """);
                break;
            case true when !genInfo.StaticVirtual:
                resFile.AppendLine($$"""
                                         
                                         {{Constants.SgAttribute}}
                                         public class Owns() : {{className}}(true), {{Constants.IConstructableHandleFullName}}<Owns>
                                         {
                                             internal static Owns Construct(bool ownsHandle)
                                             {
                                                if(!ownsHandle)
                                                    throw new InvalidOperationException("Cannot construct Owns that does not own handle");
                                                return new();
                                             }
                                         }
                                     """);
                break;
        }


        switch (genInfo.GenerateDoesntOwn)
        {
            case true when genInfo.StaticVirtual:
                resFile.AppendLine($$"""
                                         
                                         {{Constants.SgAttribute}}
                                         public class DoesntOwn() : {{className}}(false), {{Constants.IConstructableHandleFullName}}<DoesntOwn>
                                         {
                                             static DoesntOwn {{Constants.IConstructableHandleFullName}}<DoesntOwn>.Construct(bool ownsHandle)
                                             {
                                                if(ownsHandle)
                                                    throw new InvalidOperationException("Cannot construct DoesntOwn that owns handle");
                                                return new();
                                             }
                                         }
                                     """);
                break;
            case true when !genInfo.StaticVirtual:
                resFile.AppendLine($$"""
                                         
                                         {{Constants.SgAttribute}}
                                         public class DoesntOwn() : {{className}}(false), {{Constants.IConstructableHandleFullName}}<DoesntOwn>
                                         {
                                             internal static DoesntOwn Construct(bool ownsHandle)
                                             {
                                                if(ownsHandle)
                                                    throw new InvalidOperationException("Cannot construct DoesntOwn that owns handle");
                                                return new();
                                             }
                                         }
                                     """);
                break;
        }

        if (genInfo.GenerateConstructor)
        {
            resFile.AppendLine($$"""
                                 
                                     {{Constants.SgAttribute}}
                                     {{genInfo.ConstructorVisibility}} {{className}}(bool ownsHandle): base(ownsHandle) { }
                                 """);
        }

        for (int i = 0; i < genInfo.ParentClasses.Count + genInfo.ParentNamespaces.Count; i++)
            resFile.AppendLine("}");

        return resFile.ToString();
    }
}
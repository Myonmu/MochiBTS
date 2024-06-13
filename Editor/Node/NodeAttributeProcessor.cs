
#if ODIN_INSPECTOR

using System;
using System.Collections.Generic;
using System.Reflection;
using MochiBTS.Core.Primitives.Nodes;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
namespace MochiBTS.Editor
{
    public class NodeAttributeProcessor<T> : OdinAttributeProcessor<T> where T : Node
    {
        public override void ProcessChildMemberAttributes(InspectorProperty parentProperty, MemberInfo member, List<Attribute> attributes)
        {
            attributes.Add(new LabelWidthAttribute(125));
        }
    }
    
}
#endif
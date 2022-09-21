using MochiBTS.Core.ConcreteVariables;
using UnityEditor;

namespace MochiBTS.Editor.VariableDrawers
{
    [CustomPropertyDrawer(typeof(MochiFloat))]
    [CustomPropertyDrawer(typeof(MochiVector3))]
    [CustomPropertyDrawer(typeof(MochiQuaternion))]
    public class DerivedVariableDrawer : MochiVariableDrawer
    {
    }
}
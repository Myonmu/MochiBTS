using MochiBTS.Temp.ConcreteVariables;
using UnityEditor;
namespace DefaultNamespace.Editor.VariableDrawers
{
    [CustomPropertyDrawer(typeof(MochiFloat))]
    [CustomPropertyDrawer(typeof(MochiVector3))]
    [CustomPropertyDrawer(typeof(MochiQuaternion))]
    public class DerivedVariableDrawer : MochiVariableDrawer
    {
    }
}
using AceLand.Library.Editor;
using AceLand.States.ProjectSetting;
using UnityEditor;

namespace AceLand.States.Editor.Drawer
{
    [CustomEditor(typeof(StatesSettings))]
    public class StatesSettingsInspector : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            EditorHelper.DrawAllPropertiesAsDisabled(serializedObject);
        }
    }
}
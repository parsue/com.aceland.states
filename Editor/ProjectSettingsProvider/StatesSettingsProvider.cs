using AceLand.Library.Editor;
using AceLand.States.ProjectSetting;
using UnityEditor;
using UnityEngine.UIElements;

namespace AceLand.States.Editor.ProjectSettingsProvider
{
    public class StatesSettingsProvider : SettingsProvider
    {
        public const string SETTINGS_NAME = "Project/AceLand States";
        private SerializedObject _settings;
        
        private StatesSettingsProvider(string path, SettingsScope scope = SettingsScope.User) 
            : base(path, scope) { }
        
        public override void OnActivate(string searchContext, VisualElement rootElement)
        {
            _settings = StatesSettings.GetSerializedSettings();
        }

        [SettingsProvider]
        public static SettingsProvider CreateMyCustomSettingsProvider()
        {
            var provider = new StatesSettingsProvider(SETTINGS_NAME, SettingsScope.Project);
            return provider;
        }

        public override void OnGUI(string searchContext)
        {
            EditorHelper.DrawAllProperties(_settings);
        }
    }
}
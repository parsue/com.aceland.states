using AceLand.Library.Editor.Providers;
using AceLand.States.ProjectSetting;
using UnityEditor;
using UnityEngine.UIElements;

namespace AceLand.States.Editor.ProjectSettingsProvider
{
    public class StatesSettingsProvider : AceLandSettingsProvider
    {
        public const string SETTINGS_NAME = "Project/AceLand States";
        
        private StatesSettingsProvider(string path, SettingsScope scope = SettingsScope.User) 
            : base(path, scope) { }
        
        [InitializeOnLoadMethod]
        public static void CreateSettings() => StatesSettings.GetSerializedSettings();
        
        public override void OnActivate(string searchContext, VisualElement rootElement)
        {
            base.OnActivate(searchContext, rootElement);
            Settings = StatesSettings.GetSerializedSettings();
        }

        [SettingsProvider]
        public static SettingsProvider CreateMyCustomSettingsProvider()
        {
            var provider = new StatesSettingsProvider(SETTINGS_NAME, SettingsScope.Project);
            return provider;
        }
    }
}
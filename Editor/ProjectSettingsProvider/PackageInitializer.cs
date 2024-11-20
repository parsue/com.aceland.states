using AceLand.States.ProjectSetting;
using UnityEditor;

namespace AceLand.States.Editor.ProjectSettingsProvider
{
    [InitializeOnLoad]
    public static class PackageInitializer
    {
        static PackageInitializer()
        {
            StatesSettings.GetSerializedSettings();
        }
    }
}
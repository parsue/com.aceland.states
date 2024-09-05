using AceLand.Library.Attribute;
using AceLand.Library.ProjectSetting;
using UnityEngine;

namespace AceLand.States.ProjectSetting
{
    public class StatesSettings : ProjectSettings<StatesSettings>
    {
        [Header("Getter")] [Min(0)]
        public float getterTimeout = 0.15f;

        [Header("State")]
        public bool invokeEnterOnLateWith = true;
        
        [Header("Logging")]
        public bool enableLogging;
        [ConditionalShow("enableLogging")] public bool loggingOnEditor;
        [ConditionalShow("enableLogging")] public bool loggingOnBuild;

        public bool PrintLogging()
        {
            if (!enableLogging) return false;
#if UNITY_EDITOR
            return loggingOnEditor;
#endif
            return loggingOnBuild;
        }
    }
}
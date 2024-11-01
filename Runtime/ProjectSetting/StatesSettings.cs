using AceLand.Library.BuildLeveling;
using AceLand.Library.ProjectSetting;
using UnityEngine;

namespace AceLand.States.ProjectSetting
{
    public class StatesSettings : ProjectSettings<StatesSettings>
    {
        [Header("Getter")] [Min(0)]
        [SerializeField] private float getterTimeout = 0.15f;

        [Header("State")]
        [SerializeField] private bool invokeEnterOnLateWith = true;
        
        [Header("Logging")]
        [SerializeField] private BuildLevel loggingLevel = BuildLevel.DevelopmentBuild;

        public float GetterTimeout => getterTimeout;
        public bool InvokeEnterOnLateWith => invokeEnterOnLateWith;
        
        public bool PrintLogging()
        {
            return loggingLevel.IsAcceptedLevel();
        }
    }
}
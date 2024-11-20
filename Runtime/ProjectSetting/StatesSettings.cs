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
        [SerializeField] private bool invokeEnterOnLateInject = true;
        
        [Header("Logging")]
        [SerializeField] private BuildLevel loggingLevel = BuildLevel.Development;

        public float GetterTimeout => getterTimeout;
        public bool InvokeEnterOnLateInject => invokeEnterOnLateInject;
        
        public bool PrintLogging => loggingLevel.IsAcceptedLevel();
    }
}
using AceLand.Library.BuildLeveling;
using AceLand.Library.ProjectSetting;
using UnityEngine;
using UnityEngine.Serialization;

namespace AceLand.States.ProjectSetting
{
    public class StatesSettings : ProjectSettings<StatesSettings>
    {
        [Header("Getter")] [Min(0)]
        [SerializeField] private float getterTimeout = 0.15f;

        [FormerlySerializedAs("invokeEnterOnLateWith")]
        [Header("State")]
        [SerializeField] private bool invokeEnterOnLateInject = true;
        
        [Header("Logging")]
        [SerializeField] private BuildLevel loggingLevel = BuildLevel.DevelopmentBuild;

        public float GetterTimeout => getterTimeout;
        public bool InvokeEnterOnLateInject => invokeEnterOnLateInject;
        
        public bool PrintLogging => loggingLevel.IsAcceptedLevel();
    }
}

#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;


namespace FireExtinguisher.Utilities
{
    [CustomEditor(typeof(ProjectSettings))]
    public class ProjectSettingEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            ProjectSettings projectSettings = (ProjectSettings)target;

            if (GUILayout.Button("Update Version"))
            {
                ResetData(projectSettings);
            }
        }

        void ResetData(ProjectSettings projectSettings)
        {
            projectSettings.UpdateVersion();
        }
    }
}
#endif
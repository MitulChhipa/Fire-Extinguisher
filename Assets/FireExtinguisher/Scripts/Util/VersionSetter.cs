using TMPro;
using UnityEngine;

namespace FireExtinguisher.Utilities
{
    public class VersionSetter : MonoBehaviour
    {
        [SerializeField] private ProjectSettings _projectSettings;
        private TextMeshProUGUI _text;

        private void Awake()
        {
            _text = GetComponent<TextMeshProUGUI>();
            _text.text = $"V{_projectSettings.version}";
        }
    }
}
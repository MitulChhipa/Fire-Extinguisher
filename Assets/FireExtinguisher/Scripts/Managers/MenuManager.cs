using UnityEngine;

namespace FireExtinguisher.Manager
{
    public class MenuManager : MonoBehaviour
    {
        [SerializeField] private GameObject _sceneCapturePrompt;
        [SerializeField] private GameObject _sceneCapturedPrompt;

        public void FlamablePresent()
        {
            _sceneCapturePrompt.SetActive(false);
            _sceneCapturedPrompt.SetActive(true);
        }

        public void FlamableNotPresent()
        {
            _sceneCapturePrompt.SetActive(true);
            _sceneCapturedPrompt.SetActive(false);
        }
    }
}
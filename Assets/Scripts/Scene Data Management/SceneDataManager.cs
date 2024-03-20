using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System.Threading.Tasks;

namespace FireExtinguisher.SceneData
{
    public class SceneDataManager : MonoBehaviour
    {
        [SerializeField] private OVRSceneManager _sceneManager;
        public Action onSceneDataRecieved;
        public bool wallPresent;
        public bool sceneObjectPresent;

        public static SceneDataManager instance;

        private IEnumerable<string>[] _sceneObjects = { new[] { "DESK" },
                                                   new[] { "COUCH" },
                                                   new[] { "OTHER" },
                                                   new[] { "STORAGE" },
                                                   new[] { "BED" },
                                                   new[] { "SCREEN" },
                                                   new[] { "LAMP" },
                                                   new[] { "PLANT" },
                                                   new[] { "TABLE" },
                                                   new[] { "WALL_ART" }};

        private IEnumerable<string> _wallObjects = new[] { "WALL_FACE" };

        public UnityEvent onFlamableObjectPresent;
        public UnityEvent onFlamableObjectNotPresent;
        public UnityEvent onSceneAnchorLoaded;

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }

            _sceneManager.SceneCaptureReturnedWithoutError += SceneCaptured;
            _sceneManager.SceneModelLoadedSuccessfully += OnSceneAnchorLoaded;

            CheckForFlamables();
        }

        private void SceneCaptured()
        {
            CheckForFlamables();
        }

        public async Task<int> CheckForSceneObjectsAsync()
        {
            int ObjectsPresent = 0;

            for (int i = 0; i < _sceneObjects.Length; i++)
            {
                bool result = await _sceneManager.DoesRoomSetupExist(_sceneObjects[i]);
                if (result)
                {
                    ObjectsPresent++;
                }
            }

            return ObjectsPresent;
        }




        private async Task CheckForFlamables()
        {
            int result = await CheckForSceneObjectsAsync();

            if (result > 0)
            {
                sceneObjectPresent = true;
                onFlamableObjectPresent?.Invoke();
            }
            else
            {
                onFlamableObjectNotPresent?.Invoke();
            }
        }

        public void SceneRequest()
        {
            _sceneManager.RequestSceneCapture();
        }

        public void LoadSceneModels()
        {
            _sceneManager.LoadSceneModel();
        }

        private void OnSceneAnchorLoaded()
        {
            onSceneAnchorLoaded?.Invoke();
        }


        private void SceneObjectFound()
        {

        }
        private void SceneObjectNotFound()
        {

        }
    }
}
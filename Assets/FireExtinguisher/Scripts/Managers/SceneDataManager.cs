using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System.Threading.Tasks;
using FireExtinguisher.Utilities;
using Unity.VisualScripting;
using UnityEngine.Playables;

namespace FireExtinguisher.Manager
{
    public class SceneDataManager : MonoBehaviour
    {
        [SerializeField] private OVRSceneManager _sceneManager;
        public Action onSceneDataRecieved;
        public bool wallPresent;
        public bool sceneObjectPresent;
        [SerializeField] private ProjectSettings _projectSettings;

        public static SceneDataManager instance;

        private IEnumerable<string>[] _sceneObjects ;

        private IEnumerable<string> _wallObjects = new[] { SceneObjects.WALL_FACE.ToString() };

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

            _sceneObjects = _projectSettings.GetEnumerableFlamableObjects();

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
                print(_sceneObjects[i].ToString());
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
    }
}
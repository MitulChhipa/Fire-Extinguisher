using System;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Events;

public class SceneDataManager : MonoBehaviour
{
    [SerializeField] private OVRSceneManager _sceneManager;
    [SerializeField] private GameObject _setUpRequired;
    [SerializeField] private GameObject _setUpDone;
    [SerializeField] private TextMeshProUGUI _textDescription;
    public Action onSceneDataRecieved;
    public bool wallPresent;
    public bool sceneObjectPresent;

    public static SceneDataManager instance;

    public IEnumerable<string>[] _sceneObjects = { new[] { "WALL_FACE" },
                                                   new[] { "DESK" },
                                                   new[] { "COUCH" },
                                                   new[] { "OTHER" },
                                                   new[] { "STORAGE" },
                                                   new[] { "BED" },
                                                   new[] { "SCREEN" },
                                                   new[] { "LAMP" },
                                                   new[] { "PLANT" },
                                                   new[] { "TABLE" },
                                                   new[] { "WALL_ART" }};


    public Action onWallPresent;
    public Action onSceneObjectPresent;
    public Action onWallNull;
    public Action onSceneObjectNull;
    public UnityEvent onSceneAnchorLoaded;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            //DontDestroyOnLoad(gameObject);
        }
        //else if (instance != this)
        //{
        //    DestroyImmediate(gameObject);
        //}

        _sceneManager.SceneCaptureReturnedWithoutError += SceneCaptured;
        _sceneManager.SceneModelLoadedSuccessfully += OnSceneAnchorLoaded;
        CheckForWall();
        CheckForSceneObjects();
    }

    private void SceneCaptured()
    {
        GameData.sceneRequested = true;

        if (!wallPresent)
        {
            CheckForWall();
        }
    }

    public void CheckForWall()
    {
        foreach(IEnumerable<string> sceneObject in _sceneObjects)
        {
            _sceneManager.DoesRoomSetupExist(sceneObject).ContinueWith(PostSceneObjectsCheck);
        }
    }
    public void CheckForSceneObjects()
    {
        //_sceneManager.DoesRoomSetupExist(_sceneObjectData).ContinueWith(PostSceneObjectsCheck);
    }

    private void PostWallCheck(bool wallPresent)
    {
        this.wallPresent = wallPresent;

        if (wallPresent)
        {
            onWallPresent?.Invoke();
        }
        else
        {
            onWallNull?.Invoke();
        }
    }
    private void PostSceneObjectsCheck(bool sceneObjectPresent)
    {
        this.sceneObjectPresent = sceneObjectPresent;

        if (sceneObjectPresent)
        {
            onSceneObjectPresent?.Invoke();
        }
        else
        {
            onSceneObjectNull?.Invoke();
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
using System;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Events;

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
        }

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
        _sceneManager.DoesRoomSetupExist(_wallObjects).ContinueWith(PostWallCheck); 
    }
    public void CheckForSceneObjects()
    {
        foreach (IEnumerable<string> sceneObject in _sceneObjects)
        {
            _sceneManager.DoesRoomSetupExist(sceneObject).ContinueWith(PostSceneObjectsCheck);
        }
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
        if (!this.sceneObjectPresent)
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
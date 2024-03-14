using System;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

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

    IEnumerable<string> _wallData = new[] { "WALL_FACE" };
    IEnumerable<string> _sceneObjectData = new[] { "DESK", "COUCH", "OTHER", "STORAGE", "BED", "SCREEN", "LAMP", "PLANT", "TABLE", "WALL_ART" };

    public Action onWallPresent;
    public Action onSceneObjectPresent;
    public Action onWallNull;
    public Action onSceneObjectNull;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (instance != this)
        {
            DestroyImmediate(gameObject);
        }

        _sceneManager.SceneCaptureReturnedWithoutError += SceneCaptured;
        _sceneManager.DoesRoomSetupExist(_wallData).ContinueWith(PostWallCheck);
    }

    private void SceneCaptured()
    {
        GameData.sceneRequested = true;

        if (!wallPresent)
        {
            _sceneManager.DoesRoomSetupExist(_sceneObjectData).ContinueWith(PostWallCheck);
        }
    }

    public void CheckForWall()
    {

    }
    public void CheckForSceneObjects()
    {
        _sceneManager.DoesRoomSetupExist(_sceneObjectData).ContinueWith(PostWallCheck);
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
}
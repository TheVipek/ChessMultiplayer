using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class SceneController : MonoBehaviour
{
    public static SceneController instance;
    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this);
        }
        else
        {
            DontDestroyOnLoad(this);
            instance = this;
        }
    }
    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    // Update is called once per frame
    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
    public void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    public void SceneLoad(string nameofscene)
    {
        string prevScene = SceneManager.GetActiveScene().name;
        Debug.Log(prevScene);
        SceneManager.LoadScene(nameofscene,LoadSceneMode.Additive);
        SceneManager.UnloadSceneAsync(prevScene);

    }
    public void BackToLoadedScene(string nameofscene)
    {
        string prevScene = SceneManager.GetActiveScene().name;
        SceneManager.SetActiveScene(SceneManager.GetSceneByName(nameofscene));
        SceneManager.UnloadSceneAsync(prevScene);
    }
    /// <summary>
    /// load wanted scene in mode single
    /// </summary>
    /// <param name="sceneIndex"></param>
    public void SceneLoadSingle(string sceneIndex)
    {
        string prevScene = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(sceneIndex, LoadSceneMode.Single);
        SceneManager.UnloadSceneAsync(prevScene);
    }

    public void UnloadActiveScene()
    {
        SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene().buildIndex);
    }
    public void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "LocalGame")
        {
            //GameManager.instance.isOnline = false;
            //SceneManager.SetActiveScene(SceneManager.GetSceneByName("OnlineGame"));
        }
        else if(scene.name == "OnlineGame")
        {
            GameManager.instance.isOnline = true;
            Debug.Log(MirrorNetworking.PlayerLobbyController.localPlayer);
            Debug.Log(MirrorNetworking.PlayerLobbyController.localPlayer.gameObject.GetComponent<MirrorNetworking.PlayerLobbyController>().nickname);
            MirrorNetworking.PlayerController.playerController.CmdPlayerReadyForGame(MirrorNetworking.PlayerLobbyController.localPlayer);
            SceneManager.SetActiveScene(SceneManager.GetSceneByName("OnlineGame"));
            //SceneManager.MoveGameObjectToScene(MirrorNetworking.TurnManager.instance.gameObject, SceneManager.GetSceneByName("OnlineGame"));
            //SceneManager.MoveGameObjectToScene(MirrorNetworking.ServerMatchController.instance.gameObject, SceneManager.GetSceneByName("OnlineGame"));

        }
        else if(scene.name == "OnlineMode")
        {
            if(MirrorNetworking.UILobby.instance != null)
            {
                MirrorNetworking.UILobby.instance.gameObject.SetActive(true);
            }
            //CameraController.instance.cameraGlobal.gameObject.SetActive(true);
            GameManager.instance.isOnline = false;
            SceneManager.SetActiveScene(SceneManager.GetSceneByName("OnlineMode"));
        }
    }
    
    public string activeScene()
    {
        return SceneManager.GetActiveScene().name;
    }
    
}

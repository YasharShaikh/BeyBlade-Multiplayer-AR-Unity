using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{

    public static SceneLoader Instance;
    public string sceneToBeLoaded;
    private void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }



    public void LoadeScene(string _sceneName)
    {
        Debug.Log("[SceneLoader] LoadeScene called");
        sceneToBeLoaded = _sceneName;
        StartCoroutine(InitLoadScene());
    }


    IEnumerator InitLoadScene()
    {
        yield return SceneManager.LoadSceneAsync("Scene_Loading");
        
        StartCoroutine(LoadActualScene());
    }
    IEnumerator LoadActualScene()
    {
        Debug.Log("[SceneLoader] LoadActualScene called");

        var asyncSceneloading = SceneManager.LoadSceneAsync(sceneToBeLoaded);
        asyncSceneloading.allowSceneActivation = false;
        while (!asyncSceneloading.isDone)
        {
            Debug.Log("[SceneLoader] scene loading progress: " + asyncSceneloading.progress);

            if (asyncSceneloading.progress >= 0.9f)
            {
                yield return new WaitForSeconds(1f); 
                asyncSceneloading.allowSceneActivation = true;
            }
            yield return null;
        }

    }

}

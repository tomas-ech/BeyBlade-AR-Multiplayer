using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class SceneLoader : MonoBehaviour
{
    public static SceneLoader Instance {get; private set;}
    private string sceneNameToLoad;

    private void Awake( )
	{
		if(Instance != null && Instance != this)
		{
			Destroy(this);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(this);	
        }
    }

    public void LoadScene(string sceneName)
    {
        sceneNameToLoad = sceneName;

        StartCoroutine(InitializeSceneLoading());
    }

    IEnumerator InitializeSceneLoading()
    {
        //Carga la escena de carga mientras termina de cargar la escena real
        yield return SceneManager.LoadSceneAsync("Scene_Loading");
        
        //Carga la escena que se necesita
        StartCoroutine(LoadActualScene());
    }

    IEnumerator LoadActualScene()
    {
        var asyncScene = SceneManager.LoadSceneAsync(sceneNameToLoad);

        //Esta linea detiene a la escena de aparecer si no esta lista
        asyncScene.allowSceneActivation = false;

        while (!asyncScene.isDone)
        {
            Debug.Log(asyncScene.progress);

            if (asyncScene.progress >= 0.9f)
            {
            //si la escena esta mas del 90% lista, la muestra
                asyncScene.allowSceneActivation = true;
            }

            yield return null;
        }
    }
}

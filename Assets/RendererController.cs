using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class RendererController : MonoBehaviour
{
    public static RendererController instance;
    public Renderer screenRenderer;

    public IEnumerator OnDeath()
    {
        float t = 0;
        while(t < 1)
        {
            t +=Time.deltaTime * 2;
            screenRenderer.material.SetFloat("_Fade", t);
            yield return null;
        }
        screenRenderer.material.SetFloat("_Fade", 1);

        t = 1;
        while(t > 0)
        {
            t -=Time.deltaTime * 2;
            screenRenderer.material.SetFloat("_Fade", t);
            yield return null;
        }
        screenRenderer.material.SetFloat("_Fade", 0);
    }

    public IEnumerator RestartGame()
    {
        float t = 0;
        while(t < 1)
        {
            t +=Time.deltaTime * 2;
            screenRenderer.material.SetFloat("_Fade", t);
            yield return null;
        }
        screenRenderer.material.SetFloat("_Fade", 1);

        var asyncScene = SceneManager.LoadSceneAsync(0, LoadSceneMode.Single);
        while(asyncScene.progress < 0.9f) yield return null;

        t = 1;
        while(t > 0)
        {
            t -=Time.deltaTime * 2;
            screenRenderer.material.SetFloat("_Fade", t);
            yield return null;
        }
        screenRenderer.material.SetFloat("_Fade", 0);
    }

    public void DeathAnimation()
    {
        StartCoroutine(OnDeath());
    }

    public void SceneSwap()
    {
        StartCoroutine(RestartGame());
    }

    private void Awake() {
        if(instance)
        {
            Destroy(gameObject);
        }
        else{
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }
}

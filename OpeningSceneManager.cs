using System;
using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OpeningSceneManager : MonoBehaviour
{
    [SerializeField] string sceneName;
    [SerializeField] GameObject camera;

    public void Load()
    {
        StartCoroutine(WaitLoad());
        
    }

    public IEnumerator WaitLoad()
    {
        SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
        yield return new WaitForSeconds(0.5f);
        camera.SetActive(false);
        GameSaveManager.instance.Load();
    }

    public void New()
    {
        StartCoroutine(WaitNew());
    }

    public IEnumerator WaitNew()
    {
        SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
        yield return new WaitForSeconds(0.5f);
        camera.SetActive(false);
        GameSceneManager.instance.InitSwitchScene("MainScene", new Vector3(0f, 0f, 0f));
        GameSaveManager.instance.ResetGameStatus();
    }

}

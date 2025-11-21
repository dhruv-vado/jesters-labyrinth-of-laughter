using UnityEngine.SceneManagement;
using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System;

public static class SceneLoader
{
    public enum Scene
    {
        Level,
        Loading,
        MainMenu,
    }

    public static event Action OnLoaderCallback;

    public static void LoadScene(Scene scene)
    {
        //Set loader callback action to load target scene
        OnLoaderCallback = () =>
        {
            SceneManager.LoadScene(scene.ToString());
        };

        //Load loading scene

        SceneManager.LoadScene(Scene.Loading.ToString());
    }

    public static void LoaderCallback()
    {
        //trigger after the first update which lets the scene refresh
        //execute the action the loader callback action which will load the target scene
        if(OnLoaderCallback != null)
        {
            OnLoaderCallback();
            OnLoaderCallback = null;
        }
    }
}

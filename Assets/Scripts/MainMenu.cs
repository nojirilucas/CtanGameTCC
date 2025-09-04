using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    string newGameScene = "SampleScene";

    public AudioClip BackMusic;
    public AudioSource MainChannel;

    void Start()
    {
        MainChannel.PlayOneShot(BackMusic);
    }

    public void StartNewGame()
    {
        MainChannel.Stop();

        SceneManager.LoadScene(newGameScene);
    }
    
    public void ExitApplication()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;

#else

#endif
    }
}

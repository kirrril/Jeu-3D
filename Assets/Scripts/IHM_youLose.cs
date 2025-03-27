using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class IHM_youLose : MonoBehaviour
{
    public static IHM_youLose instance;

    [SerializeField]
    public GameObject panel;


    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        panel.SetActive(false);
    }


    public void PlayAgain()
    {
        SceneManager.LoadScene("GameScene");
    }

    public void Quit()
    {
        #if UNITY_EDITOR

        EditorApplication.isPlaying = false;

        #elif UNITY_STANDALONE
        
        Application.Quit();

        #endif
    }
}

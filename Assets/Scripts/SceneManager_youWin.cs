using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneManager_youWin : MonoBehaviour
{
    [SerializeField]
    AudioSource ambientMusic;

    void Start()
    {
        StartCoroutine(AmbientMusicVolume());
    }

    IEnumerator AmbientMusicVolume()
    {
        ambientMusic.loop = true;
        ambientMusic.Play();

        yield return new WaitForSeconds(6f);

        ambientMusic.volume = 0.1f;
    }
}

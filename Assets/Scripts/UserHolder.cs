using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserHolder : MonoBehaviour
{
    public static UserHolder instance;

    public UserProfile userProfile;

    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);

            return;
        }
        else
        {
            instance = this;
        }
    }

    void Start()
    {
        DontDestroyOnLoad(this.gameObject);

        userProfile = new UserProfile();
    }
}

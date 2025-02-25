using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GirlAnimation : MonoBehaviour
{
    public static GirlAnimation instance;

    [SerializeField]
    Animator girlAnimator;

    [SerializeField]
    GirlPatrol girlPatrol;

    public bool treadmillTraining;
    public bool bikeTraining;
    public bool jumpboxTraining;
    public bool squatsTraining;
    public bool makingSelfie;


    void Awake()
    {
        instance = this;
    }


    void Update()
    {
        if (GirlPatrol.instance.isMoving && !GirlPatrol.instance.isBusy)
        {
            girlAnimator.SetFloat("MovementSpeed", 2.1f);
        }

        if (GirlPatrol.instance.isBusy)
        {
            if (treadmillTraining)
            {
                girlAnimator.SetBool("isJogging", true);
            }

            if (treadmillTraining == false)
            {
                girlAnimator.SetBool("isJogging", false);
            }

            if (bikeTraining)
            {
                Debug.Log($"bikeTraining: {bikeTraining}");

                girlAnimator.SetBool("isCycling", true);
            }

            if (!bikeTraining)
            {
                girlAnimator.SetBool("isCycling", false);
            }

            if (jumpboxTraining)
            {
                girlAnimator.SetBool("isBoxJumping", true);
            }

            if (!jumpboxTraining)
            {
                girlAnimator.SetBool("isBoxJumping", false);
            }

            if (makingSelfie)
            {
                girlAnimator.SetBool("isMakingSelfie", true);
            }

            if (!makingSelfie)
            {
                girlAnimator.SetBool("isMakingSelfie", false);
            }
        }
    }
}

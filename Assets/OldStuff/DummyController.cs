using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DummyController : MonoBehaviour
{
    [SerializeField]
    private GameObject Dummy;

    private Animator dummyAnimator;

    private float moveSpeed = 3.0f;

    void Start()
    {
        dummyAnimator = Dummy.GetComponent<Animator>();
        dummyAnimator.SetBool("isWalking", false);
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.UpArrow))
        {
            Dummy.transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);
            dummyAnimator.SetBool("isWalking", true);
        }
        else
        {
            dummyAnimator.SetBool("isWalking", false);
        }

        if (Input.GetKey(KeyCode.LeftArrow))
        {
            Dummy.transform.Rotate(new Vector3(0, -1, 0), Space.Self);
        }

        if (Input.GetKey(KeyCode.RightArrow))
        {
            Dummy.transform.Rotate(new Vector3(0, 1, 0), Space.Self);
        }

    }
}
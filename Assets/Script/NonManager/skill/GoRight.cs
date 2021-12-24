using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GoRight : MonoBehaviour
{
    public float duraction;
    public float speed;
    public bool isGuide;

    private void Start()
    {
        Destroy(gameObject, duraction);
    }

    private void Update()
    {
        if (isGuide)
        {

        }
        else
        {
            transform.Translate(Vector3.right * speed * Time.deltaTime);
        }
    }
}
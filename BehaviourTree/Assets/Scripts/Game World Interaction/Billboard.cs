using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Billboard : MonoBehaviour
{
    public bool invert;

    private Camera lookAt;

    private void Awake() => lookAt = Camera.main;

    // Update is called once per frame
    private void Update()
    {
        transform.rotation = invert ? Quaternion.LookRotation(transform.position - lookAt.transform.position) : Quaternion.LookRotation(lookAt.transform.position - transform.position);
    }
}
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class ImmunityBase : MonoBehaviour
{
    public float titer = 0;

    public float boostAmount = 0.1f;
    public float decayRate = 0.02f;

    Material material;
    Color originalColor;

    void Start()
    {
        var meshRenderer = this.transform.GetComponentInChildren<MeshRenderer>();
        material = meshRenderer.materials[0];  // bunny tooth + tail color
        originalColor = material.color;
    }

    public void Boost()
    {
        titer += boostAmount;
    }

    // Update is called once per frame
    void Update()
    {
        titer *= (1 - decayRate * Time.deltaTime);

        material.color = Color.HSVToRGB(0.6f, 0.3f, Mathf.Max(0, 1-titer));
    }
}

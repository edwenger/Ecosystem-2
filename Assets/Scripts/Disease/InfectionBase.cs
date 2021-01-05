using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfectionBase : MonoBehaviour
{
    public enum StrainType { VACCINE, WILD };

    public float infectiousness = 0;
    public StrainType strainType;

    public float timeBeforePeak = 3.0f;
    public float timeAfterPeak = 5.0f;
    public float peakInfectiousness = 1.0f;
    public float reversionRate = 1 / 10.0f;

    float infectionTime = 0;
    ImmunityBase immunity;

    Material material;
    Color originalColor;

    void Init()
    {
        strainType = StrainType.VACCINE;
    }

    // Start is called before the first frame update
    void Start()
    {
        var meshRenderer = this.transform.GetComponentInChildren<MeshRenderer>();
        material = meshRenderer.materials[2];  // bunny coat color
        originalColor = material.color;

        immunity = this.GetComponent<ImmunityBase>();
        if (!immunity)
        {
            // give animal an immunity object if this is their first infection
            immunity = this.gameObject.AddComponent<ImmunityBase>();
        }

        immunity.Boost();
    }

    // Update is called once per frame
    void Update()
    {
        infectionTime += Time.deltaTime;

        if (infectionTime > (timeBeforePeak + timeAfterPeak))
        {
            Clear();
            return;
        }
        else if (infectionTime < timeBeforePeak)
        {
            infectiousness = peakInfectiousness * infectionTime / timeBeforePeak;
        }
        else
        {
            float timeSincePeak = infectionTime - timeBeforePeak;
            infectiousness = peakInfectiousness * (1 - timeSincePeak / timeAfterPeak);
        }

        if (strainType == StrainType.VACCINE && UnityEngine.Random.Range(0.0f, 1.0f) < Time.deltaTime * reversionRate)
        {
            //UnityEngine.Debug.Log("Reverting VACCINE to WILD");
            strainType = StrainType.WILD;
        }

        infectiousness *= Mathf.Max(0, 1 - immunity.titer);  // less infectious if more immune

        float r = strainType == StrainType.WILD ? infectiousness : 0;
        float b = strainType == StrainType.VACCINE ? infectiousness : 0;

        material.color = originalColor + new Color(r, 0, b, 0.5f);
    }

    void Clear()
    {
        material.color = originalColor;
        Destroy(this);
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using UnityEngine;

public class Contact : MonoBehaviour
{
    public float contactScale = 0.15f;

    private void OnTriggerStay(Collider other)
    {
        InfectiousContact(other);
    }

    //private void OnTriggerEnter(Collider other)
    //{
    //    InfectiousContact(other);
    //}

    private void InfectiousContact(Collider other)
    {
        // am i infectious?
        InfectionBase myInfection = this.GetComponentInParent<InfectionBase>();
        if (!myInfection)
        {
            return;
        }

        // did i contact an infectable animal?
        Animal otherAnimal = other.GetComponentInParent<Animal>();
        if (!otherAnimal)
        {
            return;
        }

        // do they already have an infection?
        InfectionBase otherInfection = otherAnimal.GetComponent<InfectionBase>();

        // if not, give them one...
        if (!otherInfection)
        {
            // how far apart are we?
            float distance = Vector3.Distance(this.transform.position, other.transform.position);
            float normed_distance = distance / this.GetComponent<SphereCollider>().radius;

            // what's their immunity?
            ImmunityBase otherImmunity = otherAnimal.GetComponent<ImmunityBase>();
            float x_immunity = otherImmunity ? Mathf.Max(0, 1 - otherImmunity.titer) : 1.0f;

            float p_transmit = contactScale * x_immunity * myInfection.infectiousness * (float)Math.Exp(-normed_distance);

            if (UnityEngine.Random.Range(0.0f, 1.0f) < p_transmit)
            {
                //UnityEngine.Debug.Log("Transmitting an infection from distance of " + distance.ToString());
                otherInfection = otherAnimal.gameObject.AddComponent<InfectionBase>();
                otherInfection.strainType = myInfection.strainType;
            }
        }
        else
        {
            // already have an infection...
        }
    }
}

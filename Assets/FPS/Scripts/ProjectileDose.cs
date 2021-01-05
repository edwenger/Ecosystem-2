using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class ProjectileDose : ProjectileStandard
{
    Type hittableType = typeof(Damageable);  // TODO: Doseable (?) if we need the agents to have a component that handles internal logic (analogous to Health and InflictDamage)

    protected override void OnHit(Vector3 point, Vector3 normal, Collider collider)
    {
        // damage
        if (areaOfDamage)
        {
            // area damage
            areaOfDamage.InflictDamageInArea(damage, point, hittableLayers, k_TriggerInteraction, m_ProjectileBase.owner);
        }
        else
        {
            // point damage
            Damageable damageable = collider.GetComponent<Damageable>();
            if (damageable)
            {
                damageable.InflictDamage(damage, false, m_ProjectileBase.owner);

                InfectionBase infection = collider.GetComponentInParent<InfectionBase>();

                if (!infection)
                {
                    Target target = collider.GetComponentInParent<Target>();
                    target.transform.gameObject.AddComponent<InfectionBase>();
                }
                else
                {
                    // Already have an infection...
                }
            }

        }

        // impact vfx
        if (impactVFX)
        {
            GameObject impactVFXInstance = Instantiate(impactVFX, point + (normal * impactVFXSpawnOffset), Quaternion.LookRotation(normal));
            if (impactVFXLifetime > 0)
            {
                Destroy(impactVFXInstance.gameObject, impactVFXLifetime);
            }
        }

        // impact sfx
        if (impactSFXClip)
        {
            AudioUtility.CreateSFX(impactSFXClip, point, AudioUtility.AudioGroups.Impact, 1f, 3f);
        }

        // Self Destruct
        Destroy(this.gameObject);
    }
}

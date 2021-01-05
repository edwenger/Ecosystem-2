using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Health), typeof(Actor))]
public class Target : MonoBehaviour
{
    Health m_Health;

    [Header("Parameters")]
    [Tooltip("The Y height at which the GameObject will be automatically killed (if it falls off of the level)")]
    public float selfDestructYHeight = -20f;
    [Tooltip("Delay after death where the GameObject is destroyed (to allow for animation)")]
    public float deathDuration = 0f;

    // Start is called before the first frame update
    void Start()
    {
        m_Health = GetComponent<Health>();
        DebugUtility.HandleErrorIfNullGetComponent<Health, Target>(m_Health, this, gameObject);

        // Subscribe to damage & death actions
        m_Health.onDie += OnDie;
        m_Health.onDamaged += OnDamaged;
    }

    // Update is called once per frame
    void Update()
    {
        EnsureIsWithinLevelBounds();
    }

    void EnsureIsWithinLevelBounds()
    {
        // at every frame, this tests for conditions to kill the enemy
        if (transform.position.y < selfDestructYHeight)
        {
            Destroy(gameObject);
            return;
        }
    }

    void OnDamaged(float damage, GameObject damageSource)
    {
        // placeholder to pursue damageSource...
    }

    void OnDie()
    {
        // this will call the OnDestroy function
        Destroy(gameObject, deathDuration);
    }
}

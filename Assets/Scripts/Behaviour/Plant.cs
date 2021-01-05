using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Threading;
using UnityEngine;

public class Plant : LivingEntity {
    float amountRemaining = 1;
    float timeBeforeRegrow = 0;

    const float consumeSpeed = 8;
    const float growthSpeed = 0.04f;
    const float regrowDelay = 10;

    public float Consume (float amount) {
        float amountConsumed = Mathf.Max (0, Mathf.Min (amountRemaining, amount));
        amountRemaining -= amount * consumeSpeed;

        transform.localScale = Vector3.one * amountRemaining;

        if (amountRemaining <= 0)
        {
            Die(CauseOfDeath.Eaten);

            //amountRemaining = 0.1f;  // Let's try growing back from roots (after some delay) for more stable baseline population dynamics
            //timeBeforeRegrow = regrowDelay;
        }

        return amountConsumed;
    }

    void Update()
    {
        if (amountRemaining >= 1)
            return;

        if (timeBeforeRegrow > 0)
        {
            timeBeforeRegrow -= Time.deltaTime;
            return;
        }
        else
        {
            timeBeforeRegrow = 0;  // now start regrowing
        }

        // plants can grow back if they're not eaten completely
        amountRemaining += growthSpeed * Time.deltaTime;
        amountRemaining = Mathf.Min(amountRemaining, 1);

        transform.localScale = Vector3.one * amountRemaining;
    }

    public float AmountRemaining {
        get {
            return amountRemaining;
        }
    }
}
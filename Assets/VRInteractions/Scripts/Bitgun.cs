using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bitgun : GrabberObject
{
    public Rigidbody projectilePrefab;
    public Transform barrel;
    public float firingImpulse;
    public float lifespan;

    public override void OnTriggerStart()
    {
        base.OnTriggerStart();

        // Create a new projectile at the barrel of the gun
        var projectile = Instantiate(projectilePrefab, barrel.position, barrel.rotation);

        // Fire the projectile
        projectile.AddForce(barrel.forward * firingImpulse);

        // Destroy the projectile after its lifespan has ended
        Destroy(projectile.gameObject, lifespan);
    }
}

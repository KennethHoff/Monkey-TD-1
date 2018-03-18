using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tower { 

    public class TackShooter : NovaTower {

        protected override void Start() {
            base.Start();
        }

        [SerializeField, Space(10)]
        protected int tacksToFire;

        protected override void Shoot() {
            for (int i = 0; i < tacksToFire; i++) {
                float rotationAdjustment = (360 / tacksToFire) * i;
                Quaternion newRot = Quaternion.Euler(0, 0, rotationAdjustment);

                Projectile.StandardProjectile shotProjectile = CreateProjectile(projectile, transform.position, transform.rotation, transform.parent);

                shotProjectile.transform.rotation = newRot;
                shotProjectile.despawnDistance = firingRange * 1.2f;
            }
            Debug.Log("Tack Shooter Shot!");
            base.Shoot();
        }
    }

}
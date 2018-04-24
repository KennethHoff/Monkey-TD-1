using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bloon {
    public class LeadBloon : StandardBloon {

        [SerializeField, Header("Lead Bloon specifics:")]
        private AudioClip metalHitAudioClip;

        protected override void Start() {
            base.Start();
        }

        private void MetalHitBySharpObject() {
            audioSource.clip = metalHitAudioClip;
            audioSource.Play();
            Debug.Log("plink...");
        }
        protected override void CollidedWithProjectile(Projectile.StandardProjectile projectile) {
            base.CollidedWithProjectile(projectile);

            if (!Damageable(projectile.damageType))
                MetalHitBySharpObject();
        }
    }
}


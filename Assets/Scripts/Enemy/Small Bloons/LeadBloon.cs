using System.Collections;
using System.Collections.Generic;
using Projectile;
using Tower;
using UnityEngine;

namespace Bloon {
    public class LeadBloon : StandardBloon {

        [SerializeField, Header("Lead Bloon specifics:")]
        private AudioClip metalHitAudioClip;

        protected override void Start() {
            bloonEnum = GameControl.BloonSpawner.Bloons.LeadBloon;
            base.Start();
        }

        private void MetalHitBySharpObject() {
            audioSource.clip = metalHitAudioClip;
            audioSource.Play();
            Debug.Log("plink...");
        }
        protected override void CollidedWithProjectile(StandardProjectile projectile) {
            base.CollidedWithProjectile(projectile);

            MetalHitBySharpObject();
        }
    }
}


using System.Collections;
using System.Collections.Generic;
using Tower;
using UnityEngine;

namespace Bloon {
    public class LeadBloon : StandardBloon {

        [SerializeField, Header("Lead Bloon specifics:")]
        private AudioClip metalHitAudioClip;

        protected override void Start() {
            bloonEnum = GameControl.BloonSpawner.Bloons.LeadBloon;
            amountOfBloonsToSpawn = 1;
            RBE = 23;
            base.Start();
        }

        public void MetalHitBySharpObject(AudioSource audioSource) {
            audioSource.clip = metalHitAudioClip;
            audioSource.Play();
        }

        public override void PopBloonGameObject(StandardTower _tower, ParentController _projectileParent, int _penetration) {
            base.PopBloonGameObject(_tower, _projectileParent, _penetration);

            GetComponent<LeadBloon>().MetalHitBySharpObject(audioSource);
        }
    }
}


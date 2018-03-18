using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bloon {
    public class LeadBloon : StandardBloon {

        [SerializeField]
        private AudioClip metalHitAudioClip;

        protected override void Start() {
            bloonEnum = GameControl.BloonSpawner.Bloons.LeadBloon;
            bloonChildToSpawn = GameControl.BloonSpawner.Bloons.BlackBloon;
            base.Start();
        }

        public void MetalHitBySharpObject(AudioSource audioSource) {
            audioSource.clip = metalHitAudioClip;
            audioSource.Play();
        }

        protected override void OnPopped(Tower.StandardTower tower, GameObject projectileObject) {
            base.OnPopped(tower, projectileObject);
            
            GetComponent<LeadBloon>().MetalHitBySharpObject(audioSource);
        }
    }
}


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tower {

    public class SniperMonkey : MonkeyTower {

        protected override void Start() {
            base.Start();
        }

        protected override void Shoot() {
            generalStats.target.GetComponent<Bloon.StandardBloon>().HitScanShot(this);
            Debug.Log("Sniper Monkey #" + GetInstanceID() + " shot!");

            base.Shoot();
        }
        
    }
}
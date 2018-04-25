using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tower {

    public class SniperMonkey : MonkeyTower {
        public override T GetStats<T>() {
            return sniperMonkeyTowerStats as T;
        }
        public SniperMonkeyTowerStats sniperMonkeyTowerStats = new SniperMonkeyTowerStats();

        protected override void Start() {
            sniperMonkeyTowerStats = new SniperMonkeyTowerStats();
            Debug.Log(sniperMonkeyTowerStats.attackSpeed);
            base.Start();
        }

        protected override void Shoot() {
            GetStats<Tower.SniperMonkeyTowerStats>().target.GetComponent<Bloon.StandardBloon>().HitScanShot(this);
            Debug.Log("Sniper Monkey #" + GetInstanceID() + " shot!");

            base.Shoot();
        }
        
    }
}
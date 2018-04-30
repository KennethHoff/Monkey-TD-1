using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tower;

namespace Tower {

    public class SniperMonkey : MonkeyTower {
        public override T GetStats<T>() {
            return stats as T;
        }
        public SniperMonkeyTowerStats stats = new SniperMonkeyTowerStats();

        protected override void Start() {
            stats = new SniperMonkeyTowerStats();
            base.Start();

            stats.AddPowerup(new Powerups.SniperMonkey.Full_Metal_Jacket(stats));
            stats.AddPowerup(new Powerups.SniperMonkey.Point_Five_Oh(stats));
            stats.AddPowerup(new Powerups.SniperMonkey.Deadly_Precision(stats));
            stats.AddPowerup(new Powerups.SniperMonkey.Cripple_MOAB(stats));

            stats.AddPowerup(new Powerups.SniperMonkey.Faster_Firing(stats));
            stats.AddPowerup(new Powerups.SniperMonkey.Night_Vision_Goggles(stats));
            stats.AddPowerup(new Powerups.SniperMonkey.Semi_Automatic_Rifle(stats));
            stats.AddPowerup(new Powerups.SniperMonkey.Supply_Drop(stats));

        }

        protected override void Shoot() {
            var stats = GetStats<Tower.SniperMonkeyTowerStats>();
            var targetBloon = stats.target.GetComponent<Bloon.StandardBloon>();
            targetBloon.HitScanShot(this);
            Debug.Log("Sniper Monkey #" + GetInstanceID() + " shot!");

            base.Shoot();
        }
        
    }
}


namespace Powerups.SniperMonkey {

    #region Left:
    class Full_Metal_Jacket : Powerups.PowerupBase<SniperMonkeyTowerStats> {
        public Full_Metal_Jacket(SniperMonkeyTowerStats _tower) : base(_tower, GameControl.DictionaryController.TowerUpgrades.Sniper_Monkey_Full_Metal_Jacket) { }

        public override void Powerup() {
            var penetrationToAdd = 2;
            this.Tower.penetration += penetrationToAdd;
            this.Tower.damageType = GameControl.GameController.DamageTypes.Both;
            Debug.Log("Tower can now damage Lead Bloons, and can now pierce " + this.Tower.penetration + " layers of bloons. ");
        }
    }

    class Point_Five_Oh : Powerups.PowerupBase<SniperMonkeyTowerStats> {

        public Point_Five_Oh(SniperMonkeyTowerStats _tower) : base(_tower, GameControl.DictionaryController.TowerUpgrades.Sniper_Monkey_Point_Five_Oh) { }

        public override void Powerup() {
            var penetrationToAdd = 3;
            this.Tower.penetration += penetrationToAdd;
            Debug.Log("Tower can now pierce " + this.Tower.penetration + " layers of bloons. ");
        }
    }



    class Deadly_Precision : Powerups.PowerupBase<SniperMonkeyTowerStats> {

        public Deadly_Precision(SniperMonkeyTowerStats _tower) : base(_tower, GameControl.DictionaryController.TowerUpgrades.Sniper_Monkey_Deadly_Precision) { }

        public override void Powerup() {
            Debug.LogWarning("Does nothing..");
        }
    }


    class Cripple_MOAB : Powerups.PowerupBase<SniperMonkeyTowerStats> {

        public Cripple_MOAB(SniperMonkeyTowerStats _tower) : base(_tower, GameControl.DictionaryController.TowerUpgrades.Sniper_Monkey_Cripple_MOAB) { }

        public override void Powerup() {
            Debug.LogWarning("Does nothing..");
        }
    }

    #endregion

    #region Right:
    class Faster_Firing : Powerups.PowerupBase<SniperMonkeyTowerStats> {
        public Faster_Firing(SniperMonkeyTowerStats _tower) : base(_tower, GameControl.DictionaryController.TowerUpgrades.Sniper_Monkey_Faster_Firing) { }

        public override void Powerup() {
            var speedToAdd = this.Tower.attackSpeed * 1/4;
            this.Tower.attackSpeed -= speedToAdd;
            Debug.LogWarning("Attack speed increased by " + speedToAdd + ". Now at " + this.Tower.attackSpeed);
        }
    }

    class Night_Vision_Goggles : Powerups.PowerupBase<SniperMonkeyTowerStats> {
        public Night_Vision_Goggles(SniperMonkeyTowerStats _tower) : base(_tower, GameControl.DictionaryController.TowerUpgrades.Sniper_Monkey_Night_Vision_Goggles) { }

        public override void Powerup() {
            this.Tower.CamoDetection = true;
            Debug.Log("Can now detect Camo.");
        }
    }

    class Semi_Automatic_Rifle : Powerups.PowerupBase<SniperMonkeyTowerStats> {
        public Semi_Automatic_Rifle(SniperMonkeyTowerStats _tower) : base(_tower, GameControl.DictionaryController.TowerUpgrades.Sniper_Monkey_Semi_Automatic_Rifle) { }

        public override void Powerup() {
            Debug.LogWarning("Does nothing..");
        }
    }
    class Supply_Drop : Powerups.PowerupBase<SniperMonkeyTowerStats> {
        public Supply_Drop(SniperMonkeyTowerStats _tower) : base(_tower, GameControl.DictionaryController.TowerUpgrades.Sniper_Monkey_Supply_Drop) { }

        public override void Powerup() {
            Debug.LogWarning("Does nothing..");
        }
    }
    #endregion
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tower;

namespace Tower {

    public class DartMonkey : MonkeyTower {
        public override T GetStats<T>() {
            return dartMonkeyTowerStats as T;
        }
        public DartMonkeyTowerStats dartMonkeyTowerStats = new DartMonkeyTowerStats();

        protected override void Start() {
            dartMonkeyTowerStats = new DartMonkeyTowerStats();
            base.Start();

            dartMonkeyTowerStats.AddPowerup(new Powerups.DartMonkey.Long_Range_Darts(dartMonkeyTowerStats));
            dartMonkeyTowerStats.AddPowerup(new Powerups.DartMonkey.Enhanced_Eyesight(dartMonkeyTowerStats));
            dartMonkeyTowerStats.AddPowerup(new Powerups.DartMonkey.Spike_O_Pult(dartMonkeyTowerStats));
            dartMonkeyTowerStats.AddPowerup(new Powerups.DartMonkey.Juggernaut(dartMonkeyTowerStats));

            dartMonkeyTowerStats.AddPowerup(new Powerups.DartMonkey.Sharp_Shots(dartMonkeyTowerStats));
            dartMonkeyTowerStats.AddPowerup(new Powerups.DartMonkey.Razor_Sharp_Shots(dartMonkeyTowerStats));
            dartMonkeyTowerStats.AddPowerup(new Powerups.DartMonkey.Triple_Darts(dartMonkeyTowerStats));
            dartMonkeyTowerStats.AddPowerup(new Powerups.DartMonkey.Super_Monkey_Fan_Club(dartMonkeyTowerStats));
            
        }

        protected override void Shoot() {
            List<Projectile.StandardProjectile> shotProjectileList = CreateProjectileFamilyTree(GameControl.DictionaryController.RetrieveProjectileFromProjectileDictionary_Enum(GetStats<Tower.BaseTowerStats>().projectileEnum).projectileObject, transform.position, transform.rotation);

            foreach (Projectile.StandardProjectile projectile in shotProjectileList) {
                projectile.despawnDistance = GetStats<Tower.BaseTowerStats>().firingRange * 10f;
            }

            Debug.Log("Dart Monkey #" + GetInstanceID() + " shot!");
            base.Shoot();
        }
    }
}

namespace Powerups.DartMonkey {

    #region Left:
    class Long_Range_Darts : Powerups.PowerupBase<DartMonkeyTowerStats> {
        public Long_Range_Darts(DartMonkeyTowerStats _tower) : base(_tower, GameControl.DictionaryController.TowerUpgrades.Monkey_Dart_Long_Range_Darts) { }

        public override void Powerup() {
            var rangeToAdd = 1;
            this.Tower.firingRange += rangeToAdd;
            Debug.Log("Range increased by " + rangeToAdd + ". Now at " + this.Tower.firingRange);
        }
    }

    class Enhanced_Eyesight : Powerups.PowerupBase<DartMonkeyTowerStats> {

        public Enhanced_Eyesight(DartMonkeyTowerStats _tower) : base(_tower, GameControl.DictionaryController.TowerUpgrades.Monkey_Dart_Enhanced_Eyesight) { }

        public override void Powerup() {
            var rangeToAdd = 1;
            this.Tower.CamoDetection = true;
            this.Tower.firingRange += rangeToAdd;
            Debug.Log("Can now detect camo : Range increased by " + rangeToAdd + ". Now at " + this.Tower.firingRange);
        }
    }

    

    class Spike_O_Pult : Powerups.PowerupBase<DartMonkeyTowerStats> {

        public Spike_O_Pult(DartMonkeyTowerStats _tower) : base(_tower, GameControl.DictionaryController.TowerUpgrades.Monkey_Dart_Spike_O_Pult) { }

        public override void Powerup() {
            Debug.LogWarning("Does nothing..");
            // GameControl.DictionaryController.RetrieveTowerSpriteFromTowerSpriteDictionary_Enum(this.Tower.towerEnum).currentTowerHUDIconSprite = 1;
        }
    }
    

    class Juggernaut : Powerups.PowerupBase<DartMonkeyTowerStats> {

        public Juggernaut(DartMonkeyTowerStats _tower) : base(_tower, GameControl.DictionaryController.TowerUpgrades.Monkey_Dart_Juggernaut) { }

        public override void Powerup() {
            Debug.LogWarning("Does nothing..");
        }
    }

    #endregion

    #region Right:
    class Sharp_Shots : Powerups.PowerupBase<DartMonkeyTowerStats> {
        public Sharp_Shots(DartMonkeyTowerStats _tower) : base(_tower, GameControl.DictionaryController.TowerUpgrades.Monkey_Dart_Sharp_Shots) { }

        public override void Powerup() {
            var poppingPowerToAdd = 1;
            this.Tower.poppingPower += poppingPowerToAdd;
            Debug.Log("Popping Power increased by " + poppingPowerToAdd + ". Now at " + this.Tower.poppingPower);
        }
    }

    class Razor_Sharp_Shots : Powerups.PowerupBase<DartMonkeyTowerStats> {
        public Razor_Sharp_Shots(DartMonkeyTowerStats _tower) : base(_tower, GameControl.DictionaryController.TowerUpgrades.Monkey_Dart_Razor_Sharp_Shots) { }

        public override void Powerup() {
            var poppingPowerToAdd = 2;
            this.Tower.poppingPower += poppingPowerToAdd;
            Debug.Log("Popping Power increased by " + poppingPowerToAdd + ". Now at " + this.Tower.poppingPower);
        }
    }
    
    class Triple_Darts : Powerups.PowerupBase<DartMonkeyTowerStats> {
        public Triple_Darts(DartMonkeyTowerStats _tower) : base(_tower, GameControl.DictionaryController.TowerUpgrades.Monkey_Dart_Triple_Darts) { }

        public override void Powerup() {
            Debug.LogWarning("Does nothing..");
        }
    }
    class Super_Monkey_Fan_Club : Powerups.PowerupBase<DartMonkeyTowerStats> {
        public Super_Monkey_Fan_Club(DartMonkeyTowerStats _tower) : base(_tower, GameControl.DictionaryController.TowerUpgrades.Monkey_Dart_Super_Monkey_Fan_Club) { }

        public override void Powerup() {
            Debug.LogWarning("Does nothing..");
        }
    }
    #endregion



}


/*
 * TODO: THIS IS HOW TO ADD AN UPGRADE
class XXX : Powerups.PowerupBase<YYY> {
    public XXX(YYY _tower) : base (_tower, GameControl.DictionaryController.TowerUpgrades.Undefined) { }

    public override void Powerup() {
        // DO STUFF
    }

    // Also remember to:
    // * Add it to the list of upgrades for the Towers (add the list if required) in the "TowerUpgrades" script.
    // * Create a "SetUpgradePath" (and add all upgrades there) in the "TowerStats" for that tower. (And make it sure it is running in the "OnStart" method in TowerStats of that tower as well.
    // * Add item to the dictionary.
    // 
    // ^Add more to this guide if I forgot something. (probably did.)
}

*/

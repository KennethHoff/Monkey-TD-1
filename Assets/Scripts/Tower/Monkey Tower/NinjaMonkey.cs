using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tower;

namespace Tower {

    public class NinjaMonkey : MonkeyTower {
        public override T GetStats<T>() {
            return ninjaMonkeyTowerStats as T;
        }
        public NinjaMonkeyTowerStats ninjaMonkeyTowerStats = new NinjaMonkeyTowerStats();

        protected override void Start() {
            ninjaMonkeyTowerStats = new NinjaMonkeyTowerStats();
            base.Start();

            ninjaMonkeyTowerStats.AddPowerup(new Powerups.NinjaMonkey.Ninja_Discipline(ninjaMonkeyTowerStats));
            ninjaMonkeyTowerStats.AddPowerup(new Powerups.NinjaMonkey.Sharp_Shurikens(ninjaMonkeyTowerStats));
            ninjaMonkeyTowerStats.AddPowerup(new Powerups.NinjaMonkey.Double_Shot(ninjaMonkeyTowerStats));
            ninjaMonkeyTowerStats.AddPowerup(new Powerups.NinjaMonkey.Bloonjitsu(ninjaMonkeyTowerStats));

            ninjaMonkeyTowerStats.AddPowerup(new Powerups.NinjaMonkey.Seeking_Shuriken(ninjaMonkeyTowerStats));
            ninjaMonkeyTowerStats.AddPowerup(new Powerups.NinjaMonkey.Distraction(ninjaMonkeyTowerStats));
            ninjaMonkeyTowerStats.AddPowerup(new Powerups.NinjaMonkey.Flash_Bomb(ninjaMonkeyTowerStats));
            ninjaMonkeyTowerStats.AddPowerup(new Powerups.NinjaMonkey.Sabotage_Supply_Lines(ninjaMonkeyTowerStats));

        }

        protected override void Shoot() {

            List<Projectile.StandardProjectile> shotProjectileList = CreateProjectileFamilyTree(GetStats<Tower.BaseTowerStats>().projectileObject, transform.position, transform.rotation);

            foreach (Projectile.StandardProjectile projectile in shotProjectileList) {
                projectile.despawnDistance = GetStats<Tower.BaseTowerStats>().firingRange * 10f;
            }

            Debug.Log("Ninja Monkey #" + GetInstanceID() + " shot!");
            base.Shoot();
        }
    }
}

namespace Powerups.NinjaMonkey {

    #region Left:
    class Ninja_Discipline : Powerups.PowerupBase<NinjaMonkeyTowerStats> {
        public Ninja_Discipline(NinjaMonkeyTowerStats _tower) : base(_tower, GameControl.DictionaryController.TowerUpgrades.Ninja_Monkey_Ninja_Discipline) { }

        public override void Powerup() {
            var speedToAdd = this.Tower.attackSpeed * 1 / 4;
        }
    }

    class Sharp_Shurikens : Powerups.PowerupBase<NinjaMonkeyTowerStats> {

        public Sharp_Shurikens(NinjaMonkeyTowerStats _tower) : base(_tower, GameControl.DictionaryController.TowerUpgrades.Ninja_Monkey_Sharp_Shuriken) { }

        public override void Powerup() {
            Debug.LogWarning("Does nothing..");
        }
    }



    class Double_Shot : Powerups.PowerupBase<NinjaMonkeyTowerStats> {

        public Double_Shot(NinjaMonkeyTowerStats _tower) : base(_tower, GameControl.DictionaryController.TowerUpgrades.Ninja_Monkey_Double_Shot) { }

        public override void Powerup() {
            Debug.LogWarning("Does nothing..");
        }
    }


    class Bloonjitsu : Powerups.PowerupBase<NinjaMonkeyTowerStats> {

        public Bloonjitsu(NinjaMonkeyTowerStats _tower) : base(_tower, GameControl.DictionaryController.TowerUpgrades.Ninja_Monkey_Bloonjitsu) { }

        public override void Powerup() {
            Debug.LogWarning("Does nothing..");
        }
    }

    #endregion

    #region Right:
    class Seeking_Shuriken : Powerups.PowerupBase<NinjaMonkeyTowerStats> {
        public Seeking_Shuriken(NinjaMonkeyTowerStats _tower) : base(_tower, GameControl.DictionaryController.TowerUpgrades.Ninja_Monkey_Seeking_Shuriken) { }

        public override void Powerup() {
            Debug.LogWarning("Does nothing..");
        }
    }

    class Distraction : Powerups.PowerupBase<NinjaMonkeyTowerStats> {
        public Distraction(NinjaMonkeyTowerStats _tower) : base(_tower, GameControl.DictionaryController.TowerUpgrades.Ninja_Monkey_Distraction) { }

        public override void Powerup() {
            Debug.LogWarning("Does nothing..");
        }
    }

    class Flash_Bomb : Powerups.PowerupBase<NinjaMonkeyTowerStats> {
        public Flash_Bomb(NinjaMonkeyTowerStats _tower) : base(_tower, GameControl.DictionaryController.TowerUpgrades.Ninja_Monkey_Flash_Bomb) { }

        public override void Powerup() {
            Debug.LogWarning("Does nothing..");
        }
    }
    class Sabotage_Supply_Lines : Powerups.PowerupBase<NinjaMonkeyTowerStats> {
        public Sabotage_Supply_Lines(NinjaMonkeyTowerStats _tower) : base(_tower, GameControl.DictionaryController.TowerUpgrades.Ninja_Monkey_Sabotage_Supply_Lines) { }

        public override void Powerup() {
            Debug.LogWarning("Does nothing..");
        }
    }
    #endregion
}
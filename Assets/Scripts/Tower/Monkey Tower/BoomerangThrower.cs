using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tower;

namespace Tower {
    public class BoomerangThrower : MonkeyTower {
        public override T GetStats<T>() {
            return boomerangThrowerTowerStats as T;
        }
        public BoomerangThrowerTowerStats boomerangThrowerTowerStats = new BoomerangThrowerTowerStats();

        protected override void Start() {
            boomerangThrowerTowerStats = new BoomerangThrowerTowerStats();
            Debug.Log(boomerangThrowerTowerStats.attackSpeed);
            base.Start();

            boomerangThrowerTowerStats.AddPowerup(new Powerups.BoomerangThrower.Multi_Target(boomerangThrowerTowerStats));
            boomerangThrowerTowerStats.AddPowerup(new Powerups.BoomerangThrower.Glaive_Thrower(boomerangThrowerTowerStats));
            boomerangThrowerTowerStats.AddPowerup(new Powerups.BoomerangThrower.Glaive_Ricochet(boomerangThrowerTowerStats));
            boomerangThrowerTowerStats.AddPowerup(new Powerups.BoomerangThrower.Glaive_Lord(boomerangThrowerTowerStats));

            boomerangThrowerTowerStats.AddPowerup(new Powerups.BoomerangThrower.Sonic_Boom(boomerangThrowerTowerStats));
            boomerangThrowerTowerStats.AddPowerup(new Powerups.BoomerangThrower.Red_Hot_Rangs(boomerangThrowerTowerStats));
            boomerangThrowerTowerStats.AddPowerup(new Powerups.BoomerangThrower.Bionic_Boomer(boomerangThrowerTowerStats));
            boomerangThrowerTowerStats.AddPowerup(new Powerups.BoomerangThrower.Turbo_Charge(boomerangThrowerTowerStats));
        }
        public int numberOfSpins = 1;

        protected override void Shoot() {

            if (!(boomerangThrowerTowerStats.projectileObject is Projectile.Boomerang)) {
                Debug.LogError("Not shooting a boomerang!");
                return;
            }
            List<Projectile.StandardProjectile> shotProjectileList = CreateProjectileFamilyTree(GetStats<Tower.BaseTowerStats>().projectileObject, transform.position, transform.rotation);

            foreach (Projectile.Boomerang projectile in shotProjectileList) {
                projectile.despawnDistance = GetStats<Tower.BaseTowerStats>().firingRange * 0.8f;
                projectile.totalSpins = numberOfSpins;
            }

            Debug.Log("Boomerang Thrower #" + GetInstanceID() + " shot!");
            base.Shoot();
        }
    }
}


namespace Powerups.BoomerangThrower {

    #region Left:
    class Multi_Target : Powerups.PowerupBase<BoomerangThrowerTowerStats> {
        public Multi_Target(BoomerangThrowerTowerStats _tower) : base(_tower, GameControl.DictionaryController.TowerUpgrades.Boomerang_Thrower_Multi_Target) { }

        public override void Powerup() {
            var poppingPowerToAdd = 4;
            this.Tower.poppingPower += poppingPowerToAdd;
        }
    }

    class Glaive_Thrower : Powerups.PowerupBase<BoomerangThrowerTowerStats> {

        public Glaive_Thrower(BoomerangThrowerTowerStats _tower) : base(_tower, GameControl.DictionaryController.TowerUpgrades.Boomerang_Thrower_Glaive_Thrower) { }

        public override void Powerup() {
            var poppingPowerToAdd = 5;
            this.Tower.poppingPower += poppingPowerToAdd;
        }
    }



    class Glaive_Ricochet : Powerups.PowerupBase<BoomerangThrowerTowerStats> {

        public Glaive_Ricochet(BoomerangThrowerTowerStats _tower) : base(_tower, GameControl.DictionaryController.TowerUpgrades.Boomerang_Thrower_Glaive_Ricochet) { }

        public override void Powerup() {
            Debug.LogWarning("Does nothing..");
        }
    }


    class Glaive_Lord : Powerups.PowerupBase<BoomerangThrowerTowerStats> {

        public Glaive_Lord(BoomerangThrowerTowerStats _tower) : base(_tower, GameControl.DictionaryController.TowerUpgrades.Boomerang_Thrower_Glaive_Lord) { }

        public override void Powerup() {
            Debug.LogWarning("Does nothing..");
        }
    }

    #endregion

    #region Right:
    class Sonic_Boom : Powerups.PowerupBase<BoomerangThrowerTowerStats> {
        public Sonic_Boom(BoomerangThrowerTowerStats _tower) : base(_tower, GameControl.DictionaryController.TowerUpgrades.Boomerang_Thrower_Sonic_Boom) { }

        public override void Powerup() {
            Debug.LogWarning("Does nothing..");
        }
    }

    class Red_Hot_Rangs : Powerups.PowerupBase<BoomerangThrowerTowerStats> {
        public Red_Hot_Rangs(BoomerangThrowerTowerStats _tower) : base(_tower, GameControl.DictionaryController.TowerUpgrades.Boomerang_Thrower_Red_Hot_Rangs) { }

        public override void Powerup() {
            this.Tower.damageType = GameControl.GameController.DamageTypes.Both;
        }
    }

    class Bionic_Boomer : Powerups.PowerupBase<BoomerangThrowerTowerStats> {
        public Bionic_Boomer(BoomerangThrowerTowerStats _tower) : base(_tower, GameControl.DictionaryController.TowerUpgrades.Boomerang_Thrower_Bionic_Boomer) { }

        public override void Powerup() {
            Debug.LogWarning("Does nothing..");
        }
    }
    class Turbo_Charge : Powerups.PowerupBase<BoomerangThrowerTowerStats> {
        public Turbo_Charge(BoomerangThrowerTowerStats _tower) : base(_tower, GameControl.DictionaryController.TowerUpgrades.Boomerang_Thrower_Turbo_Charge) { }

        public override void Powerup() {
            Debug.LogWarning("Does nothing..");
        }
    }
    #endregion



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
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tower;

namespace Tower { 

    public class TackShooter : NovaTower {
        public override T GetStats<T>() {
            return tackShooterTowerStats as T;
        }
        public TackShooterTowerStats tackShooterTowerStats = new TackShooterTowerStats();

        protected override void Start() {
            tackShooterTowerStats = new TackShooterTowerStats();
            base.Start();

            tackShooterTowerStats.AddPowerup(new Powerups.TackShooter.Faster_Shooting(tackShooterTowerStats));
            tackShooterTowerStats.AddPowerup(new Powerups.TackShooter.Even_Faster_Shooting(tackShooterTowerStats));
            tackShooterTowerStats.AddPowerup(new Powerups.TackShooter.Tack_Sprayer(tackShooterTowerStats));
            tackShooterTowerStats.AddPowerup(new Powerups.TackShooter.Ring_Of_Fire(tackShooterTowerStats));

            tackShooterTowerStats.AddPowerup(new Powerups.TackShooter.Extra_Range_Tacks(tackShooterTowerStats));
            tackShooterTowerStats.AddPowerup(new Powerups.TackShooter.Super_Range_Tacks(tackShooterTowerStats));
            tackShooterTowerStats.AddPowerup(new Powerups.TackShooter.Blade_Shooter(tackShooterTowerStats));
            tackShooterTowerStats.AddPowerup(new Powerups.TackShooter.Blade_Maelstrom(tackShooterTowerStats));

        }

        protected override void Shoot() {

            List<Projectile.StandardProjectile> shotProjectileList = CreateProjectileFamilyTree(GetStats<Tower.BaseTowerStats>().projectileObject, transform.position, transform.rotation);

            foreach (Projectile.StandardProjectile projectile in shotProjectileList) {
                projectile.despawnDistance = GetStats<Tower.BaseTowerStats>().firingRange * 1.2f;
            }

            Debug.Log("Tack Shooter Shot!");
            base.Shoot();
        }
    }

}

namespace Powerups.TackShooter {

    #region Left:
    class Faster_Shooting : Powerups.PowerupBase<TackShooterTowerStats> {
        public Faster_Shooting(TackShooterTowerStats _tower) : base(_tower, GameControl.DictionaryController.TowerUpgrades.Tack_Shooter_Faster_Shooting) { }

        public override void Powerup() {
            var speedToAdd = this.Tower.attackSpeed * 1/4;
            this.Tower.attackSpeed -= speedToAdd;
            Debug.Log("Firing Speed increased by " + speedToAdd + ". Now at " + this.Tower.attackSpeed);
        }
    }

    class Even_Faster_Shooting : Powerups.PowerupBase<TackShooterTowerStats> {

        public Even_Faster_Shooting(TackShooterTowerStats _tower) : base(_tower, GameControl.DictionaryController.TowerUpgrades.Tack_Shooter_Even_Faster_Shooting) { }

        public override void Powerup() {
            var speedToAdd = this.Tower.attackSpeed * 1 / 4;
            this.Tower.attackSpeed -= speedToAdd;
            Debug.Log("Firing Speed increased by " + speedToAdd + ". Now at " + this.Tower.attackSpeed);
        }
    }



    class Tack_Sprayer : Powerups.PowerupBase<TackShooterTowerStats> {

        public Tack_Sprayer(TackShooterTowerStats _tower) : base(_tower, GameControl.DictionaryController.TowerUpgrades.Tack_Shooter_Tack_Sprayer) { }

        public override void Powerup() {
            Debug.LogWarning("Does nothing..");
            // GameControl.DictionaryController.RetrieveTowerSpriteFromTowerSpriteDictionary_Enum(this.Tower.towerEnum).currentTowerHUDIconSprite = 1;
        }
    }


    class Ring_Of_Fire : Powerups.PowerupBase<TackShooterTowerStats> {

        public Ring_Of_Fire(TackShooterTowerStats _tower) : base(_tower, GameControl.DictionaryController.TowerUpgrades.Tack_Shooter_Ring_Of_Fire) { }

        public override void Powerup() {
            Debug.LogWarning("Does nothing..");
        }
    }

    #endregion

    #region Right:
    class Extra_Range_Tacks : Powerups.PowerupBase<TackShooterTowerStats> {
        public Extra_Range_Tacks(TackShooterTowerStats _tower) : base(_tower, GameControl.DictionaryController.TowerUpgrades.Tack_Shooter_Extra_Range_Tacks) { }

        public override void Powerup() {
            var rangeToAdd = 1;
            this.Tower.firingRange += rangeToAdd;
            Debug.Log("Range increased increased by " + rangeToAdd + ". Now at " + this.Tower.firingRange);
        }
    }

    class Super_Range_Tacks : Powerups.PowerupBase<TackShooterTowerStats> {
        public Super_Range_Tacks(TackShooterTowerStats _tower) : base(_tower, GameControl.DictionaryController.TowerUpgrades.Tack_Shooter_Super_Range_Tacks) { }

        public override void Powerup() {
            var rangeToAdd = 1;
            this.Tower.firingRange += rangeToAdd;
            Debug.Log("Range increased increased by " + rangeToAdd + ". Now at " + this.Tower.firingRange);
        }
    }

    class Blade_Shooter : Powerups.PowerupBase<TackShooterTowerStats> {
        public Blade_Shooter(TackShooterTowerStats _tower) : base(_tower, GameControl.DictionaryController.TowerUpgrades.Tack_Shooter_Blade_Shooter) { }

        public override void Powerup() {
            Debug.LogWarning("Does nothing..");
        }
    }
    class Blade_Maelstrom : Powerups.PowerupBase<TackShooterTowerStats> {
        public Blade_Maelstrom(TackShooterTowerStats _tower) : base(_tower, GameControl.DictionaryController.TowerUpgrades.Tack_Shooter_Blade_Maelstrom) { }

        public override void Powerup() {
            Debug.LogWarning("Does nothing..");
        }
    }
}
    #endregion
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tower {
    public class StandardTower : MonoBehaviour {

        public BaseTowerStats generalStats;

        public virtual T GetStats<T>() where T : BaseTowerStats {
            if (generalStats == null) {
                generalStats = new BaseTowerStats();
            }
            return generalStats as T;
        }

        protected virtual void Start() {

            GetStats<BaseTowerStats>().OnStart();


            foreach (Transform child in transform) {
                GetStats<BaseTowerStats>().projectileSpawnPoints.Add(child);
            }

            GetStats<BaseTowerStats>().sellValueModifier = 0.8f;
            GetStats<BaseTowerStats>().priceModifier = 1;
            GetStats<BaseTowerStats>().sellValue = GetStats<BaseTowerStats>().goldCost * 0.8f;
            ChangePop(0, false);

            GetStats<BaseTowerStats>().firingCooldown = 0;
        }

        private void OnMouseEnter() {
            GetComponent<SpriteRenderer>().color = Color.blue;
            GameControl.GameController.controllerObject.towerUnderCursor = this;
        }

        private void OnMouseExit() {
            GetComponent<SpriteRenderer>().color = Color.white;
            if (GameControl.GameController.controllerObject.towerUnderCursor == this) {
                GameControl.GameController.controllerObject.towerUnderCursor = null;
            }
        }

        protected virtual void FixedUpdate() {
            // Barebones implementation -- Override at your leasure
            // Everything else depends on the Tower. (Nova Towers do not aim, therefore all of that is unnecessary. 
            // The only thing that is universal for every kind of tower, is that all of them does something on a regular basis ("firing cooldown"), be it shooting, freezing etc..
            // ... Except for Monkey Village, which is just a Area-of-Effect Buff (will do that if/when I get there..)

            if (GameControl.WaveSpawner.controllerObject.waveActive) {
                GetStats<BaseTowerStats>().firingCooldown -= Time.fixedDeltaTime;
            }
            else GetStats<BaseTowerStats>().firingCooldown = -1;
        }

        protected virtual Collider2D[] GetEnemiesInRange() {
            if (GetStats<BaseTowerStats>().firingRange != -1)
                return Physics2D.OverlapCircleAll(transform.position, GetStats<BaseTowerStats>().EffectiveFiringRange, GameControl.PlacementController.controllerObject.enemyLayer);
            else return Physics2D.OverlapBoxAll(transform.position, Vector2.one * 100, GameControl.PlacementController.controllerObject.enemyLayer);
        }

        protected virtual Collider2D[] GetVisibleEnemiesInRange() {
            var colls = GetEnemiesInRange();
            
            if (GetStats<BaseTowerStats>().CamoDetection == true) {
                return colls;
            }

            List<Collider2D> collsNew = new List<Collider2D>();
            foreach (Collider2D coll in colls) {
                if (coll.GetComponent<Bloon.StandardBloon>().camo) {
                    collsNew.Add(coll);
                }
            }
            return collsNew.ToArray();
        }

        protected virtual void Shoot() {

        }

        // TODO: Make a method that allows (info inside):
        // Some projectiles are meant to be mutually inclusive in the "CanCollide" dictionary (Tack Shooter tacks comes to mind - If multiples hit the same bloon, only one of them should be able to do damage)
        // Some projectiles comes from the same pool, but are *not* mutually inclusive in the dictionary. (Probably multi-throw monkeys)
        // Gigantic Bloons (MOAB-Class) are meant to be hit by the same tower. (Tack Shooter can hit them multiple times..)

        protected virtual Projectile.StandardProjectile CreateProjectile(Projectile.StandardProjectile _projectile, Vector2 _position, Quaternion _rotation, Transform _parent, int _PoppingPower) {

            var projectile = Instantiate(_projectile, _position, _rotation, _parent);

            projectile.totalPower = _PoppingPower;
            projectile.penetration = GetStats<BaseTowerStats>().penetration;
            projectile.damageType = GetStats<BaseTowerStats>().damageType;
            projectile.moveSpd = GetStats<BaseTowerStats>().projectileSpeed;
            projectile.tower = this;

            return projectile;
        }

        public List<Projectile.StandardProjectile> CreateProjectileFamilyTree(Projectile.StandardProjectile _projectile, Vector2 _position, Quaternion _rotation) {

            List<Projectile.StandardProjectile> projectileList = new List<Projectile.StandardProjectile>();

            ParentController parentContainer = Instantiate(GameControl.PlacementController.controllerObject.ProjectileContainer, _position, _rotation, this.transform.parent.transform);
            parentContainer.name = "ProjectileParent_" + this.name.ToString();

            var poppingPower = Mathf.FloorToInt(GetStats<BaseTowerStats>().poppingPower);

            int pierceCheck = UnityEngine.Random.Range(0, 100);

            if (pierceCheck < (GetStats<BaseTowerStats>().poppingPower % Mathf.FloorToInt(GetStats<BaseTowerStats>().poppingPower) * 100)) {
                poppingPower++;
                Debug.Log("Added an extra pierce on a roll of " + pierceCheck + ".\nChance of " + GetStats<BaseTowerStats>().poppingPower % Mathf.FloorToInt(GetStats<BaseTowerStats>().poppingPower));
            }

            foreach (Transform child in GetStats<BaseTowerStats>().projectileSpawnPoints) {
                var projectile = CreateProjectile(_projectile, child.position, child.rotation, parentContainer.transform, poppingPower);

                projectileList.Add(projectile);
            }

            return projectileList;
        }
        public void ChangePop(int _amount, bool _additive) {
            if (_additive) {
                GetStats<BaseTowerStats>().towerPops += _amount;
            }
            else {
                GetStats<BaseTowerStats>().towerPops = _amount;
            }
        }

        public void ChangeTargettingState(bool _increase) {

            if (_increase) {
                if (GetStats<BaseTowerStats>().targettingState == TowerStats.TargettingStates.First) GetStats<BaseTowerStats>().targettingState = TowerStats.TargettingStates.Last;
                else if (GetStats<BaseTowerStats>().targettingState == TowerStats.TargettingStates.Last) GetStats<BaseTowerStats>().targettingState = TowerStats.TargettingStates.Close;
                else if (GetStats<BaseTowerStats>().targettingState == TowerStats.TargettingStates.Close) GetStats<BaseTowerStats>().targettingState = TowerStats.TargettingStates.Strong;
                else if (GetStats<BaseTowerStats>().targettingState == TowerStats.TargettingStates.Strong) GetStats<BaseTowerStats>().targettingState = TowerStats.TargettingStates.First;
            }
            else {
                if (GetStats<BaseTowerStats>().targettingState == TowerStats.TargettingStates.Strong) GetStats<BaseTowerStats>().targettingState = TowerStats.TargettingStates.Close;
                else if (GetStats<BaseTowerStats>().targettingState == TowerStats.TargettingStates.Close) GetStats<BaseTowerStats>().targettingState = TowerStats.TargettingStates.Last;
                else if (GetStats<BaseTowerStats>().targettingState == TowerStats.TargettingStates.Last) GetStats<BaseTowerStats>().targettingState = TowerStats.TargettingStates.First;
                else if (GetStats<BaseTowerStats>().targettingState == TowerStats.TargettingStates.First) GetStats<BaseTowerStats>().targettingState = TowerStats.TargettingStates.Strong;
            }
        }

        public void SellTower() {
            Destroy(gameObject);
            GameControl.InventoryController.ChangeGold(Mathf.RoundToInt(GetStats<BaseTowerStats>().sellValue));
        }
        /// <summary>
        /// True = left, false = right
        /// </summary>
        /// <param name="_Left"></param>
        public void UpgradeTower(bool _Left) {

            var upgradePaths = GetStats<BaseTowerStats>().upgradePaths;
            TowerUpgrade[] currentPath;
            int currentInt;
            TowerUpgrade currentTowerUpgrade;

            if (_Left) {
                currentPath = upgradePaths.leftUpgradePath;
                currentInt = upgradePaths.currentLeftUpgrade;
            }
            else {
                currentPath = upgradePaths.rightUpgradePath;
                currentInt = upgradePaths.currentRightUpgrade;
            }
            currentTowerUpgrade = currentPath[currentInt];
            if (currentTowerUpgrade == null) {
                return;
            }
            if (currentInt < currentPath.Length) {

                if (GameControl.InventoryController.controllerObject.gold >= currentTowerUpgrade.upgradeCost * GetStats<BaseTowerStats>().priceModifier) {
                    GameControl.InventoryController.controllerObject.gold -= currentTowerUpgrade.upgradeCost * GetStats<BaseTowerStats>().priceModifier;

                    GetStats<BaseTowerStats>().sellValue += currentTowerUpgrade.upgradeCost * GetStats<BaseTowerStats>().priceModifier * GetStats<BaseTowerStats>().sellValueModifier;
                    
                    var powerup = GetStats<BaseTowerStats>().GetPowerup(currentTowerUpgrade.upgradeEnum);

                    if (powerup != null) {
                        Debug.Log("Upgrade: " + currentTowerUpgrade.upgradeName + " unlocked.");
                        powerup.Powerup();
                    }
                    else {
                        Debug.LogError("Upgrade " + currentTowerUpgrade.upgradeName + " unable to be unlocked.");
                    }

                }
            }

            if (_Left) {
                GetStats<BaseTowerStats>().upgradePaths.currentLeftUpgrade++;
            }
            else {
                GetStats<BaseTowerStats>().upgradePaths.currentRightUpgrade++;
            }
            Debug.Log("Tower Upgraded. Now: " + upgradePaths.currentLeftUpgrade + " | " + upgradePaths.currentRightUpgrade);
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tower { 
    public class StandardTower : MonoBehaviour, TowerUpgrader.IUpgradeBase {
        
        public void Upgrade() {

        }

        public TowerStats generalStats = new TowerStats();

        protected virtual void Start() {
            if (generalStats.projectileEnum != GameControl.DictionaryController.Projectiles.Undefined)
                generalStats.projectileObject = GameControl.DictionaryController.RetrieveProjectileFromProjectileDictionary_Enum(generalStats.projectileEnum);
            else Debug.LogWarning("No Projectile set for this Tower.");

            generalStats.sellValueModifier = 0.8f;
            generalStats.priceModifier = 1;
            generalStats.sellValue = generalStats.goldCost * 0.8f;
            ChangePop(0, false);

            generalStats.firingCooldown = 0f;
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
                generalStats.firingCooldown -= Time.fixedDeltaTime;
            }
            else generalStats.firingCooldown = -1;
        }

        protected virtual Collider2D[] GetEnemiesInRange() {
            if (generalStats.firingRange != -1)
            return Physics2D.OverlapCircleAll(transform.position, generalStats.EffectiveFiringRange, GameControl.PlacementController.controllerObject.enemyLayer);
            else return Physics2D.OverlapBoxAll(transform.position, Vector2.one * 100, GameControl.PlacementController.controllerObject.enemyLayer);
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
            projectile.penetration = generalStats.penetration;
            projectile.damageType = generalStats.damageType;
            projectile.tower = this;

            return projectile;
        }

        public List<Projectile.StandardProjectile> CreateProjectileFamilyTree(Projectile.StandardProjectile _projectile, Vector2 _position, Quaternion _rotation, List<Transform> _transforms) {

            foreach (Transform transform in _transforms) { }

            ParentController parentContainer = Instantiate(GameControl.PlacementController.controllerObject.ProjectileContainer, _position, _rotation, this.transform.parent.transform);
            parentContainer.name = "ProjectileParent_" + this.name.ToString();
            List<Projectile.StandardProjectile> projectileList = new List<Projectile.StandardProjectile>();

            var poppingPower = Mathf.FloorToInt(generalStats.poppingPower);

            int pierceCheck = UnityEngine.Random.Range(0, 100);

            if (pierceCheck < (generalStats.poppingPower % Mathf.FloorToInt(generalStats.poppingPower) * 100)) {
                poppingPower++;
                Debug.Log("Added an extra pierce on a roll of " + pierceCheck + ".\nChance of " + generalStats.poppingPower % Mathf.FloorToInt(generalStats.poppingPower));
            }

            for (int i = 0; i < _transforms.Count; i++) {
                var projectile = CreateProjectile(_projectile, _transforms[i].position, _transforms[i].rotation, parentContainer.transform, poppingPower);

                projectileList.Add(projectile);
            }
            return projectileList;
        }
        
        /// <summary>
        /// Set true to change the value. Set false to set the value.
        /// </summary>
        /// <param name="_amount"></param>
        /// <param name="_additive"></param>
        public void ChangePop(int _amount, bool _additive) {
            if (_additive) {
                generalStats.towerPops += _amount;
            }
            else {
                generalStats.towerPops = _amount;
            }
            // Debug.Log("Added: " + generalStats.towerPops + " pops. Total:  " + generalStats.towerPops);
        }

        public void ChangeTargettingState(bool _increase) {

            if (_increase) {
                if (generalStats.targettingState == TowerStats.TargettingStates.First) generalStats.targettingState = TowerStats.TargettingStates.Last;
                else if (generalStats.targettingState == TowerStats.TargettingStates.Last) generalStats.targettingState = TowerStats.TargettingStates.Close;
                else if (generalStats.targettingState == TowerStats.TargettingStates.Close) generalStats.targettingState = TowerStats.TargettingStates.Strong;
                else if (generalStats.targettingState == TowerStats.TargettingStates.Strong) generalStats.targettingState = TowerStats.TargettingStates.First;
            }
            else {
                if (generalStats.targettingState == TowerStats.TargettingStates.Strong) generalStats.targettingState = TowerStats.TargettingStates.Close;
                else if (generalStats.targettingState == TowerStats.TargettingStates.Close) generalStats.targettingState = TowerStats.TargettingStates.Last;
                else if (generalStats.targettingState == TowerStats.TargettingStates.Last) generalStats.targettingState = TowerStats.TargettingStates.First;
                else if (generalStats.targettingState == TowerStats.TargettingStates.First) generalStats.targettingState = TowerStats.TargettingStates.Strong;
            }
        }

        public void SellTower() {
            Destroy(gameObject);
            GameControl.InventoryController.ChangeGold(Mathf.RoundToInt(generalStats.sellValue));
        }
        /// <summary>
        /// True = left, false = right
        /// </summary>
        /// <param name="_Left"></param>
        public void UpgradeTower(bool _Left) {
            if (_Left) {
                if (generalStats.upgradePaths.currentLeftUpgrade < generalStats.upgradePaths.leftUpgradePath.Length) {
                    if (GameControl.InventoryController.controllerObject.gold >= generalStats.upgradePaths.leftUpgradePath[generalStats.upgradePaths.currentLeftUpgrade].upgradeCost * generalStats.priceModifier) {
                        GameControl.InventoryController.controllerObject.gold -= generalStats.upgradePaths.leftUpgradePath[generalStats.upgradePaths.currentLeftUpgrade].upgradeCost * generalStats.priceModifier;

                        generalStats.sellValue += generalStats.upgradePaths.leftUpgradePath[generalStats.upgradePaths.currentLeftUpgrade].upgradeCost * generalStats.priceModifier * generalStats.sellValueModifier;

                        generalStats.upgradePaths.currentLeftUpgrade++;
                    }
                }
            }
            else {
                if (generalStats.upgradePaths.currentRightUpgrade < generalStats.upgradePaths.rightUpgradePath.Length) {
                    if (GameControl.InventoryController.controllerObject.gold >= generalStats.upgradePaths.rightUpgradePath[generalStats.upgradePaths.currentRightUpgrade].upgradeCost * generalStats.priceModifier) {
                        GameControl.InventoryController.controllerObject.gold -= generalStats.upgradePaths.rightUpgradePath[generalStats.upgradePaths.currentRightUpgrade].upgradeCost * generalStats.priceModifier;

                        generalStats.sellValue += generalStats.upgradePaths.rightUpgradePath[generalStats.upgradePaths.currentRightUpgrade].upgradeCost * generalStats.priceModifier * generalStats.sellValueModifier;

                        generalStats.upgradePaths.currentRightUpgrade++;
                    }
                }
            }
        }

    }
}
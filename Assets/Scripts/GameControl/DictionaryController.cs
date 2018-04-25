using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace GameControl {

    public class DictionaryController : MonoBehaviour {

        #region Towers

        public enum Towers {
            Undefined,
            Dart_Monkey,
            Tack_Shooter,
            Sniper_Monkey,
            Boomerang_Thrower,
            Ninja_Monkey,
            Bomb_Tower,
            Ice_Tower,
            Glue_Gunner,
            Monkey_Buccaneer,
            Monkey_Ace,
            Super_Monkey,
            Monkey_Apprentice,
            Monkey_Village,
            Banana_Farm,
            Mortar_Tower,
            Dartling_Gun,
            Spike_Factory,
            Heli_Pilot,
            Monkey_Engineer,
            Bloonchipper,
            Monkey_Sub
        }

        public Dictionaries.TowerLists towerLists = new Dictionaries.TowerLists();

        #endregion


        #region Projectiles

        public enum Projectiles {
            Undefined,
            Dart_Monkey_Default,
            Tack_Shooter_Default,
            Boomerang_Thrower_Default,
            Ninja_Monkey_Default,
        }

        public Dictionaries.ProjectilePrefabs projectilePrefabs = new Dictionaries.ProjectilePrefabs();

        #endregion


        #region Bloons

        public enum Bloons {
            Undefined,
            RedBloon,
            BlueBloon,
            GreenBloon,
            YellowBloon,
            PinkBloon,
            BlackBloon,
            LeadBloon
        }

        public Dictionaries.BloonPrefabs bloonPrefabs = new Dictionaries.BloonPrefabs();

        #endregion


        #region Tower Upgrades

        public enum TowerUpgrades {
            Undefined,
            Monkey_Dart_Long_Range_Darts,
            Monkey_Dart_Enhanced_Eyesight,
            Monkey_Dart_Spike_O_Pult,
            Monkey_Dart_Juggernaut,
            Monkey_Dart_Sharp_Shots,
            Monkey_Dart_Razor_Sharp_Shots,
            Monkey_Dart_Triple_Darts,
            Monkey_Dart_Super_Monkey_Fan_Club

        }

        public Dictionaries.TowerUpgrades towerUpgrades = new Dictionaries.TowerUpgrades();

        #endregion

        public static DictionaryController controllerObject;

        private Dictionary<ParentController, List<Bloon.StandardBloon>> collisionDictionary;

        public static Dictionary<Towers, TowerList> towerListDictionary;
        public static Dictionary<Bloons, Bloon.StandardBloon> bloonDictionary;
        public static Dictionary<Projectiles, Projectile.StandardProjectile> projectileDictionary;
        public static Dictionary<TowerUpgrades, TowerUpgrade> towerUpgradeDictionary;

        public Bloons[] BloonFamilyTreeArray;
        // public static Dictionary<GameController.Difficulties, > // TODO: Add Difficulty Dictionary. (Key = difficulty, value = new Class (Towers and upgrades change their cost.)

        private void Awake() {
            controllerObject = GetComponent<DictionaryController>();

            collisionDictionary = new Dictionary<ParentController, List<Bloon.StandardBloon>>();
            bloonDictionary = new Dictionary<Bloons, Bloon.StandardBloon>();
            projectileDictionary = new Dictionary<Projectiles, Projectile.StandardProjectile>();
            towerListDictionary = new Dictionary<Towers, TowerList>();
            towerUpgradeDictionary = new Dictionary<TowerUpgrades, TowerUpgrade>();

            AddBloonsToBloonDictionary();
            AddTowerListsToTowerListDictionary();
            AddProjectilesToProjectileDictionary();
            AddTowerUpgradesToTowerUpgradeDictionary();
        }

        public void Start() {
        }

        #region collisionDictionary

        public void OnProjectileParentDestroyed(ParentController _projectileParent) {
            // Whenever a projectile (Dart) is destroyed. Call this function
            if (collisionDictionary.ContainsKey(_projectileParent)) {
                collisionDictionary.Remove(_projectileParent);
            }
        }

        public void AddProjectileParentToCollisionDictionary(ParentController _projectileParent) {
            collisionDictionary.Add(_projectileParent, new List<Bloon.StandardBloon>());
        }

        public void OnBloonHit(ParentController _projectileParent, Bloon.StandardBloon _bloon) {
            // Whenever a Bloon is hit. Call this function.
            collisionDictionary[_projectileParent].Add(_bloon);
        }

        public bool CanCollide(ParentController _projectileParent, Bloon.StandardBloon _bloon) {

            if (!collisionDictionary.ContainsKey(_projectileParent)) {
                AddProjectileParentToCollisionDictionary(_projectileParent);
            }

            if (!collisionDictionary[_projectileParent].Contains(_bloon))
                return true;
            return false;
        }
        public void AddChildrenToCollisionDictionary(List<Bloon.StandardBloon> _childrenList, ParentController _projectileParent) {
            if (collisionDictionary.ContainsKey(_projectileParent))
                foreach (Bloon.StandardBloon child in _childrenList)
                    collisionDictionary[_projectileParent].Add(child);
        }
        public void RemoveProjectileFromCollisionDictionary(ParentController _projectileParent) {
            if (collisionDictionary.ContainsKey(_projectileParent))
                collisionDictionary.Remove(_projectileParent);
        }

        #endregion


        #region bloonDictionary

        private void AddBloonsToBloonDictionary() {
            AddBloonToBloonDictionary(Bloons.RedBloon, bloonPrefabs.RedBloon);
            AddBloonToBloonDictionary(Bloons.BlueBloon, bloonPrefabs.BlueBloon);
            AddBloonToBloonDictionary(Bloons.GreenBloon, bloonPrefabs.GreenBloon);
            AddBloonToBloonDictionary(Bloons.YellowBloon, bloonPrefabs.YellowBloon);
            AddBloonToBloonDictionary(Bloons.PinkBloon, bloonPrefabs.PinkBloon);
            AddBloonToBloonDictionary(Bloons.BlackBloon, bloonPrefabs.BlackBloon);
            AddBloonToBloonDictionary(Bloons.LeadBloon, bloonPrefabs.LeadBloon);
        }
        private void AddBloonToBloonDictionary(Bloons _Key, Bloon.StandardBloon _value) {
            bloonDictionary.Add(_Key, _value);
        }

        public static Bloon.StandardBloon RetrieveBloonFromBloonDictionary_Enum(Bloons _BloonsEnum) {
            if (bloonDictionary[_BloonsEnum] != null) {
                return bloonDictionary[_BloonsEnum];
            }
            else {
                Debug.LogError("No Projectile set for: " + _BloonsEnum);
                return null;
            }
        }
        public static Bloon.StandardBloon RetrieveBloonFromBloonDictionary_Index(int _Index) {
            return RetrieveBloonFromBloonDictionary_Enum(controllerObject.BloonFamilyTreeArray[_Index]);
        }
        #endregion


        #region towerListDictionary

        private void AddTowerListsToTowerListDictionary() {
            AddTowerListToTowerListDictionary(Towers.Dart_Monkey, towerLists.Dart_Monkey);
            AddTowerListToTowerListDictionary(Towers.Tack_Shooter, towerLists.Tack_Shooter);
            AddTowerListToTowerListDictionary(Towers.Sniper_Monkey, towerLists.Sniper_Monkey);
            AddTowerListToTowerListDictionary(Towers.Boomerang_Thrower, towerLists.Boomerang_Thrower);
            AddTowerListToTowerListDictionary(Towers.Ninja_Monkey, towerLists.Ninja_Monkey);
        }

        private void AddTowerListToTowerListDictionary(Towers _Key, TowerList _Value) {
            towerListDictionary.Add(_Key, _Value);
        }
        #endregion


        #region projectileDictionary

        private void AddProjectilesToProjectileDictionary() {
            AddProjectileToProjectileDictionary(Projectiles.Dart_Monkey_Default, projectilePrefabs.Dart_Monkey_Default);
            AddProjectileToProjectileDictionary(Projectiles.Tack_Shooter_Default, projectilePrefabs.Tack_Shooter_Default);
            AddProjectileToProjectileDictionary(Projectiles.Boomerang_Thrower_Default, projectilePrefabs.Boomerang_Thrower_Default);
            AddProjectileToProjectileDictionary(Projectiles.Ninja_Monkey_Default, projectilePrefabs.Ninja_Monkey_Default);
        }
        private void AddProjectileToProjectileDictionary(Projectiles _Key, Projectile.StandardProjectile _value) {
            projectileDictionary.Add(_Key, _value);
        }

        public static Projectile.StandardProjectile RetrieveProjectileFromProjectileDictionary_Enum(Projectiles _ProjectilesEnum) {
            if (projectileDictionary[_ProjectilesEnum] != null) {
                Debug.Log("Projectile " + _ProjectilesEnum + " Set");
                return projectileDictionary[_ProjectilesEnum];
            }
            else {
                Debug.LogError("No Projectile set for: " + _ProjectilesEnum);
                return null;
            }
        }

        #endregion

        #region towerUpgradeDictionary 

        private void AddTowerUpgradesToTowerUpgradeDictionary() {
            AddTowerUpgradeToTowerUpgradeDictionary(TowerUpgrades.Monkey_Dart_Long_Range_Darts, towerUpgrades.monkeyTower.Long_Range_Darts);
            AddTowerUpgradeToTowerUpgradeDictionary(TowerUpgrades.Monkey_Dart_Enhanced_Eyesight, towerUpgrades.monkeyTower.Enhanced_Eyesight);
            AddTowerUpgradeToTowerUpgradeDictionary(TowerUpgrades.Monkey_Dart_Spike_O_Pult, towerUpgrades.monkeyTower.Spike_O_Pult);
            AddTowerUpgradeToTowerUpgradeDictionary(TowerUpgrades.Monkey_Dart_Juggernaut, towerUpgrades.monkeyTower.Juggernaut);

            AddTowerUpgradeToTowerUpgradeDictionary(TowerUpgrades.Monkey_Dart_Sharp_Shots, towerUpgrades.monkeyTower.Sharp_Shots);
            AddTowerUpgradeToTowerUpgradeDictionary(TowerUpgrades.Monkey_Dart_Razor_Sharp_Shots, towerUpgrades.monkeyTower.Razor_Sharp_Shots);
            AddTowerUpgradeToTowerUpgradeDictionary(TowerUpgrades.Monkey_Dart_Triple_Darts, towerUpgrades.monkeyTower.Triple_Darts);
            AddTowerUpgradeToTowerUpgradeDictionary(TowerUpgrades.Monkey_Dart_Super_Monkey_Fan_Club, towerUpgrades.monkeyTower.Super_Monkey_Fan_Club);
        }

        private void AddTowerUpgradeToTowerUpgradeDictionary(TowerUpgrades _Key, TowerUpgrade _Value) {
            towerUpgradeDictionary.Add(_Key, _Value);
        }

        public static TowerUpgrade RetrieveTowerUpgradeFromTowerUpgradeDictionary_Enum(TowerUpgrades _Enum) {
            if (towerUpgradeDictionary[_Enum] != null) {
                return towerUpgradeDictionary[_Enum];
            }

            Debug.LogWarning("Tower Upgrade Dictionary does not contain: " + _Enum);
            return null;
                
        }

        #endregion

    }
}
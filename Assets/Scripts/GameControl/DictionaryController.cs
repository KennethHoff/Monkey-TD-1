using System;
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
            Monkey_Dart_Super_Monkey_Fan_Club,

            Tack_Shooter_Faster_Shooting,
            Tack_Shooter_Even_Faster_Shooting,
            Tack_Shooter_Tack_Sprayer,
            Tack_Shooter_Ring_Of_Fire,
            Tack_Shooter_Extra_Range_Tacks,
            Tack_Shooter_Super_Range_Tacks,
            Tack_Shooter_Blade_Shooter,
            Tack_Shooter_Blade_Maelstrom,

            Sniper_Monkey_Full_Metal_Jacket,
            Sniper_Monkey_Point_Five_Oh,
            Sniper_Monkey_Deadly_Precision,
            Sniper_Monkey_Cripple_MOAB,
            Sniper_Monkey_Faster_Firing,
            Sniper_Monkey_Night_Vision_Goggles,
            Sniper_Monkey_Semi_Automatic_Rifle,
            Sniper_Monkey_Supply_Drop,

            Boomerang_Thrower_Multi_Target,
            Boomerang_Thrower_Glaive_Thrower,
            Boomerang_Thrower_Glaive_Ricochet,
            Boomerang_Thrower_Glaive_Lord,
            Boomerang_Thrower_Sonic_Boom,
            Boomerang_Thrower_Red_Hot_Rangs,
            Boomerang_Thrower_Bionic_Boomer,
            Boomerang_Thrower_Turbo_Charge,

            Ninja_Monkey_Ninja_Discipline,
            Ninja_Monkey_Sharp_Shuriken,
            Ninja_Monkey_Double_Shot,
            Ninja_Monkey_Bloonjitsu,
            Ninja_Monkey_Seeking_Shuriken,
            Ninja_Monkey_Distraction,
            Ninja_Monkey_Flash_Bomb,
            Ninja_Monkey_Sabotage_Supply_Lines
        }

        public Dictionaries.TowerUpgrades towerUpgrades = new Dictionaries.TowerUpgrades();

        #endregion


        #region Tower Sprites

        public Dictionaries.TowerSprites towerSprites = new Dictionaries.TowerSprites();

        #endregion

        public static DictionaryController controllerObject;

        private Dictionary<ParentController, List<Bloon.StandardBloon>> collisionDictionary;

        public static Dictionary<Towers, TowerList> towerListDictionary;
        public static Dictionary<Bloons, Bloon.StandardBloon> bloonDictionary;
        public static Dictionary<Projectiles, Dictionaries.ProjectileStats> projectileDictionary;
        public static Dictionary<TowerUpgrades, TowerUpgrade> towerUpgradeDictionary;
        public static Dictionary<Towers, Dictionaries.SpecificTowers.BaseTowerSprites> towerSpriteDictionary;

        public Bloons[] BloonFamilyTreeArray;
        // public static Dictionary<GameController.Difficulties, > // TODO: Add Difficulty Dictionary. (Key = difficulty, value = new Class (Towers and upgrades change their cost.)

        private void Awake() {
            controllerObject = GetComponent<DictionaryController>();

            collisionDictionary = new Dictionary<ParentController, List<Bloon.StandardBloon>>();
            bloonDictionary = new Dictionary<Bloons, Bloon.StandardBloon>();
            projectileDictionary = new Dictionary<Projectiles, Dictionaries.ProjectileStats>();
            towerListDictionary = new Dictionary<Towers, TowerList>();
            towerUpgradeDictionary = new Dictionary<TowerUpgrades, TowerUpgrade>();
            towerSpriteDictionary = new Dictionary<Towers, Dictionaries.SpecificTowers.BaseTowerSprites>();

            AddBloonsToBloonDictionary();
            AddTowerListsToTowerListDictionary();
            AddProjectilesToProjectileDictionary();
            AddTowerUpgradesToTowerUpgradeDictionary();
            AddTowerSpritesToTowerSpriteDictionary();
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

        public static TowerList RetrieveTowerListFromTowerListDictionary_Enum(Towers _Enum) {
            return towerListDictionary[_Enum];
        }
        #endregion


        #region projectileDictionary

        private void AddProjectilesToProjectileDictionary() {
            AddProjectileToProjectileDictionary(Projectiles.Dart_Monkey_Default, projectilePrefabs.Dart_Monkey_Default);
            AddProjectileToProjectileDictionary(Projectiles.Tack_Shooter_Default, projectilePrefabs.Tack_Shooter_Default);
            AddProjectileToProjectileDictionary(Projectiles.Boomerang_Thrower_Default, projectilePrefabs.Boomerang_Thrower_Default);
            AddProjectileToProjectileDictionary(Projectiles.Ninja_Monkey_Default, projectilePrefabs.Ninja_Monkey_Default);
        }
        private void AddProjectileToProjectileDictionary(Projectiles _Key, Dictionaries.ProjectileStats _value) {
            projectileDictionary.Add(_Key, _value);
        }

        public static Dictionaries.ProjectileStats RetrieveProjectileFromProjectileDictionary_Enum(Projectiles _ProjectilesEnum) {
            if (projectileDictionary[_ProjectilesEnum] != null) {
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

            #region Dart Monkey

            AddTowerUpgradeToTowerUpgradeDictionary(TowerUpgrades.Monkey_Dart_Long_Range_Darts, towerUpgrades.dartMonkey.Long_Range_Darts);
            AddTowerUpgradeToTowerUpgradeDictionary(TowerUpgrades.Monkey_Dart_Enhanced_Eyesight, towerUpgrades.dartMonkey.Enhanced_Eyesight);
            AddTowerUpgradeToTowerUpgradeDictionary(TowerUpgrades.Monkey_Dart_Spike_O_Pult, towerUpgrades.dartMonkey.Spike_O_Pult);
            AddTowerUpgradeToTowerUpgradeDictionary(TowerUpgrades.Monkey_Dart_Juggernaut, towerUpgrades.dartMonkey.Juggernaut);

            AddTowerUpgradeToTowerUpgradeDictionary(TowerUpgrades.Monkey_Dart_Sharp_Shots, towerUpgrades.dartMonkey.Sharp_Shots);
            AddTowerUpgradeToTowerUpgradeDictionary(TowerUpgrades.Monkey_Dart_Razor_Sharp_Shots, towerUpgrades.dartMonkey.Razor_Sharp_Shots);
            AddTowerUpgradeToTowerUpgradeDictionary(TowerUpgrades.Monkey_Dart_Triple_Darts, towerUpgrades.dartMonkey.Triple_Darts);
            AddTowerUpgradeToTowerUpgradeDictionary(TowerUpgrades.Monkey_Dart_Super_Monkey_Fan_Club, towerUpgrades.dartMonkey.Super_Monkey_Fan_Club);

            #endregion

            #region Tack Shooter

            AddTowerUpgradeToTowerUpgradeDictionary(TowerUpgrades.Tack_Shooter_Faster_Shooting, towerUpgrades.tackShooter.Faster_Shooting);
            AddTowerUpgradeToTowerUpgradeDictionary(TowerUpgrades.Tack_Shooter_Even_Faster_Shooting, towerUpgrades.tackShooter.Even_Faster_Shooting);
            AddTowerUpgradeToTowerUpgradeDictionary(TowerUpgrades.Tack_Shooter_Tack_Sprayer, towerUpgrades.tackShooter.Tack_Sprayer);
            AddTowerUpgradeToTowerUpgradeDictionary(TowerUpgrades.Tack_Shooter_Ring_Of_Fire, towerUpgrades.tackShooter.Ring_Of_Fire);

            AddTowerUpgradeToTowerUpgradeDictionary(TowerUpgrades.Tack_Shooter_Extra_Range_Tacks, towerUpgrades.tackShooter.Extra_Range_Tacks);
            AddTowerUpgradeToTowerUpgradeDictionary(TowerUpgrades.Tack_Shooter_Super_Range_Tacks, towerUpgrades.tackShooter.Super_Range_Tacks);
            AddTowerUpgradeToTowerUpgradeDictionary(TowerUpgrades.Tack_Shooter_Blade_Shooter, towerUpgrades.tackShooter.Blade_Shooter);
            AddTowerUpgradeToTowerUpgradeDictionary(TowerUpgrades.Tack_Shooter_Blade_Maelstrom, towerUpgrades.tackShooter.Blade_Maelstrom);

            #endregion

            #region Sniper Monkey

            AddTowerUpgradeToTowerUpgradeDictionary(TowerUpgrades.Sniper_Monkey_Full_Metal_Jacket, towerUpgrades.sniperMonkey.Full_Metal_Jacket);
            AddTowerUpgradeToTowerUpgradeDictionary(TowerUpgrades.Sniper_Monkey_Point_Five_Oh, towerUpgrades.sniperMonkey.Point_Five_Oh);
            AddTowerUpgradeToTowerUpgradeDictionary(TowerUpgrades.Sniper_Monkey_Deadly_Precision, towerUpgrades.sniperMonkey.Deadly_Precision);
            AddTowerUpgradeToTowerUpgradeDictionary(TowerUpgrades.Sniper_Monkey_Cripple_MOAB, towerUpgrades.sniperMonkey.Cripple_MOAB);

            AddTowerUpgradeToTowerUpgradeDictionary(TowerUpgrades.Sniper_Monkey_Faster_Firing, towerUpgrades.sniperMonkey.Faster_Firing);
            AddTowerUpgradeToTowerUpgradeDictionary(TowerUpgrades.Sniper_Monkey_Night_Vision_Goggles, towerUpgrades.sniperMonkey.Night_Vision_Goggles);
            AddTowerUpgradeToTowerUpgradeDictionary(TowerUpgrades.Sniper_Monkey_Semi_Automatic_Rifle, towerUpgrades.sniperMonkey.Semi_Automatic_Rifle);
            AddTowerUpgradeToTowerUpgradeDictionary(TowerUpgrades.Sniper_Monkey_Supply_Drop, towerUpgrades.sniperMonkey.Supply_Drop);

            #endregion

            #region Boomerang Thrower

            AddTowerUpgradeToTowerUpgradeDictionary(TowerUpgrades.Boomerang_Thrower_Multi_Target, towerUpgrades.boomerangThrower.Multi_Target);
            AddTowerUpgradeToTowerUpgradeDictionary(TowerUpgrades.Boomerang_Thrower_Glaive_Thrower, towerUpgrades.boomerangThrower.Glaive_Thrower);
            AddTowerUpgradeToTowerUpgradeDictionary(TowerUpgrades.Boomerang_Thrower_Glaive_Ricochet, towerUpgrades.boomerangThrower.Glaive_Ricochet);
            AddTowerUpgradeToTowerUpgradeDictionary(TowerUpgrades.Boomerang_Thrower_Glaive_Lord, towerUpgrades.boomerangThrower.Glaive_Lord);

            AddTowerUpgradeToTowerUpgradeDictionary(TowerUpgrades.Boomerang_Thrower_Sonic_Boom, towerUpgrades.boomerangThrower.Sonic_Boom);
            AddTowerUpgradeToTowerUpgradeDictionary(TowerUpgrades.Boomerang_Thrower_Red_Hot_Rangs, towerUpgrades.boomerangThrower.Red_Hot_Rangs);
            AddTowerUpgradeToTowerUpgradeDictionary(TowerUpgrades.Boomerang_Thrower_Bionic_Boomer, towerUpgrades.boomerangThrower.Bionic_Boomer);
            AddTowerUpgradeToTowerUpgradeDictionary(TowerUpgrades.Boomerang_Thrower_Turbo_Charge, towerUpgrades.boomerangThrower.Turbo_Charge);

            #endregion

            #region Ninja Monkey

            AddTowerUpgradeToTowerUpgradeDictionary(TowerUpgrades.Ninja_Monkey_Ninja_Discipline, towerUpgrades.ninjaMonkey.Ninja_Discipline);
            AddTowerUpgradeToTowerUpgradeDictionary(TowerUpgrades.Ninja_Monkey_Sharp_Shuriken, towerUpgrades.ninjaMonkey.Sharp_Shurikens);
            AddTowerUpgradeToTowerUpgradeDictionary(TowerUpgrades.Ninja_Monkey_Double_Shot, towerUpgrades.ninjaMonkey.Double_Shot);
            AddTowerUpgradeToTowerUpgradeDictionary(TowerUpgrades.Ninja_Monkey_Bloonjitsu, towerUpgrades.ninjaMonkey.Bloonjitsu);

            AddTowerUpgradeToTowerUpgradeDictionary(TowerUpgrades.Ninja_Monkey_Seeking_Shuriken, towerUpgrades.ninjaMonkey.Seeking_Shuriken);
            AddTowerUpgradeToTowerUpgradeDictionary(TowerUpgrades.Ninja_Monkey_Distraction, towerUpgrades.ninjaMonkey.Distraction);
            AddTowerUpgradeToTowerUpgradeDictionary(TowerUpgrades.Ninja_Monkey_Flash_Bomb, towerUpgrades.ninjaMonkey.Flash_Bomb);
            AddTowerUpgradeToTowerUpgradeDictionary(TowerUpgrades.Ninja_Monkey_Sabotage_Supply_Lines, towerUpgrades.ninjaMonkey.Sabotage_Supply_Lines);

            #endregion
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

        #region towerSpriteDictionary 

        private void AddTowerSpritesToTowerSpriteDictionary() {
            AddTowerSpriteToTowerSpriteDictionary(Towers.Dart_Monkey, towerSprites.dartMonkey);
            AddTowerSpriteToTowerSpriteDictionary(Towers.Tack_Shooter, towerSprites.tackShooter);
            AddTowerSpriteToTowerSpriteDictionary(Towers.Sniper_Monkey, towerSprites.sniperMonkey);
            AddTowerSpriteToTowerSpriteDictionary(Towers.Boomerang_Thrower, towerSprites.boomerangThrower);
            AddTowerSpriteToTowerSpriteDictionary(Towers.Ninja_Monkey, towerSprites.ninjaMonkey);
        }

        private void AddTowerSpriteToTowerSpriteDictionary(Towers _Key, Dictionaries.SpecificTowers.BaseTowerSprites _Value) {
            towerSpriteDictionary.Add(_Key, _Value);
        }

        public static Dictionaries.SpecificTowers.BaseTowerSprites RetrieveTowerSpriteFromTowerSpriteDictionary_Enum(Towers _Enum) {
            return towerSpriteDictionary[_Enum];
        }

        #endregion

    }
}
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace GameControl { 

    public class DictionaryController : MonoBehaviour {

        public static DictionaryController controllerObject;

        private Dictionary<int, List<int>> collisionDictionary;
        private Dictionary<GameObject, List<GameObject>> collisionDictionaryGameObject;
        public static Dictionary<BloonSpawner.Bloons, GameObject> bloonDictionary;
        public static Dictionary<PlacementController.Towers, TowerList> placementDictionary; // Element 0 = Tower, element 1 = Template, element 2 = UI Button
        // public static Dictionary<GameController.Difficulties, > // TODO: Add Difficulty Dictionary. (Key = difficulty, value = new Class (Towers and upgrades change their cost.)

        private void Awake() {
            controllerObject = GetComponent<DictionaryController>();
        }

        public void Start() {
            collisionDictionary = new Dictionary<int, List<int>>();
            collisionDictionaryGameObject = new Dictionary<GameObject, List<GameObject>>();
            bloonDictionary = new Dictionary<BloonSpawner.Bloons, GameObject>();
            placementDictionary = new Dictionary<PlacementController.Towers, TowerList>();

            AddBloonsToDictionary();
            AddTowersToDictionary();
        }
        

        #region bloonDictionary
        private void AddBloonsToDictionary() {
            bloonDictionary.Add(key: BloonSpawner.Bloons.RedBloon, value: BloonSpawner.controllerObject.redBloonPrefab);
            bloonDictionary.Add(key: BloonSpawner.Bloons.BlueBloon, value: BloonSpawner.controllerObject.blueBloonPrefab);
            bloonDictionary.Add(key: BloonSpawner.Bloons.GreenBloon, value: BloonSpawner.controllerObject.greenBloonPrefab);
            bloonDictionary.Add(key: BloonSpawner.Bloons.YellowBloon, value: BloonSpawner.controllerObject.yellowBloonPrefab);
            bloonDictionary.Add(key: BloonSpawner.Bloons.PinkBloon, value: BloonSpawner.controllerObject.pinkBloonPrefab);
            bloonDictionary.Add(key: BloonSpawner.Bloons.BlackBloon, value: BloonSpawner.controllerObject.blackBloonPrefab);
            bloonDictionary.Add(key: BloonSpawner.Bloons.LeadBloon, value: BloonSpawner.controllerObject.leadBloonPrefab);
        }
        #endregion

        #region placementDictionary
        private void AddTowersToDictionary() {
            placementDictionary.Add(key: PlacementController.Towers.DartMonkey, value: PlacementController.controllerObject.TowerDict_DartMonkey);
            placementDictionary.Add(key: PlacementController.Towers.TackShooter, value: PlacementController.controllerObject.TowerDict_TackShooter);
            //placementDictionary.Add(key: PlacementController.Towers.SniperMonkey, value: PlacementController.controllerObject.TowerDict_SniperMonkey);
            placementDictionary.Add(key: PlacementController.Towers.NinjaMonkey, value: PlacementController.controllerObject.TowerDict_NinjaMonkey);
        }
        #endregion
        
        #region projectileDictionary

        public void OnProjectileDestroyed(int projectileID) {
            // Whenever a projectile (Dart) is destroyed. Call this function
            if (collisionDictionary.ContainsKey(projectileID)) {
                collisionDictionary.Remove(projectileID);
            }
        }

        public void OnProjectileCreated(int projectileID) {
            // Whenever a Projectile is accessing the "CanCollide" function, and the Key does not exist. Call this function. (This way it will never add itself to the dictionary if it never hits anything).
            collisionDictionary.Add(projectileID, new List<int>());
        }
        public void OnBloonHit(int projectileID, int BloonID) {
            // Whenever a Bloon is hit. Call this function.
            collisionDictionary[projectileID].Add(BloonID);
        }
        public bool CanCollide(int ProjectileID, int BloonID) {
            //Whenever a Projectile attempts to collide, call this function.
            if (!collisionDictionary.ContainsKey(ProjectileID))
                OnProjectileCreated(ProjectileID);
            // return (!(projectileDictionary[ProjectileID].Count(id => id == BloonID) > 0));

            if (!(collisionDictionary[ProjectileID].Count(id => id == BloonID) > 0)) { 
                //Debug.Log("Can collide");
                return true;
            }
            else { 
                //Debug.Log("Cannot collide. : " + BloonID);
                return false;
            }

        }
        
        public void AddChildrenToDictionary(int BloonID, List<int> childrenIDList, int projectileID) {
            // Whenever a bloon gets popped, add the ID of the children to the dictionary index of the projectile that popped the bloon.
            if (collisionDictionary.ContainsKey(projectileID)) { 
                foreach (int child in childrenIDList)
                    collisionDictionary[projectileID].Add(child);
            }
        }

        #endregion

        #region ProjectileDictionaryGameObject
        // Generally only used as a debugger.
        // As I wrote this however, due to the debugging, this now works, but the ID-based one doesn't.. Gotta fix that (as I can only assume it is less resource-intensive (stores less info (1 int vs an entire GameObject))
        // but not atm, as resource intensiveness


        public void OnProjectileDestroyedGameObject(GameObject projectile) {
            // Whenever a projectile (Dart) is destroyed. Call this function
            if (collisionDictionaryGameObject.ContainsKey(projectile)) {
                collisionDictionaryGameObject.Remove(projectile);
            }
        }

        public void AddProjectileToDictionaryGameObject(GameObject projectile) {
            collisionDictionaryGameObject.Add(projectile, new List<GameObject>());
        }

        public void OnBloonHitGameObject(GameObject projectile, GameObject Bloon) {
            // Whenever a Bloon is hit. Call this function.
            collisionDictionaryGameObject[projectile].Add(Bloon);
        }

        public bool CanCollideGameObject(GameObject projectile, GameObject bloon) {

            if (!collisionDictionaryGameObject.ContainsKey(projectile)) {
                AddProjectileToDictionaryGameObject(projectile);
            }
            
            if (!collisionDictionaryGameObject[projectile].Contains(bloon))
                return true;
            return false;
        }
        public void AddChildrenToDictionaryGO(GameObject bloonID, List<GameObject> childrenList, GameObject Projectile) {
            if (collisionDictionaryGameObject.ContainsKey(Projectile))
                foreach (GameObject child in childrenList)
                    collisionDictionaryGameObject[Projectile].Add(child);
        }
        #endregion
    }
}
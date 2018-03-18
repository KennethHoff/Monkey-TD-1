using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace GameControl { 

    public class DictionaryController : MonoBehaviour {

        public static DictionaryController controllerObject;
        
        private Dictionary<ParentController, List<Bloon.StandardBloon>> collisionDictionaryGameObject;
        public static Dictionary<BloonSpawner.Bloons, Bloon.StandardBloon> bloonDictionary;
        public static Dictionary<PlacementController.Towers, TowerList> placementDictionary; // Element 0 = Tower, element 1 = Template, element 2 = UI Button
        // public static Dictionary<GameController.Difficulties, > // TODO: Add Difficulty Dictionary. (Key = difficulty, value = new Class (Towers and upgrades change their cost.)

        private void Awake() {
            controllerObject = GetComponent<DictionaryController>();
        }

        public void Start() {
            
            collisionDictionaryGameObject = new Dictionary<ParentController, List<Bloon.StandardBloon>>();
            bloonDictionary = new Dictionary<BloonSpawner.Bloons, Bloon.StandardBloon>();
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
            placementDictionary.Add(key: PlacementController.Towers.SniperMonkey, value: PlacementController.controllerObject.TowerDict_SniperMonkey);
            placementDictionary.Add(key: PlacementController.Towers.NinjaMonkey, value: PlacementController.controllerObject.TowerDict_NinjaMonkey);
        }
        #endregion
        
        #region ProjectileDictionaryGameObject

        // Originally only used as a debugger.
        // As I wrote this however, due to the debugging, this now works, but the ID-based one doesn't.. Gotta fix that (as I can only assume it is less resource-intensive (stores less info (1 int vs an entire GameObject))
        // but not now, as process intensiveness is not a concern right now.


        public void OnProjectileParentDestroyedGameObject(ParentController _projectileParent) {
            // Whenever a projectile (Dart) is destroyed. Call this function
            if (collisionDictionaryGameObject.ContainsKey(_projectileParent)) {
                collisionDictionaryGameObject.Remove(_projectileParent);
            }
        }

        public void AddProjectileToDictionaryGameObject(ParentController _projectileParent) {
            collisionDictionaryGameObject.Add(_projectileParent, new List<Bloon.StandardBloon>());
        }

        public void OnBloonHitGameObject(ParentController _projectileParent, Bloon.StandardBloon _bloon) {
            // Whenever a Bloon is hit. Call this function.
            collisionDictionaryGameObject[_projectileParent].Add(_bloon);
        }

        public bool CanCollideGameObject(ParentController _projectileParent, Bloon.StandardBloon _bloon) {

            if (!collisionDictionaryGameObject.ContainsKey(_projectileParent)) {
                AddProjectileToDictionaryGameObject(_projectileParent);
            }
            
            if (!collisionDictionaryGameObject[_projectileParent].Contains(_bloon))
                return true;
            return false;
        }
        public void AddChildrenToDictionaryGameObject(List<Bloon.StandardBloon> _childrenList, ParentController _projectileParent) {
            if (collisionDictionaryGameObject.ContainsKey(_projectileParent))
                foreach (Bloon.StandardBloon child in _childrenList)
                    collisionDictionaryGameObject[_projectileParent].Add(child);
        }
        #endregion
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameControl {
    public class BloonSpawner : MonoBehaviour {

        public static BloonSpawner controllerObject;
        [SerializeField]
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

        public GameObject redBloonPrefab;
        public GameObject blueBloonPrefab;
        public GameObject greenBloonPrefab;
        public GameObject yellowBloonPrefab;
        public GameObject pinkBloonPrefab;
        public GameObject blackBloonPrefab;
        public GameObject leadBloonPrefab;

        public void Awake() {
            controllerObject = GetComponent<BloonSpawner>();
        }

        // LowPrio: Remake the Bloon & Projectile Spawning mechanic to be a Object Pooler
        public static GameObject SpawnBloon(GameObject _bloonObject, Vector3 _position, Quaternion _rotation, int _CurrentWaypoint,  bool _regrowth, bool _camo) {
            GameObject spawnedBloon = Instantiate(_bloonObject, _position, _rotation, GameController.enemyParent.transform);
            Bloon.StandardBloon spawnedScript = spawnedBloon.GetComponent<Bloon.StandardBloon>();
            spawnedBloon.GetComponent<WayPoints>().currentWayPoint = _CurrentWaypoint;
            spawnedScript.regrowth = _regrowth;
            spawnedScript.camo = _camo;
            return spawnedBloon;
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameControl {
    public class BloonSpawner : MonoBehaviour {

        public static BloonSpawner controllerObject;
        
        public void Awake() {
            controllerObject = this;
        }

        // LowPrio: Remake the Bloon & Projectile Spawning mechanic to be a Object Pooler
        public static Bloon.StandardBloon SpawnBloon(Bloon.StandardBloon _bloon, Vector3 _position, Quaternion _rotation, int _CurrentWaypoint,  bool _regrowth, bool _camo) {
            Bloon.StandardBloon spawnedBloon = Instantiate(_bloon, _position, _rotation, GameController.enemyParent.transform);
            spawnedBloon.GetComponent<Bloon.WayPoints>().currentWayPoint = _CurrentWaypoint;
            spawnedBloon.regrowth = _regrowth;
            spawnedBloon.camo = _camo;
            return spawnedBloon;
        }
    }
}
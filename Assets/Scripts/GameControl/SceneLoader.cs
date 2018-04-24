using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GameControl {
    public class SceneLoader : MonoBehaviour {

        public bool LoadMap;

        public Vector2 MapLoadOffset;

        public GameObject mapToLoad;

        public GameObject mapParent;

        private void Awake() {
            if (LoadMap) {
                mapParent = GameObject.Find("Map");
                Instantiate(mapToLoad, Vector3.zero + new Vector3(MapLoadOffset.x, MapLoadOffset.y, 0), Quaternion.identity, mapParent.transform);
            }
        }
    }
}

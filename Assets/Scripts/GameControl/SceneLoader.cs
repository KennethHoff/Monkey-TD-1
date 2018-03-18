using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GameControl {
    public class SceneLoader : MonoBehaviour {

        public bool LoadMap;

        public Vector2 MapLoadOffset;

        /*  // Unable to do cross-scene object references - Have to abandon this idea.
        public string LoadedSceneString;
        public Scene loadedScene;

        void Start () {
            LoadedSceneString = "Monkey Lane";
            loadedScene = SceneManager.GetSceneByName(LoadedSceneString);
            SceneManager.LoadScene(LoadedSceneString, LoadSceneMode.Additive);
            SceneManager.SetActiveScene(loadedScene);
        }

        */

        public GameObject loadedMap;

        public GameObject mapParent;

        private void Awake() {
            if (LoadMap) {
                mapParent = GameObject.Find("Map");
                Instantiate(loadedMap, Vector3.zero + new Vector3(MapLoadOffset.x, MapLoadOffset.y, 0), Quaternion.identity, mapParent.transform);
            }
        }
    }
}

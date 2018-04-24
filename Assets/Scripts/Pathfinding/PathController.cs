using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameControl { 
    public class PathController : MonoBehaviour {

        public static PathController controllerObject;

        [ReadOnly] public List<Transform> wayPointList;
        [ReadOnly] public GameObject pathingParent;

        public static Transform spawnPoint;

        [ReadOnly] public List<Transform> underpassesList;

        // Use this for initialization
        private void Awake() {
            controllerObject = GetComponent<PathController>();
        }
        IEnumerator Start() {
            yield return new WaitForEndOfFrame();
            SetPathingParent();
            spawnPoint = GameObject.Find("SpawnPoint").transform;
        }

        private void SetPathingParent() {
            if (pathingParent == null) {
                pathingParent = FindObjectOfType<PathingParent>().gameObject;
                Transform parentTransform = pathingParent.transform;

                for (int i = 0; i < parentTransform.childCount; i++) {
                    wayPointList.Add(parentTransform.GetChild(i));
                }
            }
        }
    }
}
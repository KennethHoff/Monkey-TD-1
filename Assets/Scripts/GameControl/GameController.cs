using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameControl;

namespace GameControl {

    public class GameController : MonoBehaviour {

        public enum Difficulties {
            Easy,
            Normal,
            Hard,
            Impoppable
        }

        public enum DamageTypes {
            Sharp,
            Explosive,
            Both
        }
        


        public Sprite camo_overlay, camo_regen_overlay;


        [ReadOnly] public Tower.StandardTower towerUnderCursor;

        [Header("Current Game Information:")]
        public Difficulties difficulty;

        public bool AutoStartNextWave = false;

        public bool fastForward = false;

        public float currentGameSpeed;

        public static GameController controllerObject;

        public static GameObject towerParent, enemyParent;
        
        public float regenerationTime = 1/1.2f; // How long it takes to go back up a tier of Bloon

        public float aimTimer = 1/30f; // (When Tower is unable to aim, how long to wait between each aim (performance).

        private void Awake() {
            controllerObject = this;
        }

        void Start() {

            Debug.Log("Started game.");
            enemyParent = GameObject.Find("Enemy");
            towerParent = GameObject.Find("Tower");
            SetGameSpeed();
        }

        // Update is called once per frame
        void Update() {
            if (Input.GetMouseButtonDown(0)) {
                if (WithinMapWorldPoint( (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition), 0)) { 
                    GameControl.UIController.controllerObject.targettedTower = towerUnderCursor;
                }
            }

            SetGameSpeed();
            // SetUpdateSpeed();
        }

        private void SetGameSpeed() {
            if (fastForward) {
                currentGameSpeed = 2f;
            }
            else {
                currentGameSpeed = 1f;
            }
        }
        private void SetUpdateSpeed() {
            Time.fixedDeltaTime = 0.01f / currentGameSpeed;
        }


        public static bool WithinMapPosition(Vector2 worldPos, float outsideRadius) {
            Vector2 worldPoint = Camera.main.ScreenToWorldPoint(worldPos);
            return WithinMapWorldPoint(worldPoint, outsideRadius);

        }
        public static bool WithinMapWorldPoint(Vector2 worldPoint, float outsideRadius) {
            float leftSide = -9.05f - outsideRadius;
            float rightSide = 6.6f + outsideRadius;
            float topSide = 6.8f + outsideRadius;
            float bottomSide = -4.5f - outsideRadius;

            return (rightSide > worldPoint.x && worldPoint.x > leftSide + outsideRadius && topSide + outsideRadius > worldPoint.y && worldPoint.y > bottomSide + outsideRadius);
        }

        public void EndGame() {
            WaveSpawner.controllerObject.state = WaveSpawner.SpawnState.GameOver;
        }
    }
}
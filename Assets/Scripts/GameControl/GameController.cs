using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameControl;
using System;
using UnityEngine.Video;
using System.Timers;

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

        public Camera mainCamera;
        public Camera videoCamera;

        public MapController mapController;

        [ReadOnly] public float timeSinceLastInteraction;

        [ReadOnly] public Tower.StandardTower towerUnderCursor;
        [ReadOnly] public TowerSelector buttonUnderCursor;
        [ReadOnly] public UpgradeButton upgradeUnderCursor;

        [Header("Current Game Information:")]
        public Difficulties difficulty;

        public bool AutoStartNextWave = false;

        public bool fastForward = false;

        public float currentGameSpeed;

        public static GameController controllerObject;
        
        public float regenerationTime = 1/1.2f; // How long it takes to go back up a tier of Bloon

        public float aimTimer = 1/30f; // (When Tower is unable to aim, how long to wait between each aim (performance).

        [Space(5)]
        public float timeBeforeRestart;

        private void Awake() {
            controllerObject = this;
        }

        void Start() {
            timeSinceLastInteraction = timeBeforeRestart;

            Debug.Log("Started game.");
            SetGameSpeed();
        }

        // Update is called once per frame
        void Update() {
            CheckTimeSinceLastInteraction();

            if (timeSinceLastInteraction > timeBeforeRestart) {
                RestartGame();
            }
            else {
                PlayTrailer(false);
            }
            

            if (Input.GetMouseButtonDown(0)) {
                if (WithinMapWorldPoint((Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition), 0)) {
                    GameControl.UIController.controllerObject.targettedTower = towerUnderCursor;
                }
            }

            if (Input.GetKeyDown(KeyCode.F10)) {
                GameControl.InventoryController.ChangeGold(1000);
            }

            SetGameSpeed();
        }


        private void SetGameSpeed() {
            if (fastForward) {
                currentGameSpeed = 2f;
            }
            else {
                currentGameSpeed = 1f;
            }
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
            UIController.controllerObject.SetGameOverText(true);
        }

        public void RestartGame() {

            timeSinceLastInteraction = timeBeforeRestart;

            WaveSpawner.ResetWaves();
            InventoryController.SetStartValues();
            PlacementController.RemoveAllTowers();
            WaveSpawner.RemoveAllBloons();
            UIController.controllerObject.SetGameOverText(false);
            
            PlayTrailer(true);

            Debug.Log("Restarting Game...");
        }

        private void CheckTimeSinceLastInteraction() {
            if (GameControl.WaveSpawner.controllerObject.state == WaveSpawner.SpawnState.GameOver) return;
            timeSinceLastInteraction += Time.deltaTime;

            if ((Input.GetAxis("Mouse X") != 0) || (Input.GetAxis("Mouse Y") != 0)) {
                Debug.Log("Mouse moved.");
                timeSinceLastInteraction = 0;
            }
            if ((Input.anyKeyDown)) {
                Debug.Log("A key is currently being pressed.");
                timeSinceLastInteraction = 0;
            }
        }

        private void SetCamera(bool _Video) {
            if (_Video) {
                videoCamera.enabled = true;
                mainCamera.enabled = false;
            }
            else {
                videoCamera.enabled = false;
                mainCamera.enabled = true;
            }
        }

        private void PlayTrailer(bool _true) {
            var videoPlayer = videoCamera.gameObject.GetComponent<VideoPlayer>();
            SetCamera(_true);

            if (_true) { 
                if (!videoPlayer.isPlaying) {
                    videoPlayer.time = 0;
                    videoPlayer.Play();
                }
            }
            else {
                videoPlayer.Pause();
            }
        }
    }
}
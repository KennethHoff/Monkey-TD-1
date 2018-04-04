using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameControl {

    [System.Serializable]
    public class WaveSpawner : MonoBehaviour {

        // Incomplete: Add a custom Property Drawer for the Wave Editor (Tried doing this : Super cancer.


        // This is a robust controller that is used to spawn waves.
        // On each wave you can multiple sets, each set contains: 'Which enemy to spawn', 'how many of that enemy to spawn', 'how long to wait between each spawn' and a 'how long to wait after this set'.
        // This allows long, sustained spawns followed by short, bursty spawns, followed by long, sustained spawns again, with possibility of some intertwined different bloons.
        // Probably the most sophisticated system I have created.
        

        // Completed: Add a variable that shows total remaining bloons this wave.

        // Completed: Rework the ways waves are spawned (and therefore stored) (More details inside)
        // I want to be able to: "spawn x Number of Y Bloon, with a delay of Z seconds between, followed by X number of Y Bloon, with a delay of Z seconds, followed by..." ((Maybe allow multiple to spawn at the same time? - Not important)) 
        // Currently I do: "Spawn X number of Y Bloon, then spawn X number of Y Bloon, then... all with a delay of Z seconds between bloons"

        public delegate void RoundEnded();

        public static event RoundEnded RoundEndedEvent;

        #region variables

        public static WaveSpawner controllerObject;

        public enum SpawnState {
            Spawning, // Actively spawning Bloons
            WaitingForBloonsToDie, // Waiting for Bloons to be destroyed.
            RoundEnded, // No more bloons, waiting for 'Start Next Wave Request'
            GameStart, // First round has not started yet.
            GameOver // Lost all lives
        }

        [Header("Wave info")]
        public SpawnState state;
        public bool waveActive = false;
        public int currentWave = 1;
        public int totalWaves = 0;

        [Header("Bloon Info:")]
        public int totalBloonsThisWave;
        public int bloonsKilledThisWave;
        public int bloonsReachedFinalDestinationThisWave;
        public int bloonsSpawnedThisWave;
        public int bloonSpawnsLeftThisWave;

        [Header("RBE Info:")]
        public int totalRBEThisWave;
        public int RBEKilledThisWave;
        public int RBEReachedFinalDestinationThisWave;
        public int RBERegeneratedThisWave;
        public int RBESpawnedThisWave;
        public int RBESpawnsLeftThisWave;

        [Space(10)]
        public int bloonsOnScreen;

        [Header("Wave Editor:")]
        public Wave[] waves;
        //public WaveList waves;

        #endregion

        private void Awake() {
            controllerObject = GetComponent<WaveSpawner>();
        }

        private void Start() {
            state = SpawnState.GameStart;
            totalWaves = waves.Length;
        }

        private void Update() {
            bloonSpawnsLeftThisWave = totalBloonsThisWave - bloonsSpawnedThisWave;
            RBESpawnsLeftThisWave = totalRBEThisWave - RBESpawnedThisWave;

            bloonsOnScreen = GameController.enemyParent.transform.childCount;
            
            if (state == SpawnState.RoundEnded) {
                // RoundEnded - AutoStart if autostart is active, otherwise do nothing.
                if (!waveActive) { 
                    if (GameController.controllerObject.AutoStartNextWave) {
                        StartNextWave();
                    }
                }
            }
            else if (state == SpawnState.WaitingForBloonsToDie) {

                if (bloonSpawnsLeftThisWave <= 0 && bloonsOnScreen <= 0) {
                    waveActive = false;
                    GameControl.GameController.controllerObject.fastForward = false;
                    RoundEndedEvent();

                    if (currentWave < totalWaves) { 
                        state = SpawnState.RoundEnded;
                    }
                    else {
                        state = SpawnState.GameOver;
                    }
                }
            }
            else if (state == SpawnState.Spawning) {
                // Actively Spawning Bloons -- Do nothing
            }
            else if (state == SpawnState.GameOver) {
                // Life is at 0 || CurrentRound = maxRounds
            }
        }

        public void StartNextWave() {
            if (state == SpawnState.RoundEnded) {
                StartWave(currentWave);
                currentWave++;
            }
            else if (state == SpawnState.GameStart) {
                StartWave(currentWave - 1);
            }
        }

        public void StartWave(int _waveToSpawn) {
            if (waves[_waveToSpawn] != null) {
                bloonsKilledThisWave = 0;
                bloonsReachedFinalDestinationThisWave = 0;
                bloonsSpawnedThisWave = 0;
                RBESpawnedThisWave = 0;
                StartCoroutine(SpawnWave(waves[_waveToSpawn]));
                waveActive = true;
            }
        }


        // TODO: Fix this to better scale with (a changing) gameSpeed
        private IEnumerator SpawnWave(Wave _waveToSpawn) {

            totalRBEThisWave = _waveToSpawn.TotalRBE;
            totalBloonsThisWave = _waveToSpawn.TotalBloons;
            state = SpawnState.Spawning;
            
            for (int i = 0; i < _waveToSpawn.waveTypeList.Count; i++) {
                for (int j = 0; j < _waveToSpawn.waveTypeList[i].amount; j++) {
                    Bloon.StandardBloon spawnedBloon =  BloonSpawner.SpawnBloon(_waveToSpawn.waveTypeList[i].bloon, PathController.spawnPoint.position, Quaternion.identity, 0, _waveToSpawn.waveTypeList[i].regrowth, _waveToSpawn.waveTypeList[i].camo);
                    WaveSpawner.controllerObject.bloonsSpawnedThisWave++;
                    WaveSpawner.controllerObject.RBESpawnedThisWave += spawnedBloon.RBE;
                    yield return new WaitForSeconds(_waveToSpawn.waveTypeList[i].interval / GameController.controllerObject.currentGameSpeed); 
                }
                yield return new WaitForSeconds(_waveToSpawn.waveTypeList[i].delay / GameController.controllerObject.currentGameSpeed);
            }
            state = SpawnState.WaitingForBloonsToDie;
            yield break;
        }
        
    }
}
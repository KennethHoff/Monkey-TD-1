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
        [ReadOnly] public SpawnState state;
        [ReadOnly] public bool waveActive = false;
        public int currentWave = 1;
        [ReadOnly] public int totalWaves = 0;

        [Header("Bloon Info:")]
        [ReadOnly] public int totalBloonsThisWave;
        [ReadOnly] public int bloonsKilledThisWave;
        [ReadOnly] public int bloonsReachedFinalDestinationThisWave;
        [ReadOnly] public int bloonsSpawnedThisWave;
        [ReadOnly] public int bloonSpawnsLeftThisWave;

        [Header("RBE Info:")]
        [ReadOnly] public int totalRBEThisWave;
        [ReadOnly] public int RBEKilledThisWave;
        [ReadOnly] public int RBEReachedFinalDestinationThisWave;
        [ReadOnly] public int RBERegeneratedThisWave;
        [ReadOnly] public int RBESpawnedThisWave;
        [ReadOnly] public int RBESpawnsLeftThisWave;

        [Space(10)]
        [ReadOnly] public int bloonsOnScreen;
        

        [Header("Wave Editor:")]
        public Wave[] waves;
        //public WaveList waves;

        #endregion

        private void Awake() {
            controllerObject = this;
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
                if (currentWave < totalWaves) { 
                    if (GameController.controllerObject.AutoStartNextWave) {
                        StartNextWave();
                    }
                }
            }
            else if (state == SpawnState.WaitingForBloonsToDie) {

                if (bloonSpawnsLeftThisWave <= 0 && bloonsOnScreen <= 0) {
                    waveActive = false;
                    GameControl.GameController.controllerObject.fastForward = false;
                    GameControl.InventoryController.AwardGoldAtEndOfRound();

                    if (currentWave < totalWaves) { 
                        state = SpawnState.RoundEnded;
                    }
                    else {
                        state = SpawnState.GameOver;
                    }
                }
            }
            else if (state == SpawnState.Spawning) {
                // Actively spawning bloons
            }
            else if (state == SpawnState.GameOver) {
                // Life is at 0 || CurrentRound == maxRounds
                GameControl.GameController.controllerObject.fastForward = false;
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
        
        private IEnumerator SpawnWave(Wave _waveToSpawn) {

            totalRBEThisWave = _waveToSpawn.TotalRBE;
            totalBloonsThisWave = _waveToSpawn.TotalBloons;
            state = SpawnState.Spawning;
            int checksPerSecond = 10;

            for (int i = 0; i < _waveToSpawn.waveTypeList.Count; i++) {
                for (int j = 0; j < _waveToSpawn.waveTypeList[i].amount; j++) {
                    Bloon.StandardBloon spawnedBloon = BloonSpawner.SpawnBloon(_waveToSpawn.waveTypeList[i].bloonPrefab, PathController.spawnPoint.position, Quaternion.identity, 0, _waveToSpawn.waveTypeList[i].regrowth, _waveToSpawn.waveTypeList[i].camo);
                        spawnedBloon.originalFamilyTreeIndex = -1;

                    ChangeWaveBloonStats(0, 0, 1);
                    ChangeWaveRBEStats(0, 0, 1, 0);

                    for (int k = 0; k < checksPerSecond; k++)
                        yield return new WaitForSeconds( (_waveToSpawn.waveTypeList[i].interval / GameController.controllerObject.currentGameSpeed) / checksPerSecond);
                }
                yield return new WaitForSeconds(_waveToSpawn.waveTypeList[i].delay / GameController.controllerObject.currentGameSpeed);
            }
            state = SpawnState.WaitingForBloonsToDie;
            yield break;
        }

        #region Change Wave Bloons

        public static void ChangeWaveBloonStats(int _killed, int _reachedFinalDestination, int _spawned) {
            ChangeBloons_Spawned(_spawned);
            ChangeBloons_Killed(_killed);
            ChangeBloons_ReachedFinalDestination(_reachedFinalDestination);
        }
        public static void ChangeBloons_ReachedFinalDestination(int _reachedFinalDestination) {
            controllerObject.bloonsReachedFinalDestinationThisWave += _reachedFinalDestination;
        }

        public static void ChangeBloons_Spawned(int _spawned) {
            controllerObject.bloonsSpawnedThisWave += _spawned;
        }

        public static void ChangeBloons_Killed(int _count) {
            controllerObject.bloonsKilledThisWave += _count;
        }

        #endregion
        
        #region Change Wave RBE

        public static void ChangeWaveRBEStats(int _killed, int _reachedFinalDestination, int _spawned, int _regenerated) {
            ChangeRBE_Killed(_killed);
            ChangeRBE_ReachedFinalDestination(_reachedFinalDestination);
            ChangeRBE_Spawned(_spawned);
            ChangeRBE_Regenerated(_regenerated);
        }
        public static void ChangeRBE_Regenerated(int _regenerated) {
            controllerObject.RBERegeneratedThisWave += _regenerated;
        }

        public static void ChangeRBE_Spawned(int _spawned) {
            controllerObject.RBESpawnedThisWave += _spawned;
        }

        public static void ChangeRBE_ReachedFinalDestination(int _reachedFinalDestination) {
            controllerObject.RBEReachedFinalDestinationThisWave += _reachedFinalDestination;
        }

        public static void ChangeRBE_Killed(int _killed) {
            controllerObject.RBEKilledThisWave += _killed;
        }

        #endregion
    }
}
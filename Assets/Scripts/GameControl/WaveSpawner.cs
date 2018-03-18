using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameControl {

    public class WaveSpawner : MonoBehaviour {

        // TODO: Add a custom Property Drawer for the Wave Editor


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

        #region Classes
        [System.Serializable]
        public class WaveTypeAmount {
            public BloonSpawner.Bloons bloonEnum;
            
            public int amount;
            public float secondsBetweenBloons;
            public bool regrowth;
            public bool camo;
            [Space(5f)]
            public float secondsToWaitAfterSet; //LowPrio: If (last set in wave) { hide in inspector }

            [HideInInspector]
            public Bloon.StandardBloon bloon;
            [Space]
            public int TotalSetRBE;

            public WaveTypeAmount(BloonSpawner.Bloons _bloonEnum, int _amount, float _secondsBetweenBloons, float _SecondsToWaitAfterSet, bool _regrowth, bool _camo) {
                bloonEnum = _bloonEnum; // Which bloon to spawn (enum value)
                amount = _amount; // How many bloons to spawn
                secondsBetweenBloons = _secondsBetweenBloons; // How often to spawn the bloons
                secondsToWaitAfterSet = _SecondsToWaitAfterSet; // How long to wait until next 'set' of bloons to spawn
                regrowth = _regrowth;
                camo = _camo;
            }

            public void SetDerivedAttributes() {
                bloon = DictionaryController.bloonDictionary[bloonEnum];
                TotalSetRBE = bloon.RBE * amount;
            }
        }

        [System.Serializable]
        public class Wave {
            public List<WaveTypeAmount> waveTypeList = new List<WaveTypeAmount>();

            public int TotalBloons {
                get {
                    int bloons = 0;

                    for (int i = 0; i < waveTypeList.Count; i++) {
                        WaveTypeAmount set = waveTypeList[i];
                        bloons += set.amount;
                    }
                    return bloons;
                }
            }

            public int TotalRBE {
                get {
                    int RBE = 0;

                    for (int i = 0; i < waveTypeList.Count; i++) {
                        WaveTypeAmount set = waveTypeList[i];
                        set.SetDerivedAttributes();
                        RBE += set.TotalSetRBE;
                    }
                    return RBE;
                }
            }
        }
        #endregion

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

        [Header("Bloon info:")]
        // Changed this to only change if a Red bloon dies (fixes regrowth)
        public int totalBloonsThisWave;
        public int remainingBloonsThisWave;
        public int bloonsKilledThisWave;
        public int bloonsReachedFinalDestination; 

        public int totalRBEThisWave;
        public int RBEKilledThisWave;
        public int RBERemainingThisWave;
        public int bloonsOnScreen;
        public int RBEReachedFinalDestinationThisWave;
        public int RBERegeneratedThisWave;

        [Header("Wave Editor:")]
        public Wave[] waves;

        #endregion

        private void Awake() {
            controllerObject = GetComponent<WaveSpawner>();
        }

        private void Start() {
            state = SpawnState.GameStart;
            totalWaves = waves.Length;
        }

        private void Update() {
            remainingBloonsThisWave = totalBloonsThisWave - (bloonsKilledThisWave + bloonsReachedFinalDestination);
            RBERemainingThisWave = totalRBEThisWave - (RBEKilledThisWave + RBEReachedFinalDestinationThisWave - RBERegeneratedThisWave);
            bloonsOnScreen = GameController.enemyParent.transform.childCount;
            
            if (state == SpawnState.RoundEnded) {
                // RoundEnded - AutoStart if autostart is active, otherwise wait.
                if (!waveActive) { 
                    if (GameController.controllerObject.AutoStartNextWave) {
                        StartNextWave();
                    }
                }
            }
            else if (state == SpawnState.WaitingForBloonsToDie) {

                if (remainingBloonsThisWave <= 0) {
                    waveActive = false;
                    RoundEndedEvent();
                    state = SpawnState.RoundEnded;
                }
            }
            else if (state == SpawnState.Spawning) {
                // Actively Spawning Bloons -- Do nothing
            }
            else if (state == SpawnState.GameOver) {
                // Life is at 0
            }
        }

        public void StartNextWave() {
            if (state == SpawnState.RoundEnded) {
                if (waves[currentWave] != null) {
                    bloonsKilledThisWave = 0;
                    bloonsReachedFinalDestination = 0;
                    StartCoroutine(SpawnWave(waves[currentWave]));
                    currentWave++;
                    //Debug.Log("Started a new wave...");
                    waveActive = true;
                }
            }
            else if (state == SpawnState.GameStart) {
                if (waves[currentWave-1] != null) {
                    bloonsKilledThisWave = 0;
                    bloonsReachedFinalDestination = 0;
                    StartCoroutine(SpawnWave(waves[currentWave-1]));
                    //Debug.Log("Started first wave...");
                    waveActive = true;
                }
            }
        }

        // TODO: Fix this to better scale with gameSpeed
        private IEnumerator SpawnWave(Wave _waveToSpawn) {

            totalRBEThisWave = _waveToSpawn.TotalRBE;
            totalBloonsThisWave = _waveToSpawn.TotalBloons;
            state = SpawnState.Spawning;
            
            for (int i = 0; i < _waveToSpawn.waveTypeList.Count; i++) {
                for (int j = 0; j < _waveToSpawn.waveTypeList[i].amount; j++) {
                    BloonSpawner.SpawnBloon(_waveToSpawn.waveTypeList[i].bloon, PathController.spawnPoint.position, Quaternion.identity, 0, _waveToSpawn.waveTypeList[i].regrowth, _waveToSpawn.waveTypeList[i].camo);
                    yield return new WaitForSeconds(_waveToSpawn.waveTypeList[i].secondsBetweenBloons / GameController.controllerObject.currentGameSpeed); 
                }
                yield return new WaitForSeconds(_waveToSpawn.waveTypeList[i].secondsToWaitAfterSet / GameController.controllerObject.currentGameSpeed);
            }
            state = SpawnState.WaitingForBloonsToDie;
            yield break;
        }
    }
}
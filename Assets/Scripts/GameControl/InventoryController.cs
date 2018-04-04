using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameControl { 

    public class InventoryController : MonoBehaviour {
        
        public static InventoryController controllerObject;

        [Header("Gold:")]
        public float goldGainMultiplier = 1f;
        public int startGold;
        public float gold;
        public int goldCap = 999999;

        [Space]

        [Header("Life:")]
        public float lifeGainMultiplier = 1f;
        public int startLife;
        public int life;
        public int lifeCap = 999999;

        private void Awake() {
            controllerObject = GetComponent<InventoryController>();
        }
        private void Start() {
            switch (GameController.controllerObject.difficulty) {
                case GameController.Difficulties.Easy:
                    startGold = 650;
                    startLife = 200;
                    //Debug.Log("Difficulty : Easy");
                    break;
                case GameController.Difficulties.Normal:
                    startGold = 650;
                    startLife = 150;
                    //Debug.Log("Difficulty : Normal");
                    break;
                case GameController.Difficulties.Hard:
                    startGold = 650;
                    startLife = 100;
                    //Debug.Log("Difficulty : Hard");
                    break;
                case GameController.Difficulties.Impoppable:
                    startGold = 650;
                    startLife = 1;
                    //Debug.Log("Difficulty : Impoppable");
                    break;
                default:
                    Debug.Log("Difficulty NOT SET");
                    break;
            }

            gold = startGold;
            life = startLife;
            WaveSpawner.RoundEndedEvent += AwardGoldAtEndOfRound;
        }

        private void LateUpdate() {
            if (gold >= goldCap) {
                gold = goldCap;
            }
            if (life >= lifeCap) {
                life = lifeCap;
            }

        }
        public void AwardGoldAtEndOfRound() {
            gold += 99 + WaveSpawner.controllerObject.currentWave;
            Debug.Log("Awarded gold at end of round " + WaveSpawner.controllerObject.currentWave);
        }
    }
}
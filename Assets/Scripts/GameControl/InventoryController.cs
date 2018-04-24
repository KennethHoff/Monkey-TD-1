using System;
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
        public float life;
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
        }

        private void LateUpdate() {
            if (gold >= goldCap) {
                gold = goldCap;
            }
            if (life >= lifeCap) {
                life = lifeCap;
            }

        }
        public static void AwardGoldAtEndOfRound() {
            int goldToAward = 99 + WaveSpawner.controllerObject.currentWave;
            ChangeGold(goldToAward);
            Debug.Log("Awarded " + goldToAward + "gold at end of round " + WaveSpawner.controllerObject.currentWave);
        }

        public static void ChangeInventory(int _life, int _gold) {
            ChangeGold(_life);
            ChangeLife(_gold);
        }

        public static void ChangeLife(int _Life) {
            controllerObject.life += _Life * controllerObject.lifeGainMultiplier;
            // Debug.Log("Life changed: " + _Life + ". Currently: " + controllerObject.life);
        }

        public static void ChangeGold(int _Gold) {
            controllerObject.gold += _Gold * controllerObject.goldGainMultiplier;
            // Debug.Log("Life changed: " + _Gold + ". Currently: " + controllerObject.gold);
        }
    }
}
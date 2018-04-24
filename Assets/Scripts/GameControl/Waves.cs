using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameControl {

    [System.Serializable]
    public class WaveTypeAmount {

        public DictionaryController.Bloons bloonEnum;
        public bool regrowth;
        public bool camo;
        public int amount;
        public float interval;

        [Space(5f)]
        public float delay;

        [HideInInspector]
        public Bloon.StandardBloon bloonPrefab;
        [Space]
        public int TotalSetRBE;

        public void SetDerivedAttributes() {
            bloonPrefab = GameControl.DictionaryController.RetrieveBloonFromBloonDictionary_Enum(bloonEnum);
            TotalSetRBE = bloonPrefab.RBE * amount;
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
}
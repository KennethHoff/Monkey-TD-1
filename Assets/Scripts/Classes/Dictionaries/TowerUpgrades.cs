using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Dictionaries {

    [System.Serializable]
    public class TowerUpgrades {
        public Dictionaries.SpecificTowers.MonkeyTower monkeyTower;
    }
}

namespace Dictionaries.SpecificTowers { 

    [System.Serializable]
    public class MonkeyTower {

        public TowerUpgrade Long_Range_Darts;
        public TowerUpgrade Enhanced_Eyesight;
        public TowerUpgrade Spike_O_Pult;
        public TowerUpgrade Juggernaut;


        public TowerUpgrade Sharp_Shots;
        public TowerUpgrade Razor_Sharp_Shots;
        public TowerUpgrade Triple_Darts;
        public TowerUpgrade Super_Monkey_Fan_Club;
    }
}
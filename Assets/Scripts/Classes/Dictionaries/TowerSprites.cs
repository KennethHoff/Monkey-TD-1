using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Dictionaries {

    [System.Serializable]
    public class TowerSprites {
        public Dictionaries.SpecificTowers.MonkeyTowerSprites dartMonkey;
        public Dictionaries.SpecificTowers.MonkeyTowerSprites tackShooter;
        public Dictionaries.SpecificTowers.MonkeyTowerSprites sniperMonkey;
        public Dictionaries.SpecificTowers.MonkeyTowerSprites boomerangThrower;
        public Dictionaries.SpecificTowers.MonkeyTowerSprites ninjaMonkey;

    }
}

namespace Dictionaries.SpecificTowers {

    [System.Serializable]
    public class MonkeyTowerSprites : BaseTowerSprites { }

    public class BaseTowerSprites {
        
        public Sprite[] towerHUDIcons;
        public int currentTowerHUDIconSprite;

        /*
        
        [Space(10)]
        public Sprite[] towerSprites;
        public int currentTowerSprite;

        */
    }
}
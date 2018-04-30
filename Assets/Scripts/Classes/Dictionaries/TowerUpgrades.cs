using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Dictionaries {

    [System.Serializable]
    public class TowerUpgrades {
        public Dictionaries.SpecificTowers.DartMonkeyUpgrades dartMonkey;
        public Dictionaries.SpecificTowers.TackShooterUpgrades tackShooter;
        public Dictionaries.SpecificTowers.SniperMonkeyUpgrades sniperMonkey;
        public Dictionaries.SpecificTowers.BoomerangThrowerUpgrades boomerangThrower;
        public Dictionaries.SpecificTowers.NinjaMonkeyUpgrades ninjaMonkey;
    }
}

namespace Dictionaries.SpecificTowers { 

    [System.Serializable]
    public class DartMonkeyUpgrades {

        public TowerUpgrade Long_Range_Darts;
        public TowerUpgrade Enhanced_Eyesight;
        public TowerUpgrade Spike_O_Pult;
        public TowerUpgrade Juggernaut;


        public TowerUpgrade Sharp_Shots;
        public TowerUpgrade Razor_Sharp_Shots;
        public TowerUpgrade Triple_Darts;
        public TowerUpgrade Super_Monkey_Fan_Club;
    }

    [System.Serializable]
    public class TackShooterUpgrades {

        public TowerUpgrade Faster_Shooting;
        public TowerUpgrade Even_Faster_Shooting;
        public TowerUpgrade Tack_Sprayer;
        public TowerUpgrade Ring_Of_Fire;


        public TowerUpgrade Extra_Range_Tacks;
        public TowerUpgrade Super_Range_Tacks;
        public TowerUpgrade Blade_Shooter;
        public TowerUpgrade Blade_Maelstrom;
    }

    [System.Serializable]
    public class SniperMonkeyUpgrades {

        public TowerUpgrade Full_Metal_Jacket;
        public TowerUpgrade Point_Five_Oh;
        public TowerUpgrade Deadly_Precision;
        public TowerUpgrade Cripple_MOAB;


        public TowerUpgrade Faster_Firing;
        public TowerUpgrade Night_Vision_Goggles;
        public TowerUpgrade Semi_Automatic_Rifle;
        public TowerUpgrade Supply_Drop;
    }

    [System.Serializable]
    public class BoomerangThrowerUpgrades {

        public TowerUpgrade Multi_Target;
        public TowerUpgrade Glaive_Thrower;
        public TowerUpgrade Glaive_Ricochet;
        public TowerUpgrade Glaive_Lord;


        public TowerUpgrade Sonic_Boom;
        public TowerUpgrade Red_Hot_Rangs;
        public TowerUpgrade Bionic_Boomer;
        public TowerUpgrade Turbo_Charge;
    }

    [System.Serializable]
    public class NinjaMonkeyUpgrades {

        public TowerUpgrade Ninja_Discipline;
        public TowerUpgrade Sharp_Shurikens;
        public TowerUpgrade Double_Shot;
        public TowerUpgrade Bloonjitsu;


        public TowerUpgrade Seeking_Shuriken;
        public TowerUpgrade Distraction;
        public TowerUpgrade Flash_Bomb;
        public TowerUpgrade Sabotage_Supply_Lines;
    }
}
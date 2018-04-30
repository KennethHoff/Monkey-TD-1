using System.Collections.Generic;
using System.Linq;

//TODO: use your own base, so if your absolute base (container of the Powerups list)
//      is called StdTowerBase then change DummyBaseTower below to that
using TOWERBASE = Tower.TowerStats;



namespace Powerups {
    public interface ITowerBase {
        IPowerup GetPowerup(GameControl.DictionaryController.TowerUpgrades i);
    }

    public interface IPowerupable<T> where T : TOWERBASE
    {
        void AddPowerup(PowerupBase<T> pu);
    }

    public abstract class PowerupBase<T> : IPowerup where T : TOWERBASE {
        public PowerupBase(T _tower, GameControl.DictionaryController.TowerUpgrades _UpgradeEnum) {
            this.Tower = _tower;
            this.Enum = _UpgradeEnum;
        }

        public abstract void Powerup();

        GameControl.DictionaryController.TowerUpgrades IPowerup.GetType() {
            return Enum;
        }

        public T Tower {get;set;}
        private GameControl.DictionaryController.TowerUpgrades Enum { get; set; }
    }
}

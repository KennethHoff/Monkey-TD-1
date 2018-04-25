namespace Powerups {
    public interface IPowerup {
        GameControl.DictionaryController.TowerUpgrades GetType();
        void Powerup();
    }
}
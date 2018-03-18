using System.Collections;
using System.Collections.Generic;
using Tower;
using UnityEngine;

namespace Bloon {
    public class RedBloon : StandardBloon {

        protected override void Start() {
            bloonEnum = GameControl.BloonSpawner.Bloons.RedBloon;
            bloonChildToSpawn = GameControl.BloonSpawner.Bloons.Undefined;
            base.Start();
        }
        public override void PopBloon(StandardTower tower, int projectileID) {
            base.PopBloon(tower, projectileID);
            GameControl.WaveSpawner.controllerObject.bloonsKilledThisWave++;
        }
    }
}


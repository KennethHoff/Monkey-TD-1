using System.Collections;
using System.Collections.Generic;
using Tower;
using UnityEngine;

namespace Bloon {
    public class BlueBloon : StandardBloon {

        protected override void Start() {
            bloonEnum = GameControl.BloonSpawner.Bloons.BlueBloon;
            bloonChildToSpawn = GameControl.BloonSpawner.Bloons.RedBloon;
            base.Start();
        }
    }
}


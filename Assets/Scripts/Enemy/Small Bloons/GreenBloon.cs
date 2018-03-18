using System.Collections;
using System.Collections.Generic;
using Tower;
using UnityEngine;

namespace Bloon {
    public class GreenBloon : StandardBloon {

        protected override void Start() {
            bloonEnum = GameControl.BloonSpawner.Bloons.GreenBloon;
            bloonChildToSpawn = GameControl.BloonSpawner.Bloons.BlueBloon;
            base.Start();
        }
    }
}


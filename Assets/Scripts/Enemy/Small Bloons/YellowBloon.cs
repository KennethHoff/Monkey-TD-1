using System.Collections;
using System.Collections.Generic;
using Tower;
using UnityEngine;
using UnityEditor;

namespace Bloon {
    public class YellowBloon : StandardBloon {

        protected override void Start() {
            bloonEnum = GameControl.BloonSpawner.Bloons.YellowBloon;

            /*
            startArmor = 0;
            childrenAmount = 1;
            RBE = 4;
            */
            base.Start();
        }
    }
}


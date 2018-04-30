using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dictionaries {

    [System.Serializable]
    public class ProjectilePrefabs {

        public ProjectileStats Dart_Monkey_Default;
        public ProjectileStats Tack_Shooter_Default;
        public ProjectileStats Boomerang_Thrower_Default;
        public ProjectileStats Ninja_Monkey_Default;

    }
    [System.Serializable]
    public class ProjectileStats {
        
        public Projectile.StandardProjectile projectileObject;
    }
}
using ZeroPhysics.Physics;
using FixMath.NET;

namespace ZeroPhysics.Generic {

    public struct CollisionModel {

        public IPhysicsBody bodyA;
        public IPhysicsBody bodyB;

        CollisionType collisionType;
        public CollisionType CollisionType => collisionType;
        public void SetCollisionType(CollisionType v) => collisionType = v;

        FPVector3 hitDirBA;
        public FPVector3 HitDirBA => hitDirBA;
        public void SetHitDirBA(in FPVector3 v) => hitDirBA = v;

        FP64 firctionCoe_combined;
        public FP64 FirctionCoe_combined => firctionCoe_combined;
        public void SetFirctionCoe_combined(in FP64 v) => firctionCoe_combined = v;

    }

}
using ZeroPhysics.Physics3D;
using FixMath.NET;

namespace ZeroPhysics.Generic
{

    public class CollisionModel
    {

        public IPhysicsBody3D bodyA;
        public IPhysicsBody3D bodyB;

        CollisionType collisionType;
        public CollisionType CollisionType => collisionType;
        public void SetCollisionType(CollisionType v) => collisionType = v;

        FPVector3 hitDirBA;
        public FPVector3 HitDirBA => hitDirBA;
        public void SetHitDirBA(FPVector3 v) => hitDirBA = v;

        FP64 firctionCoe_combined;
        public FP64 FirctionCoe_combined => firctionCoe_combined;
        public void SetFirctionCoe_combined(FP64 v) => firctionCoe_combined = v;

        public CollisionModel()
        {
        }

    }

}
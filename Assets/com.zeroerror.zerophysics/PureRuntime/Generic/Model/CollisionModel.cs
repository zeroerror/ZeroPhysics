using ZeroPhysics.Physics3D;
using FixMath.NET;

namespace ZeroPhysics.Generic
{

    public class CollisionModel
    {

        public PhysicsBody3D bodyA;
        public PhysicsBody3D bodyB;

        CollisionType collisionType;
        public CollisionType CollisionType => collisionType;
        public void SetCollisionType(CollisionType v) => collisionType = v;

        FPVector3 bHitA_Dir;
        public FPVector3 BHitA_Dir => bHitA_Dir;
        public void SetBHitA_Dir(FPVector3 v) => bHitA_Dir = v;

        FP64 firctionCoe_combined;
        public FP64 FirctionCoe_combined => firctionCoe_combined;
        public void SetFirctionCoe_combined(FP64 v) => firctionCoe_combined = v;

        public CollisionModel()
        {
        }

    }

}
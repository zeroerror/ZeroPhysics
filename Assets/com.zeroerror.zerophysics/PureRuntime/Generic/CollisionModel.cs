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

        FPVector3 beHitDirA;
        public FPVector3 BeHitDirA => beHitDirA;
        public void SetBeHitDirA(FPVector3 v) => beHitDirA = v;

        public CollisionModel()
        {
        }

    }

}
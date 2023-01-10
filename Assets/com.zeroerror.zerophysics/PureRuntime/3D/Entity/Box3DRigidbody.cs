using FixMath.NET;
using ZeroPhysics.Generic;

namespace ZeroPhysics.Physics3D
{

    public class Box3DRigidbody : PhysicsBody3D
    {

        ushort instanceID;
        public ushort InstanceID => instanceID;
        public void SetInstanceID(ushort v) => instanceID = v;

        Box3D box;
        public Box3D Box => box;

        TickType tickType;
        public TickType TickType => tickType;
        public void SetTickType(TickType v) => tickType = v;

        FPVector3 totalForce;
        public FPVector3 TotalForce => totalForce;
        public void SetTotalForce(in FPVector3 v) => totalForce = v;

        FP64 mass;
        public FP64 Mass => mass;
        public void SetMass(in FP64 v) => mass = v;

        FPVector3 linearV;
        public FPVector3 LinearV => linearV;
        public void SetLinearV(in FPVector3 v) => linearV = v;

        FP64 bounceCoefficient;
        public FP64 BounceCoefficient => bounceCoefficient;
        public void SetBounceCoefficient(FP64 v) => bounceCoefficient = v;

        FPVector3 beHitDir;
        public FPVector3 BeHitDir => beHitDir;

        PhysicsType3D PhysicsBody3D.PhysicsType => PhysicsType3D.Box3DRigidbody;

        ushort PhysicsBody3D.ID => instanceID;

        public void SetBeHitDir(FPVector3 v) => beHitDir = v;

        public bool IsCollisionStay;

        public Box3DRigidbody(Box3D box)
        {
            this.box = box;
        }

        public BoxType GetBoxType()
        {
            return box.GetBoxType();
        }

        public override string ToString()
        {
            return $"InstanceID:{instanceID}";
        }

    }

}
using FixMath.NET;
using ZeroPhysics.Generic;

namespace ZeroPhysics.Physics3D
{

    public class Box3DRigidbody
    {

        int instanceID;
        public int InstanceID => instanceID;
        public void SetInstanceID(int v) => instanceID = v;

        Box3D box;
        public Box3D Box => box;

        TickType tickType;
        public TickType TickType => tickType;
        public void SetTickType(TickType v) => tickType = v;

        FPVector3 totalForce;
        public FPVector3 TotalForce => totalForce;
        public void SetTotalForce(in FPVector3 v) => totalForce = v;

        FPVector3 frictionForce;
        public FPVector3 FrictionForce => frictionForce;
        public void SetFrictionForce(in FPVector3 v) => frictionForce = v;

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
        public void SetBeHitDir(FPVector3 v) => beHitDir = v;

        public Box3DRigidbody(Box3D box)
        {
            this.box = box;
        }

        public BoxType GetBoxType()
        {
            return box.GetBoxType();
        }

    }

}
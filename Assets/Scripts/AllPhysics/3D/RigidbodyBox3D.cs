using FixMath.NET;
using ZeroPhysics.Generic;

namespace ZeroPhysics.AllPhysics.Physics3D
{

    public class RigidbodyBox3D
    {

        Box3D box;
        public Box3D Box => box;

        TickType tickType;
        public TickType TickType => tickType;
        public void SetTickType(TickType v) => tickType = v;

        FPVector3 force;
        public FPVector3 Force => force;
        public void SetForce(in FPVector3 v) => force = v;

        FPVector3 linearV;
        public FPVector3 LinearV => linearV;
        public void SetLinearV(in FPVector3 v) => linearV = v;

        FP64 gravity;
        public FP64 Gravity => gravity;
        public void SetGravity(in FP64 v) => gravity = v;

        public RigidbodyBox3D(Box3D box)
        {
            this.box = box;
        }

        public BoxType GetBoxType()
        {
            return box.GetBoxType();
        }

    }

}
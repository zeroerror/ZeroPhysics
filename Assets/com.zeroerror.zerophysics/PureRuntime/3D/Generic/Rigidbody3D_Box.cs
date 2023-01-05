using FixMath.NET;
using ZeroPhysics.Generic;

namespace ZeroPhysics.Physics3D
{

    public class Rigidbody3D_Box
    {

        int instanceID;
        public int InstanceID => instanceID;
        public void SetInstanceID(int v) => instanceID = v;

        Box3D box;
        public Box3D Box => box;

        TickType tickType;
        public TickType TickType => tickType;
        public void SetTickType(TickType v) => tickType = v;

        FPVector3 force;
        public FPVector3 Force => force;
        public void SetForce(in FPVector3 v) => force = v;

        FP64 mass;
        public FP64 Mass => mass;
        public void SetMass(in FP64 v) => mass = v;

        FPVector3 linearV;
        public FPVector3 LinearV => linearV;
        public void SetLinearV(in FPVector3 v) => linearV = v;

        public Rigidbody3D_Box(Box3D box)
        {
            this.box = box;
        }

        public BoxType GetBoxType()
        {
            return box.GetBoxType();
        }

    }

}
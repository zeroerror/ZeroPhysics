using FixMath.NET;
using ZeroPhysics.Generic;

namespace ZeroPhysics.Physics3D {

    public class Box3DRigidbody : PhysicsBody3D {

        ushort instanceID;
        public ushort InstanceID => instanceID;
        public void SetInstanceID(ushort v) => instanceID = v;

        public string name;

        Box3D box;
        public Box3D Box => box;

        TickType tickType;
        public TickType TickType => tickType;
        public void SetTickType(TickType v) => tickType = v;

        FPVector3 outForce;
        public FPVector3 OutForce => outForce;
        public void SetOutForce(in FPVector3 v) => outForce = v;

        FP64 mass;
        public FP64 Mass => mass;
        public void SetMass(in FP64 v) => mass = v;

        FPVector3 linearV;
        public FPVector3 LinearV => linearV;
        public void SetLinearV(in FPVector3 v) => linearV = v;

        FP64 bounceCoefficient;
        public FP64 BounceCoefficient => bounceCoefficient;
        public void SetBounceCoefficient(in FP64 v) {
            bounceCoefficient = v;
            epsilon_bounce = v * 4 * FP64.EN1;
        }

        FP64 epsilon_bounce;
        public FP64 Epsilon_bounce => epsilon_bounce;

        PhysicsType3D PhysicsBody3D.PhysicsType => PhysicsType3D.Box3DRigidbody;

        ushort PhysicsBody3D.ID => instanceID;

        FPVector3 mtv;
        public FPVector3 MTV => mtv;
        public void SetMTV(in FPVector3 v) => mtv = v;

        public Box3DRigidbody(Box3D box) {
            this.box = box;
        }

        public BoxType GetBoxType() {
            return box.GetBoxType();
        }

        public void AddMTV(in FPVector3 v) {
            mtv += v;
        }

        public void ApplyMTV() {
            box.SetCenter(box.Center + mtv);
            mtv = FPVector3.Zero;
        }

        public override string ToString() {
            return $"BoxRB <Name>:{name}  <ID>:{instanceID}";
        }

    }

}
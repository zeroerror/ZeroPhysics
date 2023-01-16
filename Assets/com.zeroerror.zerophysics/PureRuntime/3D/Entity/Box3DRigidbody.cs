using FixMath.NET;
using ZeroPhysics.Generic;

namespace ZeroPhysics.Physics3D {

    public class Box3DRigidbody : IPhysicsBody3D {

        public ushort InstanceID => box.InstanceID;
        public void SetInstanceID(ushort v) => box.SetInstanceID(v);

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
        public void SetLinearV(in FPVector3 v) {
            linearV = v;
        }

        FP64 bounceCoefficient;
        public FP64 BounceCoefficient => bounceCoefficient;
        public void SetBounceCoefficient(in FP64 v) {
            bounceCoefficient = v;
        }

        // Interface
        PhysicsType3D IPhysicsBody3D.PhysicsType => PhysicsType3D.Box3DRigidbody;
        ushort IPhysicsBody3D.ID => InstanceID;
        bool IPhysicsBody3D.IsTrigger => box.IsTrigger;

        public Box3DRigidbody(Box3D box) {
            this.box = box;
        }

        public BoxType GetBoxType() {
            return box.GetBoxType();
        }

        public void ApplyMTV(in FPVector3 mtv) {
            box.SetCenter(box.Center + mtv);
        }

        public override string ToString() {
            return $"BoxRB <Name>:{name}  <ID>:{box.InstanceID}";
        }

    }

}
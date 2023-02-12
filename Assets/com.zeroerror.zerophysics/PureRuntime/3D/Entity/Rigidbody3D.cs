using FixMath.NET;
using ZeroPhysics.Generic;

namespace ZeroPhysics.Physics3D {

    public class Rigidbody3D {

        ushort rbID;
        public ushort RBID => rbID;
        public void SetRBID(ushort v) => rbID = v;

        public string name;

        IPhysicsBody3D body;
        public IPhysicsBody3D Body => body;

        TickType tickType;
        public TickType TickType => tickType;
        public void SetTickType(TickType v) => tickType = v;

        FPVector3 outForce;
        public FPVector3 OutForce => outForce;
        public void SetOutForce(in FPVector3 v) => outForce = v;

        FPVector3 dirtyOutForce;
        internal FPVector3 DirtyOutForce => dirtyOutForce;
        internal void SetDirtyOutForce(in FPVector3 v) => dirtyOutForce = v;

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
        public void SetBounceCoefficient(in FP64 v) => bounceCoefficient = v;

        public Rigidbody3D(IPhysicsBody3D body) {
            this.body = body;
        }

        bool isDirty;
        public bool IsDirty => isDirty;
        public void SetIsDirty(bool v) => isDirty = v;

        public void ApplyMTV(in FPVector3 mtv) {
            var trans = body.Trans;
            body.Trans.SetCenter(trans.Center + mtv);
        }

        public uint GetBodyKey() {
            uint key = (uint)rbID;
            return key;
        }

        public override string ToString() {
            return $"Rigidbody === <Name>:{name}  <ID>:{rbID}";
        }

    }

}
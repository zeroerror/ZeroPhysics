using FixMath.NET;
using ZeroPhysics.Generic;

namespace ZeroPhysics.Physics3D {

    public class Rigidbody3D {

        ushort rbID;
        public ushort RBID => rbID;
        public void SetRBID(ushort v) => rbID = v;
        
        public string name;

        PhysicsType3D PhysicsType => PhysicsType3D.RB;

        IPhysicsBody3D body;
        public IPhysicsBody3D Body => body;

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
        public void SetBounceCoefficient(in FP64 v) => bounceCoefficient = v;

        public Rigidbody3D(IPhysicsBody3D body) {
            this.body = body;
        }

        public void ApplyMTV(in FPVector3 mtv) {
            if (body is Cube cube) {
                cube.SetCenter(cube.Center + mtv);
                return;
            }
        }

        public uint GetBodyKey() {
            byte t = (byte)PhysicsType;
            uint key = (uint)rbID;
            key |= (uint)t << 16;
            return key;
        }

        public override string ToString() {
            return $"<Name>:{name}  <ID>:{rbID}";
        }

    }

}
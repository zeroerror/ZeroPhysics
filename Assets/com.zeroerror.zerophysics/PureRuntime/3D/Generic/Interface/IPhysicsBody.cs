using FixMath.NET;
using ZeroPhysics.Generic;
using ZeroPhysics.Physics.Generic;

namespace ZeroPhysics.Physics {

    public interface IPhysicsBody {

        PhysicsType PhysicsType { get; }
        Transform Trans { get; }

        void SetBodyID(ushort v);
        ushort BodyID { get; }

        void SetIsTrigger(bool flag);
        bool IsTrigger { get; }
        Rigidbody RB { get; }

        FP64 FrictionCoe { get; }
        void SetFirctionCoe(in FP64 v);

    }

    public static class PhysicsBodyExtensions {

        public static uint GetBodyKey(this IPhysicsBody a) {
            byte t = (byte)a.PhysicsType;
            ushort id = a.BodyID;
            uint key = (uint)id;
            key |= (uint)t << 16;
            return key;
        }

    }

}
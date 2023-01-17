using FixMath.NET;
using ZeroPhysics.Generic;

namespace ZeroPhysics.Physics3D {

    public interface IPhysicsBody3D {

        PhysicsType3D PhysicsType { get; }
        TransformComponent3D Trans { get; }

        void SetBodyID(ushort v);
        ushort BodyID { get; }

        void SetIsTrigger(bool flag);
        bool IsTrigger { get; }

        bool IsRB { get; }
        void SetIsRB(bool flag);

        FP64 FrictionCoe { get; }
        void SetFirctionCoe(in FP64 v);

    }

    public static class PhysicsBody3DExtensions {

        public static uint GetBodyKey(this IPhysicsBody3D a) {
            byte t = (byte)a.PhysicsType;
            ushort id = a.BodyID;
            uint key = (uint)id;
            key |= (uint)t << 16;
            return key;
        }

    }

}
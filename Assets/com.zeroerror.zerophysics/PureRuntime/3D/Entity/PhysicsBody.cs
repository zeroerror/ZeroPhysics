using ZeroPhysics.Generic;

namespace ZeroPhysics.Physics3D {

    public interface IPhysicsBody3D {

        PhysicsType3D PhysicsType { get; }
        ushort ID { get; }
        bool IsTrigger { get; }

    }

    public static class PhysicsBody3DExtensions {

        public static uint GetKey(this IPhysicsBody3D a) {
            byte t = (byte)a.PhysicsType;
            ushort id = a.ID;
            uint key = (uint)id;
            key |= (uint)t << 16;
            return key;
        }

    }

}
namespace ZeroPhysics.Generic {

    public enum PhysicsType3D : byte {

        Box3D = 0,
        Box3DRigidbody = 10,
        Box3DRigidbody1,

    }

    public static class PhysicsType3DExtensions {

        public static bool IsStatic(this PhysicsType3D v) {
            return (byte)v < 10;
        }

    }

}
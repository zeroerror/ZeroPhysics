namespace ZeroPhysics.Generic {

    public enum PhysicsType3D : byte {
        Cube = 0,
        RB = 10,
    }

    public static class PhysicsType3DExtensions {

        public static bool IsStatic(this PhysicsType3D v) {
            return (byte)v < 10;
        }

    }

}
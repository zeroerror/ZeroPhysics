using FixMath.NET;
using UnityEngine;
using ZeroPhysics.Generic;

namespace ZeroPhysics.AllPhysics.Physics3D
{

    public struct Box3DModel
    {

        FPVector3 center;
        public FPVector3 Center => center;

        FPQuaternion rotation;
        public FPQuaternion Rot => rotation;

        FPVector3 scaledSize;
        public FPVector3 ScaledSize => scaledSize;

        public FPVector3[] vertices;
        public FPVector3 Max => vertices[1];
        public FPVector3 Min => vertices[6];

        public void Ctor(TransformComponent trans, in FPVector3 size)
        {
            this.vertices = new FPVector3[8];
            this.center = trans.Center;
            this.scaledSize = size * trans.Scale;
            this.rotation = trans.Rotation;
            FPVector3 halfScaledSize = scaledSize * FP64.Half;
            FPVector3 p0 = new FPVector3(-halfScaledSize.x, halfScaledSize.y, halfScaledSize.z);
            FPVector3 p1 = new FPVector3(halfScaledSize.x, halfScaledSize.y, halfScaledSize.z);
            FPVector3 p2 = new FPVector3(-halfScaledSize.x, halfScaledSize.y, -halfScaledSize.z);
            FPVector3 p3 = new FPVector3(halfScaledSize.x, halfScaledSize.y, -halfScaledSize.z);
            FPVector3 p4 = new FPVector3(-halfScaledSize.x, -halfScaledSize.y, halfScaledSize.z);
            FPVector3 p5 = new FPVector3(halfScaledSize.x, -halfScaledSize.y, halfScaledSize.z);
            FPVector3 p6 = new FPVector3(-halfScaledSize.x, -halfScaledSize.y, -halfScaledSize.z);
            FPVector3 p7 = new FPVector3(halfScaledSize.x, -halfScaledSize.y, -halfScaledSize.z);
            var rot = this.rotation;
            if (rot != FPQuaternion.Identity)
            {
                p0 = rot * p0;
                p1 = rot * p1;
                p2 = rot * p2;
                p3 = rot * p3;
                p4 = rot * p4;
                p5 = rot * p5;
                p6 = rot * p6;
                p7 = rot * p7;
            }
            vertices[0] = p0 + center;
            vertices[1] = p1 + center;
            vertices[2] = p2 + center;
            vertices[3] = p3 + center;
            vertices[4] = p4 + center;
            vertices[5] = p5 + center;
            vertices[6] = p6 + center;
            vertices[7] = p7 + center;
        }

        public BoxType GetBoxType()
        {
            return rotation == FPQuaternion.Identity ? BoxType.AABB : BoxType.OBB;
        }

        public FPVector2 GetAxisX_SelfProjectionSub()
        {
            var halfX = scaledSize.x * FP64.Half;
            return new FPVector2(-halfX, halfX);
        }

        public FPVector2 GetAxisY_SelfProjectionSub()
        {
            var halfY = scaledSize.y * FP64.Half;
            return new FPVector2(-halfY, halfY);
        }

        public FPVector2 GetAxisZ_SelfProjectionSub()
        {
            var halfZ = scaledSize.z * FP64.Half;
            return new FPVector2(-halfZ, halfZ);
        }

        public Axis3D GetAxisX()
        {
            Axis3D axis = new Axis3D();
            axis.origin = center;
            var dir = FPVector3.Right;
            if (rotation != FPQuaternion.Identity) dir = rotation * dir;
            axis.dir = dir;
            return axis;
        }

        public Axis3D GetAxisY()
        {
            Axis3D axis = new Axis3D();
            axis.origin = center;
            var dir = FPVector3.Up;
            if (rotation != FPQuaternion.Identity) dir = rotation * dir;
            axis.dir = dir;
            return axis;
        }

        public Axis3D GetAxisZ()
        {
            Axis3D axis = new Axis3D();
            axis.origin = center;
            var dir = FPVector3.Forward;
            if (rotation != FPQuaternion.Identity) dir = rotation * dir;
            axis.dir = dir;
            return axis;
        }

    }

}

using ZeroPhysics.Generic;
using FixMath.NET;

namespace ZeroPhysics.AllPhysics.Physics3D
{

    public class Box3D
    {

        int instanceID;
        public int InstanceID => instanceID;
        public void SetInstanceID(int v) => instanceID = v;

        // ====== Component
        // - Trans
        TransformComponent trans;
        public TransformComponent Trans => trans;

        public FPVector3 Center => trans.Center;
        public void SetCenter(in FPVector3 v) => trans.SetCenter(v);

        public FPQuaternion Rotation => trans.Rotation;
        public void SetRotation(in FPQuaternion v) => trans.SetRotation(v);

        public FPVector3 Scale => trans.Scale;
        public void SetScale(in FPVector3 v) => trans.SetScale(v);

        FP64 width;
        public FP64 Width => width;
        public void SetWidth(in FP64 v) => width = v;
        public FP64 GetWidthHalfScaled() => width * Scale.x * FP64.Half;

        public FP64 height;
        public FP64 Height => height;
        public void SetHeight(in FP64 v) => height = v;
        public FP64 GetHeightHalfScaled() => height * Scale.y * FP64.Half;

        public FP64 length;
        public FP64 Length => length;
        public void SetLength(in FP64 v) => length = v;
        public FP64 GetLengthHalfScaled() => length * Scale.z * FP64.Half;

        public Box3D()
        {
            trans = new TransformComponent();
        }

        public BoxType GetBoxType()
        {
            return trans.Rotation == FPQuaternion.Identity ? BoxType.AABB : BoxType.OBB;
        }

        public Box3DModel GetModel()
        {
            var model = new Box3DModel();
            model.Ctor(trans, new FPVector3(width, height, length));
            return model;
        }

    }

}

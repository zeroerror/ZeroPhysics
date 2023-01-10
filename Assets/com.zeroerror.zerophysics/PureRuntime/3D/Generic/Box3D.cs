using FixMath.NET;
using ZeroPhysics.Generic;

namespace ZeroPhysics.Physics3D
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

        FPVector3 size;
        public FPVector3 Size => size;
        public void SetSize(in FPVector3 v) => size = v;

        Box3DModel model;

        FP64 frictionCoe;
        public FP64 FrictionCoe => frictionCoe;
        public void SetFirctionCoe(FP64 v) => frictionCoe = v;

        FP64 firctionCoe_combined;
        public FP64 FirctionCoe_combined => firctionCoe_combined;
        public void SetFirctionCoe_combined(FP64 v) => firctionCoe_combined = v;

        public Box3D()
        {
            trans = new TransformComponent();
            model = new Box3DModel(trans, size);
        }

        public BoxType GetBoxType()
        {
            return trans.Rotation == FPQuaternion.Identity ? BoxType.AABB : BoxType.OBB;
        }

        public Box3DModel GetModel()
        {
            model.Update(trans, size);
            return model;
        }

    }

}

using FixMath.NET;
using ZeroPhysics.Generic;

namespace ZeroPhysics.Physics3D {

    public class Cube : IPhysicsBody3D {

        public string name;

        ushort bodyID;
        public ushort BodyID => bodyID;
        public void SetBodyID(ushort v) => bodyID = v;

        Rigidbody3D rb;
        public Rigidbody3D RB => rb;
        public void SetRB(Rigidbody3D v) => rb = v;

        bool isTrigger;
        public bool IsTrigger => isTrigger;
        public void SetIsTrigger(bool v) => isTrigger = v;

        // ====== Component
        // - Trans
        TransformComponent3D trans;
        public TransformComponent3D Trans => trans;

        public FPVector3 Center => trans.Center;
        public void SetCenter(in FPVector3 v) => trans.SetCenter(v);

        public FPQuaternion Rotation => trans.Rotation;
        public void SetRotation(in FPQuaternion v) => trans.SetRotation(v);

        public FPVector3 Scale => trans.Scale;
        public void SetScale(in FPVector3 v) => trans.SetScale(v);

        FPVector3 size;
        public FPVector3 Size => size;
        public void SetSize(in FPVector3 v) => size = v;

        CubeModel model;

        FP64 frictionCoe;
        public void SetFirctionCoe(in FP64 v) => frictionCoe = v;

        PhysicsType3D IPhysicsBody3D.PhysicsType => PhysicsType3D.Cube;
        ushort IPhysicsBody3D.BodyID => bodyID;
        FP64 IPhysicsBody3D.FrictionCoe => frictionCoe;
        TransformComponent3D IPhysicsBody3D.Trans => trans;

        public Cube(in FPVector3 pos, in FPQuaternion rotation, in FPVector3 scale, in FPVector3 size) {
            trans = new TransformComponent3D();
            trans.SetCenter(pos);
            trans.SetRotation(rotation);
            trans.SetScale(scale);
            this.size = size;
            model = new CubeModel(trans, size);
        }

        public CubeType GetCubeType() {
            return trans.Rotation == FPQuaternion.Identity ? CubeType.AABB : CubeType.OBB;
        }

        public CubeModel GetModel() {
            model.Update(trans, size);
            return model;
        }


        public override string ToString() {
            return $"Cube === <Name>:{name} <ID>:{bodyID}";
        }

    }

}

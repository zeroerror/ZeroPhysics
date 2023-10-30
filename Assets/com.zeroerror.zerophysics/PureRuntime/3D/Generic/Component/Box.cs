using FixMath.NET;
using ZeroPhysics.Generic;
using ZeroPhysics.Physics.Generic;

namespace ZeroPhysics.Physics {

    public class Box : IPhysicsBody {

        public string name;

        ushort bodyID;
        public ushort BodyID => bodyID;
        public void SetBodyID(ushort v) => bodyID = v;

        Rigidbody rb;
        public Rigidbody RB => rb;
        public void SetRB(Rigidbody v) => rb = v;

        bool isTrigger;
        public bool IsTrigger => isTrigger;
        public void SetIsTrigger(bool v) => isTrigger = v;

        // ====== Component
        // - Trans
        Transform trans;
        public Transform Trans => trans;

        public FPVector3 Center => trans.Center;
        public void SetCenter(in FPVector3 v) => trans.SetCenter(v);

        public FPQuaternion Rotation => trans.Rotation;
        public void SetRotation(in FPQuaternion v) => trans.SetRotation(v);

        public FPVector3 Scale => trans.Scale;
        public void SetScale(in FPVector3 v) => trans.SetScale(v);

        FPVector3 size;
        public FPVector3 Size => size;
        public void SetSize(in FPVector3 v) => size = v;

        BoxModel model;

        FP64 frictionCoe;
        public void SetFirctionCoe(in FP64 v) => frictionCoe = v;

        ushort IPhysicsBody.BodyID => bodyID;
        FP64 IPhysicsBody.FrictionCoe => frictionCoe;
        Transform IPhysicsBody.Trans => trans;

        PhysicsType IPhysicsBody.PhysicsType => PhysicsType.Box;
        bool IPhysicsBody.IsTrigger => throw new System.NotImplementedException();
        Rigidbody IPhysicsBody.RB => throw new System.NotImplementedException();

        public Box(in FPVector3 pos, in FPQuaternion rotation, in FPVector3 scale, in FPVector3 size) {
            trans = new Transform();
            trans.SetCenter(pos);
            trans.SetRotation(rotation);
            trans.SetScale(scale);
            this.size = size;
            model = new BoxModel(trans, size);
        }

        public BoxType GetCubeType() {
            return trans.Rotation == FPQuaternion.Identity ? BoxType.AABB : BoxType.OBB;
        }

        public BoxModel GetModel() {
            model.Update(trans, size);
            return model;
        }


        public override string ToString() {
            return $"Cube === <Name>:{name} <ID>:{bodyID}";
        }

        void IPhysicsBody.SetBodyID(ushort v) {
            throw new System.NotImplementedException();
        }

        void IPhysicsBody.SetIsTrigger(bool flag) {
            throw new System.NotImplementedException();
        }

        void IPhysicsBody.SetFirctionCoe(in FP64 v) {
            throw new System.NotImplementedException();
        }
    }

}

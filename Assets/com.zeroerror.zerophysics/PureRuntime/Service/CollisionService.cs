using System.Collections.Generic;
using FixMath.NET;
using ZeroPhysics.Generic;
using ZeroPhysics.Physics3D;

namespace ZeroPhysics.Service {

    public class CollisionService {

        Dictionary<ulong, CollisionModel> collisionDic;

        public CollisionService() {
            collisionDic = new Dictionary<ulong, CollisionModel>();
        }

        public void AddCollision(IPhysicsBody3D a, IPhysicsBody3D b) {
            // 保证左边一定是RB
            SwapRBToLeft(ref a, ref b);

            var ida = a.GetKey();
            var idb = b.GetKey();
            var dicKey = CombineDicKey(ida, idb);
            bool getFromDic = collisionDic.TryGetValue(dicKey, out var collision);
            if (getFromDic && collision.CollisionType == CollisionType.Enter) {
                collision.SetCollisionType(CollisionType.Stay);
                collisionDic[dicKey] = collision;
                // UnityEngine.Debug.Log($"Collision Stay ------------  A: {a} &&&&&&&& {b}");
                return;
            }
            if (!getFromDic) {
                // 添加Enter
                collision = new CollisionModel();
                collision.bodyA = a;
                collision.bodyB = b;
                collisionDic.Add(dicKey, collision);
            }
            if (!getFromDic || collision.CollisionType == CollisionType.None || collision.CollisionType == CollisionType.Exit) {
                collision.SetCollisionType(CollisionType.Enter);
                collisionDic[dicKey] = collision;
                return;
                // UnityEngine.Debug.Log($"Collision Enter ------------  A: {a} &&&&&&&& {b}");
            }
        }

        public void RemoveCollision(IPhysicsBody3D a, IPhysicsBody3D b) {
            var ida = a.GetKey();
            var idb = b.GetKey();
            var dicKey = CombineDicKey(ida, idb);
            if (!collisionDic.TryGetValue(dicKey, out var collision)) {
                return;
            }

            if (collision.CollisionType == CollisionType.Enter || collision.CollisionType == CollisionType.Stay) {
                collision.SetCollisionType(CollisionType.Exit);
                collisionDic[dicKey] = collision;
                // UnityEngine.Debug.Log($"Collision Exit ------------  A: {a} &&&&&&&& {b}");
            } else if (collision.CollisionType == CollisionType.Exit) {
                collisionDic.Remove(dicKey);
                // UnityEngine.Debug.Log($"Collision Dic Remove  ------------  A: {a} &&&&&&&& {b}");
            }

        }

        public CollisionModel[] GetAllCollisions() {
            var values = collisionDic.Values;
            int count = values.Count;
            CollisionModel[] infoArray = new CollisionModel[count];
            values.CopyTo(infoArray, 0);
            return infoArray;
        }

        public void UpdateHitDirBA(IPhysicsBody3D a, IPhysicsBody3D b, in FPVector3 hitDirBA) {
            var ida = a.GetKey();
            var idb = b.GetKey();
            var dicKey = CombineDicKey(ida, idb);
            if (!collisionDic.TryGetValue(dicKey, out var collision)) {
                return;
            }

            if (hitDirBA == FPVector3.Zero) {
                return;
            }

            collision.SetHitDirBA(hitDirBA);
            collisionDic[dicKey] = collision;
        }

        ulong CombineDicKey(uint ida, uint idb) {
            SwapBiggerToLeft(ref ida, ref idb);
            ulong key = (ulong)(idb);
            key |= (ulong)ida << 32;
            return key;
        }

        bool SwapBiggerToLeft(ref uint ida, ref uint idb) {
            if (idb < ida) {
                return false;
            }

            ida = ida ^ idb;
            idb = ida ^ idb;
            ida = ida ^ idb;
            return true;
        }

        void SwapRBToLeft(ref IPhysicsBody3D bodyA, ref IPhysicsBody3D bodyB) {
            byte typeA = (byte)bodyA.PhysicsType;
            byte typeB = (byte)bodyB.PhysicsType;
            if (typeB > typeA) {
                IPhysicsBody3D temp = bodyA;
                bodyA = bodyB;
                bodyB = temp;
            }
        }

    }

}
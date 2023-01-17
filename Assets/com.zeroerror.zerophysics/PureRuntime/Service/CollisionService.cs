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

        public void AddCollision(Rigidbody3D rb, IPhysicsBody3D body) {
            var ida = rb.GetBodyKey();
            var idb = body.GetBodyKey();
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
                collision.bodyA = rb.Body;
                collision.bodyB = body;
                collisionDic.Add(dicKey, collision);
            }
            if (!getFromDic || collision.CollisionType == CollisionType.None || collision.CollisionType == CollisionType.Exit) {
                collision.SetCollisionType(CollisionType.Enter);
                collisionDic[dicKey] = collision;
                UnityEngine.Debug.Log($"Collision Enter ------------  rb: {rb} &&&&&&&& body {body}");
                return;
            }
        }

        public void AddCollision(Rigidbody3D rb1, Rigidbody3D rb2) {
            var ida = rb1.GetBodyKey();
            var idb = rb2.GetBodyKey();
            var dicKey = CombineDicKey(ida, idb);
            SwapBiggerToLeft(ref ida, ref idb);
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
                collision.bodyA = rb1.Body;
                collision.bodyB = rb2.Body;
                collisionDic.Add(dicKey, collision);
            }
            if (!getFromDic || collision.CollisionType == CollisionType.None || collision.CollisionType == CollisionType.Exit) {
                collision.SetCollisionType(CollisionType.Enter);
                collisionDic[dicKey] = collision;
                UnityEngine.Debug.Log($"Collision Enter ------------  rb: {rb1} &&&&&&&& body {rb2}");
                return;
            }
        }

        public void RemoveCollision(Rigidbody3D rb, IPhysicsBody3D body) {
            var ida = rb.GetBodyKey();
            var idb = body.GetBodyKey();
            var dicKey = CombineDicKey(ida, idb);
            if (!collisionDic.TryGetValue(dicKey, out var collision)) {
                return;
            }

            if (collision.CollisionType == CollisionType.Enter || collision.CollisionType == CollisionType.Stay) {
                collision.SetCollisionType(CollisionType.Exit);
                collisionDic[dicKey] = collision;
                UnityEngine.Debug.Log($"Collision Exit ------------  rb: {rb} &&&&&&&& body: {body}");
            } else if (collision.CollisionType == CollisionType.Exit) {
                collisionDic.Remove(dicKey);
                UnityEngine.Debug.Log($"Collision Dic Remove ------------  rb: {rb} &&&&&&&& body: {body}");
            }
        }

        public void RemoveCollision(Rigidbody3D rb1, Rigidbody3D rb2) {
            var ida = rb1.GetBodyKey();
            var idb = rb2.GetBodyKey();
            var dicKey = CombineDicKey(ida, idb);
            SwapBiggerToLeft(ref ida, ref idb);
            if (!collisionDic.TryGetValue(dicKey, out var collision)) {
                return;
            }

            if (collision.CollisionType == CollisionType.Enter || collision.CollisionType == CollisionType.Stay) {
                collision.SetCollisionType(CollisionType.Exit);
                collisionDic[dicKey] = collision;
                UnityEngine.Debug.Log($"Collision Exit ------------  rb1: {rb1} &&&&&&&& rb2: {rb2}");
            } else if (collision.CollisionType == CollisionType.Exit) {
                collisionDic.Remove(dicKey);
                UnityEngine.Debug.Log($"Collision Dic Remove ------------  rb1: {rb1} &&&&&&&& rb2: {rb2}");
            }
        }

        public CollisionModel[] GetAllCollisions() {
            var values = collisionDic.Values;
            int count = values.Count;
            CollisionModel[] infoArray = new CollisionModel[count];
            values.CopyTo(infoArray, 0);
            return infoArray;
        }

        public void UpdateHitDirBA(Rigidbody3D rb, IPhysicsBody3D body, in FPVector3 hitDirBA) {
            var ida = rb.GetBodyKey();
            var idb = body.GetBodyKey();
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

        public void UpdateHitDirBA(Rigidbody3D rb1, Rigidbody3D rb2, in FPVector3 hitDirBA) {
            var ida = rb1.GetBodyKey();
            var idb = rb2.GetBodyKey();
            var dicKey = CombineDicKey(ida, idb);
            SwapBiggerToLeft(ref ida, ref idb);
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

    }

}
using System.Collections.Generic;
using FixMath.NET;
using ZeroPhysics.Generic;
using ZeroPhysics.Physics3D;

namespace ZeroPhysics.Service {

    public class CollisionService {

        Dictionary<ulong, CollisionModel> collisionDic_RS;
        Dictionary<ulong, CollisionModel> collisionDic_RR;

        public CollisionService() {
            collisionDic_RS = new Dictionary<ulong, CollisionModel>();
            collisionDic_RR = new Dictionary<ulong, CollisionModel>();
        }

        public void AddCollision_RS(Rigidbody3D rb, IPhysicsBody3D body) {
            var rbID = rb.GetBodyKey();
            var bodyID = body.GetBodyKey();
            var dicKey = CombineDicKey(rbID, bodyID);
            bool getFromDic = collisionDic_RS.TryGetValue(dicKey, out var collision);
            if (getFromDic && collision.CollisionType == CollisionType.Enter) {
                collision.SetCollisionType(CollisionType.Stay);
                collisionDic_RS[dicKey] = collision;
                // Logger.Log($"Collision Stay ------------  {rb} &&&&&&&& {body}");
                return;
            }
            if (!getFromDic) {
                // 添加Enter
                collision = new CollisionModel();
                collision.bodyA = rb.Body;
                collision.bodyB = body;
                collisionDic_RS.Add(dicKey, collision);
            }
            if (!getFromDic || collision.CollisionType == CollisionType.None || collision.CollisionType == CollisionType.Exit) {
                collision.SetCollisionType(CollisionType.Enter);
                collisionDic_RS[dicKey] = collision;
                // Logger.Log($"Collision Enter ------------   {rb} &&&&&&&& {body}");
                return;
            }
        }

        public void AddCollision_RR(Rigidbody3D rb1, Rigidbody3D rb2) {
            var rbID1 = rb1.GetBodyKey();
            var rbID2 = rb2.GetBodyKey();
            var dicKey = CombineDicKey(rbID1, rbID2);
            SwapBiggerToLeft(ref rbID1, ref rbID2);
            bool getFromDic = collisionDic_RR.TryGetValue(dicKey, out var collision);
            if (getFromDic && collision.CollisionType == CollisionType.Enter) {
                collision.SetCollisionType(CollisionType.Stay);
                collisionDic_RR[dicKey] = collision;
                // Logger.Log($"Collision Stay ------------  {rb1} &&&&&&&& {rb2}");
                return;
            }
            if (!getFromDic) {
                // 添加Enter
                collision = new CollisionModel();
                collision.bodyA = rb1.Body;
                collision.bodyB = rb2.Body;
                collisionDic_RR.Add(dicKey, collision);
            }
            if (!getFromDic || collision.CollisionType == CollisionType.None || collision.CollisionType == CollisionType.Exit) {
                collision.SetCollisionType(CollisionType.Enter);
                collisionDic_RR[dicKey] = collision;
                // Logger.Log($"Collision Enter ------------  rb: {rb1} &&&&&&&& body {rb2}");
                return;
            }
        }

        public void RemoveCollision_RS(Rigidbody3D rb, IPhysicsBody3D body) {
            var ida = rb.GetBodyKey();
            var idb = body.GetBodyKey();
            var dicKey = CombineDicKey(ida, idb);
            if (!collisionDic_RS.TryGetValue(dicKey, out var collision)) {
                return;
            }

            if (collision.CollisionType == CollisionType.Enter || collision.CollisionType == CollisionType.Stay) {
                collision.SetCollisionType(CollisionType.Exit);
                collisionDic_RS[dicKey] = collision;
                // Logger.Log($"Collision Exit ------------  rb: {rb} &&&&&&&& body: {body}");
            } else if (collision.CollisionType == CollisionType.Exit) {
                collisionDic_RS.Remove(dicKey);
                // Logger.Log($"Collision Dic Remove ------------  rb: {rb} &&&&&&&& body: {body}");
            }
        }

        public void RemoveCollision_RR(Rigidbody3D rb1, Rigidbody3D rb2) {
            var rbID1 = rb1.GetBodyKey();
            var rbID2 = rb2.GetBodyKey();
            var dicKey = CombineDicKey(rbID1, rbID2);
            SwapBiggerToLeft(ref rbID1, ref rbID2);
            if (!collisionDic_RR.TryGetValue(dicKey, out var collision)) {
                return;
            }

            if (collision.CollisionType == CollisionType.Enter || collision.CollisionType == CollisionType.Stay) {
                collision.SetCollisionType(CollisionType.Exit);
                collisionDic_RR[dicKey] = collision;
                // Logger.Log($"Collision Exit ------------  rb1: {rb1} &&&&&&&& rb2: {rb2}");
            } else if (collision.CollisionType == CollisionType.Exit) {
                collisionDic_RR.Remove(dicKey);
                // Logger.Log($"Collision Dic Remove ------------  rb1: {rb1} &&&&&&&& rb2: {rb2}");
            }
        }

        public CollisionModel[] GetAllCollisions_RS() {
            var values = collisionDic_RS.Values;
            int count = values.Count;
            CollisionModel[] collisions = new CollisionModel[count];
            values.CopyTo(collisions, 0);
            return collisions;
        }

        public CollisionModel[] GetAllCollisions_RR() {
            var values = collisionDic_RR.Values;
            int count = values.Count;
            CollisionModel[] collisions = new CollisionModel[count];
            values.CopyTo(collisions, 0);
            return collisions;
        }

        public void UpdateHitDirBA_RS(Rigidbody3D rb, IPhysicsBody3D body, in FPVector3 hitDirBA) {
            var rbID = rb.GetBodyKey();
            var bodyID = body.GetBodyKey();
            var dicKey = CombineDicKey(rbID, bodyID);
            if (!collisionDic_RS.TryGetValue(dicKey, out var collision)) {
                return;
            }

            if (hitDirBA == FPVector3.Zero) {
                return;
            }

            collision.SetHitDirBA(hitDirBA);
            collisionDic_RS[dicKey] = collision;
        }

        public void UpdateHitDirBA_RR(Rigidbody3D rb1, Rigidbody3D rb2, in FPVector3 hitDirBA) {
            var rbID1 = rb1.GetBodyKey();
            var rbID2 = rb2.GetBodyKey();
            var dicKey = CombineDicKey(rbID1, rbID2);
            SwapBiggerToLeft(ref rbID1, ref rbID2);
            if (!collisionDic_RR.TryGetValue(dicKey, out var collision)) {
                return;
            }

            if (hitDirBA == FPVector3.Zero) {
                return;
            }

            collision.SetHitDirBA(hitDirBA);
            collisionDic_RR[dicKey] = collision;
        }

        public void UpdateFrictionCoeCombined_RS(Rigidbody3D rb, IPhysicsBody3D body, in FP64 frictionCombined) {
            var rbID = rb.GetBodyKey();
            var bodyID = body.GetBodyKey();
            var dicKey = CombineDicKey(rbID, bodyID);
            SwapBiggerToLeft(ref rbID, ref bodyID);
            if (!collisionDic_RS.TryGetValue(dicKey, out var collision)) {
                return;
            }

            collision.SetFirctionCoe_combined(frictionCombined);
            collisionDic_RS[dicKey] = collision;
        }

        public void UpdateFrictionCoeCombined_RR(Rigidbody3D rb1, Rigidbody3D rb2, in FP64 frictionCombined) {
            var rbID1 = rb1.GetBodyKey();
            var rbID2 = rb2.GetBodyKey();
            var dicKey = CombineDicKey(rbID1, rbID2);
            SwapBiggerToLeft(ref rbID1, ref rbID2);
            if (!collisionDic_RR.TryGetValue(dicKey, out var collision)) {
                return;
            }

            collision.SetFirctionCoe_combined(frictionCombined);
            collisionDic_RR[dicKey] = collision;
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
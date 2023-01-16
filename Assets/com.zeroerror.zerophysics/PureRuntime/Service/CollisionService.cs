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
            bool getFromDic = collisionDic.TryGetValue(dicKey, out var info);
            if (getFromDic && info.CollisionType == CollisionType.Enter) {
                info.SetCollisionType(CollisionType.Stay);
            }
            if (!getFromDic) {
                // 添加Enter
                info = new CollisionModel();
                info.bodyA = a;
                info.bodyB = b;
                collisionDic.Add(dicKey, info);
            }
            if (!getFromDic || info.CollisionType == CollisionType.None || info.CollisionType == CollisionType.Exit) {
                info.SetCollisionType(CollisionType.Enter);
            }

            // UnityEngine.Debug.Log($"Collision {info.CollisionType.ToString()} ------------  A: {a} &&&&&&&& {b}");
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

        public bool HasCollision(IPhysicsBody3D a) {
            var id = a.GetKey();
            foreach (var key in collisionDic.Keys) {
                var id1 = (uint)key;
                var id2 = (uint)(key >> 32);
                if (id == id1 || id == id2) {
                    var col = collisionDic[key];
                    if (col.CollisionType == CollisionType.Enter || col.CollisionType == CollisionType.Stay) {
                        return true;
                    }
                }
            }

            return false;
        }

        public bool TryGetCollision(IPhysicsBody3D a, out CollisionModel collision) {
            collision = null;
            var id = a.GetKey();
            foreach (var key in collisionDic.Keys) {
                var id1 = (uint)key;
                var id2 = (uint)(key >> 32);
                if (id == id1 || id == id2) {
                    var col = collisionDic[key];
                    if (col.CollisionType != CollisionType.None) {
                        collision = col;
                        return true;
                    }
                }
            }

            return false;
        }

        public bool TryGetCollision(IPhysicsBody3D a, IPhysicsBody3D b, out CollisionModel collision) {
            collision = null;
            var ida = a.GetKey();
            var idb = b.GetKey();
            var dicKey = CombineDicKey(ida, idb);
            return collisionDic.TryGetValue(dicKey, out collision) && collision.CollisionType != CollisionType.None;
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
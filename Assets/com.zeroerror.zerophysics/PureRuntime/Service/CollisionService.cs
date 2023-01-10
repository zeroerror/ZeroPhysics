using System.Collections.Generic;
using ZeroPhysics.Generic;
using ZeroPhysics.Physics3D;

namespace ZeroPhysics.Service
{

    public class CollisionService
    {

        Dictionary<ulong, Collision> infoDic;

        public CollisionService()
        {
            infoDic = new Dictionary<ulong, Collision>();
        }

        public void AddCollision(PhysicsBody3D a, PhysicsBody3D b)
        {
            var ida = CombinePhysicsBodyKey(a);
            var idb = CombinePhysicsBodyKey(b);
            var dicKey = CombineDicKey(ida, idb);
            if (infoDic.TryGetValue(dicKey, out var info))
            {
                // 已存在,修改为stay
                info.collisionType = CollisionType.Stay;
            }
            else
            {
                // 添加Enter
                info = new Collision();
                info.body_a = a;
                info.body_b = b;
                info.collisionType = CollisionType.Enter;
                infoDic.Add(dicKey, info);
            }
        }

        public void RemoveCollision(PhysicsBody3D a, PhysicsBody3D b)
        {
            var ida = CombinePhysicsBodyKey(a);
            var idb = CombinePhysicsBodyKey(b);
            var dicKey = CombineDicKey(ida, idb);
            if (!infoDic.TryGetValue(dicKey, out var info))
            {
                return;
            }

            if (info.collisionType != CollisionType.Exit)
            {
                info.collisionType = CollisionType.Exit;
            }
            else
            {
                infoDic.Remove(dicKey);
            }
        }

        // API
        public Collision[] GetAllCollisions()
        {
            var values = infoDic.Values;
            int count = values.Count;
            Collision[] infoArray=new Collision[count];
            values.CopyTo(infoArray, 0);
            return infoArray;
        }

        ulong CombineDicKey(uint ida, uint idb)
        {
            if (idb < ida)
            {
                ida = ida ^ idb;
                idb = ida ^ idb;
                ida = ida ^ idb;
            }
            ulong key = (ulong)(idb);
            key |= (ulong)ida << 32;
            return key;
        }

        uint CombinePhysicsBodyKey(PhysicsBody3D a)
        {
            byte t = (byte)a.PhysicsType;
            ushort id = a.ID;
            uint key = (uint)id;
            key |= (uint)t << 16;
            return key;
        }

    }

}
using System.Collections.Generic;
using UnityEngine;
using ZeroPhysics.Extensions;
using ZeroPhysics.Physics;

namespace ZeroPhysics.Sample
{

    public class Sample_Physics_AABB : MonoBehaviour
    {

        bool isRun = false;

        Box[] cubes;
        UnityEngine.Transform[] boxColliders;
        public UnityEngine.Transform Cubes;
        PhysicsWorld3DCore physicsCore;

        public void Start()
        {
            if (Cubes == null) return;
            isRun = true;

            physicsCore = new PhysicsWorld3DCore(new FixMath.NET.FPVector3(0, -10, 0));

            var bcCount = Cubes.childCount;
            boxColliders = new UnityEngine.Transform[bcCount];
            for (int i = 0; i < bcCount; i++)
            {
                var bc = Cubes.GetChild(i);
                boxColliders[i] = bc;
            }

            cubes = new Box[bcCount];
            var setterAPI = physicsCore.SetterAPI;
            for (int i = 0; i < bcCount; i++)
            {
                var bcTF = boxColliders[i].transform;
                cubes[i] = setterAPI.SpawnCube(bcTF.position.ToFPVector3(), bcTF.rotation.ToFPQuaternion(), bcTF.localScale.ToFPVector3(), Vector3.one.ToFPVector3());
            }
        }

        public void OnDrawGizmos()
        {
            if (!isRun) return;
            if (boxColliders == null) return;
            if (cubes == null) return;

            // Gizmos.DrawLine(Vector3.zero + Vector3.up * 10f, Vector3.zero + Vector3.down * 10f);
            // Gizmos.DrawLine(Vector3.zero + Vector3.left * 10f, Vector3.zero + Vector3.right * 10f);
            // Gizmos.DrawLine(Vector3.zero + Vector3.forward * 10f, Vector3.zero + Vector3.back * 10f);

            Dictionary<int, Box> collisionCubeDic = new Dictionary<int, Box>();
            for (int i = 0; i < cubes.Length - 1; i++)
            {
                for (int j = i + 1; j < cubes.Length; j++)
                {
                    if (Intersect3DUtil.HasCollision(cubes[i], cubes[j]))
                    {
                        collisionCubeDic[i] = cubes[i];
                        if (!collisionCubeDic.ContainsKey(j))
                        {
                            collisionCubeDic[j] = cubes[j];
                        }
                    }
                }
            }


            for (int i = 0; i < cubes.Length; i++)
            {
                var bc = boxColliders[i];
                var cube = cubes[i];
                UpdateCube(bc.transform, cube);
                Gizmos.color = Color.green;
                DrawCubePoint(cube);
                if (collisionCubeDic.ContainsKey(i))
                {
                    Gizmos.color = Color.red;
                }
                GizmosExtention.DrawPhysicsBody(cube);
            }

        }

        void UpdateCube(UnityEngine.Transform src, Box cube)
        {
            cube.SetCenter(src.position.ToFPVector3());
            cube.SetScale(src.localScale.ToFPVector3());
        }

        void DrawCubePoint(Box cube)
        {
            var min = cube.GetModel().Min.ToVector3();
            var max = cube.GetModel().Max.ToVector3();
            Gizmos.DrawSphere(min, 0.1f);
            Gizmos.DrawSphere(max, 0.1f);
        }

    }

}
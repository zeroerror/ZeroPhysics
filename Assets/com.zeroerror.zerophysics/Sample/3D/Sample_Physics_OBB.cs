using UnityEngine;
using FixMath.NET;
using ZeroPhysics.Physics;
using ZeroPhysics.Extensions;
using ZeroPhysics.Physics.Generic;

namespace ZeroPhysics.Sample
{

    public class Sample_Physics_OBB : MonoBehaviour
    {

        bool canRun = false;

        public UnityEngine.Transform Cubees;
        Box[] cubes;
        UnityEngine.Transform[] boxTfs;

        int[] collsionArray;

        PhysicsWorld3DCore physicsCore;

        void Start()
        {
            if (Cubees == null) return;
            canRun = true;
            physicsCore = new PhysicsWorld3DCore(new FPVector3(0, -10, 0));
            InitCubes();
        }

        void FixedUpdate()
        {
        }

        void InitCubes()
        {
            var bcCount = Cubees.childCount;
            collsionArray = new int[bcCount];
            boxTfs = new UnityEngine.Transform[bcCount];
            for (int i = 0; i < bcCount; i++)
            {
                var bc = Cubees.GetChild(i);
                boxTfs[i] = bc;
            }

            var setterAPI = physicsCore.SetterAPI;
            cubes = new Box[bcCount];
            for (int i = 0; i < bcCount; i++)
            {
                var bcTF = boxTfs[i].transform;
                cubes[i] = setterAPI.SpawnCube(bcTF.position.ToFPVector3(), bcTF.rotation.ToFPQuaternion(), bcTF.localScale.ToFPVector3(), Vector3.one.ToFPVector3());
            }
            Debug.Log($"Total Cube: {bcCount}");
        }

        public void OnDrawGizmos()
        {
            if (!canRun) return;
            if (boxTfs == null) return;
            if (cubes == null) return;

            // - Collision 
            for (int i = 0; i < collsionArray.Length; i++) { collsionArray[i] = 0; }
            for (int i = 0; i < cubes.Length - 1; i++)
            {
                for (int j = i + 1; j < cubes.Length; j++)
                {
                    if (Intersect3DUtil.HasCollision(cubes[i], cubes[j])) { collsionArray[i] = 1; collsionArray[j] = 1; }
                }
            }

            // - Projection
            Axis axis3D = new Axis();
            axis3D.origin = FPVector3.Zero;
            axis3D.dir = FPVector3.UnitX;
            Gizmos.DrawLine((axis3D.origin - 100 * axis3D.dir).ToVector3(), (axis3D.origin + 100 * axis3D.dir).ToVector3());

            // - Update And DrawCube
            for (int i = 0; i < cubes.Length; i++)
            {
                var bc = boxTfs[i];
                var cube = cubes[i];
                UpdateCube(bc.transform, cube);
                Gizmos.color = Color.green;
                GizmosExtention.DrawPhysicsBody(cube);
                if (collsionArray[i] == 1) Gizmos.color = Color.red;
                DrawProjectionSub(axis3D, cube);
            }

        }

        void DrawProjectionSub(Axis axis3D, Box cube)
        {
            var model = cube.GetModel();
            var proj = Projection3DUtils.GetProjectionSub(model, axis3D);
            Gizmos.color = Color.white;
            Gizmos.color = Color.black;
            axis3D.dir.Normalize();
            Gizmos.DrawLine((axis3D.dir * proj.x + axis3D.origin).ToVector3(), (axis3D.dir * proj.y + axis3D.origin).ToVector3());
        }

        void UpdateCube(UnityEngine.Transform src, Box cube)
        {
            cube.SetCenter(src.position.ToFPVector3());
            cube.SetScale(src.localScale.ToFPVector3());
            cube.SetRotation(src.rotation.ToFPQuaternion());
        }

    }

}
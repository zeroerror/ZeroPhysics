using System.Collections.Generic;
using UnityEngine;
using FixMath.NET;
using ZeroPhysics.Physics;
using ZeroPhysics.Generic;
using ZeroPhysics.Extensions;

namespace ZeroPhysics.Sample {

    public class Sample_Physics_Raycast : MonoBehaviour {

        public UnityEngine.Transform rayStart;
        public UnityEngine.Transform rayEnd;

        public UnityEngine.Transform boxRoot;
        public BoxType boxType;
        UnityEngine.Transform[] box_tfs;
        Box[] cubes;

        public UnityEngine.Transform sphereRoot;
        UnityEngine.Transform[] sphere_tfs;
        Sphere[] spheres;

        Generic.Ray ray;

        PhysicsWorld3DCore physicsCore;

        void Start() {
            if (boxRoot == null) return;
            if (sphereRoot == null) return;
            isRun = true;
            physicsCore = new PhysicsWorld3DCore(new FPVector3(0, 10, 0));

            var count = boxRoot.childCount;
            box_tfs = new UnityEngine.Transform[count];
            for (int i = 0; i < count; i++) {
                box_tfs[i] = boxRoot.GetChild(i);
            }

            var setterAPI = physicsCore.SetterAPI;
            cubes = new Box[count];
            for (int i = 0; i < count; i++) {
                var tf = box_tfs[i].transform;
                var pos = tf.position.ToFPVector3();
                var rotation = tf.rotation.ToFPQuaternion();
                var localScale = tf.localScale.ToFPVector3();
                cubes[i] = setterAPI.SpawnCube(pos, rotation, localScale, Vector3.one.ToFPVector3());
            }
            Debug.Log($"Total Cube: {count}");

            count = sphereRoot.childCount;
            sphere_tfs = new UnityEngine.Transform[count];
            for (int i = 0; i < count; i++) {
                sphere_tfs[i] = sphereRoot.GetChild(i);
            }
            spheres = new Sphere[count];
            for (int i = 0; i < count; i++) {
                var tf = sphere_tfs[i].transform;
                var pos = tf.position.ToFPVector3();
                var rotation = tf.rotation.ToFPQuaternion();
                var localScale = tf.localScale.ToFPVector3();
                var size = Vector3.one.ToFPVector3();
                spheres[i] = setterAPI.SpawnSphere(pos, rotation, localScale, size);
            }
            Debug.Log($"Total Sphere: {count}");

            var rayStartPos = rayStart.position;
            var rayEndPos = rayEnd.position;
            var posDiff = (rayEndPos - rayStartPos);
            ray = new Generic.Ray(rayStartPos.ToFPVector3(), posDiff.normalized.ToFPVector3(), FP64.ToFP64(posDiff.magnitude));
        }

        bool isRun = false;
        bool[] collisionList_box = new bool[100];
        bool[] collisionList_sphere = new bool[100];
        List<Vector3> hitPointList = new List<Vector3>();

        public void OnDrawGizmos() {
            if (!isRun) return;
            for (int i = 0; i < collisionList_box.Length; i++) { collisionList_box[i] = false; }
            for (int i = 0; i < collisionList_sphere.Length; i++) { collisionList_sphere[i] = false; }
            hitPointList.Clear();

            var rayStartPos = rayStart.position;
            var rayEndPos = rayEnd.position;
            ray.origin = rayStartPos.ToFPVector3();
            var posDiff = (rayEndPos - rayStartPos);
            ray.dir = posDiff.normalized.ToFPVector3();
            ray = new Generic.Ray(rayStartPos.ToFPVector3(), posDiff.normalized.ToFPVector3(), FP64.ToFP64(posDiff.magnitude));

            bool hasCollision = false;

            // - Cube
            for (int i = 0; i < cubes.Length; i++) {
                var b = cubes[i];
                UpdateCube(box_tfs[i], b);
                if (Raycast3DUtils.RayCubeWithPoints(ray, b.GetModel(), out var p1, out var p2)) {
                    collisionList_box[i] = true;
                    if (p1 != FPVector3.Zero) hitPointList.Add(p1.ToVector3());
                    if (p2 != FPVector3.Zero) hitPointList.Add(p2.ToVector3());
                    hasCollision = true;
                }
            }
            for (int i = 0; i < cubes.Length; i++) {
                Gizmos.color = Color.green;
                if (collisionList_box[i]) Gizmos.color = Color.red;
                GizmosExtention.DrawPhysicsBody(cubes[i]);
            }

            // - Sphere
            for (int i = 0; i < spheres.Length; i++) {
                var s = spheres[i];
                UpdateSphere3D(sphere_tfs[i], s);
                if (Raycast3DUtils.RayWithSphere(ray, s, out var hps)) {
                    collisionList_sphere[i] = true;
                    hps.ForEach((p) => {
                        hitPointList.Add(p.ToVector3());
                    });
                    hasCollision = true;
                }
            }

            Gizmos.color = Color.red;
            for (int i = 0; i < spheres.Length; i++) {
                if (collisionList_sphere[i]) {
                    DrawSphere3D(spheres[i]);
                }
            }

            // - Ray
            Gizmos.color = Color.green;
            if (hasCollision) Gizmos.color = Color.red;
            Gizmos.DrawLine(ray.origin.ToVector3(), ray.origin.ToVector3() + (ray.dir * ray.length).ToVector3());

            // - Hit Points
            Gizmos.color = Color.white;
            hitPointList.ForEach((p) => {
                Gizmos.DrawSphere(p, 0.08f);
            });
        }
        void UpdateCube(UnityEngine.Transform src, Box cube) {
            cube.SetCenter(src.position.ToFPVector3());
            cube.SetScale(src.localScale.ToFPVector3());
            cube.SetRotation(src.rotation.ToFPQuaternion());
        }

        void DrawSphere3D(Sphere sphere) {
            Gizmos.DrawSphere(sphere.Center.ToVector3(), sphere.Radius_scaled.AsFloat());
        }

        void UpdateSphere3D(UnityEngine.Transform src, Sphere sphere) {
            sphere.SetCenter(src.position.ToFPVector3());
            sphere.SetScale(src.localScale.ToFPVector3());
        }

    }

}
using UnityEngine;
using FixMath.NET;
using ZeroPhysics.Physics2D;
using ZeroPhysics.Physics2D.Generic;
using ZeroPhysics.Extensions;

namespace ZeroPhysics.Sample {

    public class Sample_Physics2D_SphereAndBox : MonoBehaviour {

        bool isRun = false;

        Box2D[] allBoxes;
        Circle[] allSpheres;
        Transform[] tfs;
        public Transform spheresAndBoxes;

        public Box2DType rectangleType;

        int[] collsionArray;

        public void Start() {
            if (spheresAndBoxes == null) return;
            isRun = true;

            var bcCount = spheresAndBoxes.childCount;
            collsionArray = new int[bcCount];
            tfs = new Transform[bcCount];
            for (int i = 0; i < bcCount; i++) {
                var bc = spheresAndBoxes.GetChild(i);
                tfs[i] = bc;
            }

            allBoxes = new Box2D[bcCount];
            allSpheres = new Circle[bcCount];
            int rectangleCount = 0;
            int sphereCount = 0;
            for (int i = 0; i < bcCount; i++) {
                var bcTF = tfs[i].transform;
                if (bcTF.GetComponent<BoxCollider>()) {
                    allBoxes[i] = new Box2D(bcTF.position.ToFPVector2(), 1, 1, FP64.ToFP64(bcTF.rotation.eulerAngles.z), bcTF.localScale.ToFPVector2());
                    allBoxes[i].SetBox2DType(rectangleType);
                    rectangleCount++;
                } else if (bcTF.GetComponent<SphereCollider>()) {
                    allSpheres[i] = new Circle(bcTF.position.ToFPVector2(), FP64.ToFP64(bcTF.GetComponent<SphereCollider>().radius), FP64.ToFP64(bcTF.rotation.eulerAngles.z), FP64.ToFP64(bcTF.localScale.x));
                    sphereCount++;
                }
            }
            Debug.Log($"Total Box: {rectangleCount}");
            Debug.Log($"Total Sphere: {sphereCount}");
        }

        public void OnDrawGizmos() {
            if (!isRun) return;
            if (tfs == null) return;
            if (allBoxes == null) return;

            for (int i = 0; i < collsionArray.Length; i++) {
                collsionArray[i] = 0;
            }

            for (int i = 0; i < allBoxes.Length - 1; i++) {
                var rectangle1 = allBoxes[i];
                if (rectangle1 == null) continue;
                for (int j = i + 1; j < allBoxes.Length; j++) {
                    var rectangle2 = allBoxes[j];
                    if (rectangle2 == null) continue;
                    if (IntersectUtil2D.HasCollision(rectangle1, rectangle2)) {
                        collsionArray[i] = 1;
                        collsionArray[j] = 1;
                    }
                }
            }

            for (int i = 0; i < allSpheres.Length - 1; i++) {
                var sphere1 = allSpheres[i];
                if (sphere1 == null) continue;
                for (int j = i + 1; j < allSpheres.Length; j++) {
                    var sphere2 = allSpheres[j];
                    if (sphere2 == null) continue;
                    if (IntersectUtil2D.HasCollision(sphere1, sphere2)) {
                        collsionArray[i] = 1;
                        collsionArray[j] = 1;
                    }
                }
            }

            for (int i = 0; i < allSpheres.Length; i++) {
                var sphere = allSpheres[i];
                if (sphere == null) continue;
                for (int j = 0; j < allBoxes.Length; j++) {
                    var rectangle = allBoxes[j];
                    if (rectangle == null) continue;
                    if (IntersectUtil2D.HasCollision(sphere, rectangle)) {
                        collsionArray[i] = 1;
                        collsionArray[j] = 1;
                    }
                }
            }

            for (int i = 0; i < allBoxes.Length; i++) {
                var bc = tfs[i];
                var rectangle = allBoxes[i];
                if (rectangle == null) continue;
                UpdateBox(bc.transform, rectangle);
                Gizmos.color = Color.green;
                DrawBoxPoint(rectangle);
                if (collsionArray[i] == 1) { Gizmos.color = Color.red; DrawBoxBorder(rectangle); }
            }

            for (int i = 0; i < allSpheres.Length; i++) {
                var bc = tfs[i];
                var sphere = allSpheres[i];
                if (sphere == null) continue;
                UpdateSphere(bc.transform, sphere);
                Gizmos.color = Color.green;
                var b = sphere.Box;
                if (collsionArray[i] == 1) {
                    Gizmos.color = Color.red;
                    DrawSphereBorder(sphere);
                }
            }

        }

        void DrawProjectionSub(Axis2D axis2D, Box2D rectangle) {
            var proj = rectangle.GetProjectionSub(axis2D);
            Gizmos.color = Color.white;
            Gizmos.color = Color.black;
            Gizmos.DrawLine((axis2D.dir * proj.x + axis2D.center).ToVector2(), (axis2D.dir * proj.y + axis2D.center).ToVector2());
        }

        void UpdateBox(Transform src, Box2D rectangle) {
            rectangle.UpdateCenter(src.position.ToFPVector2());
            rectangle.UpdateScale(src.localScale.ToFPVector2());
            rectangle.UpdateRotAngle(FP64.ToFP64(src.rotation.eulerAngles.z));
        }

        void UpdateSphere(Transform src, Circle sphere) {
            sphere.UpdateCenter(src.position.ToFPVector2());
            sphere.UpdateScale(FP64.ToFP64(src.localScale.x));
        }

        void DrawBoxPoint(Box2D rectangle) {
            var a = rectangle.A.ToVector2();
            var b = rectangle.B.ToVector2();
            var c = rectangle.C.ToVector2();
            var d = rectangle.D.ToVector2();
            Gizmos.color = Color.red;
            float size = 0.08f;
            Gizmos.DrawSphere(a, size);
            Gizmos.color = Color.blue;
            Gizmos.DrawSphere(b, size);
            Gizmos.color = Color.yellow;
            Gizmos.DrawSphere(c, size);
            Gizmos.color = Color.green;
            Gizmos.DrawSphere(d, size);
            Gizmos.color = Color.red;
        }

        void DrawBoxBorder(Box2D rectangle) {
            var a = rectangle.A.ToVector2();
            var b = rectangle.B.ToVector2();
            var c = rectangle.C.ToVector2();
            var d = rectangle.D.ToVector2();
            Gizmos.DrawLine(a, b);
            Gizmos.DrawLine(b, c);
            Gizmos.DrawLine(c, d);
            Gizmos.DrawLine(d, a);
        }

        void DrawSphereBorder(Circle sphere) {
            Gizmos.DrawSphere(sphere.Center.ToVector2(), sphere.Radius.AsFloat());
        }

    }

}
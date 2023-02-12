using System.Collections.Generic;
using UnityEngine;
using FixMath.NET;
using ZeroPhysics.Extensions;
using ZeroPhysics.Generic;
using ZeroPhysics.Physics2D;

namespace ZeroPhysics.Sample
{

    public class Sample_Phsics2D_OBB : MonoBehaviour
    {

        bool isRun = false;

        Rectangle[] rectangles;
        BoxCollider[] bcs;
        public Transform Boxes;

        public void Start()
        {
            if (Boxes == null) return;
            isRun = true;

            var bcCount = Boxes.childCount;
            bcs = new BoxCollider[bcCount];
            for (int i = 0; i < bcCount; i++)
            {
                var bc = Boxes.GetChild(i);
                bcs[i] = bc.GetComponent<BoxCollider>();
            }

            rectangles = new Rectangle[bcCount];
            for (int i = 0; i < bcCount; i++)
            {
                var bcTF = bcs[i].transform;
                rectangles[i] = new Rectangle(bcTF.position.ToFPVector2(), 1, 1, FP64.ToFP64(bcTF.rotation.z), bcTF.localScale.ToFPVector2());
                rectangles[i].SetRectangleType(RectangleType.OBB);
            }
        }

        public void OnDrawGizmos()
        {
            if (!isRun) return;
            if (bcs == null) return;
            if (rectangles == null) return;

            Dictionary<int, Rectangle> collisionBoxDic = new Dictionary<int, Rectangle>();
            for (int i = 0; i < rectangles.Length - 1; i++)
            {
                for (int j = i + 1; j < rectangles.Length; j++)
                {
                    if (IntersectUtil2D.HasCollision(rectangles[i], rectangles[j]))
                    {
                        if (!collisionBoxDic.ContainsKey(i)) collisionBoxDic[i] = rectangles[i];
                        if (!collisionBoxDic.ContainsKey(j)) collisionBoxDic[j] = rectangles[j];
                    }
                }
            }

            Gizmos.DrawLine(Vector3.zero + Vector3.up * 10f, Vector3.zero + Vector3.down * 10f);
            Gizmos.DrawLine(Vector3.zero + Vector3.left * 10f, Vector3.zero + Vector3.right * 10f);

            for (int i = 0; i < rectangles.Length; i++)
            {
                var bc = bcs[i];
                var rectangle = rectangles[i];
                UpdateBox(bc.transform, rectangle);
                Gizmos.color = Color.green;
                DrawBoxPoint(rectangle);
                if (collisionBoxDic.ContainsKey(i))
                {
                    Gizmos.color = Color.red;
                }
                DrawBoxBorder(rectangle);
            }

        }

        void DrawBoxBorder(Rectangle rectangle)
        {
            Gizmos.DrawLine(rectangle.A.ToVector2(), (rectangle.B.ToVector2()));
            Gizmos.DrawLine(rectangle.B.ToVector2(), (rectangle.C.ToVector2()));
            Gizmos.DrawLine(rectangle.C.ToVector2(), (rectangle.D.ToVector2()));
            Gizmos.DrawLine(rectangle.D.ToVector2(), (rectangle.A.ToVector2()));
        }

        void DrawBoxPoint(Rectangle rectangle)
        {
            var a = rectangle.A;
            var b = rectangle.B;
            var c = rectangle.C;
            var d = rectangle.D;
            Gizmos.DrawSphere(a.ToVector2(), 0.1f);
            Gizmos.DrawSphere(b.ToVector2(), 0.1f);
            Gizmos.DrawSphere(c.ToVector2(), 0.1f);
            Gizmos.DrawSphere(d.ToVector2(), 0.1f);
        }

        void UpdateBox(Transform src, Rectangle rectangle)
        {
            rectangle.UpdateCenter(src.position.ToFPVector2());
            rectangle.UpdateScale(src.localScale.ToFPVector2());
            rectangle.UpdateRotAngle(FP64.ToFP64(src.rotation.eulerAngles.z));
        }
    }

}
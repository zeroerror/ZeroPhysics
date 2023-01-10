using UnityEngine;
using FixMath.NET;
using ZeroPhysics.Physics3D;
using ZeroPhysics.Extensions;

namespace ZeroPhysics.Sample
{

    public class Sample_Physics3D : MonoBehaviour
    {

        bool canRun = false;

        public Transform rbBoxRoot;
        Transform[] rbBoxTfs;

        public Transform boxRoot;
        Transform[] boxTfs;

        PhysicsWorld3DCore physicsCore;
        void Start()
        {
            if (rbBoxRoot == null) return;
            canRun = true;
            physicsCore = new PhysicsWorld3DCore(new FPVector3(0, -10, 0));
            InitBox3Ds();
        }

        void Update()
        {
            if (!canRun) return;
            if (rbBoxTfs == null) return;
            if (boxTfs == null) return;

            var getterAPI = physicsCore.GetterAPI;
            var rbBoxes = getterAPI.GetAllRBBoxes();
            var boxes = getterAPI.GetAllBoxes();
            for (int i = 0; i < rbBoxes.Count; i++)
            {
                var bc = rbBoxTfs[i];
                var rb = rbBoxes[i];
                var box = rb.Box;
                UpdateBox(bc.transform, box);
                rb.SetBounceCoefficient(FP64.ToFP64(bounce));
            }

            for (int i = 0; i < boxes.Count; i++)
            {
                var bc = boxTfs[i];
                var box = boxes[i];
                UpdateBox(bc.transform, box);
            }
        }

        void FixedUpdate()
        {
            physicsCore.Tick(FP64.ToFP64(UnityEngine.Time.fixedDeltaTime));
        }

        public void OnDrawGizmos()
        {
            if (!canRun) return;
            if (rbBoxTfs == null) return;
            if (boxTfs == null) return;

            var getterAPI = physicsCore.GetterAPI;

            var rbBoxes = getterAPI.GetAllRBBoxes();
            for (int i = 0; i < rbBoxes.Count; i++)
            {
                var box = rbBoxes[i].Box;
                Gizmos.color = Color.green;
                box.DrawBoxPoint();
                box.DrawBoxBorder();
            }

            var boxes = getterAPI.GetAllBoxes();
            for (int i = 0; i < boxes.Count; i++)
            {
                var box = boxes[i];
                Gizmos.color = Color.black;
                box.DrawBoxPoint();
                box.DrawBoxBorder();
            }
        }

        void InitBox3Ds()
        {
            var setterAPI = physicsCore.SetterAPI;

            var rbCount = rbBoxRoot.childCount;
            rbBoxTfs = new Transform[rbCount];
            for (int i = 0; i < rbCount; i++)
            {
                rbBoxTfs[i] = rbBoxRoot.GetChild(i);
            }
            for (int i = 0; i < rbCount; i++)
            {
                var tf = rbBoxTfs[i].transform;
                var rb = setterAPI.SpawnRBBox(tf.position.ToFPVector3(), tf.rotation.ToFPQuaternion(), tf.localScale.ToFPVector3(), Vector3.one.ToFPVector3());
            }
            Debug.Log($"Total RBBox: {rbCount}");

            var boxCount = boxRoot.childCount;
            boxTfs = new Transform[boxCount];
            for (int i = 0; i < boxCount; i++)
            {
                boxTfs[i] = boxRoot.GetChild(i);
            }
            for (int i = 0; i < boxCount; i++)
            {
                var tf = boxTfs[i].transform;
                setterAPI.SpawnBox(tf.position.ToFPVector3(), tf.rotation.ToFPQuaternion(), tf.localScale.ToFPVector3(), Vector3.one.ToFPVector3());
            }
            Debug.Log($"Total Box: {boxCount}");
        }

        void UpdateBox(Transform src, Box3D box)
        {
            box.SetScale(src.localScale.ToFPVector3());
            box.SetRotation(src.rotation.ToFPQuaternion());
            src.position = box.Center.ToVector3();
        }

        float bounce = 1;
        void OnGUI()
        {
            GUILayout.BeginHorizontal();
            GUILayout.Label($"弹性系数:{bounce}");
            bounce = GUILayout.HorizontalSlider(bounce, 0, 1, GUILayout.Width(100));
            GUILayout.EndHorizontal();
        }

    }

}
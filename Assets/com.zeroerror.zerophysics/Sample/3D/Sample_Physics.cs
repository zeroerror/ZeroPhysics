using UnityEngine;
using FixMath.NET;
using ZeroPhysics.Physics;
using ZeroPhysics.Extensions;
using ZeroPhysics.Generic;

namespace ZeroPhysics.Sample {

    public class Sample_Physics : MonoBehaviour {

        bool canRun = false;

        [Header("打印调试信息")]
        public bool debugMode;

        UnityEngine.Transform[] rbTFs;

        UnityEngine.Transform[] cubeTFs;

        public int maxSimulateRate = 10;
        int physicsSimulateRate = 1;

        PhysicsWorld3DCore physicsCore;

        FP64 restoreTime;
        FP64 intervalTime;

        void Start() {

            canRun = true;
            physicsCore = new PhysicsWorld3DCore(new FPVector3(0, -10, 0));
            InitCubes();
            intervalTime = 1 / FP64.ToFP64(60);
            Logger.isEnable = debugMode;
        }

        void OnDestroy() {
            Logger.isEnable = false;
        }

        void Update() {

            if (!canRun) return;
            if (rbTFs == null) return;
            if (cubeTFs == null) return;

            var getterAPI = physicsCore.GetterAPI;
            var rbCubes = getterAPI.GetAllCubeRBs();
            var rbCount = rbCubes.Count;
            var cubes = getterAPI.GetAllCubes();
            var cubeCount = cubes.Count;
            for (int i = 0; i < rbCount; i++) {
                var bc = rbTFs[i];
                var rb = rbCubes[i];
                var body = rb.Body;
                UpdateCube(bc.transform, body as Box);
                rb.SetBounceCoefficient(FP64.ToFP64(bounce));
                body.SetFirctionCoe(FP64.ToFP64(firctionCoe_rbCube));
            }

            for (int i = 0; i < cubeCount; i++) {
                var bc = cubeTFs[i];
                var cube = cubes[i];
                cube.SetFirctionCoe(FP64.ToFP64(firctionCoe_box));
            }
            FixedUpdate_Physics();

        }

        void FixedUpdate_Physics() {
            var dt = UnityEngine.Time.deltaTime;
            restoreTime += FP64.ToFP64(dt);
            while (restoreTime >= intervalTime) {
                restoreTime -= intervalTime;
                for (int i = 0; i < physicsSimulateRate; i++) {
                    physicsCore.Tick(intervalTime);
                }
            }
        }

        void FixedUpdate() {
            if (!canRun) return;
            if (rbTFs == null) return;
            if (cubeTFs == null) return;

            // - Collsion Info
            var collisions_RS = physicsCore.GetterAPI.GetAllCollisions_RS();
            // Debug.Log($"碰撞事件数量: {collisionInfos.Length}");
            for (int i = 0; i < collisions_RS.Length; i++) {
                var collision = collisions_RS[i];
                var body_a = collision.bodyA;
                var body_b = collision.bodyB;
                var type_a = body_a.PhysicsType;
                var type_b = body_b.PhysicsType;
                var rb = body_a.RB;
                var cube = body_b as Box;
                OnCollision(cube, rb, collision);
            }
        }

        #region [Collision]

        void OnCollision(Box cube, Physics.Rigidbody boxRB, CollisionModel collision) {
            if (collision.CollisionType == CollisionType.Enter) OnCollsionEnter(cube, boxRB);
            else if (collision.CollisionType == CollisionType.Stay) OnCollsionStay(cube, boxRB);
            else if (collision.CollisionType == CollisionType.Exit) OnCollsionExit(cube, boxRB);
        }

        void OnCollsionEnter(Box cube, Physics.Rigidbody boxRB) {
            // Logger.Log($" OnCollsionEnter : cube:{cube.name} boxRB:{boxRB.name} ");
        }

        void OnCollsionStay(Box cube, Physics.Rigidbody boxRB) {
            // Logger.Log($" OnCollsionStay : cube:{cube.name} boxRB:{boxRB.name} ");
        }

        void OnCollsionExit(Box cube, Physics.Rigidbody boxRB) {
            // Logger.Log($" OnCollsionExit : cube:{cube.name} boxRB:{boxRB.name} ");
        }

        #endregion

        public void OnDrawGizmos() {
            if (!canRun) return;
            if (rbTFs == null) return;
            if (cubeTFs == null) return;

            var getterAPI = physicsCore.GetterAPI;

            var rbs = getterAPI.GetAllCubeRBs();
            for (int i = 0; i < rbs.Count; i++) {
                var body = rbs[i].Body;
                Gizmos.color = Color.green;
                GizmosExtention.DrawPhysicsBody(body);
            }

            var cubes = getterAPI.GetAllCubes();
            for (int i = 0; i < cubes.Count; i++) {
                var body = cubes[i];
                Gizmos.color = Color.black;
                GizmosExtention.DrawPhysicsBody(body);
            }
        }

        void InitCubes() {
            var setterAPI = physicsCore.SetterAPI;

            var rbGos = GameObject.FindGameObjectsWithTag("RB");
            var cubeGos = GameObject.FindGameObjectsWithTag("Cube");

            // - Physics RB
            var rbCount = rbGos.Length;
            rbTFs = new UnityEngine.Transform[rbCount];
            for (int i = 0; i < rbCount; i++) {
                rbTFs[i] = rbGos[i].transform;
            }

            for (int i = 0; i < rbCount; i++) {
                var tf = rbTFs[i].transform;
                var rb = setterAPI.SpawnRBCube(tf.position.ToFPVector3(), tf.rotation.ToFPQuaternion(), tf.localScale.ToFPVector3(), Vector3.one.ToFPVector3());
                rb.Body.SetIsTrigger(tf.name == "trigger");
                tf.name = $"RBBOX_{i}";
                rb.name = tf.name;
            }
            Debug.Log($"Total RBCube: {rbCount}");

            // - Physics Cube
            var boxCount = cubeGos.Length;
            cubeTFs = new UnityEngine.Transform[boxCount];
            for (int i = 0; i < boxCount; i++) {
                cubeTFs[i] = cubeGos[i].transform;
            }
            for (int i = 0; i < boxCount; i++) {
                var tf = cubeTFs[i].transform;
                var cube = setterAPI.SpawnCube(tf.position.ToFPVector3(), tf.rotation.ToFPQuaternion(), tf.localScale.ToFPVector3(), Vector3.one.ToFPVector3());
                cube.SetIsTrigger(tf.name == "trigger");
                tf.name = $"Cube_{i}";
                cube.name = tf.name;
            }
            Debug.Log($"Total Cube: {boxCount}");
        }

        void UpdateCube(UnityEngine.Transform src, Box cube) {
            cube.SetScale(src.localScale.ToFPVector3());
            cube.SetRotation(src.rotation.ToFPQuaternion());
            src.position = cube.Center.ToVector3();
        }

        public float bounce = 0f;
        public float firctionCoe_box;
        public float firctionCoe_rbCube;

        void OnGUI() {
            GUILayout.BeginHorizontal();
            GUILayout.Label($"弹性系数:{bounce}", GUILayout.Width(100));
            bounce = GUILayout.HorizontalSlider(bounce, 0, 1, GUILayout.Width(200));
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.Label($"摩擦系数(Cube):{firctionCoe_box}", GUILayout.Width(200));
            firctionCoe_box = GUILayout.HorizontalSlider(firctionCoe_box, 0, 5, GUILayout.Width(200));
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.Label($"摩擦系数(RB):{firctionCoe_rbCube}", GUILayout.Width(200));
            firctionCoe_rbCube = GUILayout.HorizontalSlider(firctionCoe_rbCube, 0, 5, GUILayout.Width(200));
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.Label($"物理模拟倍速:{physicsSimulateRate}", GUILayout.Width(200));
            physicsSimulateRate = (int)GUILayout.HorizontalSlider(physicsSimulateRate, 0, maxSimulateRate, GUILayout.Width(200));
            physicsSimulateRate = physicsSimulateRate > maxSimulateRate ? maxSimulateRate : physicsSimulateRate;
            GUILayout.EndHorizontal();
        }

    }

}
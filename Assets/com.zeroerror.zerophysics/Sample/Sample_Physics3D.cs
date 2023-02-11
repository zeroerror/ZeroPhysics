using UnityEngine;
using FixMath.NET;
using ZeroPhysics.Physics3D;
using ZeroPhysics.Extensions;
using ZeroPhysics.Generic;

namespace ZeroPhysics.Sample {

    public class Sample_Physics3D : MonoBehaviour {

        bool canRun = false;

        public Transform rbRoot;
        Transform[] rbTFs;

        public Transform cubeRoot;
        Transform[] cubeTFs;

        public int maxSimulateRate = 10;
        int physicsSimulateRate = 1;

        PhysicsWorld3DCore physicsCore;

        FP64 restoreTime;
        FP64 intervalTime;

        void Start() {
            // Simplex simplex = new Simplex();
            // simplex.Add(new FPVector3(0, 2, 0));
            // simplex.Add(new FPVector3(2, 0, 0));
            // simplex.Add(new FPVector3(-1, 0, 0));
            // var res = simplex.IsInsideSimplex(new FPVector3(-111, 0, 0));
            // Debug.Log($"res {res}");

            if (rbRoot == null) return;
            canRun = true;
            physicsCore = new PhysicsWorld3DCore(new FPVector3(0, -10, 0));
            InitCubes();
            intervalTime = 1 / FP64.ToFP64(60);
        }

        void Update() {
            if (!canRun) return;
            if (rbTFs == null) return;
            if (cubeTFs == null) return;

            var getterAPI = physicsCore.GetterAPI;
            var rbCubes = getterAPI.GetAllCubeRBs();
            var cubes = getterAPI.GetAllCubes();
            for (int i = 0; i < rbCubes.Count; i++) {
                var bc = rbTFs[i];
                var rb = rbCubes[i];
                var body = rb.Body;
                UpdateCube(bc.transform, body as Cube);
                rb.SetBounceCoefficient(FP64.ToFP64(bounce));
                body.SetFirctionCoe(FP64.ToFP64(firctionCoe_rbCube));
            }

            for (int i = 0; i < cubes.Count - rbCubes.Count; i++) {
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
                var cube = body_b as Cube;
                OnCollision(cube, rb, collision);
            }
        }

        #region [Collision]

        void OnCollision(Cube cube, Rigidbody3D boxRB, CollisionModel collision) {
            if (collision.CollisionType == CollisionType.Enter) OnCollsionEnter(cube, boxRB);
            else if (collision.CollisionType == CollisionType.Stay) OnCollsionStay(cube, boxRB);
            else if (collision.CollisionType == CollisionType.Exit) OnCollsionExit(cube, boxRB);
        }

        void OnCollsionEnter(Cube cube, Rigidbody3D boxRB) {
            // UnityEngine.Debug.Log($" OnCollsionEnter : cube:{cube.name} boxRB:{boxRB.name} ");
        }

        void OnCollsionStay(Cube cube, Rigidbody3D boxRB) {
            // UnityEngine.Debug.Log($" OnCollsionStay : cube:{cube.name} boxRB:{boxRB.name} ");
        }

        void OnCollsionExit(Cube cube, Rigidbody3D boxRB) {
            // UnityEngine.Debug.Log($" OnCollsionExit : cube:{cube.name} boxRB:{boxRB.name} ");
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

            var rbCount = rbRoot.childCount;
            rbTFs = new Transform[rbCount];
            for (int i = 0; i < rbCount; i++) {
                rbTFs[i] = rbRoot.GetChild(i);
            }
            for (int i = 0; i < rbCount; i++) {
                var tf = rbTFs[i].transform;
                var rb = setterAPI.SpawnRBCube(tf.position.ToFPVector3(), tf.rotation.ToFPQuaternion(), tf.localScale.ToFPVector3(), Vector3.one.ToFPVector3());
                rb.Body.SetIsTrigger(tf.name == "trigger");
                tf.name = $"RBBOX_{i}";
                rb.name = tf.name;
            }
            Debug.Log($"Total RBCube: {rbCount}");

            var boxCount = cubeRoot.childCount;
            cubeTFs = new Transform[boxCount];
            for (int i = 0; i < boxCount; i++) {
                cubeTFs[i] = cubeRoot.GetChild(i);
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

        void UpdateCube(Transform src, Cube cube) {
            cube.SetScale(src.localScale.ToFPVector3());
            cube.SetRotation(src.rotation.ToFPQuaternion());
            src.position = cube.Center.ToVector3();
        }

        public float bounce = 0f;
        public float firctionCoe_box = 5f;
        public float firctionCoe_rbCube = 1f;
        void OnGUI() {
            GUILayout.BeginHorizontal();
            GUILayout.Label($"弹性系数:{bounce}", GUILayout.Width(100));
            bounce = GUILayout.HorizontalSlider(bounce, 0, 1, GUILayout.Width(200));
            GUILayout.EndHorizontal();

            // GUILayout.BeginHorizontal();
            // GUILayout.Label($"摩擦系数(静态Cube):{firctionCoe_box}", GUILayout.Width(200));
            // firctionCoe_box = GUILayout.HorizontalSlider(firctionCoe_box, 0, 5, GUILayout.Width(200));
            // GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.Label($"摩擦系数(RBBOX):{firctionCoe_rbCube}", GUILayout.Width(200));
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
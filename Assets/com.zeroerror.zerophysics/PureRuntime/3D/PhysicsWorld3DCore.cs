using FixMath.NET;
using ZeroPhysics.Physics.API;
using ZeroPhysics.Physics.Context;

namespace ZeroPhysics.Physics {

    public class PhysicsWorld3DCore {

        FPVector3 gravity;

        // ====== Context
        PhysicsContext physicsContext;

        // ====== Phase
        ForcePhase forcePhase;
        VelocityPhase velocityPhase;
        IntersectPhase intersectPhase;
        PenetrationPhase penetrationPhase;
        TransformPhase transformPhase;

        // ====== API
        GetterAPI getterAPI;
        public IGetterAPI GetterAPI => getterAPI;

        SetterAPI setterAPI;
        public ISetterAPI SetterAPI => setterAPI;

        public PhysicsWorld3DCore(FPVector3 gravity, int boxMax = 1000, int rbCubeMax = 1000, int sphereMax = 1000) {
            this.gravity = gravity;

            forcePhase = new ForcePhase();
            velocityPhase = new VelocityPhase();
            intersectPhase = new IntersectPhase();
            penetrationPhase = new PenetrationPhase();
            transformPhase = new TransformPhase();

            getterAPI = new GetterAPI();
            setterAPI = new SetterAPI();

            physicsContext = new PhysicsContext(boxMax, rbCubeMax, sphereMax);

            forcePhase.Inject(physicsContext);
            velocityPhase.Inject(physicsContext);
            intersectPhase.Inject(physicsContext);
            penetrationPhase.Inject(physicsContext);
            transformPhase.Inject(physicsContext);

            getterAPI.Inject(physicsContext);
            setterAPI.Inject(physicsContext);
        }

        public void Tick(FP64 time) {
            forcePhase.Tick(time, gravity);
            velocityPhase.Tick(time);
            transformPhase.Tick(time);
            intersectPhase.Tick(time);
            penetrationPhase.Tick(time);
        }

    }

}

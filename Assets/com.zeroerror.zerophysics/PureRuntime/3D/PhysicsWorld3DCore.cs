using FixMath.NET;
using ZeroPhysics.Physics3D.API;
using ZeroPhysics.Physics3D.Facade;

namespace ZeroPhysics.Physics3D
{

    public class PhysicsWorld3DCore
    {

        FPVector3 gravity;

        // ====== Facade
        Physics3DFacade physicsFacade;

        // ====== Phase
        ForcePhase forcePhase;
        VelocityPhase velocityPhase;
        TransformPhase transformPhase;
        PenetrationPhase penetrationPhase;
        FrictionPhase frictionPhase;

        // ====== API
        GetterAPI getterAPI;
        public IGetterAPI GetterAPI => getterAPI;

        SetterAPI setterAPI;
        public ISetterAPI SetterAPI => setterAPI;

        public PhysicsWorld3DCore(FPVector3 gravity, int boxMax = 1000, int rbBoxMax = 1000, int sphereMax = 1000)
        {
            this.gravity = gravity;

            forcePhase = new ForcePhase();
            velocityPhase = new VelocityPhase();
            transformPhase = new TransformPhase();
            penetrationPhase = new PenetrationPhase();
            frictionPhase = new FrictionPhase();

            getterAPI = new GetterAPI();
            setterAPI = new SetterAPI();

            physicsFacade = new Physics3DFacade(boxMax, rbBoxMax, sphereMax);

            forcePhase.Inject(physicsFacade);
            velocityPhase.Inject(physicsFacade);
            transformPhase.Inject(physicsFacade);
            penetrationPhase.Inject(physicsFacade);
            frictionPhase.Inject(physicsFacade);

            getterAPI.Inject(physicsFacade);
            setterAPI.Inject(physicsFacade);
        }

        public void Tick(FP64 time)
        {
            forcePhase.Tick(time, gravity);
            velocityPhase.Tick(time);
            penetrationPhase.Tick(time);
            frictionPhase.Tick(time);
            transformPhase.Tick(time);
        }

    }

}

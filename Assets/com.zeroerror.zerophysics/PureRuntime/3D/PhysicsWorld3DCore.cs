using FixMath.NET;
using ZeroPhysics.Physics3D.API;
using ZeroPhysics.Physics3D.Facade;

namespace ZeroPhysics.Physics3D
{

    public class PhysicsWorld3DCore
    {

        FPVector3 gravity;

        // ====== Facade
        Physics3DFacade facade;

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

            facade = new Physics3DFacade(boxMax, rbBoxMax, sphereMax);

            forcePhase.Inject(facade);
            velocityPhase.Inject(facade);
            transformPhase.Inject(facade);
            penetrationPhase.Inject(facade);
            frictionPhase.Inject(facade);

            getterAPI.Inject(facade);
            setterAPI.Inject(facade);
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

using FixMath.NET;

namespace ZeroPhysics.AllPhysics.Physics3D.Facade
{

    public class Physics3DFactory
    {

        Physics3DFacade facade;

        public Physics3DFactory() { }

        public void Inject(Physics3DFacade facade)
        {
            this.facade = facade;
        }

        public Box3D SpawnBox3D(in FPVector3 center, in FPQuaternion rotation, in FPVector3 scale, in FPVector3 size)
        {
            Box3D box = new Box3D();
            box.SetCenter(center);
            box.SetRotation(rotation);
            box.SetScale(scale);
            box.SetWidth(size.x);
            box.SetHeight(size.y);
            box.SetLength(size.z);
            return box;
        }

    }

}
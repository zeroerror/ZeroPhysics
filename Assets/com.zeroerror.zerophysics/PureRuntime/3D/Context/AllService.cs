using ZeroPhysics.Physics.Domain;
using ZeroPhysics.Service;

namespace ZeroPhysics.Physics.Context
{

    public class AllService
    {

        public IDService IDService { get; private set; }
        public CollisionService CollisionService { get; private set; }

        public AllService(int boxMax, int rbCubeMax, int sphereMax)
        {
            IDService = new IDService(boxMax, rbCubeMax, sphereMax);
            CollisionService = new CollisionService();
        }

    }

}
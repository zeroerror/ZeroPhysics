namespace ZeroPhysics.Physics.Context
{

    public class PhysicsContext
    {

        // 对于所有Cube，都给一个id，id由service提供，id作为一个key便于查找，不需要使用字典，直接用数组下标代替，这样提升了遍历效率
        // 保证在创建时，必须确定是rigidbody还是static，其后不做更改，这样就不需要数组之间的数据转移
        // rb需要进行速度改变，和所有包括rb和static进行交叉恢复，static这2个都不需要

        public Rigidbody[] rbs;
        public Box[] cubes;
        public Sphere[] spheres;

        public AllPhysicsDomain Domain { get; private set; }
        public PhysicsFactory Factory { get; private set; }
        public AllService Service { get; private set; }

        public PhysicsContext(int boxMax, int rbMax, int sphereMax)
        {
            cubes = new Box[boxMax];
            rbs = new Rigidbody[rbMax];
            spheres = new Sphere[sphereMax];

            Domain = new AllPhysicsDomain();
            Domain.Inject(this);

            Factory = new PhysicsFactory();
            Factory.Inject(this);

            Service = new AllService(boxMax, rbMax, sphereMax);
        }

    }


}
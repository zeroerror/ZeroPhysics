namespace ZeroPhysics.Service
{

    public class IDService
    {

        public bool[] cubeIDInfos;
        public bool[] rbIDInfos;
        public bool[] sphereIDInfos;

        public IDService(int sCubeMax, int rbCubeMax, int sphereMax)
        {
            cubeIDInfos = new bool[sCubeMax];
            rbIDInfos = new bool[rbCubeMax];
            sphereIDInfos = new bool[sphereMax];
        }

        public ushort FetchID_Cube()
        {
            for (ushort i = 0; i < cubeIDInfos.Length; i++)
            {
                if (!cubeIDInfos[i])
                {
                    cubeIDInfos[i] = true;
                    return i;
                }
            }

            throw new System.Exception($"IDService: Cube ID Run Out!");
        }

        public ushort FetchID_RB()
        {
            for (ushort i = 0; i < rbIDInfos.Length; i++)
            {
                if (!rbIDInfos[i])
                {
                    rbIDInfos[i] = true;
                    return i;
                }
            }

            throw new System.Exception($"IDService: RB ID Run Out!");
        }

        public ushort FetchID_Sphere()
        {
            for (ushort i = 0; i < sphereIDInfos.Length; i++)
            {
                if (!sphereIDInfos[i])
                {
                    sphereIDInfos[i] = true;
                    return i;
                }
            }

            throw new System.Exception($"IDService: Sphere ID Run Out!");
        }

        public void PutBackID_Cube(int id)
        {
            cubeIDInfos[id] = false;
        }

        public void PutBackID_RBCube(int id)
        {
            rbIDInfos[id] = false;
        }

        public void PutBackID_Sphere(int id)
        {
            sphereIDInfos[id] = false;
        }

    }

}
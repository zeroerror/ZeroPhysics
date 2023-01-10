using System.Collections.Generic;

namespace ZeroPhysics.Service
{

    public class IDService
    {

        public bool[] boxIDInfos;
        public bool[] boxRBIDInfos;
        public bool[] sphereIDInfos;

        public IDService(int sBoxMax, int rbBoxMax, int sphereMax)
        {
            boxIDInfos = new bool[sBoxMax];
            boxRBIDInfos = new bool[rbBoxMax];
            sphereIDInfos = new bool[sphereMax];
        }

        public int FetchID_Box()
        {
            for (int i = 0; i < boxIDInfos.Length; i++)
            {
                if (!boxIDInfos[i])
                {
                    boxIDInfos[i] = true;
                    return i;
                }
            }

            return -1;
        }

        public int FetchID_RBBox()
        {
            for (int i = 0; i < boxRBIDInfos.Length; i++)
            {
                if (!boxRBIDInfos[i])
                {
                    boxRBIDInfos[i] = true;
                    return i;
                }
            }

            return -1;
        }

        public int FetchID_Sphere()
        {
            for (int i = 0; i < sphereIDInfos.Length; i++)
            {
                if (!sphereIDInfos[i])
                {
                    sphereIDInfos[i] = true;
                    return i;
                }
            }

            return -1;
        }

        public void PutBackID_Box(int id)
        {
            boxIDInfos[id] = false;
        }

        public void PutBackID_RBBox(int id)
        {
            boxRBIDInfos[id] = false;
        }

        public void PutBackID_Sphere(int id)
        {
            sphereIDInfos[id] = false;
        }

    }

}
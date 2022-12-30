using System;
using System.Collections.Generic;

namespace ZeroPhysics.AllPhysics.Physics3D.API
{

    public interface IGetterAPI
    {

        List<RigidbodyBox3D> GetAllRBBoxes();
        List<Box3D> GetAllBoxes();

    }

}
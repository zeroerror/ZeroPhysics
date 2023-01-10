using System;
using System.Collections.Generic;

namespace ZeroPhysics.Physics3D.API
{

    public interface IGetterAPI
    {

        List<Box3DRigidbody> GetAllRBBoxes();
        List<Box3D> GetAllBoxes();

    }

}
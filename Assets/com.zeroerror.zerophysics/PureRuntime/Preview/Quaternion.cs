namespace ZeroPhysics.Preview {

    public class Quaternion {

        public float x;
        public float y;
        public float z;
        public float w;

        public Quaternion(float x, float y, float z, float w) {
            this.x = x;
            this.y = y;
            this.z = z;
            this.w = w;
        }

        public static Quaternion operator *(Quaternion q1, Quaternion q2) {
            float w1 = q1.w;
            float w2 = q2.w;
            float x1 = q1.x;
            float x2 = q2.x;
            float y1 = q1.y;
            float y2 = q2.y;
            float z1 = q1.z;
            float z2 = q2.z;

            if (true) {
                float X = w1 * x2 + w2 * x1 - z1 * y2 + y1 * z2;
                float Y = w1 * y2 + w2 * y1 + z1 * x2 - x1 * z2;
                float Z = w1 * z2 + w2 * z1 - y1 * x2 + x1 * y2;
                float W = w1 * w2 - x1 * x2 - y1 * y2 - z1 * z2;
                return new Quaternion(X, Y, Z, W);
            } else {
                Matrix44 m1 = new Matrix44();
                m1.m00 = w1;
                m1.m11 = w1;
                m1.m22 = w1;
                m1.m33 = w1;

                m1.m01 = -x1;
                m1.m02 = -y1;
                m1.m03 = -z1;

                m1.m10 = x1;
                m1.m20 = y1;
                m1.m30 = z1;

                m1.m12 = -z1;
                m1.m21 = z1;

                m1.m13 = y1;
                m1.m31 = -y1;

                m1.m23 = -x1;
                m1.m32 = x1;

                Matrix44 m2 = new Matrix44();
                m2.m00 = w2;
                m2.m10 = x2;
                m2.m20 = y2;
                m2.m30 = z2;
                
                Matrix44 m3 = null;// TODO Matrix44 Mul
                return new Quaternion(m3.m00, m3.m10, m3.m20, m3.m30);
            }
        }

    }

}
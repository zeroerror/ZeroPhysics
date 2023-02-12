using UnityEngine;
using ZeroPhysics.Physics3D;

namespace ZeroPhysics {

    public static class Logger {

        public static bool isEnable;

        public static void Log(string str) {

            if (!isEnable) {
                return;
            }

            Debug.Log(str);
        }

        public static void LogError(string str) {

            if (!isEnable) {
                return;
            }

            Debug.LogError(str);
        }

        public static void LogFormat(string str) {

            if (!isEnable) {
                return;
            }

            Debug.LogFormat(str);
        }

    }

}
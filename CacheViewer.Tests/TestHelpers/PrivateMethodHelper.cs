using System;
using System.Reflection;

namespace CacheViewer.Tests.TestHelpers
{
    /// <summary>
    /// Provides access for testing private and/or static methods.
    /// </summary>
    public class PrivateMethodHelper
    {
        /// <summary>
        /// Runs the static private method.
        /// </summary>
        /// <param name="t">The type.</param>
        /// <param name="strMethod">The method name.</param>
        /// <param name="methodParams">The method params.</param>
        /// <returns></returns>
        public static object RunStaticMethod(Type t, string strMethod, object[] methodParams)
        {
            const BindingFlags eFlags = BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic;

            return RunMethod(t, strMethod, null, methodParams, eFlags);
        }

        /// <summary>
        /// Runs the private instance method.
        /// </summary>
        /// <param name="t">The type.</param>
        /// <param name="strMethod">The method name.</param>
        /// <param name="objInstance">instance of the object that contains the method</param>
        /// <param name="methodParams">The method params.</param>
        /// <returns></returns>
        public static object RunInstanceMethod(Type t, string strMethod, object objInstance, object[] methodParams)
        {
            const BindingFlags eFlags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;

            return RunMethod(t, strMethod, objInstance, methodParams, eFlags);
        }

        /// <summary>
        /// Runs the method.
        /// </summary>
        /// <param name="t">The type.</param>
        /// <param name="strMethod">The method name.</param>
        /// <param name="objInstance">instance of the object that contains the method</param>
        /// <param name="methodParams">The method params.</param>
        /// <param name="eFlags">The e flags.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"><c>ArgumentException</c>.</exception>
        private static object RunMethod(Type t, string strMethod, object objInstance, object[] methodParams, BindingFlags eFlags)
        {
            MethodInfo m = t.GetMethod(strMethod, eFlags);

            if (m == null)
            {
                throw new ArgumentException("There is no method '" + strMethod + "' for type '" + t + "'.");
            }

            object objRet = m.Invoke(objInstance, methodParams);

            return objRet;
        }
    }
}
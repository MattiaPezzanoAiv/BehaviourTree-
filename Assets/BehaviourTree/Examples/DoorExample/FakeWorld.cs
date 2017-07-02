using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mattia
{
    public static class FakeWorld
    {

        private static Dictionary<string, MonoBehaviour> WorldObjects;

        /// <summary>
        /// Initialize World system
        /// </summary>
        static FakeWorld()
        {
            WorldObjects = new Dictionary<string, MonoBehaviour>();
        }

        public static void AddWorldObject<T>(string objName, T worldObj) where T : MonoBehaviour
        {
            if (!WorldObjects.ContainsKey(objName))
                WorldObjects.Add(objName, worldObj);
        }
        public static T GetWorldObject<T>(string objectName) where T : MonoBehaviour
        {
            if (WorldObjects.ContainsKey(objectName))
                return (T)WorldObjects[objectName];
            return null;
        }
        /// <summary>
        /// Get the nearest object of type T
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="agent">The agent that want information</param>
        /// <returns>Null if there aren't object of type T</returns>
        public static T GetNearestObjectOfType<T>(Agent agent) where T : MonoBehaviour
        {
            float minDistance = float.MaxValue;
            T nearestObject = null;

            foreach (var obj in WorldObjects.Values)
            {
                if((obj as T) != null)
                {
                    if(Vector3.Distance(obj.transform.position,agent.transform.position) < minDistance)
                    {
                        nearestObject = (T)obj;
                    }
                }
            }
            return nearestObject;
        }
    }
}

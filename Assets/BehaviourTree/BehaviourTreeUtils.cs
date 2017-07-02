using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml.Serialization;
using System.Reflection;
using System;
using System.Linq;
using System.IO;

namespace Mattia
{

    public static class BehaviourTreeUtils
    {

        private static XmlSerializer serializer;
        /// <summary>
        /// XmlSerializer used for standard xml serialization. 
        /// </summary>
        public static XmlSerializer Serializer
        {
            get
            {
                if (serializer == null)
                {
                    //instance serializer
                    serializer = new XmlSerializer(typeof(Node), GetEnumerableOfType<Node>().ToArray());
                }
                return serializer;
            }
        }

        /// <summary>
        /// Used for read an xml file and generate a Node representing a behaviour tree.
        /// </summary>
        /// <param name="path">path of the xml file</param>
        /// <returns>If system are not able to parse xml file, return null.</returns>
        public static Node LoadBehaviourTree(string path)
        {
             using (FileStream stream = new FileStream(string.Format(path), FileMode.Open))
                {
                    return (Node)Serializer.Deserialize(stream);
                }
        }

        /// <summary>
        /// Used for read an xml file and generate a Node representing a behaviour tree.
        /// </summary>
        /// <returns>If system are not able to parse xml file, return null.</returns>
        public static Node LoadBehaviourTree(TextAsset textAsset)
        {
            try
            {
                using (StringReader reader = new StringReader(textAsset.text))
                {
                    return (Node)Serializer.Deserialize(reader);
                }
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Get all Type that inherit from type T.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="constructorArgs"></param>
        /// <returns></returns>
        public static IEnumerable<Type> GetEnumerableOfType<T>(params object[] constructorArgs) where T : class
        {
            List<Type> types = new List<Type>();
            foreach (Type type in
                Assembly.GetAssembly(typeof(T)).GetTypes()
                .Where(myType => myType.IsClass && !myType.IsAbstract && myType.IsSubclassOf(typeof(T))))
            {
                types.Add(type);
            }
            return types;
        }
    }
}


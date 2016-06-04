using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using UnityEngine;

namespace Assets.Scripts
{
    public class Serializer
    {
        public static T Load<T>(string filename) where T : class
        {
            filename = "SaveData/" + filename;

            if (File.Exists(filename))
            {
                try
                {
                    using (Stream stream = File.OpenRead(filename))
                    {
                        BinaryFormatter formatter = new BinaryFormatter();
                        return formatter.Deserialize(stream) as T;
                    }
                }
                catch (Exception e)
                {
                    Debug.Log(e.Message);
                }
            }
            else
            {
                Debug.LogWarning("Terrarium not found on disk.");
            }
            return default(T);
        }

        public static void Save<T>(string filename, T data) where T : class
        {
            if (!Directory.Exists("SaveData/"))
            {
                var folder = Directory.CreateDirectory("SaveData/");
            }

            filename = "SaveData/" + filename;
            using (Stream stream = File.OpenWrite(filename))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(stream, data);
            }
        }
    }
}

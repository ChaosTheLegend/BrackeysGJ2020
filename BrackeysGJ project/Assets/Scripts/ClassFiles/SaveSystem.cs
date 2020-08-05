using BrackeysGJ.Serializable;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

namespace BrackeysGJ.ClassFiles
{
    public class SaveSystem
    {
        private const string Format = "savedata.ninj";
        
        public static void DeleteSave()
        {    
            var path = Application.persistentDataPath + "/" + Format;
            File.Delete(path);
        }
        
        public static void Save(SaveData toSave)
        {
            var formatter = new BinaryFormatter();
            var path = Application.persistentDataPath + "/" + Format;
            var stream = new FileStream(path,FileMode.OpenOrCreate);
            formatter.Serialize(stream,toSave);
            stream.Close();
        }

        public static SaveData Load()
        {
            var path = Application.persistentDataPath + "/" + Format;
            if (!File.Exists(path)) return null;
            var formatter = new BinaryFormatter();
            var stream = new FileStream(path,FileMode.Open);
            var data = formatter.Deserialize(stream) as SaveData;
            stream.Close();
            return data;
        }
    }
}

using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace Fertools.Saving
{
    public class BinarySaveMethod : ISaveMethod
    {
        public void Save(object objectToSave, FileStream saveFile)
        {
            BinaryFormatter formatter = new BinaryFormatter();
            formatter.Serialize(saveFile, objectToSave);
            saveFile.Close();
        }

        public object Load(Type objectType, FileStream saveFile)
        {
            BinaryFormatter formatter = new BinaryFormatter();
            var returnObject = formatter.Deserialize(saveFile);
            saveFile.Close();
            return returnObject;
            
        }
    }
}
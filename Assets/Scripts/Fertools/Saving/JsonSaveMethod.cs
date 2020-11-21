using System;
using System.IO;
using System.Text;
using UnityEngine;

namespace Fertools.Saving
{
    public class JsonSaveMethod : ISaveMethod
    {
        public void Save(object objectToSave, FileStream saveFile)
        {
            string json = JsonUtility.ToJson(objectToSave);
            StreamWriter streamWriter = new StreamWriter(saveFile);
            streamWriter.Write(json);
            streamWriter.Close();
            saveFile.Close();
            
        }

        public object Load(Type objectType, FileStream saveFile)
        {
            object returnObject;
            
            StreamReader streamReader = new StreamReader(saveFile, Encoding.UTF8);
            string json = streamReader.ReadToEnd();
            Debug.Log(json);

            returnObject = JsonUtility.FromJson(json, objectType);
            
            streamReader.Close();
            saveFile.Close();
            return returnObject;
        }
    }
}
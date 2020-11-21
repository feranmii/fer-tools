using System;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.WSA;

namespace Fertools.Saving
{
    public class SaveTesting : MonoBehaviour
    {
        public SaveTestObject saveTestObject;

        public SaveMethods saveMethodType = SaveMethods.Binary;
        
        public string FileName = "TestObject";
        /// the name of the destination folder
        public string FolderName = "SaveTest/";
        /// the extension to use
        public string SaveFileExtension = ".testObject";

        private ISaveMethod saveMethod;
        
        
        [Button()]
        public void TestSave()
        {
            InitSaveMethod();
            SaveManager.Save(saveTestObject, FileName + SaveFileExtension, FolderName);

        }

        [Button()]
        public void TestLoad()
        {
            InitSaveMethod();
            saveTestObject=(SaveTestObject) SaveManager.Load(typeof(SaveTestObject), FileName + SaveFileExtension, FolderName);

        }
        
        
        [Button()]
        public void DeleteSaves()
        {
            SaveManager.DeleteSaveFolder(FolderName);
        }



        void InitSaveMethod()
        {
            switch (saveMethodType)
            {
                case SaveMethods.Json:
                    saveMethod = new JsonSaveMethod();
                    break;
                case SaveMethods.Binary:
                    saveMethod = new BinarySaveMethod();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            SaveManager.saveMethod = saveMethod;
        }
        
    }
}

[System.Serializable]
public class SaveTestObject
{
    public List<string> Strings;
    
}
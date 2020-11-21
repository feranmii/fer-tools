using System;
using System.IO;
using UnityEngine;

namespace Fertools.Saving
{
    public static class SaveManager
    {
        public static ISaveMethod saveMethod = new BinarySaveMethod();

        private const string baseFolderName = "/GameData/"; //+ 👇🏾
        private const string defaultFolderName = "Saves";


        static string GetSavePath(string folderName = defaultFolderName)
        {
            string path;

            if (Application.platform == RuntimePlatform.IPhonePlayer)
            {
                path = Application.persistentDataPath + baseFolderName;
            }
            else
            {
                path = Application.persistentDataPath + baseFolderName;
            }

#if UNITY_EDITOR
            path = Application.dataPath + baseFolderName;
#endif

            path = path + folderName + "/";

            return path;
        }

        public static void Save(object saveObject, string fileName, string folderName = defaultFolderName)
        {
            string path = GetSavePath(folderName);
            string saveFileName = fileName;

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            FileStream saveFile = File.Create(path + saveFileName);
            
            Debug.Log(path  + " " + folderName);
            saveMethod.Save(saveObject, saveFile);

            Debug.Log("Saved!");
            saveFile.Close();
        }

        public static object Load(Type objectType, string fileName, string folderName = defaultFolderName)
        {
            string path = GetSavePath(folderName);
            string saveFileName = path + fileName;

            object returnObject;
            if (!Directory.Exists(path) || !File.Exists(saveFileName))
            {
                Debug.LogError("Cannot Find File To Load");

                return null;
            }


            FileStream saveFile = File.Open(saveFileName, FileMode.Open, FileAccess.Read, FileShare.Read);
            returnObject = saveMethod.Load(objectType, saveFile);

            return returnObject;
        }

        public static void DeleteSaveFolder(string fileName, string folderName = defaultFolderName)
        {
            string path = GetSavePath(folderName);
            string saveFileName = fileName;
            if (File.Exists(path + saveFileName))
            {
                File.Delete(path + saveFileName);
            }
        }

        public static void DeleteSaveFolder(string folderName = defaultFolderName)
        {
            string path = GetSavePath(folderName);
            if (Directory.Exists(path))
            {
                DeleteDirectory(path);
            }
        }

        private static void DeleteDirectory(string dirPath)
        {
            string[] files = Directory.GetFiles(dirPath);
            string[] dirs = Directory.GetDirectories(dirPath);

            foreach (var file in files)
            {
                File.SetAttributes(file, FileAttributes.Normal);
                File.Delete(file);
            }

            foreach (var dir in dirs)
            {
                DeleteDirectory(dir);
            }
            
            Directory.Delete(dirPath, false);
            
        }
    }
}
using System.IO;
using UnityEditor;
using UnityEngine;

namespace Tools
{
    public static class GenerateFolderStructure
    {
        [MenuItem("Tools/Fertools/Generate Folder Structure")]
        public static void Execute()
        {
            Directory.CreateDirectory(Application.dataPath + "/[Debug]");
            Directory.CreateDirectory(Application.dataPath + "/Art");
            Directory.CreateDirectory(Application.dataPath + "/Art/Animations");
            Directory.CreateDirectory(Application.dataPath + "/Art/Materials");
            Directory.CreateDirectory(Application.dataPath + "/Art/Models");
            Directory.CreateDirectory(Application.dataPath + "/Art/Textures");
            Directory.CreateDirectory(Application.dataPath + "/Audio");
            Directory.CreateDirectory(Application.dataPath + "/Audio/Music");
            Directory.CreateDirectory(Application.dataPath + "/Audio/Sound");
            Directory.CreateDirectory(Application.dataPath + "/Code");
            Directory.CreateDirectory(Application.dataPath + "/Code/Scripts");
            Directory.CreateDirectory(Application.dataPath + "/Code/Scripts/Environment");
            Directory.CreateDirectory(Application.dataPath + "/Code/Scripts/Framework");
            Directory.CreateDirectory(Application.dataPath + "/Code/Scripts/Tools");
            Directory.CreateDirectory(Application.dataPath + "/Code/Scripts/UI");
            Directory.CreateDirectory(Application.dataPath + "/Code/Shaders");
            Directory.CreateDirectory(Application.dataPath + "/Docs");
            Directory.CreateDirectory(Application.dataPath + "/Editor");
            Directory.CreateDirectory(Application.dataPath + "/Level");
            Directory.CreateDirectory(Application.dataPath + "/Level/Physics");
            Directory.CreateDirectory(Application.dataPath + "/Level/Prefabs");
            Directory.CreateDirectory(Application.dataPath + "/Level/Scenes");
            Directory.CreateDirectory(Application.dataPath + "/Level/UI");
            Directory.CreateDirectory(Application.dataPath + "/Plugins");
            Directory.CreateDirectory(Application.dataPath + "/Resources");
            Directory.CreateDirectory(Application.dataPath + "/StreamingAssets");
            Directory.CreateDirectory(Application.dataPath + "/_ThirdParty");
            Directory.CreateDirectory(Application.dataPath + "/WebGLTemplates");
        }
    }
}
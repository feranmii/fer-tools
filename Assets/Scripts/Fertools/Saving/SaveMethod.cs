using System;
using System.IO;

namespace Fertools.Saving
{
    public interface ISaveMethod
    {
        void Save(object objectToSave, FileStream saveFile);

        object Load(Type objectType, FileStream saveFile);
    }

    public enum SaveMethods
    {
        Json,
        Binary
    }
    
  
}
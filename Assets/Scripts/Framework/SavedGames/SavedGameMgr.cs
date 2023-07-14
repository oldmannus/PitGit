using UnityEngine;
using System;
using System.IO;
using Pit.Utilities;


namespace Pit.Framework
{
    // contains quick info/data about a given save. Used to load thumbnails
    [Serializable]
    public class SaveInfo
    {
        public string Name = "Unset";
        public bool Used = false;
        public int Slot = -1;

        [NonSerialized]
        public bool Present = false;
    }

    // all saveable things must generate a save info
    public interface ISavable
    {
        SaveInfo GenerateSaveInfo();
    }

    [Serializable]
    public class SaveInfoList<T> : SerializableArray<T> where T : SaveInfo, new()
    { }

    public class SaveGameInfoChangedEvent : GameEvent
    {
    }

    public class SaveGameInfoWriteFinishedEvent : GameEvent
    {
        public bool Succeeded;
    }

 

    public class SavedGameManager<SaveType, SaveInfoType>  where SaveType : class, ISavable, new()
                                                            where SaveInfoType : SaveInfo, new()
    {
        public SaveType LoadedSave { get; private set; }
        public uint LoadedSaveNdx { get; private set; }

        SerializableArray<SaveInfoType> _info { get; set; }


        //  MUST NOT CHANGE at runtime!
        public virtual uint GetMaxNumSaveSlots() { return 10; }


        string SaveInfoFileName {  get { return Application.persistentDataPath + "/SaveInfo.gd"; } }

        // ------------------------------------------------------------------------------------------------------------------------------------
        public void Initialize()
        // ------------------------------------------------------------------------------------------------------------------------------------
        {
            LoadedSave = null;
            LoadedSaveNdx = 0;
            _info = new SaveInfoList<SaveInfoType>();
            _info.InitDefaults(GetMaxNumSaveSlots());
            ReadSaveInfo(true);
        }


        // ------------------------------------------------------------------------------------------------------------------------------------
        public bool HasSaveInSlot(uint slot)
        // ------------------------------------------------------------------------------------------------------------------------------------
        {
            Debug.Assert(_info != null);
            Debug.Assert(slot < _info.Length);
            return _info[slot] != null && _info[slot].Used;
        }

        // ------------------------------------------------------------------------------------------------------------------------------------
        public SaveInfoType GetSaveInfo(uint slot)
        // ------------------------------------------------------------------------------------------------------------------------------------
        {
            Debug.Assert(_info != null);
            Debug.Assert(slot < _info.Length);
            return _info[slot];
        }

        // ------------------------------------------------------------------------------------------------------------------------------------
        void ReadSaveInfo(bool writeDefault = false)
        // ------------------------------------------------------------------------------------------------------------------------------------
        {
            _info = FileUtils.ReadJsonObjectFromFile<SaveInfoList<SaveInfoType>>(SaveInfoFileName);
            if (_info == null)
            {
                _info = new SaveInfoList<SaveInfoType>();
                _info.InitDefaults(GetMaxNumSaveSlots());
                if (writeDefault)
                    WriteSaveInfo(); // will write out default
            }
            else
            { 
                // make sure that all of the things in the save list are actually on disk
                for (uint i = 0; i < _info.Length; i++)
                {
                    if (_info[i].Used)
                    {
                        _info[i].Present = File.Exists(MakeSaveFileName(i));
                    }
                }
            }
            Events.SendGlobal(new SaveGameInfoChangedEvent());
        }


        // ------------------------------------------------------------------------------------------------------------------------------------
        void WriteSaveInfo()
        // ------------------------------------------------------------------------------------------------------------------------------------
        {
            Events.SendGlobal(new SaveGameInfoWriteFinishedEvent() { Succeeded = FileUtils.WriteJsonObjectFromFile(SaveInfoFileName, _info) });
        }



        // --------------------------------------------------------------------------------------
        public SaveType ReadSave(uint ndx)
        // --------------------------------------------------------------------------------------
        {
            Debug.Assert(ndx < _info.Length);

            LoadedSave = null;  // zap old Save
            LoadedSaveNdx = 0;

            if (File.Exists(MakeSaveFileName(ndx)))
            {
                LoadedSave = FileUtils.ReadJsonObjectFromFile<SaveType>(MakeSaveFileName(ndx));
                if (LoadedSave == null)
                {
                    Debug.LogWarning("Failed to load saved game");      // ### send message?
                    return null;
                }
            }

            LoadedSaveNdx = ndx;
            return LoadedSave;
        }



        // --------------------------------------------------------------------------------------
        public bool WriteSave(uint ndx, SaveType obj )
        // --------------------------------------------------------------------------------------
        {
            if (FileUtils.WriteJsonObjectFromFile(MakeSaveFileName(ndx), obj))
            {
                LoadedSave = obj;
                LoadedSaveNdx = ndx;

                _info[ndx] = obj.GenerateSaveInfo() as SaveInfoType;
                _info[ndx].Used = true;
                _info[ndx].Slot = (int)ndx;
                _info[ndx].Present = true;

                WriteSaveInfo();

                Events.SendGlobal(new SaveGameInfoChangedEvent());

                return true;
            }
            return false;
            
        }


        static string MakeSaveFileName(uint ndx)
        {
            return Application.persistentDataPath + "/Save" + ndx + ".gd";
        }
    }
}


using System;
using System.IO;
using System.Collections.Generic;
using System.Text;

namespace Pit.Utilities
{

    public interface IFileAsset
    {
        void ReadMetaData(Stream stream);
        void WriteMetaData(Stream stream);
    }

    /// <summary>
    /// This class provides functions to manage a list of file assets in a given folder. 
    /// Uses include
    ///  - profile list
    ///  - saved game list
    ///  - asset list
    ///  Functionality includes
    ///  - enumerating all assets
    ///  - getting metadata on all assets
    ///  - loading, saving, writing assets
    /// </summary>
    public class FileAssetListMgr<MetaDataClass>
    {
        [Serializable]
        public class FileAsset
        {

        }


        string _extension = ".ass";
        List<string> _folders = new List<string>();

        public void Initialize( string extension)
        {
            _extension = extension;
        }


        public void Enumerate()
        {

        }

    }
}

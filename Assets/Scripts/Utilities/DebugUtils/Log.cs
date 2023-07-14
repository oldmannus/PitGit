using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Pit.Utilities
{
    class Log
    {
        public static void Add(string s)    { mInstance.AddInternal(s); }
        public static int NumMsgs           { get { return mInstance.mList.Count(); } }
        public static string GetMsg(int n)  { return mInstance.mList[n]; }

        // private stuff
        private static Log      mInstance = new Log();   // I probably shouldn't do singletons this way, but it works
        private List<string>    mList;
        private StreamWriter    mWriter;
        private FileInfo        mFileInfo;


        private Log()
        {
            mList = new List<string>();

            mFileInfo = new FileInfo("Log.txt");    // would be nice to generalize, but meh
            mWriter = mFileInfo.CreateText();
        }

        private void AddInternal(string s)
        {
            Dbg.Assert(s != null);
            if (s != null)
            {
                mList.Add(s);
                mWriter.WriteLine(s);
                Dbg.Log(s);
            }
        }

    }
}

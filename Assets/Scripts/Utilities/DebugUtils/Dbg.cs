using UnityEngine;
using UnityEngine.Serialization;
using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using System.IO;

namespace Pit.Utilities
{
    public static class Dbg
    {
        public delegate void LogMsgCallback(string msg);
        public static event LogMsgCallback OnLogMessage;

        public static bool LogToDiagnostics = true;
        public static bool StoreLogs = true;            // stored in a big list


        public static bool AssertEnabled = true;
        public static bool ShowTimeStamp = true;

        delegate void WriteStrFunc(string msg );
        

        public static void LogError(string msg, string channel = NoChannel)   { LogMsg("Error: " + msg, channel, UnityEngine.Debug.LogError); }
        public static void LogWarning(string msg, string channel = NoChannel) { LogMsg("Warning: " + msg, channel, UnityEngine.Debug.LogWarning); }
        public static void Log(StringBuilder msg, string channel = NoChannel) { LogMsg("Log: " + msg.ToString(), channel, UnityEngine.Debug.Log); }
        public static void Log(string msg, string channel = NoChannel)        { LogMsg("Log: " + msg, channel, UnityEngine.Debug.Log); }


        // -----------------------------------------------------------------------------------------
        public static IEnumerator GetLogsByChannelExact( string channel, bool includeParents)
        // -----------------------------------------------------------------------------------------
        {
            Channel chan = Channel.MakeChannel(channel);
            for (int i = 0; i < _logs.Count; i++)
                if (_logs[i].Channel.IsWithinExact(chan))
                    yield return _logs[i];
        }

        // -----------------------------------------------------------------------------------------
        public static IEnumerator GetLogsByChannelIncludeParents(string channel)
        // -----------------------------------------------------------------------------------------
        {
            Channel chan = Channel.MakeChannel(channel);
            for (int i = 0; i < _logs.Count; i++)
                if (_logs[i].Channel.IsWithinIncludeParents(chan))
                    yield return _logs[i];
        }


        // -----------------------------------------------------------------------------------------
        public static bool WriteToFile(string fileName, bool autoFlush)
        // -----------------------------------------------------------------------------------------
        {
            try
            {
                _writer = new StreamWriter(fileName);
                _writer.AutoFlush = autoFlush;
            }
            catch (Exception e)
            {
                //System.Diagnostics.Debug.Print("Failed to open log file " + fileName + ". Because " + e.Message);
                UnityEngine.Debug.LogError("Failed to open log file " + fileName + ". Because " + e.Message);
                return false;
            }

            return true;
        }

        // -----------------------------------------------------------------------------------------
        public static void Shutdown()
        // -----------------------------------------------------------------------------------------
        {
            if (_writer != null)
            {
                _writer.Close();
                _writer.Dispose();
            }
        }

        // -----------------------------------------------------------------------------------------
        public static bool Assert(bool v, string msg = "")
        // -----------------------------------------------------------------------------------------
        {
            if (!v)
            {
                if (AssertEnabled)
                {
                    if (string.IsNullOrEmpty(msg))
                        UnityEngine.Debug.Assert(v);
                    else
                        UnityEngine.Debug.Assert(v, msg);
                }
                else
                    UnityEngine.Debug.LogError("Assertion failed, but disabled: " + msg);
                return true;
            }
            else
                return false;
        }


        #region Internals
        // **************************** Storing and writing log functions  ***********************************

        static StreamWriter     _writer;
        static List<Msg>        _logs = new List<Msg>();

        const string NoChannel = "None";


        // -----------------------------------------------------------------------------------------
        // This starts from a colon-delimited-sequence
        // "Input:Joystick:Axes" depicts a channel of Input -> joystick -> axes
        // If the log viewer is set to "Input", it will show all of these. Or Input:Joystick... etc
        public class Channel
        // -----------------------------------------------------------------------------------------
        {
            public List<string> Tree;

            public bool IsWithinIncludeParents( Channel otherChannel)
            {
                for (int i = 0; i < Tree.Count && i < otherChannel.Tree.Count; i++)
                {
                    if (otherChannel.Tree[i] != Tree[i])
                        return false;
                }

                return true;
            }

            public bool IsWithinExact(Channel otherChannel)
            {
                // requested path is longer than our path, so no way we can match
                if (otherChannel.Tree.Count > Tree.Count)
                    return false;

                for (int i = 0; i < Tree.Count; i++)
                {
                    if (otherChannel.Tree[i] != Tree[i])
                        return false;
                }

                return true;
            }


            public static Channel MakeChannel( string path)
            {
                Channel newChan = new Channel();
                string[] bits = path.Split(':');
                if (bits == null || bits.Length == 0)
                {
                    newChan.Tree = new List<string>();
                    newChan.Tree.Add(NoChannel);
                }
                else
                {
                    newChan.Tree = new List<string>(bits);
                }

                return newChan;   
            }
        }

        // -----------------------------------------------------------------------------------------
        public class Msg
        // -----------------------------------------------------------------------------------------
        {
            public DateTime When { get; private set; }
            public Channel Channel { get; private set; }
            public string   Text { get; private set; }

            public static Msg MakeMsg(string text, string channel)
            {
                Msg newMsg = new Msg();
                newMsg.When = DateTime.Now;
                newMsg.Channel = Channel.MakeChannel(channel);
                newMsg.Text = text;

                //if (UseTimeStamp)
                //{
                //    newMsg = string.Format("{0:hh: mm: ss} | {1}", DateTime.Now, msg);
                //}

                return newMsg;
            }
            

            public string TextWithTimeStamp 
            {
                get { return string.Format("{0:hh:mm:ss} | {1}", When, Text); }
            }
        }




        static void LogMsg(string textMsg, string channel, WriteStrFunc logger )
        {
            Msg msg = Msg.MakeMsg(textMsg, channel);

            if (StoreLogs)
            {
                _logs.Add(msg);
            }

            string outText = ShowTimeStamp ? msg.TextWithTimeStamp : msg.Text;

            if (_writer != null)
            {
                if (_writer.AutoFlush)  // no reason to do async, since we want to make sure each msg is done before continuing
                {
                    _writer.WriteLine(outText);
                }
                //else   // Unity does not support this
                //    _writer.WriteLineAsync(outText);
            }

            if (OnLogMessage != null)
                OnLogMessage.Invoke(outText);

            if (LogToDiagnostics)
            {
                logger(outText);
                //System.Diagnostics.Debug.Print(outText);
            }
        }


        #endregion

    }
}

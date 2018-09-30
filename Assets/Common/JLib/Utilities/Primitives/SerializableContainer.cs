using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JLib.Utilities
{
    [Serializable]
    public class SerializableArray<T> where T : class, new()
    {
        public int Length { get { return Data.Length; } }

        public T this[int i]
        {
            get { return Data[i]; }
            set { Data[i] = value; }
        }
        public T this[uint i]
        {
            get { return Data[i]; }
            set { Data[i] = value; }
        }

        public T[] Data;   // public so that serializer can get to it


        public void Init(uint count)
        {
            Data = new T[count];
        }

        public void InitDefaults(uint count)
        {
            Data = new T[count];
            for (int i = 0; i < count; i++)
            {
                Data[i] = new T();
            }
        }
    }



    [Serializable]
    public class SerializableList<T> where T : class, new()
    {
        public int Count { get { return List.Count; } }

        public T this[int i]
        {
            get { return List[i]; }
            set { List[i] = value; }
        }
        public T this[uint i]
        {
            get { return List[(int)i]; }
            set { List[(int)i] = value; }
        }

        public void Add(T t)        {            List.Add(t);        }
        public void Clear()        {            List.Clear();        }

        public List<T> List = new List<T>();   // public so that serializer can get to it

        public void InitDefaults(uint count)
        {
            List = new List<T>();
            for (int i = 0; i < count; i++)
            {
                List.Add(new T());
            }
        }
    }
}

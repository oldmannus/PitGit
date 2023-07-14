using System.Collections;
using System.Collections.Generic;


namespace Pit.Utilities
{

    public class AutoAllocateSingleton<T> where T : class, new()
    {
        static T _instance;

        static T I
        {
            get
            {
                if (_instance == null)
                    _instance = new T();

                return _instance;
            }
        }
    }

    public class DeclaredSingleton<T> where T : class
    {
        public static T Instance {  get { return _instance; } }

        static T _instance;

        public DeclaredSingleton()
        {
            Dbg.Assert(_instance == null);
            _instance = this as T;
        }

        ~DeclaredSingleton()
        {
            if (_instance == this)
                _instance = null;
        }
    }
}
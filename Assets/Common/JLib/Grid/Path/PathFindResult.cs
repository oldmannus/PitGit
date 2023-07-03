using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;

namespace JLib.Grid
{
    public class PathFindResult
    {
        public enum Result
        {
            Running,
            Invalid,
            Succeeded,
            Failed
        }

        public Result GetResult(ref Path path)
        {
            lock (_resultLock)
            {
                if (_status == Result.Succeeded)
                {
                    path = _path;
                }
                return _status;
            }
        }

        public void SetResult(Path path, Result result)
        {
            lock (_resultLock)
            {
                _status = result;
                Debug.Assert(result != Result.Succeeded || path.Nodes != null);
                _path = path;
            }
        }
              
           
        Result _status = Result.Running;
        Path _path;
        object _resultLock = new object();

    }
}


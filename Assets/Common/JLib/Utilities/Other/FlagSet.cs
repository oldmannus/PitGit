using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JLib.Utilities
{
    public class FlagSet
    {
        int[] _fields = new int[0];


        protected void Reset()
        {
            _fields = new int[0];
        }

        /// <summary>
        /// Note flag must be an 0-1 count, not a bitfield
        /// </summary>
        /// <param name="flag"></param>
        protected void Add(int flag)
        {
            int fieldNdx = flag / sizeof(int);
            int offset = flag % sizeof(int);


            // add more fields if necessary
            if (_fields.Length < fieldNdx + 1)
                Array.Resize<int>(ref _fields, fieldNdx + 1);

            _fields[fieldNdx] |= 1 << offset;
        }

        public void Add(FlagSet set)
        {
            if (set._fields.Length > _fields.Length)
                Array.Resize<int>(ref _fields, set._fields.Length);

            for (int ndx = 0; ndx < set._fields.Length; ndx++)
            {
                _fields[ndx] |= set._fields[ndx];
            }
        }

        protected void Remove(int flag)
        {
            int fieldNdx = flag / sizeof(int);
            int offset = flag % sizeof(int);


            // add more fields if necessary. stupid to do on remove, but meh
            if (_fields.Length < fieldNdx + 1)
                Array.Resize<int>(ref _fields, fieldNdx + 1);

            _fields[fieldNdx] &= ~(1 << offset);
        }

        public void Remove(FlagSet set)
        {
            if (set._fields.Length > _fields.Length)
                Array.Resize<int>(ref _fields, set._fields.Length);

            for (int ndx = 0; ndx < set._fields.Length; ndx++)
            {
                _fields[ndx] &= ~(set._fields[ndx]);
            }
        }

        public bool HasFlag(int flag)
        {
            int fieldNdx = flag / sizeof(int);
            int offset = flag % sizeof(int);


            // add more fields if necessary. stupid to do on remove, but meh
            if (_fields.Length < fieldNdx + 1)
                Array.Resize<int>(ref _fields, fieldNdx + 1);

            return (_fields[fieldNdx] & (1 << offset)) != 0;
        }


        /// <summary>
        /// Does THIS set have the flags in the parameter
        /// </summary>
        /// <param name="set"></param>
        /// <returns></returns>
        public bool HasFlags(FlagSet set)
        {
            if (set._fields.Length > _fields.Length)
                Array.Resize<int>(ref _fields, set._fields.Length);

            for (int ndx = 0; ndx < set._fields.Length; ndx++)
            {
                int and = set._fields[ndx] & _fields[ndx];
                if (and != set._fields[ndx])
                    return false;
            }

            return true;
        }

        public void CopyFrom(FlagSet set)
        {
            if (set._fields.Length > _fields.Length)
                Array.Resize<int>(ref _fields, set._fields.Length);

            for (int ndx = 0; ndx < set._fields.Length; ndx++)
            {
                _fields[ndx] = set._fields[ndx];
            }
        }
    }
}

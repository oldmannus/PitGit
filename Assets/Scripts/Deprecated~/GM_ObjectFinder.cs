//using System.Collections;
//using System.Collections.Generic;

//namespace JLib.Game
//{
//    /// <summary>
//    /// Must also call  Game.Finder.Register(this); and unregister
//    /// </summary>
//    public interface GM_IIdentifiable
//    {
//        ulong Id { get; set; }
//    }
//    public class GM_Identifiable : GM_IIdentifiable
//    {
//        public ulong Id { get; set; }

//        public GM_Identifiable()
//        {
//        }

//        ~GM_Identifiable()
//        {
//        }
//    }


//    public class GM_IDList : List<ulong>
//    {
//        public bool IsEqual(GM_IDList obj1)
//        {
//            return IsEqual(this, obj1);
//        }

//        public static bool IsEqual(GM_IDList obj1, GM_IDList obj2)
//        {
//            if (ReferenceEquals(obj1, obj2))
//            {
//                return true;
//            }

//            if (ReferenceEquals(obj1, null))
//            {
//                return false;
//            }
//            if (ReferenceEquals(obj2, null))
//            {
//                return false;
//            }

//            foreach (var team1Id in obj1)
//            {
//                bool found = false;
//                foreach (var team2Id in obj2)
//                {
//                    if (team1Id == team2Id)
//                    {
//                        found = true;
//                        break;
//                    }

//                    if (found == false)
//                        return false;
//                }
//            }
//            return true;
//        }
//    }

//    public class GM_IIdentifiableList: List<GM_IIdentifiable>
//    {
//        public bool IsEqual(GM_IIdentifiableList obj1)
//        {
//            return IsEqual(this, obj1);
//        }

//        public static bool IsEqual(GM_IIdentifiableList obj1, GM_IIdentifiableList obj2)
//        {
//            if (ReferenceEquals(obj1, obj2))
//            {
//                return true;
//            }

//            if (ReferenceEquals(obj1, null))
//            {
//                return false;
//            }
//            if (ReferenceEquals(obj2, null))
//            {
//                return false;
//            }

//            foreach (var team1Id in obj1)
//            {
//                bool found = false;
//                foreach (var team2Id in obj2)
//                {
//                    if (team1Id == team2Id)
//                    {
//                        found = true;
//                        break;
//                    }

//                    if (found == false)
//                        return false;
//                }
//            }
//            return true;
//        }
//    }

//    // this is used as a global system to find a given object based on an id. All objects should get their
//    // ids through this
//    public class GM_ObjectFinder
//    {
//        public const ulong InvalidId = 0;

//        Dictionary<ulong, GM_IIdentifiable> _map = new Dictionary<ulong, GM_IIdentifiable>();
//        ulong _nextId = 1;

//        public void Register(GM_IIdentifiable obj)
//        {
//            obj.Id = _nextId++;
//            _map.Add(obj.Id, obj);
//        }

//        public void Unregister(GM_IIdentifiable obj)
//        {
//            _map.Remove(obj.Id);
//            obj.Id = InvalidId;
//        }

//        public Type Get<Type>(ulong id) where Type : GM_IIdentifiable
//        {
//            return (Type)(_map[id]);
//        }

//        public Type GetSafe<Type>(ulong id) where Type : class, GM_IIdentifiable
//        {
//            GM_IIdentifiable obj;
//            if (_map.TryGetValue(id, out obj))
//            {
//                return obj as Type;
//            }
//            return null;
//        }



//    }
//}
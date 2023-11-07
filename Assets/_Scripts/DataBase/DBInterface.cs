using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BreadFlip
{
    public static class DBInterface
    {
        public static bool RegisterPlayer(string name)
        {
            var a = new DBPlayer();
            a.player = name;
            return DBWorker.PutRecord(a);
        }

        public static bool UpdateRecord(string name, int record)
        {
            var a = new DBPlayer();
            a.player = name;
            a.record = record;
            return DBWorker.PutRecord(a);
        }

        public static DBTopRecords GetRecords()
        {
            return DBWorker.GetRecords();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyTransferSystem
{
    internal class Event
    {
        private string msg;
        private DateTime time;

        internal Event(string msg,DateTime time)
        {
            this.msg = msg;
            this.time = time;
        }

        internal string GetMessage
        {
            get
            {
                return this.msg;
            }
        }

        internal DateTime GetTime
        {
            get
            {
                return this.time;
            }
        }

        private static object mylocker3 = new object();
        private static List<Event> events = new List<Event>();

        internal static bool Add(Event e)
        {
            lock (mylocker3)
            {
                bool res = false;
                events.Add(e);
                return res;
            }
        }

        internal static bool Read(ref Event e)
        {
            lock (mylocker3)
            {
                bool res = false;
                if (events.Count > 0)
                {
                    e = events[0];
                    events.RemoveAt(0);
                    res = true;
                }
                
                return res;
            }
        }
    }
}

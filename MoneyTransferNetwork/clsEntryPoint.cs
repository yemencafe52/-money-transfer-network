using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace MoneyTransferSystem
{
    class clsEntryPoint
    {
        static void Main(string[] args)
        {
            Server server = new Server(1234);
            server.Start();

            Event.Add(new Event("Start server...", DateTime.Now));

            while (server.IsRunning)
            {
                Event e = new Event("", new DateTime());
                while (Event.Read(ref e))
                {
                    Console.WriteLine("[" + e.GetTime.ToString() + "] " + e.GetMessage);
                }

            }

            server.Stop();
            Event.Add(new Event("Shutdown server...", DateTime.Now));
        }
    }
}

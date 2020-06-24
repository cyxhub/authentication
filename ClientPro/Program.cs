using System;
using System.Net;

namespace ClientPro
{
    class Program
    {
        static void Main(string[] args)
        {
            /* WebClient client = new WebClient();
             string s=client.DownloadString("https://localhost:44397/users/jwtuserList");
             Console.WriteLine(s);*/
            var sum = 0;
            var t1 = DateTime.Now.Millisecond;
            for (var i = 0; i < 10000; i++)
            {
                for (var j = 0; j < 10000; j++)
                {
                    sum += i;
                }
            }
            var t2 = DateTime.Now.Millisecond;
            Console.WriteLine(t2 - t1);
        }
    }
}

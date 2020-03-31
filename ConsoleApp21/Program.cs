using System;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace ConsoleApp21
{
    class Program
    {
        static bool _wroteLine = false;
        static bool _firstDot = true;
        static int _chars = 0;
        static object _lockobj = new object();
        static int _procid;
        static int _minWait;
        static int _maxWait;
        static int _iter;
        static bool _done = false;

        static void Main(string[] args)
        {

            if (args.Length == 0) {
                _procid = 1;
                _minWait = 1500;
                _maxWait = 3000;
                _iter = 60;
            }
            else
            {
                _procid = int.Parse(args[0]);
                _minWait = int.Parse(args[1]);
                _maxWait = int.Parse(args[2]);
                _iter = int.Parse(args[3]);
            }
            Console.WriteLine($"starting {_procid}");
            var rnd = new Random(_procid);

            var ex = Assembly.GetExecutingAssembly().Location;
            Console.WriteLine(ex);
            if (args.Length == 0)
            {
                Process.Start("dotnet", $@"{ex} 2 1000 1000 100");                
                Process.Start("dotnet", $@"{ex} 3 100 500 100");                
                Process.Start("dotnet", $@"{ex} 4 50 50 300");
            }

            var t = new[] {
                Task.Run(() => Do(_procid)),
                Task.Run(() => Progress())
            };

            Task.WaitAll(t);
        }

        static void Do(int rng)
        {
            var rnd = new Random(rng);
            
            foreach (var _ in Enumerable.Range(0, _iter))
            {
                var next = rnd.Next(_minWait, _maxWait);
                Thread.Sleep(next);
                var line = new string(_procid.ToString()[0], rnd.Next(1, 100));
                var lines = Enumerable.Range(0, rnd.Next(1, 10)).Select((i) => line).ToList();
                var joined = string.Join(Environment.NewLine, lines);
                lock (_lockobj)
                {
                    try
                    {
                        _wroteLine = true;
                    if ( _chars > 0 && _chars < Console.CursorLeft)
                    {
                        Console.CursorLeft -= _chars;
                        Console.Write(new string(' ', _chars));
                        _chars = 0;
                    }
                    Console.WriteLine();
                    Enumerable.Range(0, 10).ToList().ForEach(n => {
                        Console.ForegroundColor = (ConsoleColor)_procid + n + 1;
                        Console.Write("aaa" + _procid);
                        //lines.ForEach(l => Console.Write(Environment.NewLine + l));
                    });

                        //Console.Write(Environment.NewLine + joined);
                    }
                    finally
                    {
                        Console.ForegroundColor = ConsoleColor.White;
                    }
                }
            }

            _done = true;
        }

        static void Progress()
        {
            while (!_done)
            {
                Thread.Sleep(300);


                lock (_lockobj)
                {
                    if (_wroteLine)
                    {
                        _wroteLine = false;
                        _firstDot = true;
                        continue;
                    }


                    if (_firstDot)
                    {
                        Console.Write(".");
                        _firstDot = false;
                        _chars += 1;
                    }
                    else
                    {
                        Console.Write(".");
                        _chars++;
                    }
                    _wroteLine = false;

                }
            }
        }
    }
}

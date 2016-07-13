using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;

namespace badpaybad.redis.repository
{
    public class CustomTaskScheduler : IDisposable// remove IDisposable if you want modify code to use as signleton
    {
        Dictionary<string, KeyValuePair<int, Action>> _scheduler = new Dictionary<string, KeyValuePair<int, Action>>();

        Dictionary<string, DateTime> _lastRun = new Dictionary<string, DateTime>();

        object _lock = new object();

        private bool _isStop = false;
        public CustomTaskScheduler()
        {
            _isStop = false;
            new Thread(() =>
            {
                while (!_isStop)
                {
                    try
                    {
                        Do();
                    }
                    finally
                    {
                        Thread.Sleep(100);
                    }

                }

            }).Start();
        }

        public void Add(string name, Action action, int intervalSecs = 30, bool doIntermediate = true)
        {
            lock (_lock)
            {
                KeyValuePair<int, Action> xxx;
                if (!_scheduler.TryGetValue(name, out xxx))
                {
                    _scheduler.Add(name, new KeyValuePair<int, Action>(intervalSecs, action));
                    _lastRun.Add(name, DateTime.Now.AddSeconds(intervalSecs));
                }
                else
                {
                    throw new Exception("Action with name: [" + name + "] is exited");
                }
            }
            if (doIntermediate)
            {
                ThreadPool.QueueUserWorkItem((o) =>
                {
                    action();
                    lock (_lock)
                    {
                        _lastRun[name] = DateTime.Now.AddSeconds(intervalSecs);
                    }
                });
            }          
        }

        public void Remove(string name)
        {
            lock (_lock)
            {
                _scheduler.Remove(name);
                _lastRun.Remove(name);
            }
        }

        private void Do()
        {
            Dictionary<string, DateTime> lastruns;
            Dictionary<string, KeyValuePair<int, Action>> tasks;
            lock (_lock)
            {
                lastruns = _lastRun.ToDictionary(k => k.Key, v => v.Value);
                tasks = _scheduler.ToDictionary(k => k.Key, v => v.Value);
            }

            foreach (var lr in lastruns)
            {
                KeyValuePair<int, Action> t;
                if (tasks.TryGetValue(lr.Key, out t))
                {
                    if (lr.Value > DateTime.Now) return;
                    lock (_lock)
                    {
                        if (_lastRun.ContainsKey(lr.Key))
                        {
                            _lastRun[lr.Key] = DateTime.MaxValue;
                        }
                    }

                    var temp = lr;
                    ThreadPool.QueueUserWorkItem((o) =>
                    {
                        var lastrun = DateTime.Now.AddSeconds(t.Key);
                        var stopwatch = new Stopwatch();
                        try
                        {
                            stopwatch.Start();
                            t.Value();
                            stopwatch.Stop();

                            var l = (int)stopwatch.ElapsedMilliseconds;
                            var diff = t.Key * 1000 - l;
                            lastrun = DateTime.Now.AddMilliseconds(diff);
                        }
                        finally
                        {
                            lock (_lock)
                            {
                                if (_lastRun.ContainsKey(temp.Key))
                                {
                                    _lastRun[temp.Key] = lastrun;
                                }
                            }
                        }

                    });
                }

            }
            lastruns.Clear();
            tasks.Clear();
        }

        public Dictionary<string, int> ListRunningTask()
        {
            Dictionary<string, KeyValuePair<int, Action>> temp ;
            lock (_lock)
            {
                temp= _scheduler.ToDictionary(k => k.Key, v => v.Value);
            }

            return temp.ToDictionary(k => k.Key, v => v.Value.Key);
        } 

        public void Dispose()
        {
            _isStop = true;
        }
    }
}

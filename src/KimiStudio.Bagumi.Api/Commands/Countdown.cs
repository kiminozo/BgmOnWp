using System.Threading;

namespace KimiStudio.Bangumi.Api.Commands
{
    class Countdown
    {
        private readonly object locker = new object();
        private int value; //使用_value来计数

        public Countdown() { }
        public Countdown(int initialCount) { value = initialCount; }

        public void Signal() { AddCount(-1); } //将计数减一

        public void AddCount(int amount)
        {
            lock (locker)
            {
                value += amount; //将计数增加或减少
                if (value <= 0) Monitor.PulseAll(locker);//如果value<=0,说明所有等待的任务都完成了。
            }
        }

        public void Free()
        {
            lock (locker)
            {
                value = 0; //将计数为0
                Monitor.PulseAll(locker); //如果value<=0,说明所有等待的任务都完成了。
            }
        }

        public void Wait()
        {
            lock (locker)
            {
                //只要计数 > 0 就等待。
                while (value > 0)
                {
                    Monitor.Wait(locker);
                }
            }
        }
    }
}
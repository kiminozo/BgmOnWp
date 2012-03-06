using System.Threading;

namespace KimiStudio.Bangumi.Api.Commands
{
    class Countdown
    {
        private readonly object locker = new object();
        private int value; //ʹ��_value������

        public Countdown() { }
        public Countdown(int initialCount) { value = initialCount; }

        public void Signal() { AddCount(-1); } //��������һ

        public void AddCount(int amount)
        {
            lock (locker)
            {
                value += amount; //���������ӻ����
                if (value <= 0) Monitor.PulseAll(locker);//���value<=0,˵�����еȴ�����������ˡ�
            }
        }

        public void Free()
        {
            lock (locker)
            {
                value = 0; //������Ϊ0
                Monitor.PulseAll(locker); //���value<=0,˵�����еȴ�����������ˡ�
            }
        }

        public void Wait()
        {
            lock (locker)
            {
                //ֻҪ���� > 0 �͵ȴ���
                while (value > 0)
                {
                    Monitor.Wait(locker);
                }
            }
        }
    }
}
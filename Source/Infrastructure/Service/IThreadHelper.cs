using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace Infrastructure.Service
{
    public interface IThreadHelper
    {
        void RunOnUIThread(Action action);
    }

    public class ThreadHelper : IThreadHelper
    {
        private Dispatcher _dis;

        public ThreadHelper(Dispatcher dis)
        {
            _dis = dis;
        }

        public void RunOnUIThread(Action action)
        {
            _dis.Invoke(action);
        }
    }
}

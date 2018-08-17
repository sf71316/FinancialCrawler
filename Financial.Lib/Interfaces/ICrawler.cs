using System;
using System.Collections.Generic;
using System.Text;

namespace Financial.Lib.Interfaces
{
    public interface ICrawler
    {
        void Execute();
        event EventHandler<NotifyArg> Notify;
    }
}

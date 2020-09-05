using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace Mochou.Forms.Async
{
    public class UITask
    {
        public static void Task(Control invoke, Action action)
        {
            if (invoke.InvokeRequired)
            {
                invoke.Invoke((EventHandler)(delegate
                {
                    action.Invoke();
                }));
            }
            else {
                action.Invoke();
            }
        }
    }
}

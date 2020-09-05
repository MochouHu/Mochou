using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace Mochou.Forms.Async
{
    public partial class WaitMessage : Form
    {
        public WaitMessage()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 关闭命令
        /// </summary>
        public void CloseOrder()
        {
            if (this.InvokeRequired)
            {
                //这里利用委托进行窗体的操作，避免跨线程调用时抛异常，后面给出具体定义
                UITask.Task(this,new Action(() =>
                {
                    while (!this.IsHandleCreated) Thread.Sleep(10);

                    if (this.IsDisposed)
                        return;
                    if (!this.IsDisposed)
                        this.Dispose();
                }));
            }
            else
            {
                if (this.IsDisposed)
                    return;
                if (!this.IsDisposed)
                    this.Dispose();
            }
        }

        private void WaitMessage_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!this.IsDisposed)
            {
                this.Dispose(true);
            }

        }



        #region 相关变量定义
        /// <summary>
        /// 定义委托进行窗口关闭
        /// </summary>
        private delegate void CloseDelegate();
        private static WaitMessage loadingForm;
        private static readonly Object syncLock = new Object();  //加锁使用

        #endregion


        /// <summary>
        /// 显示loading框
        /// </summary>
        public static void ShowLoadingScreen()
        {
            // Make sure it is only launched once.
            if (loadingForm != null)
                return;
            Thread thread = new Thread(new ThreadStart(WaitMessage.ShowForm));
            thread.IsBackground = true;
            thread.SetApartmentState(ApartmentState.STA);
            thread.Start();

        }

        /// <summary>
        /// 显示窗口
        /// </summary>
        private static void ShowForm()
        {
            if (loadingForm != null)
            {
                loadingForm.CloseOrder();
                loadingForm = null;
            }
            loadingForm = new WaitMessage();
            loadingForm.TopMost = true;
            loadingForm.ShowDialog();
        }

        /// <summary>
        /// 关闭窗口
        /// </summary>
        public static void CloseForm()
        {
            Thread.Sleep(50); //可能到这里线程还未起来，所以进行延时，可以确保线程起来，彻底关闭窗口
            if (loadingForm != null)
            {
                lock (syncLock)
                {
                    Thread.Sleep(50);
                    if (loadingForm != null)
                    {
                        Thread.Sleep(50);  //通过三次延时，确保可以彻底关闭窗口
                        loadingForm.Invoke(new CloseDelegate(WaitMessage.CloseFormInternal));
                    }
                }
            }
        }

        /// <summary>
        /// 关闭窗口，委托中使用
        /// </summary>
        private static void CloseFormInternal()
        {
            loadingForm.CloseOrder();
            loadingForm = null;
        }
    }

}

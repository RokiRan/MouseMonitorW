using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using System.Threading;

namespace MouseMonitorW
{
    [ComVisible(true)]//com+可见
    public partial class main : Form
    {

        [PermissionSet(SecurityAction.Demand, Name = "FullTrust")]
        /// <summary>
        /// 获取窗体的句柄函数
        /// </summary>
        /// <param name="lpClassName">窗口类名</param>
        /// <param name="lpWindowName">窗口标题名</param>
        /// <returns>返回句柄</returns>
        [DllImport("user32.dll", EntryPoint = "FindWindow", SetLastError = true)]
        public static extern IntPtr FindWindow(string lpClassName, string lpWindowName);
        /// <summary>
        /// 通过句柄，窗体显示函数
        /// </summary>
        /// <param name="hWnd">窗体句柄</param>
        /// <param name="cmdShow">显示方式</param>
        /// <returns>返工成功与否</returns>
        [DllImport("user32.dll", EntryPoint = "ShowWindowAsync", SetLastError = true)]
        public static extern bool ShowWindowAsync(IntPtr hWnd, int cmdShow);
        /// <summary>
        /// 通过句柄设置方法
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        [DllImport("user32.dll", EntryPoint = "SwitchToThisWindow", SetLastError = true)]
        public static extern bool SwitchToThisWindow(IntPtr hWnd, bool fAltTab);



        [DllImport("User32.dll", EntryPoint = "PostMessage")]
        private static extern int PostMessage(
        int hWnd,   //   handle   to   destination   window   
        int Msg,   //   message   
        int wParam,   //   first   message   parameter   
        int lParam   //   second   message   parameter   
        );
        private const int WM_KEYDOWN = 0x0100;
        private const int WM_KEYUP = 0x0100;
        int t = 0;

        public struct POINT
        {
            int x;
            int y;
        }
        [DllImport("user32.dll")]
        static extern IntPtr WindowFromPoint(POINT Point);

        [DllImport("user32.dll")]
        static extern IntPtr WindowFromPoint(int xPoint, int yPoint);

        [DllImport("user32.dll")]
        static extern bool GetCursorPos(out POINT lpPoint);

        [DllImport("user32.dll")]
        static extern bool SetWindowText(IntPtr hWnd, string lpString);

        public static POINT GetCursorPos()
        {
            POINT p;
            if (GetCursorPos(out p))
            {
                return p;
            }
            throw new Exception();
        }

        public static IntPtr WindowFromPoint()
        {
            POINT p = GetCursorPos();
            return WindowFromPoint(p);
        }

        public main()
        {
            InitializeComponent();
            this.webBrowser1.ObjectForScripting = this;// 将当前类设置为可由脚本访
        }
        public int isReadyForWindow = 0;
        public IntPtr tarWindow;
        private void monitor_Click(object sender, EventArgs e)
        {
            isReadyForWindow = 1;
            // 获取查找窗体句柄(通过窗体标题名)
            //IntPtr target = FindWindow(null, "金*");
            //SwitchToThisWindow(target, true);
            //Thread.Sleep(100);
            //String test = "123456";
            //for(int i = 0; i < test.Length; i++)
            //{
            //   Thread.Sleep(100);
            //   PostMessage(target.ToInt32(), 256, test[i], 0);
            //}
            //PostMessage(target.ToInt32(), 256, 0x0D, 0);
            //OddDa();

        }

        public void ShowMessage(string msg)
        {
            //MessageBox.Show(msg, "测试内容");
            //IntPtr mainHandle = FindWindow(null, "体育平台 - Google Chrome");
            //IntPtr mainHandle = FindWindow(null, "体育平台 - Google Chrome");
            if (tarWindow != IntPtr.Zero)
            {
                //通过句柄设置当前窗体最大化（0：隐藏窗体，1：默认窗体，2：最小化窗体，3：最大化窗体，....）
                SwitchToThisWindow(tarWindow, true);
                Thread.Sleep(400);
                for (int i = 0; i < msg.Length; i++)
                {
                    Thread.Sleep(30);
                    //KeyBoard.sendKey(msg[i]);
                    PostMessage(tarWindow.ToInt32(), 256, msg[i], 0);
                }
                //PostMessage(mainHandle.ToInt32(), WM_KEYDOWN, 46, 0);
            }
            else
            {
                MessageBox.Show("没有找到窗口,请重新尝试");
                //mainHandle = FindWindow(null, "体育平台 - Google Chrome");
            }

        }
        public void OddDa()
        {
            //IntPtr mainHandle = FindWindow(null, "体育平台 - Google Chrome");
            //MessageBox.Show(msg, "测试内容");
            
            if (tarWindow != IntPtr.Zero)
            {
                //通过句柄设置当前窗体最大化（0：隐藏窗体，1：默认窗体，2：最小化窗体，3：最大化窗体，....）
                SwitchToThisWindow(tarWindow, true);
                Thread.Sleep(400);
                KeyBoard.sendEnter();
                Thread.Sleep(80);
                KeyBoard.sendEnter();
                //PostMessage(mainHandle.ToInt32(), 256, 0x0D, 0);
            }
            else
            {
                MessageBox.Show("没有找到窗口,请重新尝试");
                //mainHandle = FindWindow(null, "体育平台 - Google Chrome");
            }

        }
        Int32[] keysCode=new Int32[2] { 0x30, 0x31 };
        private void webBrowser1_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {

        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void main_Load(object sender, EventArgs e)
        {

        }

        private void main_Leave(object sender, EventArgs e)
        {
            
        }

        private void main_Deactivate(object sender, EventArgs e)
        {
            //MessageBox.Show("失去焦点了 ");
            if (isReadyForWindow == 1)
            {
                //进入状态了，获取鼠标位置
                tarWindow=WindowFromPoint();
                isReadyForWindow = 0;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            SwitchToThisWindow(tarWindow, true);
        }
    }
}

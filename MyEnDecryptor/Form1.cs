using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using static MyEnDecryptor.DESOperation;

namespace MyEnDecryptor
{
    public partial class Form1 : Form
    {
        public static Form1 form1;
        IntPtr nextClipboardViewer;
        public Form1()
        {
            InitializeComponent();
            form1 = this;
            nextClipboardViewer = (IntPtr)SetClipboardViewer((int)Handle);
        }


        /// <summary>
        /// 要处理的 WindowsSystem.Windows.Forms.Message。
        /// </summary>
        /// <param name="m"></param>
        protected override void WndProc(ref Message m)
        {
            // defined in winuser.h
            const int WM_DRAWCLIPBOARD = 0x308;
            const int WM_CHANGECBCHAIN = 0x030D;

            switch (m.Msg)
            {
                case WM_DRAWCLIPBOARD:
                    DisplayClipboardData();
                    SendMessage(nextClipboardViewer, m.Msg, m.WParam, m.LParam);
                    break;
                case WM_CHANGECBCHAIN:
                    if (m.WParam == nextClipboardViewer)
                        nextClipboardViewer = m.LParam;
                    else
                        SendMessage(nextClipboardViewer, m.Msg, m.WParam, m.LParam);
                    break;
                default:
                    base.WndProc(ref m);
                    break;
            }
        }

        /// <summary>
        /// 显示剪贴板内容
        /// </summary>
        public void DisplayClipboardData()
        {
            try
            {
                IDataObject iData = new DataObject();
                iData = Clipboard.GetDataObject();

                if (iData.GetDataPresent(DataFormats.Text))
                {
                    var text = (string)iData.GetData(DataFormats.Text);
                    if (text.StartsWith(":MESSAGE-") && text != textBox2.Text)
                    {
                        textBox2.Text = text;
                        MyDecode();
                        if (textBox1.Text != "" && textBox1.Text != "Error")
                            MessageBox.Show(textBox1.Text);
                    }
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
            }
        }

        /// <summary>
        /// 关闭程序，从观察链移除
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Form2_FormClosed(object sender, FormClosedEventArgs e)
        {
            ChangeClipboardChain(Handle, nextClipboardViewer);
        }

        #region WindowsAPI
        /// <summary>
        /// 将CWnd加入一个窗口链，每当剪贴板的内容发生变化时，就会通知这些窗口
        /// </summary>
        /// <param name="hWndNewViewer">句柄</param>
        /// <returns>返回剪贴板观察器链中下一个窗口的句柄</returns>
        [DllImport("User32.dll")]
        protected static extern int SetClipboardViewer(int hWndNewViewer);

        /// <summary>
        /// 从剪贴板链中移出的窗口句柄
        /// </summary>
        /// <param name="hWndRemove">从剪贴板链中移出的窗口句柄</param>
        /// <param name="hWndNewNext">hWndRemove的下一个在剪贴板链中的窗口句柄</param>
        /// <returns>如果成功，非零;否则为0。</returns>
        [DllImport("User32.dll", CharSet = CharSet.Auto)]
        public static extern bool ChangeClipboardChain(IntPtr hWndRemove, IntPtr hWndNewNext);

        /// <summary>
        /// 将指定的消息发送到一个或多个窗口
        /// </summary>
        /// <param name="hwnd">其窗口程序将接收消息的窗口的句柄</param>
        /// <param name="wMsg">指定被发送的消息</param>
        /// <param name="wParam">指定附加的消息特定信息</param>
        /// <param name="lParam">指定附加的消息特定信息</param>
        /// <returns>消息处理的结果</returns>
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern int SendMessage(IntPtr hwnd, int wMsg, IntPtr wParam, IntPtr lParam);
        #endregion


        // de
        private void button1_Click(object sender, EventArgs e)
        {
            MyDecode();
        }

        // en
        private void button2_Click(object sender, EventArgs e)
        {
            MyEncode();
        }


        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter && e.Control && e.Shift)
            {
                MyEncode();
            }
        }

        private void MyEncode()
        {
            var str = EncryptString(textBox1.Text);
            if (str != "Error")
            {
                textBox2.Text = str;
                Clipboard.SetText(str);
            }
        }

        private void MyDecode()
        {
            textBox1.Text = "";
            var str = DecryptString(textBox2.Text);
            if (!str.StartsWith(":MESSAGE-") && str != "Error")
            {
                textBox1.Text = str;
            }
        }

        private void textBox3_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!(e.KeyChar == '\b' || (e.KeyChar >= '0' && e.KeyChar <= '9')))
            {
                e.Handled = true;
            }
        }


        private void button3_Click(object sender, EventArgs e)
        {
            MessageBox.Show(
@"- 在左框输入原文 按下CtrlShiftEnter或单击Encrypt按钮 右框会显示密文并自动复制
- 在右框输入密文 单击Decrypt按钮 左框会显示原文
- 直接复制密文 右框会自动粘贴密文 左框会自动显示原文 并且弹出内容为原文的信息框
- 可以在中间的框定义一个八位数字密码 如果为空会自动定义但不显示"
);
        }
    }
}

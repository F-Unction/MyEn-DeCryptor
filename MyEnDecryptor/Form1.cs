using System;
using System.Windows.Forms;
using static MyEnDecryptor.DESOperation;

namespace MyEnDecryptor
{
    public partial class Form1 : Form
    {
        public static Form1 form1;
        public Form1()
        {
            InitializeComponent();
            form1 = this;
        }

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
            if (!str.StartsWith("=MeSsAgE=") && str != "Error")
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

        private void timer1_Tick(object sender, EventArgs e)
        {
            var text = GetClipBoardText();
            if (text.StartsWith("=MeSsAgE=") && text != textBox2.Text)
            {
                textBox2.Text = text;
                MyDecode();
                if (textBox1.Text != "" && textBox1.Text != "Error")
                {
                    MessageBox.Show(textBox1.Text);
                }
            }

        }

        private string GetClipBoardText()
        {
            if (Clipboard.ContainsText(TextDataFormat.Text))
            {
                return Clipboard.GetText(TextDataFormat.Text);
            }
            return "";
        }
    }
}

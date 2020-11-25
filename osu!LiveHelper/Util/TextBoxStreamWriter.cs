using System;
using System.IO;
using System.Text;
using System.Windows.Controls;

namespace osu_LiveHelper.Util
{
    class TextBoxStreamWriter : TextWriter
    {
        TextBox _output = null;

        public TextBoxStreamWriter(TextBox output)
        {
            _output = output;
        }

        public override void WriteLine(string value)
        {
            base.Write(value);
            _output.Dispatcher.Invoke(new Action(() => _output.AppendText("[" + DateTime.Now.ToString("H:mm:ss") + "] " + value.ToString() + "\n")));
            _output.Dispatcher.Invoke(new Action(() => _output.ScrollToEnd()));
        }

        public override Encoding Encoding
        {
            get { return System.Text.Encoding.UTF8; }
        }
    }
}

/*  ---------------------------------------------------------------------------------------------------------------------------------------
 *  (C) 2019, Dr Warren Creemers.
 *  This file is subject to the terms and conditions defined in the included file 'LICENSE.txt'
 *  ---------------------------------------------------------------------------------------------------------------------------------------
 */
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WD_toolbox.Data.Text
{
    /// <summary>
    /// A stream handeler that redirects all characters written to an event handeler.
    /// For this times when you have a stream, but wanted events.
    /// </summary>
    public class TextWriter2Event : TextWriter
    {
        public override Encoding Encoding
        {
            get { return Encoding.ASCII; }
        }

        StringBuilder currentLine = new StringBuilder();

        public EventHandler<char> OnChar { get; set; }
        public EventHandler<string> OnLine { get; set; }

        public TextWriter2Event(EventHandler<string> _onLine)
        {
            this.OnLine = _onLine;
        }

        public TextWriter2Event(EventHandler<char> _onChar)
        {
            this.OnChar = _onChar;
        }

        /// <summary>
        /// Apparently this is the only method in TextWriter that needs to be overridden.
        /// AQll other write methods call this.
        /// </summary>
        /// <param name="value"></param>
        public override void Write(char value)
        {
            OnChar.SafeCall(this, value);

            if ((value == '\r') || (value == '\n'))
            {
                if (currentLine.Length > 0)
                {
                    OnLine.SafeCall(this, currentLine.ToString());
                }
                currentLine.Clear();
            }
            else
            {
                currentLine.Append(value);
            }
        }
    }
}

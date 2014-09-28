using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using System.Drawing;
using System.ComponentModel;

namespace MathControls
{
    class MathLabel : Control
    {
        private string _Text;
        private Color _ForeColor;
        private Color _BackColor;
        private Font _Font;

        [DefaultValue("")]
        public string Text
        {
            get
            {
                return _Text;
            }
            set
            {
                _Text = value;
                Invalidate();
            }
        }

        [DefaultValue(typeof(Color), "0; 238; 238")]
        public Color ForeColor
        {
            get
            {
                return _ForeColor;
            }
            set
            {
                _ForeColor = value;
                Invalidate();
            }
        }

        [DefaultValue(typeof(Color), "Transparent")]
        public Color BackColor
        {
            get
            {
                return _BackColor;
            }
            set
            {
                _BackColor = value;
                Invalidate();
            }
        }

        [DefaultValue(typeof(Font), "Inconsolata; 11,25pt")]
        public Font Font
        {
            get
            {
                return _Font;
            }
            set
            {
                _Font = value;
                Invalidate();
            }
        }

        public MathLabel()
        {
            
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            if (Font == null)
                Font = Parent.Font;

            if (ForeColor == null)
                ForeColor = Parent.ForeColor;

            if (BackColor == null)
                BackColor = Parent.BackColor;
           
            Graphics g = e.Graphics;
            Regex Fraction = new Regex(@"\d+\/\d+");

            int XArrival = 2;
            int SqrtStarted = 0;
            int YAugmentate = 0;
            string[] Ops = _Text.Split(' ');

            foreach (string Op in Ops)
            {
                MatchCollection mc = Fraction.Matches(Op);

                if(mc.Count > 0)
                    foreach (Match m in mc)
                    {
                        string[] MyMatches = m.Value.Split('/');
                        g.DrawString(MyMatches[0], Font, new SolidBrush(ForeColor), new Point(XArrival, 8 + YAugmentate));
                        g.DrawString(InternalMathUtils.ForEachChar(0, "_", MyMatches), Font, new SolidBrush(ForeColor), new Point(XArrival, 14 + YAugmentate));
                        g.DrawString(MyMatches[1], Font, new SolidBrush(ForeColor), new Point(XArrival, 30 + YAugmentate));
                        XArrival += TextRenderer.MeasureText(InternalMathUtils.ForEachChar(0, "9", MyMatches), Font).Width;
                    }

                if (Op == "-sqrt-")
                {
                    //√
                    Font ff = new Font(Font.FontFamily, 20, FontStyle.Regular);
                    g.DrawString("√", ff, new SolidBrush(ForeColor), new Point(XArrival + 8, 4 + YAugmentate));
                    SqrtStarted = XArrival + 14;
                    XArrival += TextRenderer.MeasureText(Op, Font).Width;
                }

                if (Op == "-esqrt-")
                {
                    g.DrawLine(new Pen(new SolidBrush(ForeColor)), new Point(SqrtStarted + 11, 4 + YAugmentate), new Point(XArrival, 4 + YAugmentate));
                }

                if (Op == "+" || Op == "-" || Op == "/" || Op == "x" || Op == "=")
                {
                    g.DrawString(Op, Font, new SolidBrush(ForeColor), new Point(XArrival, 12 + YAugmentate));
                    XArrival += TextRenderer.MeasureText(Op, Font).Width;
                }

                if (Op == "(" || Op == ")" || Op == "[" || Op == "]" || Op == "{" || Op == "}")
                {
                    Font ff = new Font(Font.FontFamily, 20, FontStyle.Regular);
                    g.DrawString(Op, ff, new SolidBrush(ForeColor), new Point(XArrival, 0 + YAugmentate));
                    XArrival += TextRenderer.MeasureText(Op, ff).Width - 6;
                }

                if (Op == "-n-")
                {
                    YAugmentate += 48;
                    XArrival = 2;
                    SqrtStarted = 0;
                }

                int TestOut = 0;
                if (int.TryParse(Op, out TestOut))
                {
                    g.DrawString(Op, Font, new SolidBrush(ForeColor), new Point(XArrival, 12 + YAugmentate));
                    XArrival += TextRenderer.MeasureText(Op, Font).Width;
                }
            }

            
            Size = new Size((int)g.ClipBounds.Width, (int)g.ClipBounds.Height);
        }
    }
	
	class InternalMathUtils
	{
		public static int MajorLength(string[] Strings)
        {
            int Len = 0;
            foreach (string str in Strings)
                if (str.Length > Len)
                    Len = str.Length;

            return Len;
        }

        public static string ForEachChar(int Length, string MyChar, string[] CaseOf = null)
        {
            if (Length == 0 && CaseOf != null)
                Length = InternalMathUtils.MajorLength(CaseOf);

            string MyString = "";
            for (int i = 0; i < Length; i++)
                MyString += MyChar;

            return MyString;
        }
	}
}

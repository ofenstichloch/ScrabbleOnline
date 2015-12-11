using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using ScrabbleClasses;

namespace ScrabbleServerTest
{
    public partial class Form_Board : Form
    {
        delegate void refreshBoardDelegate(Board b);
        delegate void refreshHandDelegate(Hand h);
        delegate void logDelegate(string s);
        bool created = false;
        public Form_Board()
        {
            InitializeComponent();
        }

        static void Main(object state)
        {
            Application.Run((Form)state);
        }

        public static Form_Board Create()
        {
            Form_Board f = new Form_Board();
            Thread t = new Thread(Main);
            t.Start(f);
            return f;
        }
        private void Form_Board_Load(object sender, EventArgs e)
        {

        }


        public void refreshHand(Hand h)
        {
            if (tblHand.InvokeRequired)
            {
                refreshHandDelegate r = new refreshHandDelegate(refreshHand);
                this.Invoke(r, new object[] { h });
            }
            else
            {
                tblHand.SuspendLayout();
                int i = 0;
                foreach (Stone s in h.getStones())
                {
                    ((Button)tblHand.GetControlFromPosition(i, 0)).Text = s.letter + "(" + s.value + ")";
                    i++;
                }
            }
        }

        /*
         * TODO
         * Refactor colors, add them to Field.Types
         * */
        public void refreshBoard(Board b)
        {
            if (tblBoard.InvokeRequired)
            {
                refreshBoardDelegate r = new refreshBoardDelegate(refreshBoard);
                this.Invoke(r, new object[] { b });
            }
            else
            {
                if (!created)
                {
                    tblBoard.SuspendLayout();
                    this.tblBoard.ColumnCount = b.getWidth();
                    this.tblBoard.RowCount = b.getHeight();
                    for (int i = 0; i < b.getWidth(); i++)
                    {
                        for (int j = 0; j < b.getHeight(); j++)
                        {
                            Label t = new Label();
                            Field f = b.getField(i, j);
                            Stone s = f.getStone();
                            t.Dock = DockStyle.Fill;
                            t.Font = new Font("Arial", 20);
                            switch (f.getType())
                            {
                                case Field.Types.DoubleLetter:
                                    t.BackColor = Color.LightSalmon;
                                    break;
                                case Field.Types.DoubleWord:
                                    t.BackColor = Color.LightGreen;
                                    break;
                                case Field.Types.TripleLetter:
                                    t.BackColor = Color.DarkSalmon;
                                    break;
                                case Field.Types.TripleWord:
                                    t.BackColor = Color.DarkGreen;
                                    break;
                                default:
                                    break;
                            }

                            if (s != null)
                            {
                                t.Text = s.letter + "(" + s.value + ")";
                            }
                            tblBoard.Controls.Add(t, i, j);
                        }
                    }
                    tblBoard.ResumeLayout();
                    created = true;
                }
                else
                {
                    tblBoard.SuspendLayout();
                    for (int i = 0; i < b.getWidth(); i++)
                    {
                        for (int j = 0; j < b.getHeight(); j++)
                        {
                            Field f = b.getField(i, j);
                            Stone s = f.getStone();
                            switch(f.getType()){
                                case Field.Types.DoubleLetter:
                                    ((Label)tblBoard.GetControlFromPosition(i, j)).BackColor = Color.LightSalmon;
                                    break;
                                case Field.Types.DoubleWord:
                                    ((Label)tblBoard.GetControlFromPosition(i, j)).BackColor = Color.LightGreen;
                                    break;
                                case Field.Types.TripleLetter:
                                    ((Label)tblBoard.GetControlFromPosition(i, j)).BackColor = Color.DarkSalmon;
                                    break;
                                case Field.Types.TripleWord:
                                    ((Label)tblBoard.GetControlFromPosition(i, j)).BackColor = Color.DarkGreen;
                                    break;
                                default:
                                    break;
                            }

                            if (s != null)
                            {
                                ((Label)tblBoard.GetControlFromPosition(i, j)).Text = s.letter + "(" + s.value + ")";
                            }
                        }
                    }
                    tblBoard.ResumeLayout();
                }
            }
                
            
        }

        internal void log(string p)
        {
            if (tblHand.InvokeRequired)
            {
                logDelegate r = new logDelegate(log);
                this.Invoke(r, new object[] { p });
            }
            else
            {
                lbStats.Text += p + "\n\r";
            }
        }
    }
}

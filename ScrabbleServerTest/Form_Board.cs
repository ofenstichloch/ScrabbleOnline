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
        private Client c;
        private Hand hand;
        private Board board;
        static private Thread formThread;

        bool created = false;
        public Form_Board()
        {
            InitializeComponent();
        }
        [STAThread]
        static void Main(object state)
        {
            Application.Run((Form)state);
        }

        public static Form_Board Create(Client c)
        {
            Form_Board f = new Form_Board();
            f.c = c;
            Form_Board.formThread = new Thread(Main);
            Form_Board.formThread.SetApartmentState(ApartmentState.STA);
            Form_Board.formThread.Start(f);
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
                    ((Button)tblHand.GetControlFromPosition(i, 0)).Enabled = true;
                    i++;
                }
                while (i < 7)
                {
                    ((Button)tblHand.GetControlFromPosition(i, 0)).Enabled = false;
                    ((Button)tblHand.GetControlFromPosition(i, 0)).Text = "";
                    i++;
                }
                hand = h;
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
                            t.AutoSize = false;
                            t.Font = new Font("Arial", 20);
                            t.AllowDrop = true;
                            t.DragDrop += new System.Windows.Forms.DragEventHandler(this.tblBoard_DragDrop);
                            t.DragEnter += new System.Windows.Forms.DragEventHandler(this.tblBoard_DragEnter);
                            t.MouseClick += new System.Windows.Forms.MouseEventHandler(this.tblBoad_Click);
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
                            else
                            {
                                t.Text = "";
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
                            else
                            {
                                ((Label)tblBoard.GetControlFromPosition(i, j)).Text ="";
                            }
                        }
                    }
                    tblBoard.ResumeLayout();
                }
                board = b;
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

        private void quitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            c.disconnect();
            Application.Exit();
        }

        private void connectToServerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            c.connect();
        }

        private void startGameToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (c.isConnected())
            {
                c.startGame();
            }
        }

        private void changeNameToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void btStone_MouseDown(object sender, MouseEventArgs e)
        {
            if (sender.GetType() != typeof(Button)) { return; }
            ((Button)sender).DoDragDrop(hand.getStones()[int.Parse(((Button)sender).Tag.ToString())-1],DragDropEffects.Move);
        }

        private void Form_Board_FormClosing(object sender, FormClosingEventArgs e)
        {
            c.disconnect();
        }

        private void tblBoard_DragDrop(object sender, DragEventArgs e)
        {
            Stone s = (Stone)e.Data.GetData(typeof(Stone));
            TableLayoutPanelCellPosition pos = tblBoard.GetCellPosition((Label)sender);
            board.placeStone(pos.Column, pos.Row,s) ;
            hand.removeStone(s.letter);
            refreshHand(hand);
            refreshBoard(board);
        }

        private void tblBoad_Click(object sender, MouseEventArgs e)
        {
            TableLayoutPanelCellPosition pos = tblBoard.GetCellPosition((Label)sender);
            if (board.getField(pos.Column, pos.Row).getStone() == null)
            {
                return;
            }
            hand.addStones(new Stone[] {board.removeStone(pos.Column, pos.Row)});
            refreshHand(hand);
            refreshBoard(board);
        }

        private void tblBoard_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Move;
        }

        private void btSubmit_Click(object sender, EventArgs e)
        {
            //Recognize word, start position and orientation
            int startx=0, starty=0, length = 0;
            bool horizontal;


            //send to server
        }

 
    }
}

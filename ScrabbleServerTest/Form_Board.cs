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
        delegate void roundControlDelegate();
        private Client c;
        private Hand hand;
        private Board board;
        private List<Field> changed;
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
            f.changed = new List<Field>();
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

        //On round end disable submit button, disable drag onto Board
        public void roundEnd(){
            if (btSubmit.InvokeRequired)
            {
                roundControlDelegate r = new roundControlDelegate(roundEnd);
                this.Invoke(r, new object[] { });
            }
            else
            {
                changed.Clear();
                btSubmit.Enabled = false;
                foreach (Control c in tblBoard.Controls)
                {
                    if (typeof(Label) == c.GetType())
                    {
                        ((Label)c).AllowDrop = false;
                    }
                }
            }
        }

        public void roundStart()
        {
            if (btSubmit.InvokeRequired)
            {
                roundControlDelegate r = new roundControlDelegate(roundStart);
                this.Invoke(r, new object[] { });
            }
            else
            {
                btSubmit.Enabled = true;
                foreach (Control c in tblBoard.Controls)
                {
                    if (typeof(Label) == c.GetType())
                    {
                        ((Label)c).AllowDrop = true;
                    }
                }
                changed = new List<Field>(7);
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
                lbStats.AppendText(p + "\r\n");
                Console.Out.WriteLine(p);
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
            changed.Add(board.getField(pos.Column, pos.Row));
            refreshHand(hand);
            refreshBoard(board);
        }

        private void tblBoad_Click(object sender, MouseEventArgs e)
        {
            TableLayoutPanelCellPosition pos = tblBoard.GetCellPosition((Label)sender);
            if (board.getField(pos.Column, pos.Row).getStone() == null || board.getField(pos.Column,pos.Row).locked)
            {
                return;
            }
            changed.Remove(board.getField(pos.Column, pos.Row));
            hand.addStone(board.removeStone(pos.Column, pos.Row));
            refreshHand(hand);
            refreshBoard(board);
        }

        private void tblBoard_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Move;
        }

        private void btSubmit_Click(object sender, EventArgs e)
        {
            if (changed.Count == 0)
            {
                c.emptyMove();
                return;
            }
            // TODO: Tidy this ugly thing up
            //Recognize word, start position and orientation
            int startx=0, starty=0, length = 0;
            bool horizontal;
            Move m ;
            changed.Sort();
            length = changed.Count;
            startx = changed[0].x;
            starty = changed[0].y;
            string word = "";
            //get orientation
            if (length == 1)
            {
                horizontal =true;   
            }else{
                if (changed[0].x != changed[1].x)
                {
                    horizontal = true;
                }
                else
                {
                    horizontal = false;
                }
            }
            word += changed[0].getStone().letter;
            //check for correct positioning and build word string
            for (int i = 1; i < changed.Count; i++)
            {
                if ((horizontal && changed[i - 1].x < changed[i].x-1) 
                    ||(!horizontal && (changed[i - 1].y < changed[i].y-1)) )
                {
                    int j = 0;
                    if (horizontal) { j = changed[i].x - changed[i - 1].x; }
                    else { j = changed[i].y - changed[i - 1].y; }
                    for (int k = 1; k < j; k++)
                    {
                        if (horizontal)
                        {
                            if (board.getField(changed[i - 1].x + k, starty).getStone() != null)
                            {
                                word += board.getField(changed[i - 1].x + k, starty).getStone().letter;
                            }
                            else {
                                log("Wrong placement!");
                                resetChanges();
                                return; 
                            }
                        }
                        else
                        {
                            if (board.getField(startx,changed[i-1].y + k).getStone() != null)
                            {
                                word += board.getField(startx, changed[i - 1].y + k).getStone().letter;
                            }
                            else {
                                log("Wrong placement!");
                                resetChanges();
                                return;
                            }
                        }
                    }
                    
                }
                word += changed[i].getStone().letter;               
                if ((horizontal && changed[i].y != starty) || (!horizontal && changed[i].x != startx))
                {
                    log("Wrong placement!");
                    //give back stones, clear board;
                    resetChanges();
                    return;
                }
                
            }
            log("Sending word: " + word);
            m = new Move(1, word, new int[] { startx, starty }, horizontal);
            //send to server
            c.move(m);
        }

        private void resetChanges()
        {
            Stone[] stones = new Stone[changed.Count];
            int i = 0;
            foreach (Field f in changed)
            {
                stones[i] = f.getStone();
                board.removeStone(f.x,f.y);
                i++;
            }
            hand.addStones(stones);
            refreshBoard(board);
            refreshHand(hand);
            changed.Clear();
        }

 
    }
}

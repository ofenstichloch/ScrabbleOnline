using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace ScrabbleClient
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            //Application.Run(new SharpGLForm());
            Form f = new Form();
            f.Size = new System.Drawing.Size(1000, 800);
            TableLayoutPanel board = new TableLayoutPanel();
            board.Size = new System.Drawing.Size(1000, 800);
            board.Anchor = (AnchorStyles.Left | AnchorStyles.Bottom|AnchorStyles.Right|AnchorStyles.Top);
            board.Location = new System.Drawing.Point(0, 0);
            board.CellBorderStyle = TableLayoutPanelCellBorderStyle.Single;
            f.Controls.Add(board);
            Application.Run(f);
            Console.Out.WriteLine("TEST");

        }
    }
}

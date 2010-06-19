using System;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using System.Drawing;
namespace peatetris
{
    /// <summary>
    /// A panel that can display blocks.
    /// </summary>
    public class BlockArea: Panel
    {
        #region public properties
        /// <summary>
        /// Gets number of columns.
        /// </summary>
        public int Cols {
            get { return cols; }
        }

        /// <summary>
        /// Gets number of rows.
        /// </summary>
        public int Rows
        {
            get { return Rows; }
        }

        /// <summary>
        /// Gets the internal game array.
        /// </summary>
        public Square[,] GameArray
        {
            get { return gameArray; }
        }

        /// <summary>
        /// Gets or sets current dropping block.
        /// </summary>
        public Block CurrentBlock
        {
            get { return currentBlock; }
            set { currentBlock = value; }
        }
        #endregion

        #region constructor
        /// <summary>
        /// Creates a BlockArea.
        /// </summary>
        /// <param name="rows">number of rows</param>
        /// <param name="cols">number of columns</param>
        public BlockArea(int rows, int cols):base()
        {
            gameArray = new Square[rows, cols];
            this.rows = rows;
            this.cols = cols;
            for (int i = 0; i < rows; i++)
                for (int j = 0; j < cols; j++) {
                    gameArray[i, j] = new Square(j, i);
                    gameArray[i, j].ShowEvent += new EventHandler(ShowSquare);
                    gameArray[i, j].HideEvent += new EventHandler(HideSquare);
                }
        }
        #endregion

        #region public methods
        /// <summary>
        /// Get the square from the sender object,
        /// define the size of a square, and create a gradient
        /// brush to fill the square with.
        /// </summary>
        public void ShowSquare(object sender, EventArgs e)
        {
            Square sq = sender as Square;
            Graphics g = CreateGraphics();
            DrawSquare(g, sq);
            g.Dispose();
        }
        /// <summary>
        /// Draw over the square with the background colour.
        /// </summary>
        public void HideSquare(object sender, EventArgs e)
        {
            Square sq = sender as Square;
            Graphics g = CreateGraphics();
            g.FillRectangle(new SolidBrush(BackColor), sq.Location.X * squareSize, sq.Location.Y * squareSize, squareSize, squareSize);
            g.Dispose();
        }

        /// <summary>
        /// Return the square at the specified location. Otherwise,
        /// throw an exception.
        /// </summary>
        public Square GetSquare(int x, int y)
        {
            if (x < 0 || x >= cols || y < 0 || y >= rows)
                return null;
            //throw new ArgumentOutOfRangeException();
            return GameArray[y, x];
        }

        #endregion
        
        #region protected methods

        /// <summary>
        /// Repaint the visible squares when the game area needs to be repainted.
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            for (int i = 0; i < rows; i++)
                for (int j = 0; j < cols; j++) {
                    Square sq = gameArray[i, j];
                    if (sq.Visible)
                        DrawSquare(e.Graphics, sq);
                }
        }
        #endregion

        #region private methods
        /// <summary>
        /// draw a square on a Graphics surface.
        /// </summary>
        private void DrawSquare(Graphics g, Square sq)
        {
            GraphicsPath path = new GraphicsPath();
            Rectangle rect = new Rectangle(sq.Location.X * squareSize, sq.Location.Y * squareSize, squareSize, squareSize);
            path.AddRectangle(rect);
            PathGradientBrush pthGrBrush = new PathGradientBrush(path);
            pthGrBrush.CenterColor = sq.BackColor;
            Color[] halo = { sq.ForeColor };
            pthGrBrush.SurroundColors = halo;
            g.FillRectangle(pthGrBrush, rect);
        }
        #endregion

        #region private members
        /// <summary>
        /// Represents the squares array.
        /// </summary>
        private Square[,] gameArray;
        /// <summary>
        /// Number of rows of the game area;
        /// </summary>
        private int rows = 20;
        /// <summary>
        /// Number of columns of the game area;
        /// </summary>
        private int cols = 10;
        /// <summary>
        /// the size of a square.
        /// </summary>
        private int squareSize = 20;
        /// <summary>
        /// the current dropping block.
        /// </summary>
        private Block currentBlock;
        #endregion
    }
}

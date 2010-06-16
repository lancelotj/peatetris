/*
   Author: Lance Jian

   Distributed under the terms of the GPL (GNU Public License)

   peatetris is free software; you can redistribute it and/or modify
   it under the terms of the GNU General Public License as published by
   the Free Software Foundation; either version 2 of the License, or
   (at your option) any later version.

   This program is distributed in the hope that it will be useful,
   but WITHOUT ANY WARRANTY; without even the implied warranty of
   MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
   GNU General Public License for more details.

   You should have received a copy of the GNU General Public License
   along with this program; if not, write to the Free Software
   Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA  02111-1307  USA
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Drawing2D;
namespace peatetris {
    /// <summary>
    /// This is where all the blocks will be displayed and manipulated.
    /// The game area does not include the title screen, the score,
    /// or the Next Block section.
    /// </summary>
    public class GameArea : Panel {
        #region constructor
        /// <summary>
        /// Create a two-dimensional array of pre-defined size.
        /// add an event handler to show and hide the squares
        /// in each block.
        /// </summary>
        public GameArea() {
            gameArray = new Square[rows, columns];
            for (int i = 0; i < rows; i++)
                for (int j = 0; j < columns; j++) {
                    gameArray[i, j] = new Square(j, i);
                    gameArray[i, j].ShowEvent += new EventHandler(ShowSquare);
                    gameArray[i, j].HideEvent += new EventHandler(HideSquare);
                }
        }
        #endregion

        #region Public events
        /// <summary>
        /// Fires when the block reaches the bottom and cannot be moved.
        /// </summary>
        public event EventHandler StopMoveEvent;
        /// <summary>
        /// Fires when the lines are eliminated, the current block is cleared
        /// and ready to start a new block.
        /// </summary>
        public event EventHandler StartNewEvent;
        #endregion

        #region public properties
        /// <summary>
        /// Number of rows
        /// </summary>
        public int Rows {
            get { return rows; }
        }
        /// <summary>
        /// Number of columns;
        /// </summary>
        public int Columns {
            get { return columns; }
        }

        #endregion

        #region public methods
        /// <summary>
        /// Return the square at the specified location. Otherwise,
        /// throw an exception.
        /// </summary>
        public Square GetSquare(int x, int y) {
            if (x < 0 || x >= columns || y < 0 || y >= rows)
                return null;
            //throw new ArgumentOutOfRangeException();
            return gameArray[y, x];
        }


        /// <summary>
        /// Get the square from the sender object,
        /// define the size of a square, and create a gradient
        /// brush to fill the square with.
        /// </summary>
        public void ShowSquare(object sender, EventArgs e) {
            Square sq = sender as Square;
            Graphics g = CreateGraphics();
            DrawSquare(g, sq);
            g.Dispose();
        }
        /// <summary>
        /// Draw over the square with the background colour.
        /// </summary>
        public void HideSquare(object sender, EventArgs e) {
            Square sq = sender as Square;
            Graphics g = CreateGraphics();
            g.FillRectangle(new SolidBrush(BackColor), sq.Location.X * squareSize, sq.Location.Y * squareSize, squareSize, squareSize);
            g.Dispose();
        }
        /// <summary>
        /// Create a new random block, selecting from all possible blocks.
        /// </summary>
        public void NewBlock() {

            BlockType type = (BlockType)rnd.Next(7);
            switch (type) {
                case BlockType.I:
                    currentBlock = new IBlock(this, 3, 0);
                    break;
                case BlockType.J:
                    currentBlock = new JBlock(this, 3, 0);
                    break;
                case BlockType.L:
                    currentBlock = new LBlock(this, 3, 0);
                    break;
                case BlockType.S:
                    currentBlock = new SBlock(this, 3, 0);
                    break;
                case BlockType.Z:
                    currentBlock = new ZBlock(this, 3, 0);
                    break;
                case BlockType.O:
                    currentBlock = new OBlock(this, 3, 0);
                    break;
                case BlockType.T:
                    currentBlock = new TBlock(this, 3, 0);
                    break;
            }
            currentBlock.Show();
        }
        /// <summary>
        /// Move block left.
        /// </summary>
        public void MoveLeft() {
            if (currentBlock != null)
                currentBlock.Left();
        }
        /// <summary>
        /// Move block right
        /// </summary>
        public void MoveRight() {
            if (currentBlock != null)
                currentBlock.Right();
        }
        /// <summary>
        /// Move block down. If it reaches the bottom, fire the StopMoveEvent. 
        /// It will then check if there are any lines that need to be eliminated.
        /// </summary>
        public void MoveDown() {
            if (currentBlock == null) {
                return;
            }
            if (currentBlock.CanMoveDown()) {
                currentBlock.Down();
            }
            else {
                if (StopMoveEvent != null) {
                    StopMoveEvent(this, null);
                }
                EliminateLines(currentBlock.BottomIndex());
                currentBlock = null;
                if (StartNewEvent != null) {
                    StartNewEvent(this, null);
                }
            }

        }

        /// <summary>
        /// Rotate block
        /// </summary>
        public void RotateBlock() {
            if (currentBlock != null)
                currentBlock.Rotate();
        }

        /// <summary>
        /// Eliminate full lines from the specific row.
        /// </summary>
        /// <param name="row">the row to start checking</param>
        public void EliminateLines(int row) {
            int max = Math.Max(row - 3, 0);
            for (int i = row; i >= max; i--) {
                bool elim = true;
                for (int j = 0; j < columns; j++)
                    if (!gameArray[i, j].Visible) {
                        elim = false;
                        break;
                    }
                if (!elim) {
                    break;
                }
                for (int j = 0; j < columns; j++)
                    gameArray[i, j].ClearEvents(); // unregister events, prevent memory leak.
                for (int k = i; k > 0; k--)
                    for (int j = 0; j < columns; j++) {
                        gameArray[k, j] = gameArray[k - 1, j];
                        gameArray[k, j].Location = new Point(j, k);
                    }
                for (int j = 0; j < columns; j++) {
                    gameArray[0, j] = new Square(j, 0);
                    gameArray[0, j].ShowEvent += new EventHandler(ShowSquare);
                    gameArray[0, j].HideEvent += new EventHandler(HideSquare);
                }
                i++; // one line is eliminated, so recheck this line.
                max++; // the upper bound is moved down
                Refresh();
            }

        }

        #endregion

        #region protected methods

        /// <summary>
        /// Repaint the visible squares when the game area needs to be repainted.
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPaint(PaintEventArgs e) {
            base.OnPaint(e);
            for (int i = 0; i < rows; i++)
                for (int j = 0; j < columns; j++) {
                    Square sq = gameArray[i, j];
                    if (sq.Visible)
                        DrawSquare(e.Graphics, sq);
                }
        }
        #endregion

        #region private members
        /// <summary>
        /// draw a square on a Graphics surface.
        /// </summary>
        private void DrawSquare(Graphics g, Square sq) {
            GraphicsPath path = new GraphicsPath();
            Rectangle rect = new Rectangle(sq.Location.X * squareSize, sq.Location.Y * squareSize, squareSize, squareSize);
            path.AddRectangle(rect);
            PathGradientBrush pthGrBrush = new PathGradientBrush(path);
            pthGrBrush.CenterColor = sq.BackColor;
            Color[] halo = { sq.ForeColor };
            pthGrBrush.SurroundColors = halo;
            g.FillRectangle(pthGrBrush, rect);
        }

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
        private int columns = 10;
        /// <summary>
        /// the current dropping block.
        /// </summary>
        private Block currentBlock;
        /// <summary>
        /// a random number generator.
        /// </summary>
        private Random rnd = new Random((int)DateTime.Now.Ticks);
        /// <summary>
        /// the size of a square.
        /// </summary>
        private int squareSize = 20;
        #endregion
    }
}

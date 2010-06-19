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
using System.Drawing;

namespace peatetris {
    /// <summary>
    /// The type of the blocks.
    /// </summary>
    public enum BlockType { J, L, O, T, Z, S, I }

    /// <summary>
    /// Represents a block of the game.
    /// <summary>
    public abstract class Block {
        /// <summary>
        /// Creates a block.
        /// </summary>
        /// <param name="blockArea">the BlockArea to stay on</param>
        /// <param name="x">the initial x</param>
        /// <param name="y">the inital y</param>
        public Block(BlockArea blockArea, int x, int y) {
            this.blockArea = blockArea;
            location.X = x;
            location.Y = y;
            patterns = new List<Point[]>();
            InitPatterns();
            squares = GetSquares(location.X, location.Y, 0);
        }

        /// <summary>
        /// The location of the block.
        /// </summary>
        public Point Location {
            get { return location; }
            set {
                location = value;
                squares = GetSquares(location.X, location.Y, patternId);
            }
        }
        /// <summary>
        /// The foreground color of the block.
        /// </summary>
        public Color ForeColor {
            get { return foreColor; }
            set { foreColor = value; }
        }
        /// <summary>
        /// The background color of the block.
        /// </summary>
        public Color CenterColor {
            get { return centerColor; }
            set { centerColor = value; }
        }
        /// <summary>
        /// The squares array of the block.
        /// </summary>
        public Square[] Squares {
            get { return squares; }
        }

        /// <summary>
        /// Gets or sets the BlockArea that the current block belongs to.
        /// </summary>
        public BlockArea BlockArea {
            get { return blockArea; }
            set {
                blockArea = value;
                squares = GetSquares(location.X, location.Y, 0);
            }
        }
        /// <summary>
        /// Collision detection, tests if next position is not occupied.
        /// </summary>
        /// <param name="dest">the destination position</param>
        /// <returns>true if the block can move to the destination</returns>
        public bool CanMove(Square[] dest) {
            for (int i = 0; i < dest.Length; i++) {
                if (dest[i] == null || dest[i].Visible)
                    return false;
            }
            return true;
        }
        /// <summary>
        /// Moves the block left.
        /// </summary>
        public void Left() {
            Square[] dest = GetSquares(location.X - 1, location.Y, patternId);
            Hide();
            if (CanMove(dest)) {
                location.X -= 1;
                squares = dest;
            }
            Show();
        }

        /// <summary>
        /// Moves the block right.
        /// </summary>
        public void Right() {
            Square[] dest = GetSquares(location.X + 1, location.Y, patternId);
            Hide();
            if (CanMove(dest)) {
                location.X += 1;
                squares = dest;
            }
            Show();
        }
        /// <summary>
        /// Moves the block down.
        /// </summary>
        public void Down() {
            Square[] dest = GetSquares(location.X, location.Y + 1, patternId);
            Hide();
            if (CanMove(dest)) {
                location.Y += 1;
                squares = dest;
            }
            Show();
        }

        /// <summary>
        /// Tests if the block can move down, used for detect end of the movements.
        /// </summary>
        public bool CanMoveDown() {
            Hide();
            Square[] dest = GetSquares(location.X, location.Y + 1, patternId);
            bool ret = CanMove(dest);
            Show();
            return ret;
        }

        public bool CanShow() {
            return CanMove(squares);
        }

        /// <summary>
        /// Rotate the block
        /// </summary>
        public void Rotate() {
            if (patterns.Count == 0)
                return;
            int pid = (patternId + 1) % patterns.Count;
            Hide();
            Square[] dest = GetSquares(location.X, location.Y, pid);
            if (CanMove(dest)) {
                patternId = pid;
                squares = dest;
            }
            Show();
        }

        /// <summary>
        /// Shows the block.
        /// </summary>
        public void Show() {
            foreach (Square sq in squares)
                if (sq != null)
                    sq.Show(foreColor, centerColor);
        }

        /// <summary>
        /// Hides the block.
        /// </summary>
        public void Hide() {
            foreach (Square sq in squares)
                if (sq != null)
                    sq.Hide();
        }

        /// <summary>
        /// Get the lowest square's row index.
        /// </summary>
        /// <returns>the lowest row index</returns>
        public int BottomIndex() {
            int result = squares[0].Location.Y;
            for (int i = 1; i < 4; i++)
                if (squares[i].Location.Y > result)
                    result = squares[i].Location.Y;
            return result;
        }



        /// <summary>
        /// Init all the patterns of the block, needs to be overriden by child
        /// classes.
        /// </summary>
        protected abstract void InitPatterns();

        /// <summary>
        /// Get the squares from the current pattern.
        /// </summary>
        protected virtual Square[] GetSquares(int x, int y, int patternId) {
            if (patterns.Count == 0)
                return null;
            Square[] result = new Square[4];
            for (int i = 0; i < 4; i++)
                result[i] = blockArea.GetSquare(x + patterns[patternId][i].X, y + patterns[patternId][i].Y);
            return result;
        }

        /// <summary>
        /// Gets the patterns which stores all the available pattern.
        /// </summary>
        protected List<Point[]> Patterns {
            get { return patterns; }
        }

        /// <summary>
        /// the current pattern id.
        /// </summary>
        private int patternId = 0;
        /// <summary>
        /// the game area to display the block.
        /// </summary>
        private BlockArea blockArea;
        /// <summary>
        /// The forecolor of the block.
        /// </summary>
        private Color foreColor;
        /// <summary>
        /// the center color of the block.
        /// </summary>
        private Color centerColor;
        /// <summary>
        /// Stores the current squares retrieved from the game area.
        /// </summary>
        private Square[] squares = null;
        /// <summary>
        /// the current location of the block.
        /// </summary>
        private Point location;
        /// <summary>
        /// Stores all the patterns of a block.
        /// </summary>
        private List<Point[]> patterns;
    }
    /// <summary>
    /// T block. Top left location is the top left square of the block.
    /// </summary>
    public class TBlock : Block {
        public TBlock(BlockArea blockArea, int x, int y)
            : base(blockArea, x, y) {
            ForeColor = Color.Purple;
            CenterColor = Color.Azure;
        }

        protected override void InitPatterns() {
            Patterns.Add(new Point[] { new Point(0, 1), new Point(1, 1), new Point(2, 1), new Point(1, 2) });
            Patterns.Add(new Point[] { new Point(1, 0), new Point(0, 1), new Point(1, 1), new Point(1, 2) });
            Patterns.Add(new Point[] { new Point(1, 0), new Point(0, 1), new Point(1, 1), new Point(2, 1) });
            Patterns.Add(new Point[] { new Point(1, 0), new Point(1, 1), new Point(2, 1), new Point(1, 2) });
        }
    }
    /// <summary>
    /// I Block. Top left location is one square to the left of the top square.
    /// </summary>
    public class IBlock : Block {
        public IBlock(BlockArea blockArea, int x, int y)
            : base(blockArea, x, y) {
            ForeColor = Color.Red;
            CenterColor = Color.Azure;
        }
        protected override void InitPatterns() {
            Patterns.Add(new Point[] { new Point(0, 1), new Point(1, 1), new Point(2, 1), new Point(3, 1) });
            Patterns.Add(new Point[] { new Point(1, 0), new Point(1, 1), new Point(1, 2), new Point(1, 3) });
        }
    }
    /// <summary>
    /// J Block. Top left location is one square to the left of the top square.
    /// </summary>
    public class JBlock : Block {
        public JBlock(BlockArea blockArea, int x, int y)
            : base(blockArea, x, y) {
            ForeColor = Color.Black;
            CenterColor = Color.Azure;
        }

        protected override void InitPatterns() {
            Patterns.Add(new Point[] { new Point(1, 0), new Point(1, 1), new Point(1, 2), new Point(0, 2) });
            Patterns.Add(new Point[] { new Point(0, 0), new Point(0, 1), new Point(1, 1), new Point(2, 1) });
            Patterns.Add(new Point[] { new Point(1, 0), new Point(2, 0), new Point(1, 1), new Point(1, 2) });
            Patterns.Add(new Point[] { new Point(0, 1), new Point(1, 1), new Point(2, 1), new Point(2, 2) });
        }

    }
    /// <summary>
    /// L Block. Top left location is one square to the left of the top square.
    /// </summary>
    public class LBlock : Block {
        public LBlock(BlockArea blockArea, int x, int y)
            : base(blockArea, x, y) {
            ForeColor = Color.Magenta;
            CenterColor = Color.Azure;
        }
        protected override void InitPatterns() {
            Patterns.Add(new Point[] { new Point(1, 0), new Point(1, 1), new Point(1, 2), new Point(2, 2) });
            Patterns.Add(new Point[] { new Point(0, 1), new Point(1, 1), new Point(2, 1), new Point(0, 2) });
            Patterns.Add(new Point[] { new Point(0, 0), new Point(1, 0), new Point(1, 1), new Point(1, 2) });
            Patterns.Add(new Point[] { new Point(2, 0), new Point(0, 1), new Point(1, 1), new Point(2, 1) });
        }
    }
    /// <summary>
    /// S Block. Top left location is one square to the left of the top two squares.
    /// </summary>
    public class SBlock : Block {
        public SBlock(BlockArea blockArea, int x, int y)
            : base(blockArea, x, y) {
            ForeColor = Color.Green;
            CenterColor = Color.Azure;
        }
        protected override void InitPatterns() {
            Patterns.Add(new Point[] { new Point(1, 0), new Point(2, 0), new Point(0, 1), new Point(1, 1) });
            Patterns.Add(new Point[] { new Point(1, 0), new Point(1, 1), new Point(2, 1), new Point(2, 2) });
        }
    }
    /// <summary>
    /// Z Block. Top left location is the top left square.
    /// </summary>
    public class ZBlock : Block {
        public ZBlock(BlockArea blockArea, int x, int y)
            : base(blockArea, x, y) {
            ForeColor = Color.Orange;
            CenterColor = Color.Azure;
        }
        protected override void InitPatterns() {
            Patterns.Add(new Point[] { new Point(0, 0), new Point(1, 0), new Point(1, 1), new Point(2, 1) });
            Patterns.Add(new Point[] { new Point(2, 0), new Point(1, 1), new Point(2, 1), new Point(1, 2) });
        }
    }
    /// <summary>
    /// O Block. Top left location is the top left square.
    /// </summary>
    public class OBlock : Block {
        public OBlock(BlockArea blockArea, int x, int y)
            : base(blockArea, x, y) {
            ForeColor = Color.Blue;
            CenterColor = Color.Azure;
        }
        protected override void InitPatterns() {
            Patterns.Add(new Point[] { new Point(0, 0), new Point(1, 0), new Point(0, 1), new Point(1, 1) });
        }
    }
}

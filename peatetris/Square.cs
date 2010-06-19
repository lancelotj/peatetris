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
    /// Represens a square in a block.
    /// </summary>
    public class Square {
        /// <summary>
        /// The event fired when the square needs to be shown.
        /// </summary>
        public event EventHandler ShowEvent;
        /// <summary>
        /// The event fired when the square needs to be hide.
        /// </summary>
        public event EventHandler HideEvent;

        /// <summary>
        /// Creates a Square object
        /// <summary>
        /// <param name="x">the horizontal index of the square in the game
        /// area</param>
        /// <param name="y">the vertical index of the square in the game
        /// area</param>
        public Square(int x, int y) {
            location.X = x;
            location.Y = y;
        }

        /// <summary>
        /// Shows the square.
        /// </summary>
        public void Show(Color foreColor, Color centerColor) {
            this.foreColor = foreColor;
            this.centerColor = centerColor;

            visible = true;
            if (ShowEvent != null)
                ShowEvent(this, null);
        }

        /// <summary>
        /// Hides the square.
        /// </summary>
        public void Hide() {
            visible = false;
            if (HideEvent != null)
                HideEvent(this, null);
        }

        public void ClearEvents() {
            ShowEvent = null;
            HideEvent = null;
        }
        /// <summary>
        /// The location of the square.
        /// <summary>
        public Point Location {
            get { return location; }
            set { location = value; }
        }

        /// <summary>
        /// Gets the visibility of the square.
        /// </summary>
        public bool Visible {
            get { return visible; }
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
        public Color BackColor {
            get { return centerColor; }
            set { centerColor = value; }
        }
        /// <summary>
        /// Stores the location of the square.
        /// </summary>
        private Point location;
        /// <summary>
        /// Stores the visibility of the square.
        /// </summary>
        private bool visible;
        /// <summary>
        /// Stores the forecolor of the square.
        /// </summary>
        private Color foreColor;
        /// <summary>
        /// Stores the center color of the square.
        /// </summary>
        private Color centerColor;
    }
}

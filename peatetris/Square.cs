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

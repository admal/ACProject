using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACProject.Domain.Models
{
    /// <summary>
    /// Interface for a block represented in a well, it extends basic IBlock by adding plane interaction
    /// </summary>
    public interface IBoardBlock : IBlock
    {
        /// <summary>
        /// Position of a tile on a board
        /// </summary>
        Point Position { get; set; }
        /// <summary>
        /// Rotates the tile counter clockwise
        /// </summary>
        /// <returns>Rotated tile</returns>
        BoardBlock RotateClockwise();
        /// <summary>
        /// Rotates the tile counter clockwise
        /// </summary>
        /// <returns>Rotated tile</returns>
        BoardBlock RotateCounterClockwise();
        /// <summary>
        /// Draws a block on it's position in a well
        /// </summary>
        /// <param name="graphics"> context for drawing </param>
        /// <param name="brush"> brush for drawing </param>
        /// <param name="cellSize"> Size of each square cell of a tile in pixels </param>
        void Draw(Graphics graphics, Brush brush, int cellSize);
    }
}

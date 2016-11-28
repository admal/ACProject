using System.Collections.Generic;
using System.Drawing;

namespace ACProject.Domain.Models
{
    /// <summary>
    /// Inteface for a basic block
    /// </summary>
    public interface IBlock
    {
        /// <summary>
        /// Grid of a tile which is represented by the block
        /// </summary>
        int[,] Grid { get; set; }
        /// <summary>
        /// Color of a tile
        /// </summary>
        Color Color { get; set; }
        /// <summary>
        /// Number of this type of tiles
        /// </summary>
        int Count { get; set; }
        /// <summary>
        /// Draws a tile in a given context
        /// </summary>
        /// <param name="graphics">Context for drawing</param>
        /// <param name="cellSize">Size of each square cell of a tile in pixels</param>
        void Draw(Graphics graphics, int cellSize);
        /// <summary>
        /// Draws a tile in a given context in container
        /// </summary>
        /// <param name="graphics">Context for drawing</param>
        /// <param name="cellSize">Size of each square cell of a tile in pixels</param>
        /// <param name="containerHeight">Height of a container we draw in pixels</param>
        void Draw(Graphics graphics, int cellSize, int containerHeight);
    }
}
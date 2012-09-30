namespace ClassLibrary1
{
    using NUnit.Framework;

    /// <summary>
    /// Rules:
    /// Any live cell with fewer than two live neighbours dies, as if caused by underpopulation.
    /// Any live cell with more than three live neighbours dies, as if by overcrowding.
    /// Any live cell with two or three live neighbours lives on to the next generation.
    /// Any dead cell with exactly three live neighbours becomes a live cell.
    /// </summary>
    [TestFixture]
    public class GoLApproach1
    {
        [Test]
        public void CellDiesInUnderpopulation()
        {
            var board = new[]
                {
                    new[] { 0, 0, 1 },
                    new[] { 0, 1, 0 },
                    new[] { 0, 0, 0 }
                };

            var expectedBoard = new[]
                {
                    new[] { 0, 0, 0 },
                    new[] { 0, 0, 0 },
                    new[] { 0, 0, 0 }
                };

            var newBoard = Tick(board);

            Assert.AreNotEqual(board, newBoard);
            Assert.That(newBoard, Is.EquivalentTo(expectedBoard));
        }

        [Test]
        public void CellDiesInOverpopulation()
        {
            var board = new[]
                {
                    new[] { 1, 1, 1 },
                    new[] { 1, 1, 1 },
                    new[] { 1, 1, 1 }
                };

            var expectedBoard = new[]
                {
                    new[] { 1, 0, 1 },
                    new[] { 0, 0, 0 },
                    new[] { 1, 0, 1 }
                };

            var newBoard = this.Tick(board);

            Assert.That(newBoard, Is.EquivalentTo(expectedBoard));
        }

        [Test]
        public void CellBornsWithThreeNeighbours()
        {
            var board = new[]
                {
                    new[] { 0, 1, 0 },
                    new[] { 1, 1, 1 },
                    new[] { 0, 1, 0 }
                };

            var expectedBoard = new[]
                {
                    new[] { 1, 1, 1 },
                    new[] { 1, 0, 1 },
                    new[] { 1, 1, 1 }
                };

            var newBoard = this.Tick(board);

            Assert.That(newBoard, Is.EquivalentTo(expectedBoard));
        }

        private int[][] Tick(int[][] board)
        {
            var newBoard = new int[board.Length][];

            for (int x = 0; x < board.Length; x++)
            {
                newBoard[x] = new int[board[0].Length];

                for (int y = 0; y < board[x].Length; y++)
                {
                    // calculate neighbours
                    int neighbours = 0;

                    // north
                    if (x > 0 && y > 0) neighbours += board[x - 1][y - 1];
                    if (y > 0) neighbours += board[x][y - 1];
                    if (x < board.Length-1 && y > 0) neighbours += board[x + 1][y - 1];
                    
                    // in-line
                    if (x > 0) neighbours += board[x - 1][y];
                    if (x < board.Length-1) neighbours += board[x + 1][y];

                    // south
                    if (x > 0 && y < board[x].Length-1) neighbours += board[x - 1][y + 1];
                    if (y < board[x].Length-1) neighbours += board[x][y + 1];
                    if (x < board.Length-1 && y < board[x].Length-1) neighbours += board[x + 1][y + 1];

                    if (neighbours < 2 || neighbours > 3)
                    {
                        // underpopulation or overpopulation
                        newBoard[x][y] = 0;
                    }
                    else if (neighbours == 3)
                    {
                        // new born
                        newBoard[x][y] = 1;
                    }
                    else
                    {
                        // stays the same
                        newBoard[x][y] = board[x][y];
                    }
                }
            }

            return newBoard;
        }
    }
}

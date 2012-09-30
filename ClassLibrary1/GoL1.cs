namespace ClassLibrary1
{
    using NUnit.Framework;
    using System.Collections.Generic;
    using System.Linq;

    [TestFixture]
    public class GoL1
    {
        [TestCase(0)]
        [TestCase(1)]
        public void LiveCellWithLessThanTwoNeighbourShouldDie(int noOfNeighbours)
        {
            bool nextState = Cell.IsAlive(noOfNeighbours);

            Assert.That(nextState, Is.False);
        }

        [TestCase(2)]
        [TestCase(3)]
        public void LiveCellWithTwoOrThreeNeighbourShouldLive(int noOfNeighbours)
        {
            bool nextState = Cell.IsAlive(noOfNeighbours);

            Assert.That(nextState, Is.True);
        }

        [TestCase(4)]
        [TestCase(5)]
        [TestCase(6)]
        [TestCase(7)]
        [TestCase(8)]
        public void LiveCellWithMoreThanThreeNighboursShouldDie(int noOfNeighbours)
        {
            bool nextState = Cell.IsAlive(noOfNeighbours);

            Assert.That(nextState, Is.False);
        }

        [Test]
        public void DeadCellWithThreeNeighboursShouldBecomeLive()
        {
            bool nextState = Cell.IsAlive(3);

            Assert.That(nextState, Is.True);
        }

        [Test]
        public void EmptyBoardShouldReturnEmptyNextTick()
        {
            Position[] newLiveCellsPositions = Board.Tick(null);
            Assert.That(newLiveCellsPositions, Is.Not.Null);

        }

        [Test]
        public void OneLiveCellOnBoardDiesInNextTick()
        {
            Position[] positions = Board.Tick(new[] { new Position(5, 5) });

            Assert.AreEqual(0, positions.Length);
        }

        [Test]
        public void ThreeAdjacentCellsOnlyMiddleCellStaysAlive()
        {
            Position[] positions = Board.Tick(new[]
                {
                    new Position(4, 5),
                    new Position(5, 5), 
                    new Position(6, 5)
                });

            CollectionAssert.DoesNotContain(positions, new Position(4, 5));
            CollectionAssert.DoesNotContain(positions, new Position(6, 5));
            CollectionAssert.Contains(positions, new Position(5, 5));
        }

        [Test]
        public void ThreeAdjacentHorizontalCellsSpawnTwoNewCells()
        {
            Position[] positions = Board.Tick(new[]
                {
                    new Position(4, 5),
                    new Position(5, 5), 
                    new Position(6, 5)
                });

            CollectionAssert.Contains(positions, new Position(5, 5));
            CollectionAssert.Contains(positions, new Position(5, 4));
            CollectionAssert.Contains(positions, new Position(5, 6));
        }
    }

    public class Position
    {
        private readonly int x;
        private readonly int y;

        public Position(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        public IEnumerable<Position> GetNeighbourPositions()
        {
            return new[]
                {
                    new Position(x - 1, y - 1),
                    new Position(x, y - 1),
                    new Position(x + 1, y - 1),
                    new Position(x - 1, y),
                    new Position(x + 1, y),
                    new Position(x - 1, y + 1),
                    new Position(x, y + 1),
                    new Position(x + 1, y + 1)
                };
        }

        public override bool Equals(object obj)
        {
            var other = obj as Position;

            return other != null && (this.x == other.x && this.y == other.y);
        }
    }

    public static class Board
    {
        public static Position[] Tick(Position[] positions)
        {
            if (positions == null || positions.Length == 0)
                return new Position[0];

            // check the dead neighbours of positions if it needs to spawn new cells
            var newLivePositions = new List<Position>();
            foreach (var position in positions)
            {
                var deadNeighbours = position.GetNeighbourPositions().Where(p => !positions.Contains(p));

                // spawn a new cell if it has 3 neighbours
                newLivePositions.AddRange(deadNeighbours.Where(deadNeighbour => deadNeighbour.GetNeighbourPositions().Count(positions.Contains) == 3));
            }

            // check all the live cells, deci
            return (from position in positions
                    let neighbourCount = position.GetNeighbourPositions().Count(positions.Contains)
                    where Cell.IsAlive(neighbourCount)
                    select position)
                    .Concat(newLivePositions)
                    .ToArray();
        }
    }

    public static class Cell
    {
        public static bool IsAlive(int noOfNeighbours)
        {
            return noOfNeighbours > 1 && noOfNeighbours < 4;
        }
    }
}

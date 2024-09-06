using PrimalTestDotNet.Entities;

namespace PrimalTestDotNet
{
    class DistanceData
    {
        public int ShortestDistance { get; set; } = int.MaxValue;
        public IntVector2? PreviousVertex { get; set; }
    }

    enum AIState
    {
        LOOKING_FOR_TREASURE,
        LOOKING_FOR_SWORD,
        LOOKING_FOR_POTION,
        LEAVING_LEVEL,
        UNWINNABLE
    }

    internal class AI(Level.Level level, Hero hero, List<IGameObject> gameObjects)
    {
        private AIState _state = AIState.LOOKING_FOR_TREASURE;

        public void Update()
        {
            var distanceData = CalculateDistanceData([]);
            var allMonsters = gameObjects.Where(o => o is Monster);

            if (!hero.HasTreasure)
            {
                // First try direct path to the treasure
                var treasure = gameObjects.First(o => o is Treasure);
                var pathToTreasure = GetPath(treasure.Position, distanceData);
                // check if path has monsters
                var monstersOnPath = gameObjects.Where(o => o is Monster)
                    .Where(m => pathToTreasure.Contains(m.Position)).ToArray();

                if (hero.Health < 2 && gameObjects.Any(o => o is Potion) && monstersOnPath.Length > 0)
                {
                    // Look for potion
                    var recalculatedDistanceData = CalculateDistanceData(allMonsters);
                    var potion = gameObjects.Where(o => o is Potion).First();
                    var pathToPotion = GetPath(potion.Position, recalculatedDistanceData);

                    var node = pathToPotion.Last();
                    var direction = node - hero.Position;
                    hero.Move(direction);
                }
                else if (monstersOnPath.Length == 0 || hero.HasSword)
                {
                    // Look for treasure
                    var node = pathToTreasure.Last();
                    var direction = node - hero.Position;
                    if (direction.Length > 1)
                        level.OnLeave(hero); // No path to the treasure // TODO: unwinnable state
                    else
                        hero.Move(direction);
                }
                else if (gameObjects.Any(o => o is Sword))
                {
                    // Look for sword
                    // In that case look for a sword
                    var sword = gameObjects.Where(o => o is Sword).First();
                    // Consider monsters as walls in that case
                    var recalculatedDistanceData = CalculateDistanceData(allMonsters);
                    var pathToSword = GetPath(sword.Position, recalculatedDistanceData);

                    var node = pathToSword.Last();
                    var direction = node - hero.Position;
                    hero.Move(direction);
                }
                else
                {
                    // TODO: forfeit when unwinnable
                    // TODO: a => try to exit
                    // TODO: b => kill hero when everything else fails
                }

            }
            else
            {
                // Try to exit the level
                var exit = gameObjects.First(o => o is Exit);
                var pathToExit = GetPath(exit.Position, distanceData);
                var node = pathToExit.Last();
                var direction = node - hero.Position;
                if (direction.Length > 1)
                {
                    // A hero only can move one cell. If the path has longer movement vector it must be the end-goal without any real paths to it
                    hero.Kill();
                }

                hero.Move(direction);
            }
        }

        private Dictionary<IntVector2, DistanceData> CalculateDistanceData(IEnumerable<IGameObject> additionalWalls)
        {
            var distanceData = new Dictionary<IntVector2, DistanceData>();

            var visited = new List<IntVector2>();
            var unvisited = new List<IntVector2>();
            for (int x = 0; x < level.CollisionData.GetLength(1); x++)
            {
                for (int y = 0; y < level.CollisionData.GetLength(0); y++)
                {
                    if (level.CollisionData[y, x] == false && !additionalWalls.Any(o => o.Position == new IntVector2(x, y)))
                        unvisited.Add(new IntVector2(x, y));
                }
            }

            unvisited.ForEach(x =>
            {
                distanceData[x] = new DistanceData
                {
                    PreviousVertex = null,
                    ShortestDistance = x == hero.Position ? 0 : int.MaxValue // TODO: ne csak a hero hanem bármelyik 2 pont között lehessen
                };
            });


            while (unvisited.Count > 0 
                && unvisited.Any(u => distanceData[u].ShortestDistance < int.MaxValue)) // disregard isolated nodes
            {
                var node = unvisited.OrderBy(n => distanceData[n].ShortestDistance).First();
                unvisited.Remove(node);

                var neighbours = GetNeighbours(node, unvisited);
                foreach (var neighbour in neighbours)
                {
                    var distance = distanceData[node].ShortestDistance + 1;
                    if (distance < distanceData[neighbour].ShortestDistance)
                    {
                        distanceData[neighbour].ShortestDistance = distance;
                        distanceData[neighbour].PreviousVertex = node;
                    }
                }

                visited.Add(node);
            }

            return distanceData;
        }

        private static IEnumerable<IntVector2> GetNeighbours(IntVector2 node, List<IntVector2> unvisited)
        {
            var possibleMoves = new IntVector2[]
            {
                new(0, 1),  // Up
                new(0, -1), // Down
                new(1, 0),  // Right
                new(-1, 0)  // Left
            };

            return possibleMoves.Select(move => new IntVector2(node.X + move.X, node.Y + move.Y)).Where(unvisited.Contains);
        }

        private List<IntVector2> GetPath(IntVector2 to, Dictionary<IntVector2, DistanceData> distanceData)
        {
            var path = new List<IntVector2>
            {
                to
            };

            IntVector2? node = to;
            while (node != null)
            {
                var prev = distanceData[node.Value].PreviousVertex;
                if (prev != null && prev.Value != hero.Position)
                    path.Add(prev.Value);
                node = prev;
            }

            return path;
        }
    }
}
using PrimalTestDotNet.Entities;

namespace PrimalTestDotNet.AI.States;

public class DistanceData
{
    public int ShortestDistance { get; set; } = int.MaxValue;
    public IntVector2? PreviousVertex { get; set; }
}

public abstract class StateBase(Level.Level level)
{
    protected Level.Level _level = level;

    protected List<IntVector2> GetPath(IntVector2 from, IntVector2 to, Dictionary<IntVector2, DistanceData> distanceData)
    {
        var path = new List<IntVector2>
        {
            to
        };

        IntVector2? node = to;
        while (node != null)
        {
            var prev = distanceData[node.Value].PreviousVertex;
            if (prev != null && prev.Value != from)
                path.Add(prev.Value);
            node = prev;
        }

        return path;
    }

    protected Dictionary<IntVector2, DistanceData> CalculateDistanceData(IntVector2 from, IEnumerable<IGameObject> additionalWalls)
    {
        var distanceData = new Dictionary<IntVector2, DistanceData>();

        var visited = new List<IntVector2>();
        var unvisited = new List<IntVector2>();
        for (int x = 0; x < _level.CollisionData.GetLength(1); x++)
        {
            for (int y = 0; y < _level.CollisionData.GetLength(0); y++)
            {
                if (_level.CollisionData[y, x] == false && !additionalWalls.Any(o => o.Position == new IntVector2(x, y)))
                    unvisited.Add(new IntVector2(x, y));
            }
        }

        unvisited.ForEach(x =>
        {
            distanceData[x] = new DistanceData
            {
                PreviousVertex = null,
                ShortestDistance = x == from ? 0 : int.MaxValue
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
}

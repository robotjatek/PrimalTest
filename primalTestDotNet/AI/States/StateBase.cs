using PrimalTestDotNet.Entities;

namespace PrimalTestDotNet.AI.States;

public class DistanceData
{
    public int ShortestDistance { get; set; } = int.MaxValue;
    public IntVector2? PreviousVertex { get; set; }
}

/// <summary>
/// Base class for AI states. Contains a crude implementation of Dijkstra's algorithm for path calculations
/// </summary>
public abstract class StateBase(Level.Level level)
{
    protected Level.Level _level = level;

    /// <summary>
    /// Returns a list of coordinates of the shortest path possible between two given coordinates.
    /// </summary>
    /// <param name="from">The start coordinate</param>
    /// <param name="to">The final destination</param>
    /// <param name="distanceData">A map of values calculated by <see cref="CalculateDistanceData(IntVector2, IEnumerable{IGameObject})"/></param>
    /// <returns></returns>
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

    /// <summary>
    /// Calculates shortest paths from all nodes to the start node for the <see cref="Level.Level"/> data with Dijkstra's algorithm
    /// </summary>
    /// <param name="start"></param>
    /// <param name="additionalWalls">Additional walls other than the one provided by <see cref="Level.Level"/></param>
    /// <returns></returns>
    protected Dictionary<IntVector2, DistanceData> CalculateDistanceData(IntVector2 start, IEnumerable<IGameObject> additionalWalls)
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
                ShortestDistance = x == start ? 0 : int.MaxValue
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

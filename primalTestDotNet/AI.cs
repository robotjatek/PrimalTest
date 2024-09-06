﻿using PrimalTestDotNet.Entities;

namespace PrimalTestDotNet
{
    public class DistanceData
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

    public interface IState
    {
        void Update(Hero hero, Dictionary<IntVector2, DistanceData> distanceData, IEnumerable<IGameObject> gameObjects);
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

    public class LookingForTreasure(AI context, Level.Level level) : StateBase(level), IState
    {
        public void Update(Hero hero, Dictionary<IntVector2, DistanceData> distanceData, IEnumerable<IGameObject> gameObjects)
        {
            if (hero.HasTreasure)
            {
                context.ChangeState(context.LeaveLevelState);
                return;
            }

            var treasure = gameObjects.FirstOrDefault(o => o is Treasure);
            if (treasure == null)
            {
                context.ChangeState(context.UnwinnableState);
                return;
            }
            var pathToTreasure = GetPath(hero.Position, treasure.Position, distanceData);
            var monstersOnPath = gameObjects.Where(o => o is Monster) // filter to monsters only
                .Where(m => pathToTreasure.Contains(m.Position)) // keep monsters that are on the path
                .ToArray();

            if (monstersOnPath.Length > 0 && hero.Health < 2)
            {
                context.ChangeState(context.LookingForPotionState);
                return;
            }
            else if (monstersOnPath.Length > 0 && !hero.HasSword)
            {
                context.ChangeState(context.LookingForSwordState);
                return;
            }
            else if (hero.HasTreasure)
            {
                context.ChangeState(context.LeaveLevelState);
                return;
            }
            else
            {
                // Go for the treasure
                var node = pathToTreasure.Last();
                var direction = node - hero.Position;
                if (direction.Length > 1)
                {
                    context.ChangeState(context.UnwinnableState);
                    return;
                }
                else
                {
                    hero.Move(direction);
                    return;
                }
            }
        }
    }

    public class Unwinnable(Level.Level level) : StateBase(level), IState
    {
        public void Update(Hero hero, Dictionary<IntVector2, DistanceData> distanceData, IEnumerable<IGameObject> gameObjects)
        {
            // forfeit when unwinnable
            // try to exit
            var exit = gameObjects.FirstOrDefault(o => o is Exit);
            var pathToExit = GetPath(hero.Position, exit.Position, distanceData);
            var node = pathToExit.Last();
            var direction = node - hero.Position;

            if (direction.Length > 1)
            {
                // If leaving without the treasure also fails => game over 
                level.OnStuck();
                return;
            }

            hero.Move(direction);
        }
    }

    public class LookingForPotion(AI context, Level.Level level) : StateBase(level), IState
    {
        public void Update(Hero hero, Dictionary<IntVector2, DistanceData> distanceData, IEnumerable<IGameObject> gameObjects)
        {
            if (hero.Health > 1)
            {
                context.ChangeState(context.LookingForTreasureState);
                return;
            }

            // Look for potion
            var allMonsters = gameObjects.Where(o => o is Monster);
            var recalculatedDistanceData = CalculateDistanceData(hero.Position, allMonsters);
            var potion = gameObjects.Where(o => o is Potion).FirstOrDefault();
            if (potion == null)
            {
                context.ChangeState(context.UnwinnableState);
                return;
            }
            var pathToPotion = GetPath(hero.Position, potion.Position, recalculatedDistanceData);

            var node = pathToPotion.Last();
            var direction = node - hero.Position;
            hero.Move(direction);
        }
    }

    public class LookingForSword(AI context, Level.Level level) : StateBase(level), IState
    {
        public void Update(Hero hero, Dictionary<IntVector2, DistanceData> distanceData, IEnumerable<IGameObject> gameObjects)
        {
            if (hero.HasSword)
            {
                context.ChangeState(context.LookingForTreasureState);
                return;
            }

            // Look for a sword without monsters on the path
            var allMonsters = gameObjects.Where(o => o is Monster);
            var sword = gameObjects.Where(o => o is Sword).FirstOrDefault();
            if (sword == null)
            {
                context.ChangeState(context.UnwinnableState);
                return;
            }
            // Consider monsters as walls in that case
            var recalculatedDistanceData = CalculateDistanceData(hero.Position, allMonsters);
            var pathToSword = GetPath(hero.Position, sword.Position, recalculatedDistanceData);

            var node = pathToSword.Last();
            var direction = node - hero.Position;
            hero.Move(direction);
        }
    }

    public class LeavingLevel(AI context, Level.Level level) : StateBase(level), IState
    {
        public void Update(Hero hero, Dictionary<IntVector2, DistanceData> distanceData, IEnumerable<IGameObject> gameObjects)
        {
            // Try to exit the level
            var exit = gameObjects.First(o => o is Exit);
            var pathToExit = GetPath(hero.Position, exit.Position, distanceData);
            var node = pathToExit.Last();
            var direction = node - hero.Position;
            if (direction.Length > 1)
            {
                // A hero only can move one cell. If the path has longer movement vector it must be the end-goal without any real paths to it
                context.ChangeState(context.UnwinnableState);
                return;
            }

            hero.Move(direction);
        }
    }

    public class AI
    {
        private IState _s;
        private Level.Level level;
        private Hero hero;
        private List<IGameObject> gameObjects;

        public IState LeaveLevelState => new LeavingLevel(this, level);
        public IState UnwinnableState => new Unwinnable(level);
        public IState LookingForPotionState => new LookingForPotion(this, level);
        public IState LookingForSwordState => new LookingForSword(this, level);
        public IState LookingForTreasureState => new LookingForTreasure(this, level);

        public AI(Level.Level level, Hero hero, List<IGameObject> gameObjects)
        {
            _s = LookingForTreasureState;

            this.level = level;
            this.hero = hero;
            this.gameObjects = gameObjects;
        }
        private AIState _state = AIState.LOOKING_FOR_TREASURE;

        public void ChangeState(IState state)
        {
            //TODO: exit old state
            _s = state;
            //TODO: enter new state
        }

        public void Update()
        {
            var distanceData = CalculateDistanceData([]);
            var allMonsters = gameObjects.Where(o => o is Monster);

             _s.Update(hero, distanceData, gameObjects);

            //if (_state == AIState.LOOKING_FOR_TREASURE)
            //{
            //    if (hero.HasTreasure)
            //    {
            //        _state = AIState.LEAVING_LEVEL;
            //        return;
            //    }

            //    var treasure = gameObjects.FirstOrDefault(o => o is Treasure);
            //    if (treasure == null)
            //    {
            //        _state = AIState.UNWINNABLE;
            //        return;
            //    }
            //    var pathToTreasure = GetPath(hero.Position, treasure.Position, distanceData);
            //    var monstersOnPath = gameObjects.Where(o => o is Monster) // filter to monsters only
            //        .Where(m => pathToTreasure.Contains(m.Position)) // keep monsters that are on the path
            //        .ToArray();

            //    if (monstersOnPath.Length > 0 && hero.Health < 2)
            //    {
            //        _state = AIState.LOOKING_FOR_POTION;
            //        return;
            //    }
            //    else if (monstersOnPath.Length > 0 && !hero.HasSword)
            //    {
            //        _state = AIState.LOOKING_FOR_SWORD;
            //        return;
            //    }
            //    else if (hero.HasTreasure)
            //    {
            //        _state = AIState.LEAVING_LEVEL;
            //        return;
            //    }
            //    else
            //    {
            //        // Go for the treasure
            //        var node = pathToTreasure.Last();
            //        var direction = node - hero.Position;
            //        if (direction.Length > 1)
            //            _state = AIState.UNWINNABLE;
            //        // level.OnLeave(hero); // No path to the treasure // TODO: unwinnable state
            //        else
            //        {
            //            hero.Move(direction);
            //            return;
            //        }
            //    }

            //}
            //else if (_state == AIState.LOOKING_FOR_SWORD)
            //{
            //    if (hero.HasSword)
            //    {
            //        _state = AIState.LOOKING_FOR_TREASURE;
            //        return;
            //    }

            //    // Look for a sword without monsters on the path
            //    var sword = gameObjects.Where(o => o is Sword).FirstOrDefault();
            //    if (sword == null)
            //    {
            //        _state = AIState.UNWINNABLE;
            //        return;
            //    }
            //    // Consider monsters as walls in that case
            //    var recalculatedDistanceData = CalculateDistanceData(allMonsters);
            //    var pathToSword = GetPath(hero.Position, sword.Position, recalculatedDistanceData);

            //    var node = pathToSword.Last();
            //    var direction = node - hero.Position;
            //    hero.Move(direction);
            //}
            //else if (_state == AIState.LOOKING_FOR_POTION)
            //{
            //    if (hero.Health > 1)
            //    {
            //        _state = AIState.LOOKING_FOR_TREASURE;
            //        return;
            //    }

            //    // Look for potion
            //    var recalculatedDistanceData = CalculateDistanceData(allMonsters);
            //    var potion = gameObjects.Where(o => o is Potion).FirstOrDefault();
            //    if (potion == null)
            //    {
            //        _state = AIState.UNWINNABLE;
            //        return;
            //    }
            //    var pathToPotion = GetPath(hero.Position, potion.Position, recalculatedDistanceData);

            //    var node = pathToPotion.Last();
            //    var direction = node - hero.Position;
            //    hero.Move(direction);
            //}
            //else if (_state == AIState.LEAVING_LEVEL)
            //{
            //    // Try to exit the level
            //    var exit = gameObjects.First(o => o is Exit);
            //    var pathToExit = GetPath(hero.Position, exit.Position, distanceData);
            //    var node = pathToExit.Last();
            //    var direction = node - hero.Position;
            //    if (direction.Length > 1)
            //    {
            //        // A hero only can move one cell. If the path has longer movement vector it must be the end-goal without any real paths to it
            //        _state = AIState.UNWINNABLE;
            //        return;
            //        //hero.Kill(); // TODO unwinnable state
            //    }

            //    hero.Move(direction);
            //}
            //else if (_state == AIState.UNWINNABLE)
            //{
            //    // forfeit when unwinnable
            //    // try to exit
            //    var exit = gameObjects.FirstOrDefault(o => o is Exit);
            //    var pathToExit = GetPath(hero.Position, exit.Position, distanceData);
            //    var node = pathToExit.Last();
            //    var direction = node - hero.Position;

            //    if (direction.Length > 1)
            //    {
            //        // Kill hero when everything else fails
            //        hero.Kill();
            //        return;
            //    }

            //    hero.Move(direction);
            //}
            //else
            //{
            //    // error
            //}



            //if (!hero.HasTreasure)
            //{
            //    // First try direct path to the treasure
            //    var treasure = gameObjects.First(o => o is Treasure);
            //    var pathToTreasure = GetPath(treasure.Position, distanceData);
            //    // check if path has monsters
            //    var monstersOnPath = gameObjects.Where(o => o is Monster)
            //        .Where(m => pathToTreasure.Contains(m.Position)).ToArray();

            //    if (hero.Health < 2 && gameObjects.Any(o => o is Potion) && monstersOnPath.Length > 0)
            //    {
            //        // Look for potion
            //        var recalculatedDistanceData = CalculateDistanceData(allMonsters);
            //        var potion = gameObjects.Where(o => o is Potion).First();
            //        var pathToPotion = GetPath(potion.Position, recalculatedDistanceData);

            //        var node = pathToPotion.Last();
            //        var direction = node - hero.Position;
            //        hero.Move(direction);
            //    }
            //    else if (monstersOnPath.Length == 0 || hero.HasSword)
            //    {
            //        // Look for treasure
            //        var node = pathToTreasure.Last();
            //        var direction = node - hero.Position;
            //        if (direction.Length > 1)
            //            level.OnLeave(hero); // No path to the treasure // TODO: unwinnable state
            //        else
            //            hero.Move(direction);
            //    }
            //    else if (gameObjects.Any(o => o is Sword))
            //    {
            //        // Look for sword
            //        // In that case look for a sword
            //        var sword = gameObjects.Where(o => o is Sword).First();
            //        // Consider monsters as walls in that case
            //        var recalculatedDistanceData = CalculateDistanceData(allMonsters);
            //        var pathToSword = GetPath(sword.Position, recalculatedDistanceData);

            //        var node = pathToSword.Last();
            //        var direction = node - hero.Position;
            //        hero.Move(direction);
            //    }
            //    else
            //    {
            //        // TODO: forfeit when unwinnable
            //        // TODO: a => try to exit
            //        // TODO: b => kill hero when everything else fails
            //    }

            //}
            //else
            //{
            //    // Try to exit the level
            //    var exit = gameObjects.First(o => o is Exit);
            //    var pathToExit = GetPath(exit.Position, distanceData);
            //    var node = pathToExit.Last();
            //    var direction = node - hero.Position;
            //    if (direction.Length > 1)
            //    {
            //        // A hero only can move one cell. If the path has longer movement vector it must be the end-goal without any real paths to it
            //        hero.Kill();
            //    }

            //    hero.Move(direction);
            //}
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

        private List<IntVector2> GetPath(IntVector2 from, IntVector2 to, Dictionary<IntVector2, DistanceData> distanceData)
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
    }
}
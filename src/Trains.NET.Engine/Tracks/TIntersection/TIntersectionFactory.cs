﻿using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Trains.NET.Engine
{
    [Order(2)]
    public class TIntersectionFactory : IStaticEntityFactory<Track>
    {
        private readonly ITerrainMap _terrainMap;
        private readonly ILayout _layout;

        public TIntersectionFactory(ITerrainMap terrainMap, ILayout layout)
        {
            _terrainMap = terrainMap;
            _layout = layout;
        }

        public IEnumerable<Track> GetPossibleReplacements(int column, int row, Track track)
        {
            if (_terrainMap.Get(column, row).IsWater)
            {
                yield break;
            }

            var neighbours = track.GetAllNeighbors();
            if (neighbours.Count < 3)
            {
                yield break;
            }

            if (AreAllPresent(neighbours.Up, neighbours.Left, neighbours.Right))
            {
                yield return new TIntersection() { Direction = TIntersectionDirection.LeftUp_RightUp };
                yield return new TIntersection() { Direction = TIntersectionDirection.LeftUp_RightUp, AlternateState = true };
            }
            if (AreAllPresent(neighbours.Up, neighbours.Left, neighbours.Down))
            {
                yield return new TIntersection() { Direction = TIntersectionDirection.LeftDown_LeftUp };
                yield return new TIntersection() { Direction = TIntersectionDirection.LeftDown_LeftUp, AlternateState = true };
            }
            if (AreAllPresent(neighbours.Up, neighbours.Right, neighbours.Down))
            {
                yield return new TIntersection() { Direction = TIntersectionDirection.RightUp_RightDown };
                yield return new TIntersection() { Direction = TIntersectionDirection.RightUp_RightDown, AlternateState = true };
            }
            if (AreAllPresent(neighbours.Down, neighbours.Left, neighbours.Right))
            {
                yield return new TIntersection() { Direction = TIntersectionDirection.RightDown_LeftDown };
                yield return new TIntersection() { Direction = TIntersectionDirection.RightDown_LeftDown, AlternateState = true };
            }
        }

        private static bool AreAllPresent(Track? track1, Track? track2, Track? track3)
            => track1 is not null
            && track2 is not null
            && track3 is not null;

        public bool TryCreateEntity(int column, int row, bool isPartOfDrag, [NotNullWhen(true)] out Track? entity)
        {
            if (_terrainMap.Get(column, row).IsWater)
            {
                entity = null;
                return false;
            }

            var neighbours = TrackNeighbors.GetConnectedNeighbours(_layout, column, row, emptyIsConsideredConnected: true, ignoreCurrent: isPartOfDrag);

            entity = null;
            if (neighbours.Count == 3)
            {
                if (AreAllPresent(neighbours.Down, neighbours.Left, neighbours.Right))
                {
                    entity = new TIntersection() { Direction = TIntersectionDirection.RightDown_LeftDown };
                }
                else if (AreAllPresent(neighbours.Up, neighbours.Left, neighbours.Right))
                {
                    entity = new TIntersection() { Direction = TIntersectionDirection.LeftUp_RightUp };
                }
                else if (AreAllPresent(neighbours.Up, neighbours.Right, neighbours.Down))
                {
                    entity = new TIntersection() { Direction = TIntersectionDirection.RightUp_RightDown };
                }
                else if (AreAllPresent(neighbours.Up, neighbours.Left, neighbours.Down))
                {
                    entity = new TIntersection() { Direction = TIntersectionDirection.LeftDown_LeftUp };
                }
            }

            return entity is not null;
        }
    }
}

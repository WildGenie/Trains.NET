﻿using System.Collections.Generic;

namespace Trains.NET.Engine
{
    public interface IGameBoard
    {
        int Columns { get; set; }
        int Rows { get; set; }
        bool Enabled { get; set; }

        void ClearAll();
        IMovable? AddTrain(int column, int row);
        IEnumerable<IMovable> GetMovables();
        void RemoveMovable(IMovable thing);
        IEnumerable<T> GetMovables<T>() where T : IMovable;
        IMovable? GetMovableAt(int column, int row);
        List<TrainPosition> GetNextSteps(Train train, float distanceToMove);
    }
}

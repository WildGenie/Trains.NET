﻿using System;
using System.Linq;
using Trains.NET.Engine;
using Trains.NET.Engine.Tracks;
using Xunit;
using static Trains.NET.Tests.TrainMovementTestsHelper;

namespace Trains.NET.Tests.FullGameTests.MovementTest
{
    public class PointToPoint_SingleStep : PointToPoint
    {
        public PointToPoint_SingleStep() : base(1) { }
    }
    public class PointToPoint_2Step : PointToPoint
    {
        public PointToPoint_2Step() : base(2) { }
    }
    public class PointToPoint_3Step : PointToPoint
    {
        public PointToPoint_3Step() : base(3) { }
    }
    public class PointToPoint_10Step : PointToPoint
    {
        public PointToPoint_10Step() : base(10) { }
    }
    public class PointToPoint_100Step : PointToPoint
    {
        public PointToPoint_100Step() : base(100) { }
    }
    public class PointToPoint_1000Step : PointToPoint
    {
        public PointToPoint_1000Step() : base(1000) { }
    }
    public abstract class PointToPoint : IDisposable
    {
        private readonly int _movementSteps;
        private const int MovementPrecision = 4;
        private readonly ITrackLayout _trackLayout;
        private readonly GameBoard _gameBoard;

        public PointToPoint(int movementSteps)
        {
            _movementSteps = movementSteps;
            _trackLayout = new TrackLayout();
            _gameBoard = new GameBoard(_trackLayout, null, null);
        }

        [Theory]
        [InlineData(1, 1, 90.0f, 1, 3)]
        [InlineData(1, 3, 270.0f, 1, 1)]
        public void MovementTest_PointToPoint_3VerticalTracks(int startingColumn, int startingRow, float angle, int expectedColumn, int expectedRow)
        {
            _trackLayout.AddTrack(1, 1);
            _trackLayout.AddTrack(1, 2);
            _trackLayout.AddTrack(1, 3);

            _gameBoard.AddTrain(startingColumn, startingRow);

            float distance = (float)(HalfStraightTrackDistance + StraightTrackDistance + HalfStraightTrackDistance);

            var train = (Train)_gameBoard.GetMovables().Single();
            train.ForceSpeed(distance / _movementSteps / Train.SpeedScaleModifier);
            train.Angle = angle;
            // We have an edge coming up, disable lookahead
            train.LookaheadDistance = 0.0f;

            Assert.Equal(startingColumn, train.Column);
            Assert.Equal(startingRow, train.Row);
            Assert.Equal(0.5f, train.RelativeLeft, MovementPrecision);
            Assert.Equal(0.5f, train.RelativeTop, MovementPrecision);
            Assert.Equal(angle, train.Angle, MovementPrecision);

            

            // Move it!
            for (int i = 0; i < _movementSteps; i++)
                _gameBoard.GameLoopStep();

            Assert.Equal(expectedColumn, train.Column);
            Assert.Equal(expectedRow, train.Row);
            Assert.Equal(0.5f, train.RelativeLeft, MovementPrecision);
            Assert.Equal(0.5f, train.RelativeTop, MovementPrecision);
            Assert.Equal(angle, train.Angle, MovementPrecision);
        }

        [Theory]
        [InlineData(1, 1, 0.0f, 3, 1)]
        [InlineData(3, 1, 180.0f, 1, 1)]
        public void MovementTest_PointToPoint_3HorizontalTracks(int startingColumn, int startingRow, float angle, int expectedColumn, int expectedRow)
        {
            

            _trackLayout.AddTrack(1, 1);
            _trackLayout.AddTrack(2, 1);
            _trackLayout.AddTrack(3, 1);

            _gameBoard.AddTrain(startingColumn, startingRow);

            float distance = (float)(HalfStraightTrackDistance + StraightTrackDistance + HalfStraightTrackDistance);

            var train = (Train)_gameBoard.GetMovables().Single();
            train.ForceSpeed(distance / _movementSteps / Train.SpeedScaleModifier);
            train.Angle = angle;
            // We have an edge coming up, disable lookahead
            train.LookaheadDistance = 0.0f;

            Assert.Equal(startingColumn, train.Column);
            Assert.Equal(startingRow, train.Row);
            Assert.Equal(0.5f, train.RelativeLeft, MovementPrecision);
            Assert.Equal(0.5f, train.RelativeTop, MovementPrecision);
            Assert.Equal(angle, train.Angle, MovementPrecision);

            // Move it!
            for (int i = 0; i < _movementSteps; i++)
                _gameBoard.GameLoopStep();

            Assert.Equal(expectedColumn, train.Column);
            Assert.Equal(expectedRow, train.Row);
            Assert.Equal(0.5f, train.RelativeLeft, MovementPrecision);
            Assert.Equal(0.5f, train.RelativeTop, MovementPrecision);
            Assert.Equal(angle, train.Angle, MovementPrecision);
        }


        [Theory]
        [InlineData(1, 2, 270.0f, 2, 1, 0.0f)]
        [InlineData(2, 1, 180.0f, 1, 2, 90.0f)]
        public void MovementTest_PointToPoint_Vertical_RightDown_Horizontal(int startingColumn, int startingRow, float startingAngle, int expectedColumn, int expectedRow, float expectedAngle)
        {
            

            _trackLayout.AddTrack(1, 2); // Vertical
            _trackLayout.AddTrack(1, 1); // Corner
            _trackLayout.AddTrack(2, 1); // Horizontal

            _gameBoard.AddTrain(startingColumn, startingRow);

            float distance = (float)(HalfStraightTrackDistance + CornerTrackDistance + HalfStraightTrackDistance);

            var train = (Train)_gameBoard.GetMovables().Single();
            train.ForceSpeed(distance / _movementSteps / Train.SpeedScaleModifier);
            train.Angle = startingAngle;
            // We have an edge coming up, disable lookahead
            train.LookaheadDistance = 0.0f;

            Assert.Equal(startingColumn, train.Column);
            Assert.Equal(startingRow, train.Row);
            Assert.Equal(0.5f, train.RelativeLeft, MovementPrecision);
            Assert.Equal(0.5f, train.RelativeTop, MovementPrecision);
            Assert.Equal(startingAngle, train.Angle, MovementPrecision);

            // Move it!
            for (int i = 0; i < _movementSteps; i++)
                _gameBoard.GameLoopStep();

            Assert.Equal(expectedColumn, train.Column);
            Assert.Equal(expectedRow, train.Row);
            Assert.Equal(0.5f, train.RelativeLeft, MovementPrecision);
            Assert.Equal(0.5f, train.RelativeTop, MovementPrecision);
            Assert.Equal(expectedAngle, train.Angle, MovementPrecision);
        }

        [Theory]
        [InlineData(1, 1, 90.0f, 2, 2, 0.0f)]
        [InlineData(2, 2, 180.0f, 1, 1, 270.0f)]
        public void MovementTest_PointToPoint_Horizontal_RightUp_Vertical(int startingColumn, int startingRow, float startingAngle, int expectedColumn, int expectedRow, float expectedAngle)
        {
            

            _trackLayout.AddTrack(1, 1); // Vertical
            _trackLayout.AddTrack(1, 2); // Corner
            _trackLayout.AddTrack(2, 2); // Horizontal

            _gameBoard.AddTrain(startingColumn, startingRow);

            float distance = (float)(HalfStraightTrackDistance + CornerTrackDistance + HalfStraightTrackDistance);

            var train = (Train)_gameBoard.GetMovables().Single();
            train.ForceSpeed(distance / _movementSteps / Train.SpeedScaleModifier);
            train.Angle = startingAngle;
            // We have an edge coming up, disable lookahead
            train.LookaheadDistance = 0.0f;

            Assert.Equal(startingColumn, train.Column);
            Assert.Equal(startingRow, train.Row);
            Assert.Equal(0.5f, train.RelativeLeft, MovementPrecision);
            Assert.Equal(0.5f, train.RelativeTop, MovementPrecision);
            Assert.Equal(startingAngle, train.Angle, MovementPrecision);

            // Move it!
            for (int i = 0; i < _movementSteps; i++)
                _gameBoard.GameLoopStep();

            Assert.Equal(expectedColumn, train.Column);
            Assert.Equal(expectedRow, train.Row);
            Assert.Equal(0.5f, train.RelativeLeft, MovementPrecision);
            Assert.Equal(0.5f, train.RelativeTop, MovementPrecision);
            Assert.Equal(expectedAngle, train.Angle, MovementPrecision);
        }


        [Theory]
        [InlineData(1, 1, 0.0f, 2, 2, 90.0)]
        [InlineData(2, 2, 270.0f, 1, 1, 180.0f)]
        public void MovementTest_PointToPoint_Horizontal_LeftDown_Vertical(int startingColumn, int startingRow, float startingAngle, int expectedColumn, int expectedRow, float expectedAngle)
        {
            

            _trackLayout.AddTrack(1, 1); // Horizontal
            _trackLayout.AddTrack(2, 1); // Corner
            _trackLayout.AddTrack(2, 2); // Vertical

            _gameBoard.AddTrain(startingColumn, startingRow);

            float distance = (float)(HalfStraightTrackDistance + CornerTrackDistance + HalfStraightTrackDistance);

            var train = (Train)_gameBoard.GetMovables().Single();
            train.ForceSpeed(distance / _movementSteps / Train.SpeedScaleModifier);
            train.Angle = startingAngle;
            // We have an edge coming up, disable lookahead
            train.LookaheadDistance = 0.0f;

            Assert.Equal(startingColumn, train.Column);
            Assert.Equal(startingRow, train.Row);
            Assert.Equal(0.5f, train.RelativeLeft, MovementPrecision);
            Assert.Equal(0.5f, train.RelativeTop, MovementPrecision);
            Assert.Equal(startingAngle, train.Angle, MovementPrecision);

            // Move it!
            for (int i = 0; i < _movementSteps; i++)
                _gameBoard.GameLoopStep();

            Assert.Equal(expectedColumn, train.Column);
            Assert.Equal(expectedRow, train.Row);
            Assert.Equal(0.5f, train.RelativeLeft, MovementPrecision);
            Assert.Equal(0.5f, train.RelativeTop, MovementPrecision);
            Assert.Equal(expectedAngle, train.Angle, MovementPrecision);
        }

        [Theory]
        [InlineData(1, 2, 0.0f, 2, 1, 270.0f)]
        [InlineData(2, 1, 90.0f, 1, 2, 180.0f)]
        public void MovementTest_PointToPoint_Horizontal_LeftUp_Vertical(int startingColumn, int startingRow, float startingAngle, int expectedColumn, int expectedRow, float expectedAngle)
        {
            

            _trackLayout.AddTrack(1, 2); // Horizontal
            _trackLayout.AddTrack(2, 2); // Corner
            _trackLayout.AddTrack(2, 1); // Vertical

            _gameBoard.AddTrain(startingColumn, startingRow);

            float distance = (float)(HalfStraightTrackDistance + CornerTrackDistance + HalfStraightTrackDistance);

            var train = (Train)_gameBoard.GetMovables().Single();
            train.ForceSpeed(distance / _movementSteps / Train.SpeedScaleModifier);
            train.Angle = startingAngle;
            // We have an edge coming up, disable lookahead
            train.LookaheadDistance = 0.0f;

            Assert.Equal(startingColumn, train.Column);
            Assert.Equal(startingRow, train.Row);
            Assert.Equal(0.5f, train.RelativeLeft, MovementPrecision);
            Assert.Equal(0.5f, train.RelativeTop, MovementPrecision);
            Assert.Equal(startingAngle, train.Angle, MovementPrecision);

            // Move it!
            for (int i = 0; i < _movementSteps; i++)
                _gameBoard.GameLoopStep();

            Assert.Equal(expectedColumn, train.Column);
            Assert.Equal(expectedRow, train.Row);
            Assert.Equal(0.5f, train.RelativeLeft, MovementPrecision);
            Assert.Equal(0.5f, train.RelativeTop, MovementPrecision);
            Assert.Equal(expectedAngle, train.Angle, MovementPrecision);
        }

        [Theory]
        [InlineData(2, 1, 90.0f, 2, 3)] // Down
        [InlineData(2, 3, 270.0f, 2, 1)] // Up
        [InlineData(1, 2, 0.0f, 3, 2)] // Right
        [InlineData(3, 2, 180.0f, 1, 2)] // Left
        public void MovementTest_PointToPoint_HorizontalVertical_Cross_HorizontalVertical(int startingColumn, int startingRow, float angle, int expectedColumn, int expectedRow)
        {
            

            _trackLayout.AddTrack(2, 1); // Vertical
            _trackLayout.AddTrack(1, 2); // Horizontal
            _trackLayout.AddTrack(2, 2); // Cross
            _trackLayout.AddTrack(3, 2); // Horizontal
            _trackLayout.AddTrack(2, 3); // Vertical

            _gameBoard.AddTrain(startingColumn, startingRow);

            float distance = (float)(HalfStraightTrackDistance + StraightTrackDistance + HalfStraightTrackDistance);

            var train = (Train)_gameBoard.GetMovables().Single();
            train.ForceSpeed(distance / _movementSteps / Train.SpeedScaleModifier);
            train.Angle = angle;
            // We have an edge coming up, disable lookahead
            train.LookaheadDistance = 0.0f;

            Assert.Equal(startingColumn, train.Column);
            Assert.Equal(startingRow, train.Row);
            Assert.Equal(0.5f, train.RelativeLeft, MovementPrecision);
            Assert.Equal(0.5f, train.RelativeTop, MovementPrecision);
            Assert.Equal(angle, train.Angle, MovementPrecision);

            // Move it!
            for (int i = 0; i < _movementSteps; i++)
                _gameBoard.GameLoopStep();

            Assert.Equal(expectedColumn, train.Column);
            Assert.Equal(expectedRow, train.Row);
            Assert.Equal(0.5f, train.RelativeLeft, MovementPrecision);
            Assert.Equal(0.5f, train.RelativeTop, MovementPrecision);
            Assert.Equal(angle, train.Angle, MovementPrecision);
        }

        [Theory]
        [InlineData(1, 2, 0.0f, 2, 3, 90.0f)] // Left to Down
        [InlineData(2, 1, 90.0f, 1, 2, 180.0f)] // Up to Left
        [InlineData(2, 3, 270.0f, 1, 2, 180.0f)] // Down to Left
        public void MovementTest_PointToPoint_Horizontal_LeftUpDown_VerticalVertical(int startingColumn, int startingRow, float startingAngle, int expectedColumn, int expectedRow, float expectedAngle)
        {
            

            _trackLayout.AddTrack(1, 2); // Horizontal
            _trackLayout.AddTrack(2, 1); // Vertical
            _trackLayout.AddTrack(2, 3); // Vertical
            _trackLayout.AddTrack(2, 2); // Y Track

            _gameBoard.AddTrain(startingColumn, startingRow);

            float distance = (float)(HalfStraightTrackDistance + CornerTrackDistance + HalfStraightTrackDistance);

            var train = (Train)_gameBoard.GetMovables().Single();
            train.ForceSpeed(distance / _movementSteps / Train.SpeedScaleModifier);
            train.Angle = startingAngle;
            // We have an edge coming up, disable lookahead
            train.LookaheadDistance = 0.0f;

            Assert.Equal(startingColumn, train.Column);
            Assert.Equal(startingRow, train.Row);
            Assert.Equal(0.5f, train.RelativeLeft, MovementPrecision);
            Assert.Equal(0.5f, train.RelativeTop, MovementPrecision);
            Assert.Equal(startingAngle, train.Angle, MovementPrecision);

            // Move it!
            for (int i = 0; i < _movementSteps; i++)
                _gameBoard.GameLoopStep();

            Assert.Equal(expectedColumn, train.Column);
            Assert.Equal(expectedRow, train.Row);
            Assert.Equal(0.5f, train.RelativeLeft, MovementPrecision);
            Assert.Equal(0.5f, train.RelativeTop, MovementPrecision);
            Assert.Equal(expectedAngle, train.Angle, MovementPrecision);
        }

        [Theory]
        [InlineData(2, 2, 180.0f, 1, 1, 270.0f)] // Right to Up
        [InlineData(1, 1, 90.0f, 2, 2, 0.0f)] // Up to Right
        [InlineData(1, 3, 270.0f, 2, 2, 0.0f)] // Down to Right
        public void MovementTest_PointToPoint_VerticalVertical_RightUpDown_Horizontal(int startingColumn, int startingRow, float startingAngle, int expectedColumn, int expectedRow, float expectedAngle)
        {
            

            _trackLayout.AddTrack(1, 1); // Vertical
            _trackLayout.AddTrack(1, 3); // Vertical
            _trackLayout.AddTrack(2, 2); // Horizontal
            _trackLayout.AddTrack(1, 2); // Y Track

            _gameBoard.AddTrain(startingColumn, startingRow);

            float distance = (float)(HalfStraightTrackDistance + CornerTrackDistance + HalfStraightTrackDistance);

            var train = (Train)_gameBoard.GetMovables().Single();
            train.ForceSpeed(distance / _movementSteps / Train.SpeedScaleModifier);
            train.Angle = startingAngle;
            // We have an edge coming up, disable lookahead
            train.LookaheadDistance = 0.0f;

            Assert.Equal(startingColumn, train.Column);
            Assert.Equal(startingRow, train.Row);
            Assert.Equal(0.5f, train.RelativeLeft, MovementPrecision);
            Assert.Equal(0.5f, train.RelativeTop, MovementPrecision);
            Assert.Equal(startingAngle, train.Angle, MovementPrecision);

            // Move it!
            for (int i = 0; i < _movementSteps; i++)
                _gameBoard.GameLoopStep();

            Assert.Equal(expectedColumn, train.Column);
            Assert.Equal(expectedRow, train.Row);
            Assert.Equal(0.5f, train.RelativeLeft, MovementPrecision);
            Assert.Equal(0.5f, train.RelativeTop, MovementPrecision);
            Assert.Equal(expectedAngle, train.Angle, MovementPrecision);
        }

        [Theory]
        [InlineData(1, 1, 0.0f, 2, 2, 90.0f)] // Left to Down
        [InlineData(2, 2, 270.0f, 3, 1, 0.0f)] // Down to Left
        [InlineData(3, 1, 180.0f, 2, 2, 90.0f)] // Right to Down
        public void MovementTest_PointToPoint_HorizontalHorizontal_LeftRightDown_Vertical(int startingColumn, int startingRow, float startingAngle, int expectedColumn, int expectedRow, float expectedAngle)
        {
            

            _trackLayout.AddTrack(1, 1); // Horizontal
            _trackLayout.AddTrack(3, 1); // Horizontal
            _trackLayout.AddTrack(2, 2); // Vertical
            _trackLayout.AddTrack(2, 1); // Y Track
            
            _gameBoard.AddTrain(startingColumn, startingRow);

            float distance = (float)(HalfStraightTrackDistance + CornerTrackDistance + HalfStraightTrackDistance);

            var train = (Train)_gameBoard.GetMovables().Single();
            train.ForceSpeed(distance / _movementSteps / Train.SpeedScaleModifier);
            train.Angle = startingAngle;
            // We have an edge coming up, disable lookahead
            train.LookaheadDistance = 0.0f;

            Assert.Equal(startingColumn, train.Column);
            Assert.Equal(startingRow, train.Row);
            Assert.Equal(0.5f, train.RelativeLeft, MovementPrecision);
            Assert.Equal(0.5f, train.RelativeTop, MovementPrecision);
            Assert.Equal(startingAngle, train.Angle, MovementPrecision);

            // Move it!
            for (int i = 0; i < _movementSteps; i++)
                _gameBoard.GameLoopStep();

            Assert.Equal(expectedColumn, train.Column);
            Assert.Equal(expectedRow, train.Row);
            Assert.Equal(0.5f, train.RelativeLeft, MovementPrecision);
            Assert.Equal(0.5f, train.RelativeTop, MovementPrecision);
            Assert.Equal(expectedAngle, train.Angle, MovementPrecision);
        }

        [Theory]
        [InlineData(1, 2, 0.0f, 2, 1, 270.0f)] // Left to Up
        [InlineData(2, 1, 90.0f, 1, 2, 180.0f)] // Up to Left
        [InlineData(3, 2, 180.0f, 2, 1, 270.0f)] // Right to Up
        public void MovementTest_PointToPoint_HorizontalHorizontal_LeftRightUp_Vertical(int startingColumn, int startingRow, float startingAngle, int expectedColumn, int expectedRow, float expectedAngle)
        {
            

            _trackLayout.AddTrack(1, 2); // Horizontal
            _trackLayout.AddTrack(3, 2); // Horizontal
            _trackLayout.AddTrack(2, 1); // Vertical
            _trackLayout.AddTrack(2, 2); // Y Track

            _gameBoard.AddTrain(startingColumn, startingRow);

            float distance = (float)(HalfStraightTrackDistance + CornerTrackDistance + HalfStraightTrackDistance);

            var train = (Train)_gameBoard.GetMovables().Single();
            train.ForceSpeed(distance / _movementSteps / Train.SpeedScaleModifier);
            train.Angle = startingAngle;
            // We have an edge coming up, disable lookahead
            train.LookaheadDistance = 0.0f;

            Assert.Equal(startingColumn, train.Column);
            Assert.Equal(startingRow, train.Row);
            Assert.Equal(0.5f, train.RelativeLeft, MovementPrecision);
            Assert.Equal(0.5f, train.RelativeTop, MovementPrecision);
            Assert.Equal(startingAngle, train.Angle, MovementPrecision);

            // Move it!
            for (int i = 0; i < _movementSteps; i++)
                _gameBoard.GameLoopStep();

            Assert.Equal(expectedColumn, train.Column);
            Assert.Equal(expectedRow, train.Row);
            Assert.Equal(0.5f, train.RelativeLeft, MovementPrecision);
            Assert.Equal(0.5f, train.RelativeTop, MovementPrecision);
            Assert.Equal(expectedAngle, train.Angle, MovementPrecision);
        }

        public void Dispose()
        {
            ((IDisposable)_gameBoard).Dispose();
        }
    }
}

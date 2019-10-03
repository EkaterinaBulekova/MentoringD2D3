using System;
using System.Diagnostics;
using TasksInterface;

namespace Task3
{
    enum MultiplicationType
    {
        Parallel,
        Sequential
    }

    public class MatrixMultiplier : IStartable
    {
        const string textTemplate = "Time Matrix {0}*{0} {1} multiplication elapsed: {2}";
        private Matrix firstMatrix, secondMatrix;

        private void InitializeMatrixes(int dimension)
        {
            firstMatrix = new Matrix(dimension);
            firstMatrix.FillRandom();
            secondMatrix = new Matrix(dimension);
            secondMatrix.FillRandom();
        }

        private void MatrixMultiplicationEstimation(int dimension)
        {
            InitializeMatrixes(dimension);

            Multiplication(MultiplicationType.Parallel, dimension, () => firstMatrix * secondMatrix);

            Multiplication(MultiplicationType.Sequential, dimension, () => Matrix.Multiple(firstMatrix,secondMatrix));

            Console.WriteLine();
        }

        private void Multiplication(MultiplicationType type, int dimension, Func<Matrix> p)
        {
            var stopWatch = new Stopwatch();
            stopWatch.Start();
            var result = p();
            stopWatch.Stop();
            Console.WriteLine(textTemplate, dimension, type.ToString(), stopWatch.Elapsed);
        }

        public void Start()
        {
            MatrixMultiplicationEstimation(50);
            MatrixMultiplicationEstimation(100);
            MatrixMultiplicationEstimation(200);
            MatrixMultiplicationEstimation(300);
            MatrixMultiplicationEstimation(400);
        }

    }
}

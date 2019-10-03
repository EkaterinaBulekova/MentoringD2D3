using System;
using System.Collections;
using System.Threading.Tasks;

namespace Task3
{
    class Matrix
    {
        private long[] _data;

        private int _rows { get; }
        private int _columns { get; }

        /// <summary>
        /// Constructor to create a new Matrix while specifying the number of
        /// rows and columns.
        /// </summary>
        /// <param name="rows">The number of rows to initialise the Matrix with.</param>
        /// <param name="cols">The number of columns to initialise the Matrix with.</param>
        public Matrix(int rows, int columns)
        {
            _rows = rows;
            _columns = columns;
            _data = new long[rows * columns];
        }

        /// <summary>
        /// Constructor to create a new square Matrix.
        /// </summary>
        /// <param name="dimension">The number of rows and columns to initialise the
        /// Matrix with. There will be an equal number of rows and columns.</param>
        public Matrix(int dimension) : this(dimension, dimension)
        {
        }

        public Matrix(long[,] array) : this(array.GetLength(0), array.GetLength(1))
        {
            int index = 0;
            for (int row = 0; row < _rows; row++)
            {
                for (int column = 0; column < _columns; column++)
                {
                    _data[index++] = array[row, column];
                }
            }
        }

        public Matrix(Matrix m) : this(m._rows, m._columns)
        {
            for (int i = 0; i < _data.Length; i++)
                _data[i] = m._data[i];
        }

        /// <summary>
        /// Indexer to easily access a specific location in this Matrix.
        /// </summary>
        /// <param name="row">The row of the Matrix location to access.</param>
        /// <param name="column">The column of the Matrix location to access.</param>
        /// <returns>The value stored at the given row/column location.</returns>
        /// <remarks>Matrices are zero-indexed.</remarks>
        public long this[int row, int column]
        {
            get { return _data[(row * _columns) + column]; }
            set { _data[(row * _columns) + column] = value; }
        }

        /// <summary>
        /// Fill the matrix with random number
        /// </summary>
        public void FillRandom()
        {
            var rnd = new Random(int.Parse(DateTime.Now.ToString("HHmmssfff")));
            for (int i = 0; i < _columns * _rows; i++)
            {
                _data[i] = rnd.Next(100);
            }
        }

        /// <summary>
        /// Multiply two matrices together parallel.
        /// </summary>
        /// <param name="m1">An nxm dimension Matrix.</param>
        /// <param name="m2">An mxp dimension Matrix.</param>
        /// <returns>An nxp Matrix that is the product of m1 and m2.</returns>
        public static Matrix operator *(Matrix m1, Matrix m2)
        {
            if (m1._columns == m2._rows)
            {
                Matrix output = new Matrix(m1._rows, m2._columns);
                Parallel.For(0, m1._rows, i => MultiplyRow(i, m1, m2, ref output));
                return output;
            }
            else
            {
                throw new InvalidMatrixDimensionException("Multiplication cannot be performed on matrices with these dimensions.");
            }
        }

        /// <summary>
        /// Multiply two matrices together sequential.
        /// </summary>
        /// <param name="m1">An nxm dimension Matrix.</param>
        /// <param name="m2">An mxp dimension Matrix.</param>
        /// <returns>An nxp Matrix that is the product of m1 and m2.</returns>
        public static Matrix Multiple(Matrix m1, Matrix m2)
        {
            if (m1._columns == m2._rows)
            {
                Matrix output = new Matrix(m1._rows, m2._columns);
                for (int i = 0; i < m1._rows; i++)
                {
                    for (int j = 0; j < m2._columns; j++)
                    {
                        output[i, j] = 0;

                        for (int k = 0; k < m1._columns; k++)
                        {
                            output[i, j] += m1[i, k] * m2[k, j];
                        }
                    }
                }
                return output;
            }
            else
            {
                throw new InvalidMatrixDimensionException("Multiplication cannot be performed on matrices with these dimension.");
            }
        }

        /// <summary>
        /// Convert the Matrix to a string representation.
        /// </summary>
        /// <returns>A string representation of this Matrix.</returns>
        /// <remarks>All elements are rounded to two decimal places.</remarks>
        public override string ToString()
        {
            string res = "";
            for (int i = 0; i < _rows; ++i)
            {
                for (int j = 0; j < _columns; ++j)
                {
                    res += (_data[i * _columns + j] + " ");
                }
                res += "\n";
            }
            return res;
        }

        public IEnumerator GetEnumerator()
        {
            return _data.GetEnumerator();
        }

        private static void MultiplyRow(int row, Matrix m1, Matrix m2, ref Matrix output)
        {
            int m1_index = row * m1._columns;
            int m2_index;

            for (int column = 0; column < output._columns; column++)
            {
                long result = 0;
                m2_index = column;

                for (int i = 0; i < m1._columns; i++)
                {
                    result += m1._data[m1_index + i] * m2._data[m2_index];
                    m2_index += m2._columns;
                }

                output[row, column] = result;
            }
        }
    }

    /// <summary>
    /// Custom exception for Matrix operations using incorrect dimension.
    /// </summary>
    public class InvalidMatrixDimensionException : InvalidOperationException
    {
        public InvalidMatrixDimensionException()
        {
        }

        public InvalidMatrixDimensionException(string message)
            : base(message)
        {
        }

        public InvalidMatrixDimensionException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}

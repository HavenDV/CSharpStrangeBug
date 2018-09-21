using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CSharpStrangeBug
{
    [TestClass]
    public class StrangeBugTests
    {
        private static float[][] GenerateDctMatrix(int size)
        {
            var c1 = Math.Sqrt(2.0f / size);

            var matrix = new float[size][];
            for (var i = 0; i < size; i++)
            {
                matrix[i] = new float[size];
            }

            for (var i = 0; i < size; i++)
            {
                matrix[0][i] = (float)Math.Sqrt(1.0d / size);

                for (var j = 1; j < size; j++)
                {
                    matrix[j][i] = (float)(c1 * Math.Cos((2 * i + 1) * j * Math.PI / (2.0d * size)));
                }
            }
            return matrix;
        }

        private static float[][] Multiply(float[][] a, float[][] b, bool cast)
        {
            var n = a[0].Length;
            var c = new float[n][];
            for (var i = 0; i < n; i++)
            {
                c[i] = new float[n];
                for (var k = 0; k < n; k++)
                {
                    for (var j = 0; j < n; j++)
                    {
                        if (cast)
                        {
                            // ReSharper disable once RedundantCast
                            c[i][j] += (float)(a[i][k] * b[k][j]);
                        }
                        else
                        {
                            c[i][j] += a[i][k] * b[k][j];
                        }
                    }
                }
            }

            return c;
        }

        [TestMethod]
        public void StrangeBugTest()
        {
            const int size = 32;

            var a = GenerateDctMatrix(size);

            var b = new float[size][];
            for (var i = 0; i < size; i++)
            {
                b[i] = new float[size];
                for (var j = 0; j < size; j++)
                {
                    var shift = j + i * size;
                    var value = shift % 256 / 255.0f;

                    b[i][j] = value;
                }
            }

            var array1 = Multiply(a, b, true);
            var array2 = Multiply(a, b, false);

            for (var i = 0; i < size; i++)
            {
                for (var j = 0; j < size; j++)
                {
                    Assert.AreEqual(array1[i][j], array2[i][j]);
                }
            }
        }
    }
}

using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CSharpStrangeBug
{
    [TestClass]
    public class StrangeBugTests
    {
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

            var random = new Random();
            var a = Enumerable.Range(0, size)
                .Select(
                    i => Enumerable.Range(0, size)
                        .Select(j => (float)random.NextDouble())
                        .ToArray())
                .ToArray();

            var array1 = Multiply(a, a, true);
            var array2 = Multiply(a, a, false);

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

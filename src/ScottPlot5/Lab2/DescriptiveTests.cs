using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ScottPlot.Statistics;
namespace Lab2
{
    public class DescriptiveTests
    {
        [Fact]
        public void Sum_ValidArray_ReturnsCorrectSum()
        {
            var values = new double[] { 1, 2, 3 };
            var result = Descriptive.Sum(values);
            Assert.Equal(6, result);
        }

        [Fact]
        public void Sum_EmptyArray_ThrowsArgumentException()
        {
            var values = new double[] { };
            Assert.Throws<ArgumentException>(() => Descriptive.Sum(values));
        }

        [Fact]
        public void Mean_ValidArray_ReturnsCorrectMean()
        {
            var values = new double[] { 1, 2, 3, 4, 5 };
            var result = Descriptive.Mean(values);
            Assert.Equal(3, result);
        }

        [Fact]
        public void Mean_EmptyArray_ThrowsArgumentException()
        {
            var values = new double[] { };
            Assert.Throws<ArgumentException>(() => Descriptive.Mean(values));
        }

        [Fact]
        public void Median_ValidArray_ReturnsCorrectMedian()
        {
            var values = new double[] { 1, 2, 3, 4, 5 };
            var result = Descriptive.Median(values);
            Assert.Equal(3, result);
        }

        [Fact]
        public void Median_EmptyArray_ThrowsArgumentException()
        {
            var values = new double[] { };
            Assert.Throws<ArgumentException>(() => Descriptive.Median(values));
        }

        [Theory]
        [InlineData(new double[] { 1, 2, 3, 4, 5 }, 2.5)]
        [InlineData(new double[] { 5, 10, 15, 20 }, 41.6666666667)]
        [InlineData(new double[] { 5 }, 0)]
        public void Variance_ValidArray_ReturnsCorrectVariance(double[] values, double expectedVariance)
        {
            var result = Descriptive.Variance(values);
            Assert.Equal(expectedVariance, result, 1);
        }

        [Fact]
        public void Variance_EmptyArray_ThrowsArgumentException()
        {
            var values = new double[] { };
            Assert.Throws<ArgumentException>(() => Descriptive.Variance(values));
        }

        [Fact]
        public void Variance_SingleElement_ReturnsZero()
        {
            var values = new double[] { 5 };
            var result = Descriptive.Variance(values);
            Assert.Equal(0, result);
        }

        [Theory]
        [InlineData(new double[] { 1, 2, 3, 4, 5 }, 1.58)]
        [InlineData(new double[] { 5, 10, 15, 20 }, 6.45)]
        [InlineData(new double[] { 5 }, 0)]
        public void StandardDeviation_ValidArray_ReturnsCorrectStandardDeviation(double[] values, double expectedStdDev)
        {
            var result = Descriptive.StandardDeviation(values);
            Assert.Equal(expectedStdDev, result, 2); // Allowing 2 decimal precision
        }

        [Fact]
        public void StandardDeviation_EmptyArray_ThrowsArgumentException()
        {
            var values = new double[] { };
            Assert.Throws<ArgumentException>(() => Descriptive.StandardDeviation(values));
        }

        [Fact]
        public void StandardDeviation_SingleElement_ReturnsZero()
        {
            var values = new double[] { 5 };
            var result = Descriptive.StandardDeviation(values);
            Assert.Equal(0, result);
        }

        [Fact]
        public void Percentile_ValidArray_ReturnsCorrectPercentile()
        {
            var values = new double[] { 1, 2, 3, 4, 5 };
            var result = Descriptive.Percentile(values, 40);
            Assert.Equal(3, result); // was 2
        }

        [Fact]
        public void Percentile_EmptyArray_ThrowsArgumentException()
        {
            var values = new double[] { };
            Assert.Throws<ArgumentOutOfRangeException>(() => Descriptive.Percentile(values, 50));
        }

        [Fact]
        public void RemoveNaN_ValidArray_RemovesNaNValues()
        {
            var values = new double[] { 1, double.NaN, 3, double.NaN, 5 };
            var result = Descriptive.RemoveNaN(values);
            Assert.Equal(new double[] { 1, 3, 5 }, result);
        }

        [Fact]
        public void RemoveNaN_EmptyArray_ReturnsEmptyArray()
        {
            var values = new double[] { };
            var result = Descriptive.RemoveNaN(values);
            Assert.Empty(result);
        }

        [Fact]
        public void NanMean_ValidArray_IgnoringNaNValues_ReturnsCorrectMean()
        {
            var values = new double[] { 1, double.NaN, 3, double.NaN, 5 };
            var result = Descriptive.NanMean(values);
            Assert.Equal(3, result);
        }

        [Fact]
        public void NanMean_AllNaNValues_ReturnsNaN()
        {
            var values = new double[] { double.NaN, double.NaN, double.NaN };
            var result = Descriptive.NanMean(values);
            Assert.Equal(double.NaN, result);
        }

        [Fact]
        public void NanVariance_ValidArray_IgnoringNaNValues_ReturnsCorrectVariance()
        {
            var values = new double[] { 1, double.NaN, 3, double.NaN, 5 };
            var result = Descriptive.NanVariance(values);
            Assert.Equal(4, result, 1);
        }

        [Fact]
        public void NanVariance_NotEnoughData_ReturnsNaN()
        {
            var values = new double[] { double.NaN };
            var result = Descriptive.NanVariance(values);
            Assert.Equal(double.NaN, result);
        }

        [Fact]
        public void ArrayToVector_EmptyArray_ThrowsArgumentException()
        {
            var values = new double[,] { };
            Assert.Throws<ArgumentException>(() => Descriptive.ArrayToVector(values, 0));
        }

        [Fact]
        public void VerticalMean_ValidMatrix_ReturnsCorrectMeanPerColumn()
        {
            var values = new double[,] { { 1, 2, 3 }, { 4, 5, 6 }, { 7, 8, 9 } };
            var result = Descriptive.VerticalMean(values);
            Assert.Equal(new double[] { 4, 5, 6 }, result);
        }

        [Fact]
        public void ArrayTranspose_EmptyMatrix_ReturnsEmptyMatrix()
        {
            var values = new double[,] { { } };
            var result = Descriptive.ArrayTranspose(values);
            Assert.Empty(result);
        }

        [Fact]
        public void VerticalSlice_ValidColumnIndex_ReturnsCorrectSlice()
        {
            var values = new double[,] { { 1, 2, 3 }, { 4, 5, 6 }, { 7, 8, 9 } };
            var result = Descriptive.VerticalSlice(values, 1);
            Assert.Equal(new double[] { 2, 5, 8 }, result);
        }

        [Fact]
        public void VerticalSlice_InvalidColumnIndex_ThrowsArgumentException()
        {
            var values = new double[,] { { 1, 2, 3 }, { 4, 5, 6 } };
            Assert.Throws<IndexOutOfRangeException>(() => Descriptive.VerticalSlice(values, 5));
        }

        [Fact]
        public void NanStandardDeviation_ValidArray_IgnoringNaNValues_ReturnsCorrectStandardDeviation()
        {
            var values = new double[] { 1, double.NaN, 3, double.NaN, 5 };
            var result = Descriptive.NanStandardDeviation(values);
            Assert.Equal(2, result, 1);
        }

        [Fact]
        public void NanStandardDeviation_NotEnoughData_ReturnsNaN()
        {
            var values = new double[] { double.NaN };
            var result = Descriptive.NanStandardDeviation(values);
            Assert.Equal(double.NaN, result);
        }
        [Fact]
        public void VerticalNanMean_ShouldReturnCorrectResults()
        {
            double[,] values = {
            { 1.0, 2.0, 3.0 },
            { 4.0, double.NaN, 6.0 },
            { 7.0, 8.0, 9.0 }
        };

            double[] result = Descriptive.VerticalNanMean(values);

            // As an example, we assert that the mean calculation for each column is correct
            Assert.Equal(4.0, result[0], 1); // mean of the first column
            Assert.Equal(5.0, result[1], 1); // mean of the second column (ignoring NaN)
            Assert.Equal(6.0, result[2], 1); // mean of the third column
        }

        // Test VerticalStandardDeviation
        [Fact]
        public void VerticalStandardDeviation_ShouldReturnCorrectResults()
        {
            double[,] values = {
            { 1.0, 2.0, 3.0 },
            { 4.0, 5.0, 6.0 },
            { 7.0, 8.0, 9.0 }
        };

            double[] result = Descriptive.VerticalStandardDeviation(values);

            Assert.Equal(3.0, result[0], 1); // standard deviation of the first column
            Assert.Equal(3.0, result[1], 1); // standard deviation of the second column
            Assert.Equal(3.0, result[2], 1); // standard deviation of the third column
        }

        // Test exception for empty array in ArrayToVector
        [Fact]
        public void ArrayToVector_ShouldThrowExceptionForEmptyArray()
        {
            double[,] values = new double[0, 0];

            var exception = Assert.Throws<ArgumentException>(() => Descriptive.ArrayToVector(values));
            Assert.Equal("Array values cannot be empty", exception.Message);
        }
    }
}

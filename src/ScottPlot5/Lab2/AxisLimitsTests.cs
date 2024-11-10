using Moq;
using ScottPlot;
using ScottPlot.DataSources;
using Xunit;

namespace Lab2
{
    public class AxisLimitsTests
    {
        [Fact]
        public void Test_ScatterLimits()
        {
            double[] xs = Generate.Consecutive(51);
            double[] ys = Generate.Sin(51);

            ScatterSourceDoubleArray source = new(xs, ys);
            AxisLimits limits = source.GetLimits();

            Assert.Equal(0, limits.Left);
            Assert.Equal(50, limits.Right);
            Assert.Equal(-1, limits.Bottom, 1);
            Assert.Equal(1, limits.Top, 1);
        }

        [Fact]
        public void Test_ScatterLimits_WithNoRealPoint()
        {
            double[] xs = Generate.NaN(51);
            double[] ys = Generate.NaN(51);

            xs[22] = 5;
            ys[33] = 7;

            ScatterSourceDoubleArray source = new(xs, ys);
            AxisLimits limits = source.GetLimits();

            Assert.Equal(5, limits.Left);
            Assert.Equal(5, limits.Right);
            Assert.Equal(7, limits.Bottom);
            Assert.Equal(7, limits.Top);
        }

        [Fact]
        public void Test_ScatterLimits_WithOnePoint_DoubleArray()
        {
            double[] xs = Generate.NaN(51);
            double[] ys = Generate.NaN(51);

            xs[44] = 5;
            ys[44] = 7;

            ScatterSourceDoubleArray source = new(xs, ys);
            AxisLimits limits = source.GetLimits();

            Assert.Equal(5, limits.Left);
            Assert.Equal(5, limits.Right);
            Assert.Equal(7, limits.Bottom);
            Assert.Equal(7, limits.Top);
        }

        [Fact]
        public void Test_ScatterLimits_WithOnePoint_CoordinatesArray()
        {
            double[] xs = Generate.NaN(51);
            double[] ys = Generate.NaN(51);

            xs[44] = 5;
            ys[44] = 7;

            Coordinates[] cs = Enumerable
                .Range(0, xs.Length)
                .Select(x => new Coordinates(xs[x], ys[x]))
                .ToArray();

            ScatterSourceCoordinatesArray source = new(cs);
            AxisLimits limits = source.GetLimits();

            Assert.Equal(5, limits.Left);
            Assert.Equal(5, limits.Right);
            Assert.Equal(7, limits.Bottom);
            Assert.Equal(7, limits.Top);
        }

        [Fact]
        public void Test_ScatterLimits_WithOnePoint_CoordinatesList()
        {
            double[] xs = Generate.NaN(51);
            double[] ys = Generate.NaN(51);

            xs[44] = 5;
            ys[44] = 7;

            List<Coordinates> cs = Enumerable
                .Range(0, xs.Length)
                .Select(x => new Coordinates(xs[x], ys[x]))
                .ToList();

            ScatterSourceCoordinatesList source = new(cs);
            AxisLimits limits = source.GetLimits();

            Assert.Equal(5, limits.Left);
            Assert.Equal(5, limits.Right);
            Assert.Equal(7, limits.Bottom);
            Assert.Equal(7, limits.Top);
        }

        [Fact]
        public void Test_ScatterLimits_WithOnePoint_CoordinatesGenericArray()
        {
            float[] xs = Enumerable.Range(0, 51).Select(x => float.NaN).ToArray();
            float[] ys = Enumerable.Range(0, 51).Select(x => float.NaN).ToArray();

            xs[44] = 5;
            ys[44] = 7;

            ScatterSourceGenericArray<float, float> source = new(xs, ys);
            AxisLimits limits = source.GetLimits();

            Assert.Equal(5, limits.Left);
            Assert.Equal(5, limits.Right);
            Assert.Equal(7, limits.Bottom);
            Assert.Equal(7, limits.Top);
        }

        [Fact]
        public void Test_ScatterLimits_WithOnePoint_CoordinatesGenericList()
        {
            List<float> xs = Enumerable.Range(0, 51).Select(x => float.NaN).ToList();
            List<float> ys = Enumerable.Range(0, 51).Select(x => float.NaN).ToList();

            xs[44] = 5;
            ys[44] = 7;

            ScatterSourceGenericList<float, float> source = new(xs, ys);
            AxisLimits limits = source.GetLimits();

            Assert.Equal(5, limits.Left);
            Assert.Equal(5, limits.Right);
            Assert.Equal(7, limits.Bottom);
            Assert.Equal(7, limits.Top);
        }

        [Fact]
        public void Test_ScatterLimits_WithOneMissingPoint()
        {
            double[] xs = Generate.Consecutive(51);
            double[] ys = Generate.Sin(51);

            xs[44] = double.NaN;
            ys[44] = double.NaN;

            ScatterSourceDoubleArray source = new(xs, ys);
            AxisLimits limits = source.GetLimits();

            Assert.Equal(0, limits.Left);
            Assert.Equal(50, limits.Right);
            Assert.Equal(-1, limits.Bottom, 1);
            Assert.Equal(1, limits.Top, 1);
        }

        [Fact]
        public void Test_ScatterLimits_MissingLeft()
        {
            double[] xs = Generate.Consecutive(51);
            double[] ys = Generate.Sin(51);

            for (int i = 0; i < 25; i++)
            {
                xs[i] = double.NaN;
                ys[i] = double.NaN;
            }

            ScatterSourceDoubleArray source = new(xs, ys);
            AxisLimits limits = source.GetLimits();

            Assert.Equal(25, limits.Left);
            Assert.Equal(50, limits.Right);
            Assert.Equal(-1, limits.Bottom, 1);
            Assert.Equal(0, limits.Top, 1);
        }

        [Fact]
        public void Test_ScatterLimits_MissingRight()
        {
            double[] xs = Generate.Consecutive(51);
            double[] ys = Generate.Sin(51);

            for (int i = 26; i < ys.Length; i++)
            {
                xs[i] = double.NaN;
                ys[i] = double.NaN;
            }

            ScatterSourceDoubleArray source = new(xs, ys);
            AxisLimits limits = source.GetLimits();

            Assert.Equal(0, limits.Left);
            Assert.Equal(25, limits.Right);
            Assert.Equal(0, limits.Bottom, 1);
            Assert.Equal(1, limits.Top, 1);
        }

        [Fact]
        public void Test_Scatter_GetNearest_CoordinatesArray()
        {
            double[] xs = Generate.Consecutive(51);
            double[] ys = Generate.Sin(51);
            Coordinates[] cs = Enumerable
                .Range(0, xs.Length)
                .Select(x => new Coordinates(xs[x], ys[x]))
                .ToArray();

            ScottPlot.Plot plot = new();
            var spDoubleArray = plot.Add.Scatter(cs);

            plot.GetImage(600, 400);
            var renderInfo = plot.RenderManager.LastRender;

            Coordinates location = new(25, 0.8);
            DataPoint nearest = spDoubleArray.Data.GetNearest(location, renderInfo, maxDistance: 100);

            Assert.Equal(20, nearest.Index);
            Assert.Equal(20, nearest.X);
            Assert.Equal(0.58778, nearest.Y, 3);
        }

        [Fact]
        public void Test_Scatter_GetNearest_CoordinatesList()
        {
            double[] xs = Generate.Consecutive(51);
            double[] ys = Generate.Sin(51);
            List<Coordinates> cs = Enumerable
                .Range(0, xs.Length)
                .Select(x => new Coordinates(xs[x], ys[x]))
                .ToList();

            ScottPlot.Plot plot = new();
            var spDoubleArray = plot.Add.Scatter(cs);

            plot.GetImage(600, 400);
            var renderInfo = plot.RenderManager.LastRender;

            Coordinates location = new(25, 0.8);
            DataPoint nearest = spDoubleArray.Data.GetNearest(location, renderInfo, maxDistance: 100);

            Assert.Equal(20, nearest.Index);
            Assert.Equal(20, nearest.X);
            Assert.Equal(0.58778, nearest.Y, 3);
        }

        [Fact]
        public void Test_Scatter_GetNearest_DoubleArray()
        {
            double[] xs = Generate.Consecutive(51);
            double[] ys = Generate.Sin(51);

            ScottPlot.Plot plot = new();
            var spDoubleArray = plot.Add.Scatter(xs, ys);

            plot.GetImage(600, 400);
            var renderInfo = plot.RenderManager.LastRender;

            Coordinates location = new(25, 0.8);
            DataPoint nearest = spDoubleArray.Data.GetNearest(location, renderInfo, maxDistance: 100);
            Assert.Equal(20, nearest.Index);
            Assert.Equal(20, nearest.X);
            Assert.Equal(0.58778, nearest.Y, 3);
        }

        [Fact]
        public void Test_Scatter_GetNearest_GenericArray()
        {
            float[] xs = Generate.Consecutive(51).Select(x => (float)x).ToArray();
            float[] ys = Generate.Sin(51).Select(x => (float)x).ToArray();

            ScottPlot.Plot plot = new();
            var spDoubleArray = plot.Add.Scatter(xs, ys);

            plot.GetImage(600, 400);
            var renderInfo = plot.RenderManager.LastRender;

            Coordinates location = new(25, 0.8);
            DataPoint nearest = spDoubleArray.Data.GetNearest(location, renderInfo, maxDistance: 100);
            Assert.Equal(20, nearest.Index);
            Assert.Equal(20, nearest.X);
            Assert.Equal(0.58778, nearest.Y, 3);
        }

        [Fact]
        public void Test_Scatter_GetNearest_GenericList()
        {
            List<float> xs = Generate.Consecutive(51).Select(x => (float)x).ToList();
            List<float> ys = Generate.Sin(51).Select(x => (float)x).ToList();

            ScottPlot.Plot plot = new();
            var spDoubleArray = plot.Add.Scatter(xs, ys);

            plot.GetImage(600, 400);
            var renderInfo = plot.RenderManager.LastRender;

            Coordinates location = new(25, 0.8);
            DataPoint nearest = spDoubleArray.Data.GetNearest(location, renderInfo, maxDistance: 100);

            Assert.Equal(20, nearest.Index);
            Assert.Equal(20, nearest.X);
            Assert.Equal(0.58778, nearest.Y, 3);
        }


        [Fact]
        public void Constructor_WithValidPlottables_SetsAxisLimits()
        {
            var mockPlottable1 = new Mock<IPlottable>();
            var mockPlottable2 = new Mock<IPlottable>();

            mockPlottable1.Setup(p => p.GetAxisLimits()).Returns(new AxisLimits(0, 10, 0, 10));
            mockPlottable2.Setup(p => p.GetAxisLimits()).Returns(new AxisLimits(5, 15, 5, 15));

            var plottables = new List<IPlottable> { mockPlottable1.Object, mockPlottable2.Object };

            var axisLimits = new AxisLimits(plottables);

            Assert.Equal(0, axisLimits.Left);
            Assert.Equal(15, axisLimits.Right);
            Assert.Equal(0, axisLimits.Bottom);
            Assert.Equal(15, axisLimits.Top);
        }


        [Fact]
        public void Constructor_WithMixedPlottables_SetsCorrectLimits()
        {
            var mockPlottable1 = new Mock<IPlottable>();
            var mockPlottable2 = new Mock<IPlottable>();
            var mockPlottable3 = new Mock<IPlottable>();

            mockPlottable1.Setup(p => p.GetAxisLimits()).Returns(new AxisLimits(0, 5, 0, 5));
            mockPlottable2.Setup(p => p.GetAxisLimits()).Returns(new AxisLimits(10, 20, -5, 5));
            mockPlottable3.Setup(p => p.GetAxisLimits()).Returns(new AxisLimits(-10, -5, -10, -5));

            var plottables = new List<IPlottable> { mockPlottable1.Object, mockPlottable2.Object, mockPlottable3.Object };

            var axisLimits = new AxisLimits(plottables);

            Assert.Equal(-10, axisLimits.Left);
            Assert.Equal(20, axisLimits.Right);
            Assert.Equal(-10, axisLimits.Bottom);
            Assert.Equal(5, axisLimits.Top);
        }

        [Fact]
        public void Constructor_HandlesPlottablesWithIdenticalLimits()
        {
            var mockPlottable1 = new Mock<IPlottable>();
            var mockPlottable2 = new Mock<IPlottable>();

            var identicalLimits = new AxisLimits(0, 10, 0, 10);
            mockPlottable1.Setup(p => p.GetAxisLimits()).Returns(identicalLimits);
            mockPlottable2.Setup(p => p.GetAxisLimits()).Returns(identicalLimits);

            var plottables = new List<IPlottable> { mockPlottable1.Object, mockPlottable2.Object };

            var axisLimits = new AxisLimits(plottables);

            Assert.Equal(0, axisLimits.Left);
            Assert.Equal(10, axisLimits.Right);
            Assert.Equal(0, axisLimits.Bottom);
            Assert.Equal(10, axisLimits.Top);
        }

        [Fact]
        public void Constructor_WithValidCoordinates_SetsAxisLimits()
        {
            var coordinates = new List<Coordinates>
        {
            new Coordinates(0, 10),
            new Coordinates(5, 15),
            new Coordinates(10, 20)
        };

            var axisLimits = new AxisLimits(coordinates);

            Assert.Equal(0, axisLimits.Left);
            Assert.Equal(10, axisLimits.Right);
            Assert.Equal(10, axisLimits.Bottom);
            Assert.Equal(20, axisLimits.Top);
        }

        [Fact]
        public void Constructor_WithSingleCoordinate_SetsLimitsToThatCoordinate()
        {
            var coordinates = new List<Coordinates>
        {
            new Coordinates(5, 10)
        };

            var axisLimits = new AxisLimits(coordinates);

            Assert.Equal(5, axisLimits.Left);
            Assert.Equal(5, axisLimits.Right);
            Assert.Equal(10, axisLimits.Bottom);
            Assert.Equal(10, axisLimits.Top);
        }

        [Fact]
        public void Constructor_WithNegativeAndPositiveCoordinates_SetsCorrectLimits()
        {
            var coordinates = new List<Coordinates>
        {
            new Coordinates(-5, 10),
            new Coordinates(0, -5),
            new Coordinates(5, 15)
        };
            var axisLimits = new AxisLimits(coordinates);

            Assert.Equal(-5, axisLimits.Left);
            Assert.Equal(5, axisLimits.Right);
            Assert.Equal(-5, axisLimits.Bottom);
            Assert.Equal(15, axisLimits.Top);
        }

        [Fact]
        public void Constructor_WithCoordinatesContainingIdenticalValues_SetsCorrectLimits()
        {
            var coordinates = new List<Coordinates>
        {
            new Coordinates(5, 10),
            new Coordinates(5, 10),
            new Coordinates(5, 10)
        };
            var axisLimits = new AxisLimits(coordinates);

            Assert.Equal(5, axisLimits.Left);
            Assert.Equal(5, axisLimits.Right);
            Assert.Equal(10, axisLimits.Bottom);
            Assert.Equal(10, axisLimits.Top);
        }

        [Fact]
        public void Constructor_WithValidCoordinates3d_SetsCorrectAxisLimits()
        {
            var coordinates = new List<Coordinates3d>
        {
            new Coordinates3d(0, 10, 5),
            new Coordinates3d(5, 15, 3),
            new Coordinates3d(10, 20, 8)
        };
            var axisLimits = new AxisLimits(coordinates);

            Assert.Equal(0, axisLimits.Left);
            Assert.Equal(10, axisLimits.Right);
            Assert.Equal(10, axisLimits.Bottom);
            Assert.Equal(20, axisLimits.Top);
        }

        [Fact]
        public void Constructor_WithSingleCoordinate3d_SetsLimitsToThatCoordinate()
        {
            var coordinates = new List<Coordinates3d>
        {
            new Coordinates3d(5, 10, 15)
        };
            var axisLimits = new AxisLimits(coordinates);

            Assert.Equal(5, axisLimits.Left);
            Assert.Equal(5, axisLimits.Right);
            Assert.Equal(10, axisLimits.Bottom);
            Assert.Equal(10, axisLimits.Top);
        }

        [Fact]
        public void Constructor_WithNegativeAndPositiveCoordinates3d_SetsCorrectLimits()
        {
            var coordinates = new List<Coordinates3d>
        {
            new Coordinates3d(-5, 10, 1),
            new Coordinates3d(0, -5, 2),
            new Coordinates3d(5, 15, 3)
        };
            var axisLimits = new AxisLimits(coordinates);

            Assert.Equal(-5, axisLimits.Left);
            Assert.Equal(5, axisLimits.Right);
            Assert.Equal(-5, axisLimits.Bottom);
            Assert.Equal(15, axisLimits.Top);
        }

        [Fact]
        public void Constructor_WithCoordinates3dContainingIdenticalValues_SetsCorrectLimits()
        {
            var coordinates = new List<Coordinates3d>
        {
            new Coordinates3d(5, 10, 1),
            new Coordinates3d(5, 10, 2),
            new Coordinates3d(5, 10, 3)
        };
            var axisLimits = new AxisLimits(coordinates);

            Assert.Equal(5, axisLimits.Left);
            Assert.Equal(5, axisLimits.Right);
            Assert.Equal(10, axisLimits.Bottom);
            Assert.Equal(10, axisLimits.Top);
        }

        [Fact]
        public void InvertedVertically_SwapsTopAndBottom()
        {
            var axisLimits = new AxisLimits(0, 10, 20, 30);

            var invertedLimits = axisLimits.InvertedVertically();

            Assert.Equal(0, invertedLimits.Left);
            Assert.Equal(10, invertedLimits.Right);
            Assert.Equal(30, invertedLimits.Bottom);
            Assert.Equal(20, invertedLimits.Top);
        }

        [Fact]
        public void InvertedHorizontally_SwapsLeftAndRight()
        {
            var axisLimits = new AxisLimits(0, 10, 20, 30);

            var invertedLimits = axisLimits.InvertedHorizontally();

            Assert.Equal(10, invertedLimits.Left);
            Assert.Equal(0, invertedLimits.Right);
            Assert.Equal(20, invertedLimits.Bottom);
            Assert.Equal(30, invertedLimits.Top);
        }

        [Fact]
        public void InvertedVertically_WithNegativeAndPositiveValues_SwapsTopAndBottomCorrectly()
        {
            var axisLimits = new AxisLimits(-5, 5, -10, 10);

            var invertedLimits = axisLimits.InvertedVertically();

            Assert.Equal(-5, invertedLimits.Left);
            Assert.Equal(5, invertedLimits.Right);
            Assert.Equal(10, invertedLimits.Bottom);
            Assert.Equal(-10, invertedLimits.Top);
        }

        [Fact]
        public void InvertedHorizontally_WithNegativeAndPositiveValues_SwapsLeftAndRightCorrectly()
        {
            var axisLimits = new AxisLimits(-5, 5, -10, 10);

            var invertedLimits = axisLimits.InvertedHorizontally();

            Assert.Equal(5, invertedLimits.Left);
            Assert.Equal(-5, invertedLimits.Right);
            Assert.Equal(-10, invertedLimits.Bottom);
            Assert.Equal(10, invertedLimits.Top);
        }

        [Fact]
        public void InvertedVertically_HandlesIdenticalTopAndBottomValues()
        {
            var axisLimits = new AxisLimits(0, 10, 10, 10);

            var invertedLimits = axisLimits.InvertedVertically();

            Assert.Equal(0, invertedLimits.Left);
            Assert.Equal(10, invertedLimits.Right);
            Assert.Equal(10, invertedLimits.Top);
            Assert.Equal(10, invertedLimits.Bottom);
        }

        [Fact]
        public void InvertedHorizontally_HandlesIdenticalLeftAndRightValues()
        {
            var axisLimits = new AxisLimits(0, 10, 0, 10);

            var invertedLimits = axisLimits.InvertedHorizontally();

            Assert.Equal(10, invertedLimits.Left);
            Assert.Equal(0, invertedLimits.Right);
            Assert.Equal(0, invertedLimits.Bottom);
            Assert.Equal(10, invertedLimits.Top);
        }


        [Fact]
        public void Constructor_WithValidCoordinates_SetsCorrectAxisLimits()
        {
            var coordinates = new Coordinates(5, 10);

            var axisLimits = new AxisLimits(coordinates);

            Assert.Equal(5, axisLimits.Left);
            Assert.Equal(5, axisLimits.Right);
            Assert.Equal(10, axisLimits.Bottom);
            Assert.Equal(10, axisLimits.Top);
        }

        [Fact]
        public void Constructor_WithNegativeCoordinates_SetsCorrectAxisLimits()
        {
            var coordinates = new Coordinates(-5, -10);

            var axisLimits = new AxisLimits(coordinates);

            Assert.Equal(-5, axisLimits.Left);
            Assert.Equal(-5, axisLimits.Right);
            Assert.Equal(-10, axisLimits.Bottom);
            Assert.Equal(-10, axisLimits.Top);
        }

        [Fact]
        public void Constructor_WithZeroCoordinates_SetsCorrectAxisLimits()
        {
            var coordinates = new Coordinates(0, 0);

            var axisLimits = new AxisLimits(coordinates);

            Assert.Equal(0, axisLimits.Left);
            Assert.Equal(0, axisLimits.Right);
            Assert.Equal(0, axisLimits.Bottom);
            Assert.Equal(0, axisLimits.Top);
        }

        [Fact]
        public void Constructor_WithLargeCoordinates_SetsCorrectAxisLimits()
        {
            var coordinates = new Coordinates(1000000, 5000000);

            var axisLimits = new AxisLimits(coordinates);

            Assert.Equal(1000000, axisLimits.Left);
            Assert.Equal(1000000, axisLimits.Right);
            Assert.Equal(5000000, axisLimits.Bottom);
            Assert.Equal(5000000, axisLimits.Top);
        }

        [Fact]
        public void Constructor_WithValidCoordinateRect_SetsCorrectAxisLimits()
        {
            var rect = new CoordinateRect(0, 10, 20, 30);
            var axisLimits = new AxisLimits(rect);
            Assert.Equal(0, axisLimits.Left);
            Assert.Equal(10, axisLimits.Right);
            Assert.Equal(20, axisLimits.Bottom);
            Assert.Equal(30, axisLimits.Top);
        }

        [Fact]
        public void Constructor_WithNegativeCoordinateRect_SetsCorrectAxisLimits()
        {
            var rect = new CoordinateRect(-5, 5, -10, 10);
            var axisLimits = new AxisLimits(rect);
            Assert.Equal(-5, axisLimits.Left);
            Assert.Equal(5, axisLimits.Right);
            Assert.Equal(-10, axisLimits.Bottom);
            Assert.Equal(10, axisLimits.Top);
        }

        [Fact]
        public void Constructor_WithZeroCoordinateRect_SetsCorrectAxisLimits()
        {
            var rect = new CoordinateRect(0, 0, 0, 0);
            var axisLimits = new AxisLimits(rect);
            Assert.Equal(0, axisLimits.Left);
            Assert.Equal(0, axisLimits.Right);
            Assert.Equal(0, axisLimits.Bottom);
            Assert.Equal(0, axisLimits.Top);
        }

        [Fact]
        public void Constructor_WithLargeCoordinateRect_SetsCorrectAxisLimits()
        {
            var rect = new CoordinateRect(1000000, 2000000, 3000000, 4000000);
            var axisLimits = new AxisLimits(rect);
            Assert.Equal(1000000, axisLimits.Left);
            Assert.Equal(2000000, axisLimits.Right);
            Assert.Equal(3000000, axisLimits.Bottom);
            Assert.Equal(4000000, axisLimits.Top);
        }

        [Fact]
        public void IsReal_WhenBothXAndYLimitsAreValid_ReturnsTrue()
        {
            var axisLimits = new AxisLimits(0, 10, 0, 20);
            Assert.True(axisLimits.IsReal);
        }

        [Fact]
        public void IsReal_WhenXLimitsAreInfinite_ReturnsFalse()
        {
            var axisLimits = new AxisLimits(double.NegativeInfinity, double.PositiveInfinity, 0, 10);
            Assert.False(axisLimits.IsReal);
        }

        [Fact]
        public void IsReal_WhenYLimitsAreInfinite_ReturnsFalse()
        {
            var axisLimits = new AxisLimits(0, 10, double.NegativeInfinity, double.PositiveInfinity);
            Assert.False(axisLimits.IsReal);
        }

        [Fact]
        public void IsReal_WhenBothXAndYLimitsAreInfinite_ReturnsFalse()
        {
            var axisLimits = new AxisLimits(double.NegativeInfinity, double.PositiveInfinity, double.NegativeInfinity, double.PositiveInfinity);
            Assert.False(axisLimits.IsReal);
        }

        [Fact]
        public void HasArea_WhenIsRealIsTrueAndHorizontalAndVerticalSpanAreNonZero_ReturnsTrue()
        {
            var axisLimits = new AxisLimits(0, 10, 0, 20);
            Assert.True(axisLimits.HasArea);
        }

        [Fact]
        public void HasArea_WhenIsRealIsTrueAndHorizontalSpanIsZero_ReturnsFalse()
        {
            var axisLimits = new AxisLimits(0, 0, 0, 20);
            Assert.False(axisLimits.HasArea);
        }

        [Fact]
        public void HasArea_WhenIsRealIsTrueAndVerticalSpanIsZero_ReturnsFalse()
        {
            var axisLimits = new AxisLimits(0, 10, 0, 0);
            Assert.False(axisLimits.HasArea);
        }

        [Fact]
        public void HasArea_WhenIsRealIsFalse_ReturnsFalse()
        {
            var axisLimits = new AxisLimits(double.NegativeInfinity, double.PositiveInfinity, 0, 20);
            Assert.False(axisLimits.HasArea);
        }

        [Fact]
        public void HorizontalCenter_WhenLeftAndRightAreValid_ReturnsCorrectValue()
        {
            var axisLimits = new AxisLimits(0, 10, 0, 20);
            Assert.Equal(5, axisLimits.HorizontalCenter);
        }

        [Fact]
        public void VerticalCenter_WhenTopAndBottomAreValid_ReturnsCorrectValue()
        {
            var axisLimits = new AxisLimits(0, 10, 0, 20);
            Assert.Equal(10, axisLimits.VerticalCenter);
        }

        [Fact]
        public void Center_WhenLeftRightTopBottomAreValid_ReturnsCorrectCoordinates()
        {
            var axisLimits = new AxisLimits(0, 10, 0, 20);
            var expectedCenter = new Coordinates(5, 10);
            Assert.Equal(expectedCenter, axisLimits.Center);
        }

        [Fact]
        public void HorizontalCenter_WhenNegativeLeftAndRight_ReturnsCorrectValue()
        {
            var axisLimits = new AxisLimits(-10, -2, 0, 20);
            Assert.Equal(-6, axisLimits.HorizontalCenter);
        }

        [Fact]
        public void VerticalCenter_WhenNegativeTopAndBottom_ReturnsCorrectValue()
        {
            var axisLimits = new AxisLimits(0, 10, -20, -10);
            Assert.Equal(-15, axisLimits.VerticalCenter);
        }

        [Fact]
        public void Center_WhenNegativeLeftRightTopBottom_ReturnsCorrectCoordinates()
        {
            var axisLimits = new AxisLimits(-10, -2, -20, -10);
            var expectedCenter = new Coordinates(-6, -15);
            Assert.Equal(expectedCenter, axisLimits.Center);
        }

        [Fact]
        public void HorizontalCenter_WhenLeftEqualsRight_ReturnsCorrectValue()
        {
            var axisLimits = new AxisLimits(5, 5, 0, 20);
            Assert.Equal(5, axisLimits.HorizontalCenter);
        }

        [Fact]
        public void VerticalCenter_WhenTopEqualsBottom_ReturnsCorrectValue()
        {
            var axisLimits = new AxisLimits(0, 10, 10, 10);
            Assert.Equal(10, axisLimits.VerticalCenter);
        }

        [Fact]
        public void XRange_WhenLeftAndRightAreValid_ReturnsCorrectRange()
        {
            var axisLimits = new AxisLimits(0, 10, 0, 20);
            var expectedRange = new CoordinateRange(0, 10);
            Assert.Equal(expectedRange, axisLimits.XRange);
        }

        [Fact]
        public void YRange_WhenBottomAndTopAreValid_ReturnsCorrectRange()
        {
            var axisLimits = new AxisLimits(0, 10, 0, 20);
            var expectedRange = new CoordinateRange(0, 20);
            Assert.Equal(expectedRange, axisLimits.YRange);
        }

        [Fact]
        public void XRange_WhenLeftIsNegativeAndRightIsPositive_ReturnsCorrectRange()
        {
            var axisLimits = new AxisLimits(-5, 5, 0, 10);
            var expectedRange = new CoordinateRange(-5, 5);
            Assert.Equal(expectedRange, axisLimits.XRange);
        }

        [Fact]
        public void YRange_WhenBottomIsNegativeAndTopIsPositive_ReturnsCorrectRange()
        {
            var axisLimits = new AxisLimits(0, 10, -10, 10);
            var expectedRange = new CoordinateRange(-10, 10);
            Assert.Equal(expectedRange, axisLimits.YRange);
        }

        [Fact]
        public void XRange_WhenLeftEqualsRight_ReturnsZeroRange()
        {
            var axisLimits = new AxisLimits(5, 5, 0, 10);
            var expectedRange = new CoordinateRange(5, 5);
            Assert.Equal(expectedRange, axisLimits.XRange);
        }

        [Fact]
        public void YRange_WhenBottomEqualsTop_ReturnsZeroRange()
        {
            var axisLimits = new AxisLimits(0, 10, 5, 5);
            var expectedRange = new CoordinateRange(5, 5);
            Assert.Equal(expectedRange, axisLimits.YRange);
        }

        [Fact]
        public void Rect_WhenLeftRightBottomTopAreValid_ReturnsCorrectRect()
        {
            var axisLimits = new AxisLimits(0, 10, 0, 20);
            var expectedRect = new CoordinateRect(0, 10, 0, 20);
            Assert.Equal(expectedRect, axisLimits.Rect);
        }

        [Fact]
        public void Rect_WhenLeftRightAreNegativeAndBottomTopArePositive_ReturnsCorrectRect()
        {
            var axisLimits = new AxisLimits(-5, 5, 0, 10);
            var expectedRect = new CoordinateRect(-5, 5, 0, 10);
            Assert.Equal(expectedRect, axisLimits.Rect);
        }

        [Fact]
        public void Default_HasCorrectLeftRightBottomTop()
        {
            var defaultLimits = AxisLimits.Default;
            Assert.Equal(-10, defaultLimits.Left);
            Assert.Equal(10, defaultLimits.Right);
            Assert.Equal(-10, defaultLimits.Bottom);
            Assert.Equal(10, defaultLimits.Top);
        }

        [Fact]
        public void ExpandedToInclude_WhenOtherLimitsAreWithin_ReturnsCorrectExpandedLimits()
        {
            var axisLimits1 = new AxisLimits(0, 10, 0, 20);
            var axisLimits2 = new AxisLimits(5, 15, 5, 25);
            var expandedLimits = axisLimits1.ExpandedToInclude(axisLimits2);
            var expectedLimits = new AxisLimits(0, 15, 0, 25);
            Assert.Equal(expectedLimits, expandedLimits);
        }

        [Fact]
        public void ExpandedToInclude_WhenOtherLimitsAreLarger_ReturnsCorrectExpandedLimits()
        {
            var axisLimits1 = new AxisLimits(0, 10, 0, 20);
            var axisLimits2 = new AxisLimits(15, 25, 15, 30);
            var expandedLimits = axisLimits1.ExpandedToInclude(axisLimits2);
            var expectedLimits = new AxisLimits(0, 25, 0, 30);
            Assert.Equal(expectedLimits, expandedLimits);
        }

        [Fact]
        public void ExpandedToInclude_WhenOtherLimitsAreSmaller_ReturnsCorrectExpandedLimits()
        {
            var axisLimits1 = new AxisLimits(0, 10, 0, 20);
            var axisLimits2 = new AxisLimits(-5, 5, -5, 5);
            var expandedLimits = axisLimits1.ExpandedToInclude(axisLimits2);
            var expectedLimits = new AxisLimits(-5, 10, -5, 20);
            Assert.Equal(expectedLimits, expandedLimits);
        }

        [Fact]
        public void ExpandedToInclude_WhenOtherLimitsAreEqual_ReturnsSameLimits()
        {
            var axisLimits1 = new AxisLimits(0, 10, 0, 20);
            var axisLimits2 = new AxisLimits(0, 10, 0, 20);
            var expandedLimits = axisLimits1.ExpandedToInclude(axisLimits2);
            Assert.Equal(axisLimits1, expandedLimits);
        }


        [Fact]
        public void FromPoint_WithValidCoordinates_ReturnsCorrectAxisLimits()
        {
            var axisLimits = AxisLimits.FromPoint(5, 10);
            var expectedLimits = new AxisLimits(5, 5, 10, 10);
            Assert.Equal(expectedLimits, axisLimits);
        }

        [Fact]
        public void FromPoint_WithNegativeCoordinates_ReturnsCorrectAxisLimits()
        {
            var axisLimits = AxisLimits.FromPoint(-5, -10);
            var expectedLimits = new AxisLimits(-5, -5, -10, -10);
            Assert.Equal(expectedLimits, axisLimits);
        }

        [Fact]
        public void FromPoint_WithZeroCoordinates_ReturnsCorrectAxisLimits()
        {
            var axisLimits = AxisLimits.FromPoint(0, 0);
            var expectedLimits = new AxisLimits(0, 0, 0, 0);
            Assert.Equal(expectedLimits, axisLimits);
        }

        [Fact]
        public void FromPoint_WithValidCoordinatesObject_ReturnsCorrectAxisLimits()
        {
            var coordinates = new Coordinates(5, 10);
            var axisLimits = AxisLimits.FromPoint(coordinates);
            var expectedLimits = new AxisLimits(5, 5, 10, 10);
            Assert.Equal(expectedLimits, axisLimits);
        }

        [Fact]
        public void FromPoint_WithNegativeCoordinatesObject_ReturnsCorrectAxisLimits()
        {
            var coordinates = new Coordinates(-5, -10);
            var axisLimits = AxisLimits.FromPoint(coordinates);
            var expectedLimits = new AxisLimits(-5, -5, -10, -10);
            Assert.Equal(expectedLimits, axisLimits);
        }

        [Fact]
        public void FromPoint_WithZeroCoordinatesObject_ReturnsCorrectAxisLimits()
        {
            var coordinates = new Coordinates(0, 0);
            var axisLimits = AxisLimits.FromPoint(coordinates);
            var expectedLimits = new AxisLimits(0, 0, 0, 0);
            Assert.Equal(expectedLimits, axisLimits);
        }

        [Fact]
        public void ToString_WhenLimitsAreValid_ReturnsCorrectString()
        {
            var axisLimits = new AxisLimits(0, 10, 0, 20);
            var expectedString = "AxisLimits: X=[0, 10], Y=[0, 20]";
            Assert.Equal(expectedString, axisLimits.ToString());
        }

        [Fact]
        public void ToString_WhenLimitsAreNegative_ReturnsCorrectString()
        {
            var axisLimits = new AxisLimits(-5, 5, -10, 10);
            var expectedString = "AxisLimits: X=[-5, 5], Y=[-10, 10]";
            Assert.Equal(expectedString, axisLimits.ToString());
        }

        [Fact]
        public void ToString_WhenLimitsAreZero_ReturnsCorrectString()
        {
            var axisLimits = new AxisLimits(0, 0, 0, 0);
            var expectedString = "AxisLimits: X=[0, 0], Y=[0, 0]";
            Assert.Equal(expectedString, axisLimits.ToString());
        }

        [Fact]
        public void ToString_WithDigits_ReturnsCorrectString()
        {
            var axisLimits = new AxisLimits(0, 10.12345, 0, 20.6789);
            var expectedString = "AxisLimits: X=[0, 10,12], Y=[0, 20,68]";
            Assert.Equal(expectedString, axisLimits.ToString(2));
        }

        [Fact]
        public void ToString_WithMoreDigits_ReturnsCorrectString()
        {
            var axisLimits = new AxisLimits(0, 10.12345, 0, 20.6789);
            var expectedString = "AxisLimits: X=[0, 10,12345], Y=[0, 20,6789]";
            Assert.Equal(expectedString, axisLimits.ToString(5));
        }

        [Fact]
        public void ToString_WithZeroDigits_ReturnsCorrectString()
        {
            var axisLimits = new AxisLimits(0, 10.12345, 0, 20.6789);
            var expectedString = "AxisLimits: X=[0, 10], Y=[0, 21]";
            Assert.Equal(expectedString, axisLimits.ToString(0));
        }

        [Fact]
        public void ToString_WithNegativeValues_ReturnsCorrectString()
        {
            var axisLimits = new AxisLimits(-5.98765, 5.4321, -10.12345, 10.6789);
            var expectedString = "AxisLimits: X=[-5,99, 5,43], Y=[-10,12, 10,68]";
            Assert.Equal(expectedString, axisLimits.ToString(2));
        }

        [Fact]
        public void NoLimits_ReturnsCorrectAxisLimits()
        {
            var noLimits = AxisLimits.NoLimits;
            var expectedLimits = new AxisLimits(double.NaN, double.NaN, double.NaN, double.NaN);
            Assert.Equal(expectedLimits, noLimits);
        }

        [Fact]
        public void Unset_ReturnsCorrectAxisLimits()
        {
            var unsetLimits = AxisLimits.Unset;
            var expectedLimits = new AxisLimits(double.PositiveInfinity, double.NegativeInfinity, double.PositiveInfinity, double.NegativeInfinity);
            Assert.Equal(expectedLimits, unsetLimits);
        }

        [Fact]
        public void VerticalOnly_WithValidYMinYMax_ReturnsCorrectAxisLimits()
        {
            var axisLimits = AxisLimits.VerticalOnly(0, 20);
            var expectedLimits = new AxisLimits(double.NaN, double.NaN, 0, 20);
            Assert.Equal(expectedLimits, axisLimits);
        }

        [Fact]
        public void VerticalOnly_WithNegativeYMinYMax_ReturnsCorrectAxisLimits()
        {
            var axisLimits = AxisLimits.VerticalOnly(-10, 10);
            var expectedLimits = new AxisLimits(double.NaN, double.NaN, -10, 10);
            Assert.Equal(expectedLimits, axisLimits);
        }

        [Fact]
        public void VerticalOnly_WithZeroYMinYMax_ReturnsCorrectAxisLimits()
        {
            var axisLimits = AxisLimits.VerticalOnly(0, 0);
            var expectedLimits = new AxisLimits(double.NaN, double.NaN, 0, 0);
            Assert.Equal(expectedLimits, axisLimits);
        }

        [Fact]
        public void HorizontalOnly_WithValidXMinXMax_ReturnsCorrectAxisLimits()
        {
            var axisLimits = AxisLimits.HorizontalOnly(0, 20);
            var expectedLimits = new AxisLimits(0, 20, double.NaN, double.NaN);
            Assert.Equal(expectedLimits, axisLimits);
        }

        [Fact]
        public void HorizontalOnly_WithNegativeXMinXMax_ReturnsCorrectAxisLimits()
        {
            var axisLimits = AxisLimits.HorizontalOnly(-10, 10);
            var expectedLimits = new AxisLimits(-10, 10, double.NaN, double.NaN);
            Assert.Equal(expectedLimits, axisLimits);
        }

        [Fact]
        public void HorizontalOnly_WithZeroXMinXMax_ReturnsCorrectAxisLimits()
        {
            var axisLimits = AxisLimits.HorizontalOnly(0, 0);
            var expectedLimits = new AxisLimits(0, 0, double.NaN, double.NaN);
            Assert.Equal(expectedLimits, axisLimits);
        }

        [Fact]
        public void Expanded_WithValidXAndY_ReturnsCorrectExpandedLimits()
        {
            var axisLimits = new AxisLimits(0, 10, 0, 20);
            var expandedLimits = axisLimits.Expanded(5, 15);
            var expectedLimits = new AxisLimits(0, 10, 0, 20);
            Assert.Equal(expectedLimits, expandedLimits);
        }

        [Fact]
        public void Expanded_WithXSmallerThanLeftAndYLargerThanTop_ReturnsCorrectExpandedLimits()
        {
            var axisLimits = new AxisLimits(0, 10, 0, 20);
            var expandedLimits = axisLimits.Expanded(-5, 25);
            var expectedLimits = new AxisLimits(-5, 10, 0, 25);
            Assert.Equal(expectedLimits, expandedLimits);
        }

        [Fact]
        public void Expanded_WithNaNLeftAndTop_ReturnsCorrectExpandedLimits()
        {
            var axisLimits = new AxisLimits(double.NaN, 10, double.NaN, 20);
            var expandedLimits = axisLimits.Expanded(5, 15);
            var expectedLimits = new AxisLimits(5, 10, 15, 20);
            Assert.Equal(expectedLimits, expandedLimits);
        }

        [Fact]
        public void Expanded_WithNaNRightAndBottom_ReturnsCorrectExpandedLimits()
        {
            var axisLimits = new AxisLimits(0, double.NaN, 0, double.NaN);
            var expandedLimits = axisLimits.Expanded(5, 15);
            var expectedLimits = new AxisLimits(0, 5, 0, 15);
            Assert.Equal(expectedLimits, expandedLimits);
        }

        [Fact]
        public void Expanded_WithAlreadyExpandedLimits_ReturnsSameLimits()
        {
            var axisLimits = new AxisLimits(-5, 15, -5, 25);
            var expandedLimits = axisLimits.Expanded(0, 20);
            var expectedLimits = new AxisLimits(-5, 15, -5, 25);
            Assert.Equal(expectedLimits, expandedLimits);
        }

        [Fact]
        public void Expanded_WithValidCoordinates_ReturnsCorrectExpandedLimits()
        {
            var axisLimits = new AxisLimits(0, 10, 0, 20);
            var coordinates = new Coordinates(5, 15);
            var expandedLimits = axisLimits.Expanded(coordinates);
            var expectedLimits = new AxisLimits(0, 10, 0, 20);
            Assert.Equal(expectedLimits, expandedLimits);
        }

        [Fact]
        public void Expanded_WithCoordinatesSmallerThanLimits_ReturnsCorrectExpandedLimits()
        {
            var axisLimits = new AxisLimits(0, 10, 0, 20);
            var coordinates = new Coordinates(-5, 25);
            var expandedLimits = axisLimits.Expanded(coordinates);
            var expectedLimits = new AxisLimits(-5, 10, 0, 25);
            Assert.Equal(expectedLimits, expandedLimits);
        }

        [Fact]
        public void Expanded_WithNaNInLimits_ReturnsCorrectExpandedLimits()
        {
            var axisLimits = new AxisLimits(double.NaN, 10, double.NaN, 20);
            var coordinates = new Coordinates(5, 15);
            var expandedLimits = axisLimits.Expanded(coordinates);
            var expectedLimits = new AxisLimits(5, 10, 15, 20);
            Assert.Equal(expectedLimits, expandedLimits);
        }

        [Fact]
        public void Expanded_WithCoordinatesWithinLimits_ReturnsSameLimits()
        {
            var axisLimits = new AxisLimits(0, 10, 0, 20);
            var coordinates = new Coordinates(5, 15);
            var expandedLimits = axisLimits.Expanded(coordinates);
            var expectedLimits = new AxisLimits(0, 10, 0, 20);
            Assert.Equal(expectedLimits, expandedLimits);
        }

        [Fact]
        public void Expanded_WithCoordinatesOutsideLimits_ReturnsExpandedLimits()
        {
            var axisLimits = new AxisLimits(0, 10, 0, 20);
            var coordinates = new Coordinates(-5, 25);
            var expandedLimits = axisLimits.Expanded(coordinates);
            var expectedLimits = new AxisLimits(-5, 10, 0, 25);
            Assert.Equal(expectedLimits, expandedLimits);
        }

        [Fact]
        public void Expanded_WithValidCoordinateRect_ReturnsCorrectExpandedLimits()
        {
            var axisLimits = new AxisLimits(0, 10, 0, 20);
            var rect = new CoordinateRect(5, 5, 15, 25);
            var expandedLimits = axisLimits.Expanded(rect);
            var expectedLimits = new AxisLimits(0, 10, 0, 25);
            Assert.Equal(expectedLimits, expandedLimits);
        }

        [Fact]
        public void Expanded_WithRectSmallerThanLimits_ReturnsSameLimits()
        {
            var axisLimits = new AxisLimits(0, 10, 0, 20);
            var rect = new CoordinateRect(2, 2, 8, 18);
            var expandedLimits = axisLimits.Expanded(rect);
            var expectedLimits = new AxisLimits(0, 10, 0, 20);
            Assert.Equal(expectedLimits, expandedLimits);
        }

        [Fact]
        public void Expanded_WithRectLargerThanLimits_ReturnsCorrectExpandedLimits()
        {
            var axisLimits = new AxisLimits(0, 10, 0, 20);
            var rect = new CoordinateRect(-5, -5, 15, 25);
            var expandedLimits = axisLimits.Expanded(rect);
            var expectedLimits = new AxisLimits(-5, 10, 0, 25);
            Assert.Equal(expectedLimits, expandedLimits);
        }

        [Fact]
        public void Expanded_WithRectContainingNaNValues_ReturnsCorrectExpandedLimits()
        {
            var axisLimits = new AxisLimits(0, 10, 0, 20);
            var rect = new CoordinateRect(double.NaN, double.NaN, 10, 20);
            var expandedLimits = axisLimits.Expanded(rect);
            var expectedLimits = new AxisLimits(double.NaN, double.NaN, 0, 20);
            Assert.Equal(expectedLimits, expandedLimits);
        }

        [Fact]
        public void Expanded_WithRectWithinLimits_ReturnsSameLimits()
        {
            var axisLimits = new AxisLimits(0, 10, 0, 20);
            var rect = new CoordinateRect(1, 1, 9, 19);
            var expandedLimits = axisLimits.Expanded(rect);
            var expectedLimits = new AxisLimits(0, 10, 0, 20);
            Assert.Equal(expectedLimits, expandedLimits);
        }

        [Fact]
        public void Expanded_WithValidAxisLimits_ReturnsCorrectExpandedLimits()
        {
            var axisLimits = new AxisLimits(0, 10, 0, 20);
            var otherLimits = new AxisLimits(5, 15, 5, 25);
            var expandedLimits = axisLimits.Expanded(otherLimits);
            var expectedLimits = new AxisLimits(0, 15, 0, 25);
            Assert.Equal(expectedLimits, expandedLimits);
        }

        [Fact]
        public void Expanded_WithAxisLimitsSmallerThanCurrentLimits_ReturnsSameLimits()
        {
            var axisLimits = new AxisLimits(0, 10, 0, 20);
            var otherLimits = new AxisLimits(3, 8, 5, 15);
            var expandedLimits = axisLimits.Expanded(otherLimits);
            var expectedLimits = new AxisLimits(0, 10, 0, 20);
            Assert.Equal(expectedLimits, expandedLimits);
        }

        [Fact]
        public void Expanded_WithAxisLimitsLargerThanCurrentLimits_ReturnsCorrectExpandedLimits()
        {
            var axisLimits = new AxisLimits(0, 10, 0, 20);
            var otherLimits = new AxisLimits(-5, 15, -5, 25);
            var expandedLimits = axisLimits.Expanded(otherLimits);
            var expectedLimits = new AxisLimits(-5, 15, -5, 25);
            Assert.Equal(expectedLimits, expandedLimits);
        }

        [Fact]
        public void Expanded_WithAxisLimitsThatContainNaN_ReturnsCorrectExpandedLimits()
        {
            var axisLimits = new AxisLimits(0, 10, 0, 20);
            var otherLimits = new AxisLimits(double.NaN, 5, double.NaN, 15);
            var expandedLimits = axisLimits.Expanded(otherLimits);
            var expectedLimits = new AxisLimits(0, 10, 0, 20);
            Assert.Equal(expectedLimits, expandedLimits);
        }

        [Fact]
        public void Expanded_WithEqualLimits_ReturnsSameLimits()
        {
            var axisLimits = new AxisLimits(0, 10, 0, 20);
            var otherLimits = new AxisLimits(0, 10, 0, 20);
            var expandedLimits = axisLimits.Expanded(otherLimits);
            var expectedLimits = new AxisLimits(0, 10, 0, 20);
            Assert.Equal(expectedLimits, expandedLimits);
        }

        [Fact]
        public void WithPan_WithValidDeltaValues_ReturnsCorrectPanLimits()
        {
            var axisLimits = new AxisLimits(0, 10, 0, 20);
            var panLimits = axisLimits.WithPan(5, 5);
            var expectedLimits = new AxisLimits(5, 15, 5, 25);
            Assert.Equal(expectedLimits, panLimits);
        }

        [Fact]
        public void WithPan_WithNegativeDeltaValues_ReturnsCorrectPanLimits()
        {
            var axisLimits = new AxisLimits(0, 10, 0, 20);
            var panLimits = axisLimits.WithPan(-5, -5);
            var expectedLimits = new AxisLimits(-5, 5, -5, 15);
            Assert.Equal(expectedLimits, panLimits);
        }

        [Fact]
        public void WithPan_WithZeroDeltaValues_ReturnsSameLimits()
        {
            var axisLimits = new AxisLimits(0, 10, 0, 20);
            var panLimits = axisLimits.WithPan(0, 0);
            var expectedLimits = new AxisLimits(0, 10, 0, 20);
            Assert.Equal(expectedLimits, panLimits);
        }

        [Fact]
        public void WithPan_WithLargeDeltaValues_ReturnsCorrectPanLimits()
        {
            var axisLimits = new AxisLimits(0, 10, 0, 20);
            var panLimits = axisLimits.WithPan(100, -50);
            var expectedLimits = new AxisLimits(100, 110, -50, -30);
            Assert.Equal(expectedLimits, panLimits);
        }

        [Fact]
        public void WithPan_WithDeltaValuesOnNegativeLimits_ReturnsCorrectPanLimits()
        {
            var axisLimits = new AxisLimits(-10, 0, -20, 0);
            var panLimits = axisLimits.WithPan(5, 5);
            var expectedLimits = new AxisLimits(-5, 5, -15, 5);
            Assert.Equal(expectedLimits, panLimits);
        }

        [Fact]
        public void WithZoom_WithFractionGreaterThanOne_ReturnsExpandedLimits()
        {
            var axisLimits = new AxisLimits(0, 10, 0, 20);
            var zoomedLimits = axisLimits.WithZoom(2, 2, 5, 10);
            var expectedLimits = new AxisLimits(2.5, 7.5, 5, 15);
            Assert.Equal(expectedLimits, zoomedLimits);
        }

        [Fact]
        public void WithZoom_WithFractionSmallerThanOne_ReturnsContractedLimits()
        {
            var axisLimits = new AxisLimits(0, 10, 0, 20);
            var zoomedLimits = axisLimits.WithZoom(0.5, 0.5, 5, 10);
            var expectedLimits = new AxisLimits(-5, 15, -10, 30);
            Assert.Equal(expectedLimits, zoomedLimits);
        }

        [Fact]
        public void WithZoom_WithNegativeFractions_ReturnsInvertedLimits()
        {
            var axisLimits = new AxisLimits(0, 10, 0, 20);
            var zoomedLimits = axisLimits.WithZoom(-0.5, -0.5, 5, 10);
            var expectedLimits = new AxisLimits(15, -5, 30, -10);
            Assert.Equal(expectedLimits, zoomedLimits);
        }

        [Fact]
        public void Contains_WithPointInside_ReturnsTrue()
        {
            var axisLimits = new AxisLimits(0, 10, 0, 20);
            bool contains = axisLimits.Contains(5, 10);
            Assert.True(contains);
        }

        [Fact]
        public void Contains_WithPointOutside_ReturnsFalse()
        {
            var axisLimits = new AxisLimits(0, 10, 0, 20);
            bool contains = axisLimits.Contains(15, 25);
            Assert.False(contains);
        }

        [Fact]
        public void Contains_WithPointOnEdge_ReturnsTrue()
        {
            var axisLimits = new AxisLimits(0, 10, 0, 20);
            bool contains = axisLimits.Contains(0, 10);
            Assert.True(contains);
        }

        [Fact]
        public void Contains_WithCoordinatesInside_ReturnsTrue()
        {
            var axisLimits = new AxisLimits(0, 10, 0, 20);
            var point = new Coordinates(5, 10);
            bool contains = axisLimits.Contains(point);
            Assert.True(contains);
        }

        [Fact]
        public void Contains_WithCoordinatesOutside_ReturnsFalse()
        {
            var axisLimits = new AxisLimits(0, 10, 0, 20);
            var point = new Coordinates(15, 25);
            bool contains = axisLimits.Contains(point);
            Assert.False(contains);
        }

        [Fact]
        public void Contains_WithCoordinatesOnEdge_ReturnsTrue()
        {
            var axisLimits = new AxisLimits(0, 10, 0, 20);
            var point = new Coordinates(0, 10);
            bool contains = axisLimits.Contains(point);
            Assert.True(contains);
        }

        [Fact]
        public void WithZoom_WithValidFractionValues_ReturnsCorrectZoomedLimits()
        {
            var axisLimits = new AxisLimits(0, 10, 0, 20);
            var zoomedLimits = axisLimits.WithZoom(0.5, 0.5);
            var expectedLimits = new AxisLimits(-5, 15, -10, 30);
            Assert.Equal(expectedLimits, zoomedLimits);
        }

        [Fact]
        public void WithZoom_WithFractionGreaterThanOne_ReturnsExpandedLimits2()
        {
            var axisLimits = new AxisLimits(0, 10, 0, 20);
            var zoomedLimits = axisLimits.WithZoom(2, 2);
            var expectedLimits = new AxisLimits(2.5, 7.5, 5, 15);
            Assert.Equal(expectedLimits, zoomedLimits);
        }

        [Fact]
        public void WithZoom_WithFractionSmallerThanOne_ReturnsContractedLimits2()
        {
            var axisLimits = new AxisLimits(0, 10, 0, 20);
            var zoomedLimits = axisLimits.WithZoom(0.5, 0.5);
            var expectedLimits = new AxisLimits(-5, 15, -10, 30);
            Assert.Equal(expectedLimits, zoomedLimits);
        }

        [Fact]
        public void WithZoom_WithNegativeFractions_ReturnsInvertedLimits2()
        {
            var axisLimits = new AxisLimits(0, 10, 0, 20);
            var zoomedLimits = axisLimits.WithZoom(-0.5, -0.5);
            var expectedLimits = new AxisLimits(15, -5, 30, -10);
            Assert.Equal(expectedLimits, zoomedLimits);
        }

        [Fact]
        public void Equals_WithSameLimits_ReturnsTrue()
        {
            var axisLimits1 = new AxisLimits(0, 10, 0, 20);
            var axisLimits2 = new AxisLimits(0, 10, 0, 20);
            Assert.True(axisLimits1.Equals(axisLimits2));
        }

        [Fact]
        public void Equals_WithDifferentLimits_ReturnsFalse()
        {
            var axisLimits1 = new AxisLimits(0, 10, 0, 20);
            var axisLimits2 = new AxisLimits(5, 15, 5, 25);
            Assert.False(axisLimits1.Equals(axisLimits2));
        }

        [Fact]
        public void Equals_WithSameLimitsObject_ReturnsTrue()
        {
            var axisLimits1 = new AxisLimits(0, 10, 0, 20);
            object axisLimits2 = new AxisLimits(0, 10, 0, 20);
            Assert.True(axisLimits1.Equals(axisLimits2));
        }

        [Fact]
        public void Equals_WithDifferentLimitsObject_ReturnsFalse()
        {
            var axisLimits1 = new AxisLimits(0, 10, 0, 20);
            object axisLimits2 = new AxisLimits(5, 15, 5, 25);
            Assert.False(axisLimits1.Equals(axisLimits2));
        }

        [Fact]
        public void Equals_WithNullObject_ReturnsFalse()
        {
            var axisLimits = new AxisLimits(0, 10, 0, 20);
            Assert.False(axisLimits.Equals(null));
        }

        [Fact]
        public void Equals_WithDifferentObjectType_ReturnsFalse()
        {
            var axisLimits = new AxisLimits(0, 10, 0, 20);
            var nonAxisLimitsObject = new object();
            Assert.False(axisLimits.Equals(nonAxisLimitsObject));
        }

        [Fact]
        public void Equals_WithIdenticalReference_ReturnsTrue()
        {
            var axisLimits = new AxisLimits(0, 10, 0, 20);
            var sameReference = axisLimits;
            Assert.True(axisLimits.Equals(sameReference));
        }

        [Fact]
        public void OperatorEquals_WithSameLimits_ReturnsTrue()
        {
            var axisLimits1 = new AxisLimits(0, 10, 0, 20);
            var axisLimits2 = new AxisLimits(0, 10, 0, 20);
            Assert.True(axisLimits1 == axisLimits2);
        }

        [Fact]
        public void OperatorEquals_WithDifferentLimits_ReturnsFalse()
        {
            var axisLimits1 = new AxisLimits(0, 10, 0, 20);
            var axisLimits2 = new AxisLimits(5, 15, 5, 25);
            Assert.False(axisLimits1 == axisLimits2);
        }

        [Fact]
        public void OperatorEquals_WithNull_ReturnsFalse()
        {
            var axisLimits = new AxisLimits(0, 10, 0, 20);
            AxisLimits? nullLimits = null;
            Assert.False(axisLimits == nullLimits);
        }

        [Fact]
        public void OperatorEquals_WithIdenticalReference_ReturnsTrue()
        {
            var axisLimits = new AxisLimits(0, 10, 0, 20);
            var sameReference = axisLimits;
            Assert.True(axisLimits == sameReference);
        }

        [Fact]
        public void OperatorNotEquals_WithSameLimits_ReturnsFalse()
        {
            var axisLimits1 = new AxisLimits(0, 10, 0, 20);
            var axisLimits2 = new AxisLimits(0, 10, 0, 20);
            Assert.False(axisLimits1 != axisLimits2);
        }

        [Fact]
        public void OperatorNotEquals_WithDifferentLimits_ReturnsTrue()
        {
            var axisLimits1 = new AxisLimits(0, 10, 0, 20);
            var axisLimits2 = new AxisLimits(5, 15, 5, 25);
            Assert.True(axisLimits1 != axisLimits2);
        }

        [Fact]
        public void OperatorNotEquals_WithNull_ReturnsTrue()
        {
            var axisLimits = new AxisLimits(0, 10, 0, 20);
            AxisLimits? nullLimits = null;
            Assert.True(axisLimits != nullLimits);
        }

        [Fact]
        public void OperatorNotEquals_WithIdenticalReference_ReturnsFalse()
        {
            var axisLimits = new AxisLimits(0, 10, 0, 20);
            var sameReference = axisLimits;
            Assert.False(axisLimits != sameReference);
        }

        [Fact]
        public void GetHashCode_WithSameLimits_ReturnsSameHashCode()
        {
            var axisLimits1 = new AxisLimits(0, 10, 0, 20);
            var axisLimits2 = new AxisLimits(0, 10, 0, 20);
            Assert.Equal(axisLimits1.GetHashCode(), axisLimits2.GetHashCode());
        }

        [Fact]
        public void GetHashCode_WithDifferentLimits_ReturnsDifferentHashCode()
        {
            var axisLimits1 = new AxisLimits(0, 10, 0, 20);
            var axisLimits2 = new AxisLimits(5, 15, 5, 25);
            Assert.NotEqual(axisLimits1.GetHashCode(), axisLimits2.GetHashCode());
        }

        [Fact]
        public void GetHashCode_WithIdenticalReference_ReturnsSameHashCode()
        {
            var axisLimits = new AxisLimits(0, 10, 0, 20);
            var sameReference = axisLimits;
            Assert.Equal(axisLimits.GetHashCode(), sameReference.GetHashCode());
        }
    }
}

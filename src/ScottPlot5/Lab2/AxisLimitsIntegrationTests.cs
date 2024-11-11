using ScottPlot;

namespace Lab2
{
    public class AxisLimitsIntegrationTests
    {
        [Fact]
        public void SetAxisLimits_AppliesLimitsCorrectly()
        {
            ScottPlot.Plot myPlot = new();

            var axisLimits = new AxisLimits(0, 10, 0, 20);

            myPlot.Axes.SetLimitsX(axisLimits);
            myPlot.Axes.SetLimitsY(axisLimits);

            Assert.Equal(axisLimits.Left, myPlot.Axes.Bottom.Min);
            Assert.Equal(axisLimits.Right, myPlot.Axes.Bottom.Max);
            Assert.Equal(axisLimits.Bottom, myPlot.Axes.Left.Min);
            Assert.Equal(axisLimits.Top, myPlot.Axes.Left.Max);
        }

        [Fact]
        public void WithZoom_ChangesAxisLimitsCorrectly()
        {
            ScottPlot.Plot myPlot = new();

            var axisLimits = new AxisLimits(0, 10, 0, 20);
            myPlot.Axes.SetLimitsX(axisLimits);
            myPlot.Axes.SetLimitsY(axisLimits);

            var zoomedLimits = axisLimits.WithZoom(0.5, 0.5);
            myPlot.Axes.SetLimitsX(zoomedLimits);
            myPlot.Axes.SetLimitsY(zoomedLimits);

            Assert.Equal(zoomedLimits.Left, myPlot.Axes.Bottom.Min);
            Assert.Equal(zoomedLimits.Right, myPlot.Axes.Bottom.Max);
            Assert.Equal(zoomedLimits.Bottom, myPlot.Axes.Left.Min);
            Assert.Equal(zoomedLimits.Top, myPlot.Axes.Left.Max);
        }

        [Fact]
        public void WithPan_ChangesAxisLimitsCorrectly()
        {
            ScottPlot.Plot myPlot = new();

            var axisLimits = new AxisLimits(0, 10, 0, 20);
            myPlot.Axes.SetLimitsX(axisLimits);
            myPlot.Axes.SetLimitsY(axisLimits);

            var pannedLimits = axisLimits.WithPan(5, 5);
            myPlot.Axes.SetLimitsX(pannedLimits);
            myPlot.Axes.SetLimitsY(pannedLimits);

            Assert.Equal(pannedLimits.Left, myPlot.Axes.Bottom.Min);
            Assert.Equal(pannedLimits.Right, myPlot.Axes.Bottom.Max);
            Assert.Equal(pannedLimits.Bottom, myPlot.Axes.Left.Min);
            Assert.Equal(pannedLimits.Top, myPlot.Axes.Left.Max);
        }
    }
}

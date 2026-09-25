using System.Drawing;
using GPC.Utilities.Graphics;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GPC.Utilities.UnitTest
{
    [TestClass]
    public class GraphicCreatorTest
    {
        private static GraphicCreator CreateGraphic(double scale)
        {
            var graphic = new GraphicCreator(400, 300) { Title = "Test" };
            graphic.AddCurve("curve", Color.Red);
            for (int i = 0; i <= 10; i++)
            {
                graphic.AddCurveValue("curve", i, (i - 3) * scale);
                graphic.AddMaximumValues(i, -5 * scale, (i + 1) * scale);
            }
            return graphic;
        }

        private static bool AreEqual(Bitmap a, Bitmap b)
        {
            if (a.Width != b.Width || a.Height != b.Height)
                return false;
            for (int y = 0; y < a.Height; y++)
                for (int x = 0; x < a.Width; x++)
                    if (a.GetPixel(x, y) != b.GetPixel(x, y))
                        return false;
            return true;
        }

        [TestMethod]
        public void CreateTwiceGivesSameImage()
        {
            using (var graphic = CreateGraphic(10))
            using (Bitmap first = graphic.Create(true))
            using (Bitmap second = graphic.Create(true))
            {
                Assert.IsNotNull(first);
                Assert.IsNotNull(second);
                Assert.IsTrue(AreEqual(first, second), "The margins must not grow at every call");
            }
        }

        [TestMethod]
        public void SmallValuesAndEmptyGraphic()
        {
            using (var graphic = CreateGraphic(0.01))
            using (Bitmap image = graphic.Create(true))
                Assert.IsNotNull(image);

            using (var empty = new GraphicCreator(400, 300))
            using (Bitmap image = empty.Create(false))
                Assert.IsNotNull(image);
        }
    }
}

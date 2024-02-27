using NUnit.Framework;
using Paints.PaintItems;

namespace Tests.EditMode.Paints.PaintItems
{
    public class PaintDataEditorTest
    {
        [Test]
        public void TestNoPaintDataAssets()
        {
            // Arrange
            PaintData[] paints = new PaintData[0];

            // Act
            PaintDataEditor.EnsureUniqueIDs();

            // Assert
            // No errors should be thrown
        }
        
        [Test]
        public void TestSinglePaintDataAsset()
        {
            // Arrange
            PaintData paint = new PaintData();
            paint.PaintItem = new PaintItem();

            PaintData[] paints = new PaintData[] { paint };

            // Act
            PaintDataEditor.EnsureUniqueIDs();

            // Assert
            Assert.AreEqual(0, paint.PaintItem.ID);
        }
        
        [Test]
        public void TestNullPaintDataAssets()
        {
            // Arrange
            PaintData[] paints = null;

            // Act
            PaintDataEditor.EnsureUniqueIDs();

            // Assert
            // No errors should be thrown
        }
        
    }
}

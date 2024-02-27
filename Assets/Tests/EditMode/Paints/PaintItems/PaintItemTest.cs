using NUnit.Framework;
using Paints;
using Paints.PaintItems;
using UnityEngine;

namespace Tests.EditMode.Paints.PaintItems
{
    public class PaintItemTest
    {
        [Test]
        public void PaintItem_CanBeCreatedAndFieldsCanBeSet()
        {
            // Arrange
            var expectedId = 1;
            var expectedName = "Test Paint";
            var expectedBrand = new PaintBrand(); // Assuming PaintBrand is a class with a parameterless constructor
            var expectedSprite = Sprite.Create(null,Rect.zero, Vector2.one); // Assuming Sprite is a class with a parameterless constructor
            var expectedColor = Color.red;

            // Act
            var paintItem = new PaintItem
            {
                ID = expectedId,
                Name = expectedName,
                Brand = expectedBrand,
                PaintSprite = expectedSprite,
                PaintColor = expectedColor
            };

            // Assert
            Assert.AreEqual(expectedId, paintItem.ID);
            Assert.AreEqual(expectedName, paintItem.Name);
            Assert.AreEqual(expectedBrand, paintItem.Brand);
            Assert.AreEqual(expectedSprite, paintItem.PaintSprite);
            Assert.AreEqual(expectedColor, paintItem.PaintColor);
        }
    }
}

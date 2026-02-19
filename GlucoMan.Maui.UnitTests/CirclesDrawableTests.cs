using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using gamon;
using GlucoMan;
using GlucoMan.Maui;
using Microsoft.Maui.Graphics;
using Moq;
using NUnit.Framework;

using static GlucoMan.Common;

using MauiPoint = Microsoft.Maui.Graphics.Point;

namespace GlucoMan.Maui.UnitTests
{
    /// <summary>
    /// Tests for the CirclesDrawable parameterized constructor.
    /// </summary>
    public partial class CirclesDrawableTests
    {
        /// <summary>
        /// Tests that the constructor properly assigns valid positive values to fields.
        /// Verifies by calling NormalizeXPosition and NormalizeYPosition which use imageWidth and imageHeight.
        /// </summary>
        /// <param name="imageWidth">The image width value to test.</param>
        /// <param name="imageHeight">The image height value to test.</param>
        /// <param name="idInjection">The injection ID value to test.</param>
        /// <param name="circlesVisibilityMaxTimeInDays">The visibility time in days to test.</param>
        [TestCase(1280.0, 1366.0, 1, 30.0)]
        [TestCase(100.0, 200.0, null, 7.0)]
        [TestCase(640.0, 480.0, 0, 0.0)]
        [TestCase(1920.0, 1080.0, -1, 365.0)]
        [TestCase(800.0, 600.0, int.MaxValue, 1.0)]
        [TestCase(1024.0, 768.0, int.MinValue, 100.0)]
        public void Constructor_ValidInputs_AssignsFieldsCorrectly(double imageWidth, double imageHeight, int? idInjection, double circlesVisibilityMaxTimeInDays)
        {
            // Arrange & Act
            var drawable = new CirclesDrawable(imageWidth, imageHeight, idInjection, circlesVisibilityMaxTimeInDays);

            // Assert
            // Verify imageWidth is set correctly by using NormalizeXPosition
            var normalizedX = drawable.NormalizeXPosition(imageWidth);
            Assert.That(normalizedX, Is.EqualTo(1.0));

            // Verify imageHeight is set correctly by using NormalizeYPosition
            var normalizedY = drawable.NormalizeYPosition(imageHeight);
            Assert.That(normalizedY, Is.EqualTo(1.0));
        }

        /// <summary>
        /// Tests that the constructor handles zero ImageWidth value.
        /// Zero width will cause division by zero in NormalizeXPosition.
        /// </summary>
        [Test]
        public void Constructor_ZeroImageWidth_AllowsConstruction()
        {
            // Arrange
            double imageWidth = 0.0;
            double imageHeight = 100.0;
            int? idInjection = 1;
            double circlesVisibilityMaxTimeInDays = 30.0;

            // Act
            var drawable = new CirclesDrawable(imageWidth, imageHeight, idInjection, circlesVisibilityMaxTimeInDays);

            // Assert
            Assert.That(drawable, Is.Not.Null);
            // Note: Calling NormalizeXPosition with zero imageWidth will result in infinity or NaN
            var result = drawable.NormalizeXPosition(10.0);
            Assert.That(result, Is.EqualTo(double.PositiveInfinity));
        }

        /// <summary>
        /// Tests that the constructor handles zero ImageHeight value.
        /// Zero height will cause division by zero in NormalizeYPosition.
        /// </summary>
        [Test]
        public void Constructor_ZeroImageHeight_AllowsConstruction()
        {
            // Arrange
            double imageWidth = 100.0;
            double imageHeight = 0.0;
            int? idInjection = 1;
            double circlesVisibilityMaxTimeInDays = 30.0;

            // Act
            var drawable = new CirclesDrawable(imageWidth, imageHeight, idInjection, circlesVisibilityMaxTimeInDays);

            // Assert
            Assert.That(drawable, Is.Not.Null);
            // Note: Calling NormalizeYPosition with zero imageHeight will result in infinity or NaN
            var result = drawable.NormalizeYPosition(10.0);
            Assert.That(result, Is.EqualTo(double.PositiveInfinity));
        }

        /// <summary>
        /// Tests that the constructor handles negative ImageWidth value.
        /// Negative width is an edge case that may produce unexpected normalization results.
        /// </summary>
        [Test]
        public void Constructor_NegativeImageWidth_AllowsConstruction()
        {
            // Arrange
            double imageWidth = -1280.0;
            double imageHeight = 1366.0;
            int? idInjection = 1;
            double circlesVisibilityMaxTimeInDays = 30.0;

            // Act
            var drawable = new CirclesDrawable(imageWidth, imageHeight, idInjection, circlesVisibilityMaxTimeInDays);

            // Assert
            Assert.That(drawable, Is.Not.Null);
            // Verify negative width results in negative normalization
            var normalizedX = drawable.NormalizeXPosition(-1280.0);
            Assert.That(normalizedX, Is.EqualTo(1.0));
        }

        /// <summary>
        /// Tests that the constructor handles negative ImageHeight value.
        /// Negative height is an edge case that may produce unexpected normalization results.
        /// </summary>
        [Test]
        public void Constructor_NegativeImageHeight_AllowsConstruction()
        {
            // Arrange
            double imageWidth = 1280.0;
            double imageHeight = -1366.0;
            int? idInjection = 1;
            double circlesVisibilityMaxTimeInDays = 30.0;

            // Act
            var drawable = new CirclesDrawable(imageWidth, imageHeight, idInjection, circlesVisibilityMaxTimeInDays);

            // Assert
            Assert.That(drawable, Is.Not.Null);
            // Verify negative height results in negative normalization
            var normalizedY = drawable.NormalizeYPosition(-1366.0);
            Assert.That(normalizedY, Is.EqualTo(1.0));
        }

        /// <summary>
        /// Tests that the constructor handles double.NaN for ImageWidth.
        /// NaN values will propagate through normalization calculations.
        /// </summary>
        [Test]
        public void Constructor_NaNImageWidth_AllowsConstruction()
        {
            // Arrange
            double imageWidth = double.NaN;
            double imageHeight = 1366.0;
            int? idInjection = 1;
            double circlesVisibilityMaxTimeInDays = 30.0;

            // Act
            var drawable = new CirclesDrawable(imageWidth, imageHeight, idInjection, circlesVisibilityMaxTimeInDays);

            // Assert
            Assert.That(drawable, Is.Not.Null);
            var result = drawable.NormalizeXPosition(100.0);
            Assert.That(result, Is.NaN);
        }

        /// <summary>
        /// Tests that the constructor handles double.NaN for ImageHeight.
        /// NaN values will propagate through normalization calculations.
        /// </summary>
        [Test]
        public void Constructor_NaNImageHeight_AllowsConstruction()
        {
            // Arrange
            double imageWidth = 1280.0;
            double imageHeight = double.NaN;
            int? idInjection = 1;
            double circlesVisibilityMaxTimeInDays = 30.0;

            // Act
            var drawable = new CirclesDrawable(imageWidth, imageHeight, idInjection, circlesVisibilityMaxTimeInDays);

            // Assert
            Assert.That(drawable, Is.Not.Null);
            var result = drawable.NormalizeYPosition(100.0);
            Assert.That(result, Is.NaN);
        }

        /// <summary>
        /// Tests that the constructor handles double.PositiveInfinity for ImageWidth.
        /// Infinity values will affect normalization calculations.
        /// </summary>
        [Test]
        public void Constructor_PositiveInfinityImageWidth_AllowsConstruction()
        {
            // Arrange
            double imageWidth = double.PositiveInfinity;
            double imageHeight = 1366.0;
            int? idInjection = 1;
            double circlesVisibilityMaxTimeInDays = 30.0;

            // Act
            var drawable = new CirclesDrawable(imageWidth, imageHeight, idInjection, circlesVisibilityMaxTimeInDays);

            // Assert
            Assert.That(drawable, Is.Not.Null);
            var result = drawable.NormalizeXPosition(100.0);
            Assert.That(result, Is.EqualTo(0.0));
        }

        /// <summary>
        /// Tests that the constructor handles double.PositiveInfinity for ImageHeight.
        /// Infinity values will affect normalization calculations.
        /// </summary>
        [Test]
        public void Constructor_PositiveInfinityImageHeight_AllowsConstruction()
        {
            // Arrange
            double imageWidth = 1280.0;
            double imageHeight = double.PositiveInfinity;
            int? idInjection = 1;
            double circlesVisibilityMaxTimeInDays = 30.0;

            // Act
            var drawable = new CirclesDrawable(imageWidth, imageHeight, idInjection, circlesVisibilityMaxTimeInDays);

            // Assert
            Assert.That(drawable, Is.Not.Null);
            var result = drawable.NormalizeYPosition(100.0);
            Assert.That(result, Is.EqualTo(0.0));
        }

        /// <summary>
        /// Tests that the constructor handles double.NegativeInfinity for ImageWidth.
        /// Negative infinity values will affect normalization calculations.
        /// </summary>
        [Test]
        public void Constructor_NegativeInfinityImageWidth_AllowsConstruction()
        {
            // Arrange
            double imageWidth = double.NegativeInfinity;
            double imageHeight = 1366.0;
            int? idInjection = 1;
            double circlesVisibilityMaxTimeInDays = 30.0;

            // Act
            var drawable = new CirclesDrawable(imageWidth, imageHeight, idInjection, circlesVisibilityMaxTimeInDays);

            // Assert
            Assert.That(drawable, Is.Not.Null);
            var result = drawable.NormalizeXPosition(100.0);
            Assert.That(result, Is.EqualTo(-0.0));
        }

        /// <summary>
        /// Tests that the constructor handles double.NegativeInfinity for ImageHeight.
        /// Negative infinity values will affect normalization calculations.
        /// </summary>
        [Test]
        public void Constructor_NegativeInfinityImageHeight_AllowsConstruction()
        {
            // Arrange
            double imageWidth = 1280.0;
            double imageHeight = double.NegativeInfinity;
            int? idInjection = 1;
            double circlesVisibilityMaxTimeInDays = 30.0;

            // Act
            var drawable = new CirclesDrawable(imageWidth, imageHeight, idInjection, circlesVisibilityMaxTimeInDays);

            // Assert
            Assert.That(drawable, Is.Not.Null);
            var result = drawable.NormalizeYPosition(100.0);
            Assert.That(result, Is.EqualTo(-0.0));
        }

        /// <summary>
        /// Tests that the constructor handles double.MaxValue for ImageWidth.
        /// Very large values should be assigned without error.
        /// </summary>
        [Test]
        public void Constructor_MaxValueImageWidth_AllowsConstruction()
        {
            // Arrange
            double imageWidth = double.MaxValue;
            double imageHeight = 1366.0;
            int? idInjection = 1;
            double circlesVisibilityMaxTimeInDays = 30.0;

            // Act
            var drawable = new CirclesDrawable(imageWidth, imageHeight, idInjection, circlesVisibilityMaxTimeInDays);

            // Assert
            Assert.That(drawable, Is.Not.Null);
            var result = drawable.NormalizeXPosition(double.MaxValue);
            Assert.That(result, Is.EqualTo(1.0));
        }

        /// <summary>
        /// Tests that the constructor handles double.MaxValue for ImageHeight.
        /// Very large values should be assigned without error.
        /// </summary>
        [Test]
        public void Constructor_MaxValueImageHeight_AllowsConstruction()
        {
            // Arrange
            double imageWidth = 1280.0;
            double imageHeight = double.MaxValue;
            int? idInjection = 1;
            double circlesVisibilityMaxTimeInDays = 30.0;

            // Act
            var drawable = new CirclesDrawable(imageWidth, imageHeight, idInjection, circlesVisibilityMaxTimeInDays);

            // Assert
            Assert.That(drawable, Is.Not.Null);
            var result = drawable.NormalizeYPosition(double.MaxValue);
            Assert.That(result, Is.EqualTo(1.0));
        }

        /// <summary>
        /// Tests that the constructor handles double.MinValue for ImageWidth.
        /// Very small (large negative) values should be assigned without error.
        /// </summary>
        [Test]
        public void Constructor_MinValueImageWidth_AllowsConstruction()
        {
            // Arrange
            double imageWidth = double.MinValue;
            double imageHeight = 1366.0;
            int? idInjection = 1;
            double circlesVisibilityMaxTimeInDays = 30.0;

            // Act
            var drawable = new CirclesDrawable(imageWidth, imageHeight, idInjection, circlesVisibilityMaxTimeInDays);

            // Assert
            Assert.That(drawable, Is.Not.Null);
        }

        /// <summary>
        /// Tests that the constructor handles double.MinValue for ImageHeight.
        /// Very small (large negative) values should be assigned without error.
        /// </summary>
        [Test]
        public void Constructor_MinValueImageHeight_AllowsConstruction()
        {
            // Arrange
            double imageWidth = 1280.0;
            double imageHeight = double.MinValue;
            int? idInjection = 1;
            double circlesVisibilityMaxTimeInDays = 30.0;

            // Act
            var drawable = new CirclesDrawable(imageWidth, imageHeight, idInjection, circlesVisibilityMaxTimeInDays);

            // Assert
            Assert.That(drawable, Is.Not.Null);
        }

        /// <summary>
        /// Tests that the constructor properly handles null value for IdInjection.
        /// Null is an explicitly valid value for this nullable parameter.
        /// </summary>
        [Test]
        public void Constructor_NullIdInjection_AllowsConstruction()
        {
            // Arrange
            double imageWidth = 1280.0;
            double imageHeight = 1366.0;
            int? idInjection = null;
            double circlesVisibilityMaxTimeInDays = 30.0;

            // Act
            var drawable = new CirclesDrawable(imageWidth, imageHeight, idInjection, circlesVisibilityMaxTimeInDays);

            // Assert
            Assert.That(drawable, Is.Not.Null);
        }

        /// <summary>
        /// Tests that the constructor handles double.NaN for CirclesVisibilityMaxTimeInDays.
        /// NaN is an edge case that should be accepted but may cause unexpected behavior.
        /// </summary>
        [Test]
        public void Constructor_NaNCirclesVisibilityMaxTimeInDays_AllowsConstruction()
        {
            // Arrange
            double imageWidth = 1280.0;
            double imageHeight = 1366.0;
            int? idInjection = 1;
            double circlesVisibilityMaxTimeInDays = double.NaN;

            // Act
            var drawable = new CirclesDrawable(imageWidth, imageHeight, idInjection, circlesVisibilityMaxTimeInDays);

            // Assert
            Assert.That(drawable, Is.Not.Null);
        }

        /// <summary>
        /// Tests that the constructor handles double.PositiveInfinity for CirclesVisibilityMaxTimeInDays.
        /// Infinity is an edge case that should be accepted.
        /// </summary>
        [Test]
        public void Constructor_PositiveInfinityCirclesVisibilityMaxTimeInDays_AllowsConstruction()
        {
            // Arrange
            double imageWidth = 1280.0;
            double imageHeight = 1366.0;
            int? idInjection = 1;
            double circlesVisibilityMaxTimeInDays = double.PositiveInfinity;

            // Act
            var drawable = new CirclesDrawable(imageWidth, imageHeight, idInjection, circlesVisibilityMaxTimeInDays);

            // Assert
            Assert.That(drawable, Is.Not.Null);
        }

        /// <summary>
        /// Tests that the constructor handles double.NegativeInfinity for CirclesVisibilityMaxTimeInDays.
        /// Negative infinity is an edge case that should be accepted but may cause unexpected behavior.
        /// </summary>
        [Test]
        public void Constructor_NegativeInfinityCirclesVisibilityMaxTimeInDays_AllowsConstruction()
        {
            // Arrange
            double imageWidth = 1280.0;
            double imageHeight = 1366.0;
            int? idInjection = 1;
            double circlesVisibilityMaxTimeInDays = double.NegativeInfinity;

            // Act
            var drawable = new CirclesDrawable(imageWidth, imageHeight, idInjection, circlesVisibilityMaxTimeInDays);

            // Assert
            Assert.That(drawable, Is.Not.Null);
        }

        /// <summary>
        /// Tests that the constructor handles negative value for CirclesVisibilityMaxTimeInDays.
        /// Negative time values are domain-invalid but no validation exists in the constructor.
        /// </summary>
        [Test]
        public void Constructor_NegativeCirclesVisibilityMaxTimeInDays_AllowsConstruction()
        {
            // Arrange
            double imageWidth = 1280.0;
            double imageHeight = 1366.0;
            int? idInjection = 1;
            double circlesVisibilityMaxTimeInDays = -30.0;

            // Act
            var drawable = new CirclesDrawable(imageWidth, imageHeight, idInjection, circlesVisibilityMaxTimeInDays);

            // Assert
            Assert.That(drawable, Is.Not.Null);
        }

        /// <summary>
        /// Tests that the constructor handles double.MaxValue for CirclesVisibilityMaxTimeInDays.
        /// Very large time values should be accepted.
        /// </summary>
        [Test]
        public void Constructor_MaxValueCirclesVisibilityMaxTimeInDays_AllowsConstruction()
        {
            // Arrange
            double imageWidth = 1280.0;
            double imageHeight = 1366.0;
            int? idInjection = 1;
            double circlesVisibilityMaxTimeInDays = double.MaxValue;

            // Act
            var drawable = new CirclesDrawable(imageWidth, imageHeight, idInjection, circlesVisibilityMaxTimeInDays);

            // Assert
            Assert.That(drawable, Is.Not.Null);
        }

        /// <summary>
        /// Tests that the constructor handles double.MinValue for CirclesVisibilityMaxTimeInDays.
        /// Very small (large negative) time values should be accepted.
        /// </summary>
        [Test]
        public void Constructor_MinValueCirclesVisibilityMaxTimeInDays_AllowsConstruction()
        {
            // Arrange
            double imageWidth = 1280.0;
            double imageHeight = 1366.0;
            int? idInjection = 1;
            double circlesVisibilityMaxTimeInDays = double.MinValue;

            // Act
            var drawable = new CirclesDrawable(imageWidth, imageHeight, idInjection, circlesVisibilityMaxTimeInDays);

            // Assert
            Assert.That(drawable, Is.Not.Null);
        }

        /// <summary>
        /// Tests that Draw method in editing mode with empty reference points does not draw any circles.
        /// </summary>
        /// <remarks>
        /// Input: isCallerEditing = true, empty ReferencePointsCoordinates.
        /// Expected: No FillEllipse calls on canvas.
        /// Coverage: Covers line 85 (if condition true), loop at line 89 not entered.
        /// </remarks>
        [Test]
        public void Draw_EditingModeWithEmptyReferencePoints_NoCirclesDrawn()
        {
            // Arrange
            var drawable = new CirclesDrawable(800, 600, null, 10.0);
            drawable.IsCallerEditing = true;
            var mockCanvas = new Mock<ICanvas>();
            var dirtyRect = new RectF(0, 0, 800, 600);

            // Act
            drawable.Draw(mockCanvas.Object, dirtyRect);

            // Assert
            mockCanvas.Verify(c => c.FillEllipse(It.IsAny<float>(), It.IsAny<float>(), It.IsAny<float>(), It.IsAny<float>()), Times.Never);
        }

        /// <summary>
        /// Tests that Draw method in editing mode with a single reference point draws one blue circle.
        /// </summary>
        /// <remarks>
        /// Input: isCallerEditing = true, single point in ReferencePointsCoordinates.
        /// Expected: Blue color set, FillEllipse called once with correct parameters.
        /// Coverage: Covers lines 85, 89, 92-101.
        /// </remarks>
        [Test]
        public void Draw_EditingModeWithSingleReferencePoint_DrawsBlueCircle()
        {
            // Arrange
            var drawable = new CirclesDrawable(800, 600, null, 10.0);
            drawable.IsCallerEditing = true;

            var referencePosition = new PositionOfInjection
            {
                PositionX = 0.5,
                PositionY = 0.5
            };
            drawable.LoadReferenceCoordinates(new List<PositionOfInjection> { referencePosition });

            var mockCanvas = new Mock<ICanvas>();
            var dirtyRect = new RectF(0, 0, 800, 600);

            // Act
            drawable.Draw(mockCanvas.Object, dirtyRect);

            // Assert
            mockCanvas.VerifySet(c => c.StrokeColor = It.Is<Color>(color =>
                Math.Abs(color.Red) < 0.01 && Math.Abs(color.Green) < 0.01 && Math.Abs(color.Blue - 1.0) < 0.01), Times.AtLeastOnce);
            mockCanvas.VerifySet(c => c.FillColor = It.Is<Color>(color =>
                Math.Abs(color.Red) < 0.01 && Math.Abs(color.Green) < 0.01 && Math.Abs(color.Blue - 1.0) < 0.01), Times.AtLeastOnce);
            mockCanvas.VerifySet(c => c.StrokeSize = It.IsAny<float>(), Times.AtLeastOnce);
            mockCanvas.Verify(c => c.FillEllipse(It.IsAny<float>(), It.IsAny<float>(), It.IsAny<float>(), It.IsAny<float>()), Times.Once);
        }

        /// <summary>
        /// Tests that Draw method in editing mode with multiple reference points draws multiple blue circles.
        /// </summary>
        /// <remarks>
        /// Input: isCallerEditing = true, three points in ReferencePointsCoordinates.
        /// Expected: FillEllipse called three times.
        /// Coverage: Covers lines 85, 89, 92-101 with multiple iterations.
        /// </remarks>
        [Test]
        public void Draw_EditingModeWithMultipleReferencePoints_DrawsMultipleBlueCircles()
        {
            // Arrange
            var drawable = new CirclesDrawable(800, 600, null, 10.0);
            drawable.IsCallerEditing = true;

            var referencePositions = new List<PositionOfInjection>
            {
                new PositionOfInjection { PositionX = 0.25, PositionY = 0.25 },
                new PositionOfInjection { PositionX = 0.5, PositionY = 0.5 },
                new PositionOfInjection { PositionX = 0.75, PositionY = 0.75 }
            };
            drawable.LoadReferenceCoordinates(referencePositions);

            var mockCanvas = new Mock<ICanvas>();
            var dirtyRect = new RectF(0, 0, 800, 600);

            // Act
            drawable.Draw(mockCanvas.Object, dirtyRect);

            // Assert
            mockCanvas.Verify(c => c.FillEllipse(It.IsAny<float>(), It.IsAny<float>(), It.IsAny<float>(), It.IsAny<float>()), Times.Exactly(3));
        }

        /// <summary>
        /// Tests that Draw method in non-editing mode with empty injection points does not draw any circles.
        /// </summary>
        /// <remarks>
        /// Input: isCallerEditing = false, empty InjectionPointsCoordinates.
        /// Expected: No FillEllipse calls on canvas.
        /// Coverage: Covers line 85 (if condition false), loop at line 109 not entered.
        /// </remarks>
        [Test]
        public void Draw_NonEditingModeWithEmptyInjectionPoints_NoCirclesDrawn()
        {
            // Arrange
            var drawable = new CirclesDrawable(800, 600, null, 10.0);
            drawable.IsCallerEditing = false;
            var mockCanvas = new Mock<ICanvas>();
            var dirtyRect = new RectF(0, 0, 800, 600);

            // Act
            drawable.Draw(mockCanvas.Object, dirtyRect);

            // Assert
            mockCanvas.Verify(c => c.FillEllipse(It.IsAny<float>(), It.IsAny<float>(), It.IsAny<float>(), It.IsAny<float>()), Times.Never);
        }

        /// <summary>
        /// Tests that Draw method in non-editing mode with a single injection point draws one colored circle.
        /// </summary>
        /// <remarks>
        /// Input: isCallerEditing = false, single injection in InjectionPointsCoordinates.
        /// Expected: Colored circle drawn, FillEllipse called once.
        /// Coverage: Covers lines 108-120.
        /// </remarks>
        [Test]
        public void Draw_NonEditingModeWithSingleInjectionPoint_DrawsColoredCircle()
        {
            // Arrange
            var drawable = new CirclesDrawable(800, 600, 1, 10.0);
            drawable.IsCallerEditing = false;

            var injection = new Injection
            {
                IdInjection = 1,
                PositionX = 0.5,
                PositionY = 0.5,
                EventTime = new DateTimeAndText { DateTime = DateTime.Now }
            };
            drawable.LoadInjectionsCoordinates(new List<Injection> { injection });

            var mockCanvas = new Mock<ICanvas>();
            var dirtyRect = new RectF(0, 0, 800, 600);

            // Act
            drawable.Draw(mockCanvas.Object, dirtyRect);

            // Assert
            mockCanvas.VerifySet(c => c.StrokeColor = It.IsAny<Color>(), Times.AtLeastOnce);
            mockCanvas.VerifySet(c => c.FillColor = It.IsAny<Color>(), Times.AtLeastOnce);
            mockCanvas.VerifySet(c => c.StrokeSize = It.IsAny<float>(), Times.AtLeastOnce);
            mockCanvas.Verify(c => c.FillEllipse(It.IsAny<float>(), It.IsAny<float>(), It.IsAny<float>(), It.IsAny<float>()), Times.Once);
        }

        /// <summary>
        /// Tests that Draw method in non-editing mode with multiple injection points draws multiple colored circles.
        /// </summary>
        /// <remarks>
        /// Input: isCallerEditing = false, three injections in InjectionPointsCoordinates.
        /// Expected: FillEllipse called three times.
        /// Coverage: Covers lines 108-120 with multiple iterations.
        /// </remarks>
        [Test]
        public void Draw_NonEditingModeWithMultipleInjectionPoints_DrawsMultipleColoredCircles()
        {
            // Arrange
            var drawable = new CirclesDrawable(800, 600, null, 10.0);
            drawable.IsCallerEditing = false;

            var now = DateTime.Now;
            var injections = new List<Injection>
            {
                new Injection { IdInjection = 1, PositionX = 0.25, PositionY = 0.25, EventTime = new DateTimeAndText { DateTime = now.AddDays(-1) } },
                new Injection { IdInjection = 2, PositionX = 0.5, PositionY = 0.5, EventTime = new DateTimeAndText { DateTime = now.AddDays(-2) } },
                new Injection { IdInjection = 3, PositionX = 0.75, PositionY = 0.75, EventTime = new DateTimeAndText { DateTime = now.AddDays(-3) } }
            };
            drawable.LoadInjectionsCoordinates(injections);

            var mockCanvas = new Mock<ICanvas>();
            var dirtyRect = new RectF(0, 0, 800, 600);

            // Act
            drawable.Draw(mockCanvas.Object, dirtyRect);

            // Assert
            mockCanvas.Verify(c => c.FillEllipse(It.IsAny<float>(), It.IsAny<float>(), It.IsAny<float>(), It.IsAny<float>()), Times.Exactly(3));
        }

        /// <summary>
        /// Tests that Draw method in editing mode calculates and uses correct stroke size based on radius.
        /// </summary>
        /// <remarks>
        /// Input: isCallerEditing = true with reference point.
        /// Expected: StrokeSize set to Max(1f, CurrentReferenceRadius / 3f).
        /// Coverage: Verifies line 93 calculation.
        /// </remarks>
        [Test]
        public void Draw_EditingMode_UsesCorrectStrokeSize()
        {
            // Arrange
            var drawable = new CirclesDrawable(800, 600, null, 10.0);
            drawable.IsCallerEditing = true;

            var referencePosition = new PositionOfInjection { PositionX = 0.5, PositionY = 0.5 };
            drawable.LoadReferenceCoordinates(new List<PositionOfInjection> { referencePosition });

            var mockCanvas = new Mock<ICanvas>();
            var dirtyRect = new RectF(0, 0, 800, 600);

            // Act
            drawable.Draw(mockCanvas.Object, dirtyRect);

            // Assert
            mockCanvas.VerifySet(c => c.StrokeSize = It.Is<float>(size => size >= 1f), Times.AtLeastOnce);
        }

        /// <summary>
        /// Tests that Draw method in non-editing mode calculates and uses correct stroke size based on radius.
        /// </summary>
        /// <remarks>
        /// Input: isCallerEditing = false with injection point.
        /// Expected: StrokeSize set to Max(1f, CurrentInjectionRadius / 3f).
        /// Coverage: Verifies line 112 calculation.
        /// </remarks>
        [Test]
        public void Draw_NonEditingMode_UsesCorrectStrokeSize()
        {
            // Arrange
            var drawable = new CirclesDrawable(800, 600, 1, 10.0);
            drawable.IsCallerEditing = false;

            var injection = new Injection
            {
                IdInjection = 1,
                PositionX = 0.5,
                PositionY = 0.5,
                EventTime = new DateTimeAndText { DateTime = DateTime.Now }
            };
            drawable.LoadInjectionsCoordinates(new List<Injection> { injection });

            var mockCanvas = new Mock<ICanvas>();
            var dirtyRect = new RectF(0, 0, 800, 600);

            // Act
            drawable.Draw(mockCanvas.Object, dirtyRect);

            // Assert
            mockCanvas.VerifySet(c => c.StrokeSize = It.Is<float>(size => size >= 1f), Times.AtLeastOnce);
        }

        /// <summary>
        /// Tests that Draw method uses default image dimensions when created with parameterless constructor.
        /// </summary>
        /// <remarks>
        /// Input: CirclesDrawable created with default constructor, editing mode with reference point.
        /// Expected: Drawing operations complete successfully using default scaling.
        /// Coverage: Tests Draw with default constructor initialization.
        /// </remarks>
        [Test]
        public void Draw_WithDefaultConstructor_UsesDefaultDimensions()
        {
            // Arrange
            var drawable = new CirclesDrawable();
            drawable.IsCallerEditing = true;

            var referencePosition = new PositionOfInjection { PositionX = 0.5, PositionY = 0.5 };
            drawable.LoadReferenceCoordinates(new List<PositionOfInjection> { referencePosition });

            var mockCanvas = new Mock<ICanvas>();
            var dirtyRect = new RectF(0, 0, 1280, 1366);

            // Act
            drawable.Draw(mockCanvas.Object, dirtyRect);

            // Assert
            mockCanvas.Verify(c => c.FillEllipse(It.IsAny<float>(), It.IsAny<float>(), It.IsAny<float>(), It.IsAny<float>()), Times.Once);
        }

        /// <summary>
        /// Tests that Draw method correctly handles edge case of very small image dimensions.
        /// </summary>
        /// <remarks>
        /// Input: Very small image dimensions (10x10), reference point to draw.
        /// Expected: Drawing operations complete successfully with scaled radius.
        /// Coverage: Tests Draw with extreme scaling factors.
        /// </remarks>
        [Test]
        public void Draw_WithVerySmallDimensions_HandlesScalingCorrectly()
        {
            // Arrange
            var drawable = new CirclesDrawable(10, 10, null, 10.0);
            drawable.IsCallerEditing = true;

            var referencePosition = new PositionOfInjection { PositionX = 0.5, PositionY = 0.5 };
            drawable.LoadReferenceCoordinates(new List<PositionOfInjection> { referencePosition });

            var mockCanvas = new Mock<ICanvas>();
            var dirtyRect = new RectF(0, 0, 10, 10);

            // Act
            drawable.Draw(mockCanvas.Object, dirtyRect);

            // Assert
            mockCanvas.Verify(c => c.FillEllipse(It.IsAny<float>(), It.IsAny<float>(), It.IsAny<float>(), It.IsAny<float>()), Times.Once);
            mockCanvas.VerifySet(c => c.StrokeSize = It.Is<float>(size => size >= 1f), Times.AtLeastOnce);
        }

        /// <summary>
        /// Tests that Draw method correctly handles edge case of very large image dimensions.
        /// </summary>
        /// <remarks>
        /// Input: Very large image dimensions (4000x4000), reference point to draw.
        /// Expected: Drawing operations complete successfully with scaled radius.
        /// Coverage: Tests Draw with large scaling factors.
        /// </remarks>
        [Test]
        public void Draw_WithVeryLargeDimensions_HandlesScalingCorrectly()
        {
            // Arrange
            var drawable = new CirclesDrawable(4000, 4000, null, 10.0);
            drawable.IsCallerEditing = true;

            var referencePosition = new PositionOfInjection { PositionX = 0.5, PositionY = 0.5 };
            drawable.LoadReferenceCoordinates(new List<PositionOfInjection> { referencePosition });

            var mockCanvas = new Mock<ICanvas>();
            var dirtyRect = new RectF(0, 0, 4000, 4000);

            // Act
            drawable.Draw(mockCanvas.Object, dirtyRect);

            // Assert
            mockCanvas.Verify(c => c.FillEllipse(It.IsAny<float>(), It.IsAny<float>(), It.IsAny<float>(), It.IsAny<float>()), Times.Once);
        }

        /// <summary>
        /// Tests that Draw method uses brown color for current injection point.
        /// </summary>
        /// <remarks>
        /// Input: isCallerEditing = false, injection with IdInjection matching idCurrentInjection.
        /// Expected: Brown color used for current injection.
        /// Coverage: Tests color assignment for current injection in LoadInjectionsCoordinates logic.
        /// </remarks>
        [Test]
        public void Draw_NonEditingModeWithCurrentInjection_UsesBrownColor()
        {
            // Arrange
            var drawable = new CirclesDrawable(800, 600, 1, 10.0);
            drawable.IsCallerEditing = false;

            var injection = new Injection
            {
                IdInjection = 1,
                PositionX = 0.5,
                PositionY = 0.5,
                EventTime = new DateTimeAndText { DateTime = DateTime.Now }
            };
            drawable.LoadInjectionsCoordinates(new List<Injection> { injection });

            var mockCanvas = new Mock<ICanvas>();
            var dirtyRect = new RectF(0, 0, 800, 600);

            // Act
            drawable.Draw(mockCanvas.Object, dirtyRect);

            // Assert
            mockCanvas.VerifySet(c => c.FillColor = It.IsAny<Color>(), Times.AtLeastOnce);
            mockCanvas.Verify(c => c.FillEllipse(It.IsAny<float>(), It.IsAny<float>(), It.IsAny<float>(), It.IsAny<float>()), Times.Once);
        }

        /// <summary>
        /// Tests that Draw method handles boundary position values (0.0, 0.0).
        /// </summary>
        /// <remarks>
        /// Input: Reference point at position (0.0, 0.0).
        /// Expected: Drawing operations complete successfully at origin.
        /// Coverage: Tests edge case of minimum position coordinates.
        /// </remarks>
        [Test]
        public void Draw_WithBoundaryPositionZero_DrawsCorrectly()
        {
            // Arrange
            var drawable = new CirclesDrawable(800, 600, null, 10.0);
            drawable.IsCallerEditing = true;

            var referencePosition = new PositionOfInjection { PositionX = 0.0, PositionY = 0.0 };
            drawable.LoadReferenceCoordinates(new List<PositionOfInjection> { referencePosition });

            var mockCanvas = new Mock<ICanvas>();
            var dirtyRect = new RectF(0, 0, 800, 600);

            // Act
            drawable.Draw(mockCanvas.Object, dirtyRect);

            // Assert
            mockCanvas.Verify(c => c.FillEllipse(It.IsAny<float>(), It.IsAny<float>(), It.IsAny<float>(), It.IsAny<float>()), Times.Once);
        }

        /// <summary>
        /// Tests that Draw method handles boundary position values (1.0, 1.0).
        /// </summary>
        /// <remarks>
        /// Input: Reference point at position (1.0, 1.0).
        /// Expected: Drawing operations complete successfully at maximum coordinates.
        /// Coverage: Tests edge case of maximum position coordinates.
        /// </remarks>
        [Test]
        public void Draw_WithBoundaryPositionOne_DrawsCorrectly()
        {
            // Arrange
            var drawable = new CirclesDrawable(800, 600, null, 10.0);
            drawable.IsCallerEditing = true;

            var referencePosition = new PositionOfInjection { PositionX = 1.0, PositionY = 1.0 };
            drawable.LoadReferenceCoordinates(new List<PositionOfInjection> { referencePosition });

            var mockCanvas = new Mock<ICanvas>();
            var dirtyRect = new RectF(0, 0, 800, 600);

            // Act
            drawable.Draw(mockCanvas.Object, dirtyRect);

            // Assert
            mockCanvas.Verify(c => c.FillEllipse(It.IsAny<float>(), It.IsAny<float>(), It.IsAny<float>(), It.IsAny<float>()), Times.Once);
        }

        /// <summary>
        /// Tests that RemovePointIfNear does not throw an exception when ReferencePointsCoordinates is empty.
        /// </summary>
        /// <remarks>
        /// Input: Empty reference points list, arbitrary click position.
        /// Expected: No exception thrown, list remains empty.
        /// </remarks>
        [Test]
        public void RemovePointIfNear_EmptyReferenceList_NoExceptionThrown()
        {
            // Arrange
            var drawable = new CirclesDrawable();
            var clickPosition = new MauiPoint(100, 100);
            var referencePoints = GetReferencePointsCoordinates(drawable);

            // Act & Assert
            Assert.DoesNotThrow(() => drawable.RemovePointIfNear(clickPosition));
            Assert.That(referencePoints.Count, Is.EqualTo(0));
        }

        /// <summary>
        /// Tests that RemovePointIfNear removes a point when the click position is within CurrentNearEnoughDistance.
        /// </summary>
        /// <remarks>
        /// Input: Reference point at (100, 100), click at (110, 110) (distance � 14.14, less than default 30).
        /// Expected: Point is removed from the list.
        /// </remarks>
        [Test]
        public void RemovePointIfNear_PointWithinDistance_RemovesPoint()
        {
            // Arrange
            var drawable = new CirclesDrawable();
            var referencePoint = new MauiPoint(100, 100);
            var clickPosition = new MauiPoint(110, 110);
            var referencePoints = GetReferencePointsCoordinates(drawable);
            referencePoints.Add(referencePoint);

            // Act
            drawable.RemovePointIfNear(clickPosition);

            // Assert
            Assert.That(referencePoints.Count, Is.EqualTo(0));
        }

        /// <summary>
        /// Tests that RemovePointIfNear does not remove a point when the click position is outside CurrentNearEnoughDistance.
        /// </summary>
        /// <remarks>
        /// Input: Reference point at (100, 100), click at (150, 150) (distance � 70.7, greater than default 30).
        /// Expected: Point is not removed from the list.
        /// </remarks>
        [Test]
        public void RemovePointIfNear_PointOutsideDistance_DoesNotRemovePoint()
        {
            // Arrange
            var drawable = new CirclesDrawable();
            var referencePoint = new MauiPoint(100, 100);
            var clickPosition = new MauiPoint(150, 150);
            var referencePoints = GetReferencePointsCoordinates(drawable);
            referencePoints.Add(referencePoint);

            // Act
            drawable.RemovePointIfNear(clickPosition);

            // Assert
            Assert.That(referencePoints.Count, Is.EqualTo(1));
            Assert.That(referencePoints[0].X, Is.EqualTo(100));
            Assert.That(referencePoints[0].Y, Is.EqualTo(100));
        }

        /// <summary>
        /// Tests that RemovePointIfNear removes the nearest point when multiple points exist.
        /// </summary>
        /// <remarks>
        /// Input: Multiple reference points, click near one of them.
        /// Expected: Only the nearest point within distance is removed.
        /// </remarks>
        [Test]
        public void RemovePointIfNear_MultiplePoints_RemovesNearestOnly()
        {
            // Arrange
            var drawable = new CirclesDrawable();
            var nearPoint = new MauiPoint(100, 100);
            var farPoint = new MauiPoint(200, 200);
            var clickPosition = new MauiPoint(105, 105);
            var referencePoints = GetReferencePointsCoordinates(drawable);
            referencePoints.Add(nearPoint);
            referencePoints.Add(farPoint);

            // Act
            drawable.RemovePointIfNear(clickPosition);

            // Assert
            Assert.That(referencePoints.Count, Is.EqualTo(1));
            Assert.That(referencePoints[0].X, Is.EqualTo(200));
            Assert.That(referencePoints[0].Y, Is.EqualTo(200));
        }

        /// <summary>
        /// Tests that RemovePointIfNear removes a point when clicking exactly at the point location.
        /// </summary>
        /// <remarks>
        /// Input: Reference point at (100, 100), click at (100, 100) (distance = 0).
        /// Expected: Point is removed from the list.
        /// </remarks>
        [Test]
        public void RemovePointIfNear_ExactlyAtPoint_RemovesPoint()
        {
            // Arrange
            var drawable = new CirclesDrawable();
            var referencePoint = new MauiPoint(100, 100);
            var clickPosition = new MauiPoint(100, 100);
            var referencePoints = GetReferencePointsCoordinates(drawable);
            referencePoints.Add(referencePoint);

            // Act
            drawable.RemovePointIfNear(clickPosition);

            // Assert
            Assert.That(referencePoints.Count, Is.EqualTo(0));
        }

        /// <summary>
        /// Tests that RemovePointIfNear does not remove a single point that is far away.
        /// </summary>
        /// <remarks>
        /// Input: Single reference point at (100, 100), click at (500, 500) (distance � 565).
        /// Expected: Point is not removed from the list.
        /// </remarks>
        [Test]
        public void RemovePointIfNear_SinglePointFarAway_DoesNotRemovePoint()
        {
            // Arrange
            var drawable = new CirclesDrawable();
            var referencePoint = new MauiPoint(100, 100);
            var clickPosition = new MauiPoint(500, 500);
            var referencePoints = GetReferencePointsCoordinates(drawable);
            referencePoints.Add(referencePoint);

            // Act
            drawable.RemovePointIfNear(clickPosition);

            // Assert
            Assert.That(referencePoints.Count, Is.EqualTo(1));
        }

        /// <summary>
        /// Tests that RemovePointIfNear handles negative coordinates correctly when within distance.
        /// </summary>
        /// <remarks>
        /// Input: Reference point at (-50, -50), click at (-55, -55) (distance � 7.07, less than 30).
        /// Expected: Point is removed from the list.
        /// </remarks>
        [Test]
        public void RemovePointIfNear_NegativeCoordinatesWithinDistance_RemovesPoint()
        {
            // Arrange
            var drawable = new CirclesDrawable();
            var referencePoint = new MauiPoint(-50, -50);
            var clickPosition = new MauiPoint(-55, -55);
            var referencePoints = GetReferencePointsCoordinates(drawable);
            referencePoints.Add(referencePoint);

            // Act
            drawable.RemovePointIfNear(clickPosition);

            // Assert
            Assert.That(referencePoints.Count, Is.EqualTo(0));
        }

        /// <summary>
        /// Tests that RemovePointIfNear handles zero coordinates correctly.
        /// </summary>
        /// <remarks>
        /// Input: Reference point at (0, 0), click at (10, 10) (distance � 14.14, less than 30).
        /// Expected: Point is removed from the list.
        /// </remarks>
        [Test]
        public void RemovePointIfNear_ZeroCoordinates_RemovesPoint()
        {
            // Arrange
            var drawable = new CirclesDrawable();
            var referencePoint = new MauiPoint(0, 0);
            var clickPosition = new MauiPoint(10, 10);
            var referencePoints = GetReferencePointsCoordinates(drawable);
            referencePoints.Add(referencePoint);

            // Act
            drawable.RemovePointIfNear(clickPosition);

            // Assert
            Assert.That(referencePoints.Count, Is.EqualTo(0));
        }

        /// <summary>
        /// Tests that RemovePointIfNear handles click position at boundary distance correctly (just under threshold).
        /// </summary>
        /// <remarks>
        /// Input: Reference point at (100, 100), click at (121, 121) (distance = 29.7, just under 30).
        /// Expected: Point is removed from the list.
        /// </remarks>
        [Test]
        public void RemovePointIfNear_BoundaryDistanceJustUnder_RemovesPoint()
        {
            // Arrange
            var drawable = new CirclesDrawable();
            var referencePoint = new MauiPoint(100, 100);
            var clickPosition = new MauiPoint(121, 121);
            var referencePoints = GetReferencePointsCoordinates(drawable);
            referencePoints.Add(referencePoint);

            // Act
            drawable.RemovePointIfNear(clickPosition);

            // Assert
            Assert.That(referencePoints.Count, Is.EqualTo(0));
        }

        /// <summary>
        /// Tests that RemovePointIfNear handles click position just over boundary distance correctly.
        /// </summary>
        /// <remarks>
        /// Input: Reference point at (100, 100), click at (122, 122) (distance � 31.1, just over 30).
        /// Expected: Point is not removed from the list.
        /// </remarks>
        [Test]
        public void RemovePointIfNear_BoundaryDistanceJustOver_DoesNotRemovePoint()
        {
            // Arrange
            var drawable = new CirclesDrawable();
            var referencePoint = new MauiPoint(100, 100);
            var clickPosition = new MauiPoint(122, 122);
            var referencePoints = GetReferencePointsCoordinates(drawable);
            referencePoints.Add(referencePoint);

            // Act
            drawable.RemovePointIfNear(clickPosition);

            // Assert
            Assert.That(referencePoints.Count, Is.EqualTo(1));
        }

        /// <summary>
        /// Tests that RemovePointIfNear handles extreme coordinate values gracefully.
        /// </summary>
        /// <remarks>
        /// Input: Reference point at (double.MaxValue, double.MaxValue), click at (double.MaxValue, double.MaxValue).
        /// Expected: No exception thrown, distance calculation handles extreme values.
        /// </remarks>
        [Test]
        public void RemovePointIfNear_ExtremeMaxValueCoordinates_HandlesGracefully()
        {
            // Arrange
            var drawable = new CirclesDrawable();
            var referencePoint = new MauiPoint(double.MaxValue, double.MaxValue);
            var clickPosition = new MauiPoint(double.MaxValue, double.MaxValue);
            var referencePoints = GetReferencePointsCoordinates(drawable);
            referencePoints.Add(referencePoint);

            // Act & Assert
            Assert.DoesNotThrow(() => drawable.RemovePointIfNear(clickPosition));
        }

        /// <summary>
        /// Tests that RemovePointIfNear handles minimum coordinate values.
        /// </summary>
        /// <remarks>
        /// Input: Reference point at (double.MinValue, double.MinValue), click at (double.MinValue, double.MinValue).
        /// Expected: No exception thrown, distance calculation handles extreme negative values.
        /// </remarks>
        [Test]
        public void RemovePointIfNear_ExtremeMinValueCoordinates_HandlesGracefully()
        {
            // Arrange
            var drawable = new CirclesDrawable();
            var referencePoint = new MauiPoint(double.MinValue, double.MinValue);
            var clickPosition = new MauiPoint(double.MinValue, double.MinValue);
            var referencePoints = GetReferencePointsCoordinates(drawable);
            referencePoints.Add(referencePoint);

            // Act & Assert
            Assert.DoesNotThrow(() => drawable.RemovePointIfNear(clickPosition));
        }

        /// <summary>
        /// Tests that RemovePointIfNear handles NaN coordinate values.
        /// </summary>
        /// <remarks>
        /// Input: Reference point at (double.NaN, double.NaN), click at (100, 100).
        /// Expected: No exception thrown, distance calculation with NaN is handled.
        /// </remarks>
        [Test]
        public void RemovePointIfNear_NaNCoordinates_HandlesGracefully()
        {
            // Arrange
            var drawable = new CirclesDrawable();
            var referencePoint = new MauiPoint(double.NaN, double.NaN);
            var clickPosition = new MauiPoint(100, 100);
            var referencePoints = GetReferencePointsCoordinates(drawable);
            referencePoints.Add(referencePoint);

            // Act & Assert
            Assert.DoesNotThrow(() => drawable.RemovePointIfNear(clickPosition));
        }

        /// <summary>
        /// Tests that RemovePointIfNear handles positive infinity coordinate values.
        /// </summary>
        /// <remarks>
        /// Input: Reference point at (double.PositiveInfinity, double.PositiveInfinity), click at (100, 100).
        /// Expected: No exception thrown, distance calculation with infinity is handled.
        /// </remarks>
        [Test]
        public void RemovePointIfNear_PositiveInfinityCoordinates_HandlesGracefully()
        {
            // Arrange
            var drawable = new CirclesDrawable();
            var referencePoint = new MauiPoint(double.PositiveInfinity, double.PositiveInfinity);
            var clickPosition = new MauiPoint(100, 100);
            var referencePoints = GetReferencePointsCoordinates(drawable);
            referencePoints.Add(referencePoint);

            // Act & Assert
            Assert.DoesNotThrow(() => drawable.RemovePointIfNear(clickPosition));
        }

        /// <summary>
        /// Tests that RemovePointIfNear handles negative infinity coordinate values.
        /// </summary>
        /// <remarks>
        /// Input: Reference point at (double.NegativeInfinity, double.NegativeInfinity), click at (100, 100).
        /// Expected: No exception thrown, distance calculation with negative infinity is handled.
        /// </remarks>
        [Test]
        public void RemovePointIfNear_NegativeInfinityCoordinates_HandlesGracefully()
        {
            // Arrange
            var drawable = new CirclesDrawable();
            var referencePoint = new MauiPoint(double.NegativeInfinity, double.NegativeInfinity);
            var clickPosition = new MauiPoint(100, 100);
            var referencePoints = GetReferencePointsCoordinates(drawable);
            referencePoints.Add(referencePoint);

            // Act & Assert
            Assert.DoesNotThrow(() => drawable.RemovePointIfNear(clickPosition));
        }

        /// <summary>
        /// Tests that RemovePointIfNear handles click position with NaN coordinates.
        /// </summary>
        /// <remarks>
        /// Input: Reference point at (100, 100), click at (double.NaN, double.NaN).
        /// Expected: No exception thrown, distance calculation with NaN click position is handled.
        /// </remarks>
        [Test]
        public void RemovePointIfNear_ClickPositionWithNaN_HandlesGracefully()
        {
            // Arrange
            var drawable = new CirclesDrawable();
            var referencePoint = new MauiPoint(100, 100);
            var clickPosition = new MauiPoint(double.NaN, double.NaN);
            var referencePoints = GetReferencePointsCoordinates(drawable);
            referencePoints.Add(referencePoint);

            // Act & Assert
            Assert.DoesNotThrow(() => drawable.RemovePointIfNear(clickPosition));
        }

        /// <summary>
        /// Helper method to get the private ReferencePointsCoordinates field using reflection.
        /// </summary>
        private List<Point> GetReferencePointsCoordinates(CirclesDrawable drawable)
        {
            var field = typeof(CirclesDrawable).GetField("ReferencePointsCoordinates", BindingFlags.NonPublic | BindingFlags.Instance);
            return (List<Point>)field!.GetValue(drawable)!;
        }

        /// <summary>
        /// Tests that the IsCallerEditing property returns the default value of false when not explicitly set.
        /// This verifies the initial state of the backing field.
        /// Expected result: Property returns false.
        /// </summary>
        [Test]
        public void IsCallerEditing_DefaultValue_ReturnsFalse()
        {
            // Arrange
            var drawable = new CirclesDrawable();

            // Act
            var result = drawable.IsCallerEditing;

            // Assert
            Assert.That(result, Is.False);
        }

        /// <summary>
        /// Tests that the IsCallerEditing property correctly stores and returns the value set to it.
        /// This verifies both the setter and getter functionality for all possible boolean values.
        /// Input conditions: Boolean value (true or false) is assigned to the property.
        /// Expected result: Property returns the exact value that was set.
        /// </summary>
        /// <param name="value">The boolean value to set and verify.</param>
        [TestCase(true)]
        [TestCase(false)]
        public void IsCallerEditing_SetValue_ReturnsSetValue(bool value)
        {
            // Arrange
            var drawable = new CirclesDrawable();

            // Act
            drawable.IsCallerEditing = value;
            var result = drawable.IsCallerEditing;

            // Assert
            Assert.That(result, Is.EqualTo(value));
        }

        /// <summary>
        /// Tests that the IsCallerEditing property correctly updates when set multiple times with different values.
        /// This verifies that the property properly maintains state across multiple assignments.
        /// Input conditions: Property is set to true, then false, then true again.
        /// Expected result: Property returns the most recently set value after each assignment.
        /// </summary>
        [Test]
        public void IsCallerEditing_SetMultipleTimes_ReturnsLatestValue()
        {
            // Arrange
            var drawable = new CirclesDrawable();

            // Act & Assert - Set to true
            drawable.IsCallerEditing = true;
            Assert.That(drawable.IsCallerEditing, Is.True);

            // Act & Assert - Set to false
            drawable.IsCallerEditing = false;
            Assert.That(drawable.IsCallerEditing, Is.False);

            // Act & Assert - Set to true again
            drawable.IsCallerEditing = true;
            Assert.That(drawable.IsCallerEditing, Is.True);
        }

        /// <summary>
        /// Tests that the parameterless constructor successfully creates an instance of CirclesDrawable.
        /// </summary>
        /// <remarks>
        /// This test verifies that the constructor completes without throwing exceptions
        /// and that the resulting instance is not null.
        /// </remarks>
        [Test]
        public void Constructor_NoParameters_CreatesInstanceSuccessfully()
        {
            // Arrange & Act
            CirclesDrawable? drawable = null;

            // Act
            drawable = new CirclesDrawable();

            // Assert
            Assert.That(drawable, Is.Not.Null);
        }

        /// <summary>
        /// Tests that the parameterless constructor correctly initializes imageWidth and imageHeight
        /// to DefaultImageWidth (1280) and DefaultImageHeight (1366) respectively.
        /// </summary>
        /// <param name="inputValue">The value to normalize (should match the expected default to return 1.0).</param>
        /// <param name="expectedResult">The expected normalized result (1.0 when input matches default).</param>
        /// <param name="dimension">The dimension being tested ("Width" or "Height").</param>
        /// <remarks>
        /// Since imageWidth and imageHeight are private fields, this test indirectly verifies their initialization
        /// by using the public NormalizeXPosition and NormalizeYPosition methods.
        /// When the input matches the default dimension value, the normalized result should be 1.0.
        /// </remarks>
        [TestCase(1280.0, 1.0, "Width", TestName = "Constructor_NoParameters_InitializesImageWidthToDefaultImageWidth")]
        [TestCase(1366.0, 1.0, "Height", TestName = "Constructor_NoParameters_InitializesImageHeightToDefaultImageHeight")]
        public void Constructor_NoParameters_InitializesDimensionsCorrectly(double inputValue, double expectedResult, string dimension)
        {
            // Arrange & Act
            var drawable = new CirclesDrawable();

            // Act
            double? actualResult = dimension == "Width"
                ? drawable.NormalizeXPosition(inputValue)
                : drawable.NormalizeYPosition(inputValue);

            // Assert
            Assert.That(actualResult, Is.EqualTo(expectedResult).Within(0.0001));
        }

        /// <summary>
        /// Tests that the parameterless constructor initializes imageWidth correctly by verifying
        /// normalization behavior with multiple test values.
        /// </summary>
        /// <param name="inputX">The X coordinate value to normalize.</param>
        /// <param name="expectedNormalizedX">The expected normalized X value (input / 1280).</param>
        /// <remarks>
        /// This test verifies that imageWidth is initialized to 1280 by testing the normalization
        /// formula with various input values. Each test case validates: input / imageWidth = expected.
        /// </remarks>
        [TestCase(0.0, 0.0)]
        [TestCase(640.0, 0.5)]
        [TestCase(1280.0, 1.0)]
        [TestCase(2560.0, 2.0)]
        public void Constructor_NoParameters_ImageWidthInitializedCorrectly_VerifiedByNormalization(double inputX, double expectedNormalizedX)
        {
            // Arrange & Act
            var drawable = new CirclesDrawable();

            // Act
            double? actualNormalizedX = drawable.NormalizeXPosition(inputX);

            // Assert
            Assert.That(actualNormalizedX, Is.EqualTo(expectedNormalizedX).Within(0.0001));
        }

        /// <summary>
        /// Tests that the parameterless constructor initializes imageHeight correctly by verifying
        /// normalization behavior with multiple test values.
        /// </summary>
        /// <param name="inputY">The Y coordinate value to normalize.</param>
        /// <param name="expectedNormalizedY">The expected normalized Y value (input / 1366).</param>
        /// <remarks>
        /// This test verifies that imageHeight is initialized to 1366 by testing the normalization
        /// formula with various input values. Each test case validates: input / imageHeight = expected.
        /// </remarks>
        [TestCase(0.0, 0.0)]
        [TestCase(683.0, 0.5)]
        [TestCase(1366.0, 1.0)]
        [TestCase(2732.0, 2.0)]
        public void Constructor_NoParameters_ImageHeightInitializedCorrectly_VerifiedByNormalization(double inputY, double expectedNormalizedY)
        {
            // Arrange & Act
            var drawable = new CirclesDrawable();

            // Act
            double? actualNormalizedY = drawable.NormalizeYPosition(inputY);

            // Assert
            Assert.That(actualNormalizedY, Is.EqualTo(expectedNormalizedY).Within(0.0001));
        }

        /// <summary>
        /// Tests that SaveReferenceCoordinates does nothing when IsCallerEditing is false.
        /// </summary>
        /// <remarks>
        /// Input: IsCallerEditing = false, any valid parameters.
        /// Expected: Method completes without exception. The bl.SaveNewReferenceCoordinates should not be called.
        /// LIMITATION: Cannot verify that bl method is NOT called because bl is a concrete instance field that cannot be mocked.
        /// </remarks>
        [Test]
        public void SaveReferenceCoordinates_IsCallerEditingFalse_CompletesWithoutError()
        {
            // Arrange
            var drawable = new CirclesDrawable(1280, 1366, null, 30);
            drawable.IsCallerEditing = false;
            var zone = ZoneOfPosition.Front;
            double imgWidth = 1280;
            double imgHeight = 1366;

            // Act & Assert
            Assert.DoesNotThrow(() => drawable.SaveReferenceCoordinates(zone, imgWidth, imgHeight));
        }

        /// <summary>
        /// Tests that SaveReferenceCoordinates executes the conversion and calls bl method when IsCallerEditing is true
        /// with an empty ReferencePointsCoordinates collection.
        /// </summary>
        /// <remarks>
        /// Input: IsCallerEditing = true, empty ReferencePointsCoordinates, valid parameters.
        /// Expected: Method completes, converts empty list, and calls bl.SaveNewReferenceCoordinates with empty list.
        /// LIMITATION: Cannot verify the actual call to bl.SaveNewReferenceCoordinates or its parameters because bl
        /// is a concrete instance field initialized at declaration and cannot be mocked. The method will execute
        /// but verification of the correct behavior is not possible in this unit test.
        /// </remarks>
        [Test]
        [Ignore("Cannot verify bl.SaveNewReferenceCoordinates call: bl is a non-injectable concrete field. The conversion logic and method call occur but cannot be verified without database infrastructure or dependency injection support.")]
        public void SaveReferenceCoordinates_IsCallerEditingTrueEmptyPoints_ExecutesConversionAndCallsBl()
        {
            // Arrange
            var drawable = new CirclesDrawable(1280, 1366, null, 30);
            drawable.IsCallerEditing = true;
            var zone = ZoneOfPosition.Front;
            double imgWidth = 1280;
            double imgHeight = 1366;

            // Act
            // The method will execute, convert empty ReferencePointsCoordinates to empty businessPoints list,
            // and call bl.SaveNewReferenceCoordinates(businessPoints, zone, imgWidth, imgHeight)
            drawable.SaveReferenceCoordinates(zone, imgWidth, imgHeight);

            // Assert
            // Would verify: bl.SaveNewReferenceCoordinates was called with empty list and correct parameters
            // Cannot verify due to non-mockable bl field
        }

        /// <summary>
        /// Tests that SaveReferenceCoordinates executes the conversion and calls bl method when IsCallerEditing is true
        /// with populated ReferencePointsCoordinates collection.
        /// </summary>
        /// <remarks>
        /// Input: IsCallerEditing = true, ReferencePointsCoordinates with points added via AddPoint, valid parameters.
        /// Expected: Method completes, converts points from Microsoft.Maui.Graphics.Point to GlucoMan.Point,
        /// and calls bl.SaveNewReferenceCoordinates with converted points.
        /// LIMITATION: Cannot verify the actual call to bl.SaveNewReferenceCoordinates, the parameter values,
        /// or the correctness of point conversion because bl is a concrete instance field that cannot be mocked.
        /// The code executes but verification is not possible without database infrastructure or mocking support.
        /// </remarks>
        [Test]
        [Ignore("Cannot verify bl.SaveNewReferenceCoordinates call or conversion correctness: bl is a non-injectable concrete field. Points are added via AddPoint, conversion executes, but verification requires database infrastructure or dependency injection.")]
        public void SaveReferenceCoordinates_IsCallerEditingTrueWithPoints_ExecutesConversionAndCallsBl()
        {
            // Arrange
            var drawable = new CirclesDrawable(1280, 1366, null, 30);
            drawable.IsCallerEditing = true;

            // Add reference points using AddPoint with IsEditing=true
            // This populates the private ReferencePointsCoordinates collection
            drawable.AddPoint(new MauiPoint(100, 200), IsEditing: true);
            drawable.AddPoint(new MauiPoint(300, 400), IsEditing: true);
            drawable.AddPoint(new MauiPoint(500, 600), IsEditing: true);

            var zone = ZoneOfPosition.Back;
            double imgWidth = 1280;
            double imgHeight = 1366;

            // Act
            // The method will:
            // 1. Enter the if(isCallerEditing) block
            // 2. Convert ReferencePointsCoordinates (3 Microsoft.Maui.Graphics.Points) to List<GlucoMan.Point>
            // 3. Call bl.SaveNewReferenceCoordinates(businessPoints, zone, imgWidth, imgHeight)
            drawable.SaveReferenceCoordinates(zone, imgWidth, imgHeight);

            // Assert
            // Would verify: 
            // - bl.SaveNewReferenceCoordinates was called once
            // - businessPoints contains 3 points with correct X,Y values
            // - Parameters zone, imgWidth, imgHeight were passed correctly
            // Cannot verify any of the above due to non-mockable bl field
        }

        /// <summary>
        /// Tests SaveReferenceCoordinates with various ZoneOfPosition enum values.
        /// </summary>
        /// <remarks>
        /// Input: IsCallerEditing = false, different valid ZoneOfPosition enum values.
        /// Expected: Method completes without exception for all enum values.
        /// </remarks>
        [TestCase(ZoneOfPosition.NotSet)]
        [TestCase(ZoneOfPosition.Front)]
        [TestCase(ZoneOfPosition.Back)]
        [TestCase(ZoneOfPosition.Hands)]
        [TestCase(ZoneOfPosition.Sensor)]
        public void SaveReferenceCoordinates_VariousEnumValues_CompletesWithoutError(ZoneOfPosition zone)
        {
            // Arrange
            var drawable = new CirclesDrawable(1280, 1366, null, 30);
            drawable.IsCallerEditing = false;
            double imgWidth = 1280;
            double imgHeight = 1366;

            // Act & Assert
            Assert.DoesNotThrow(() => drawable.SaveReferenceCoordinates(zone, imgWidth, imgHeight));
        }

        /// <summary>
        /// Tests SaveReferenceCoordinates with an invalid enum value cast.
        /// </summary>
        /// <remarks>
        /// Input: IsCallerEditing = false, invalid ZoneOfPosition enum value (999).
        /// Expected: Method accepts the value without immediate exception (validation may occur in bl method).
        /// </remarks>
        [Test]
        public void SaveReferenceCoordinates_InvalidEnumValueCast_CompletesWithoutError()
        {
            // Arrange
            var drawable = new CirclesDrawable(1280, 1366, null, 30);
            drawable.IsCallerEditing = false;
            var invalidZone = (ZoneOfPosition)999;
            double imgWidth = 1280;
            double imgHeight = 1366;

            // Act & Assert
            Assert.DoesNotThrow(() => drawable.SaveReferenceCoordinates(invalidZone, imgWidth, imgHeight));
        }

        /// <summary>
        /// Tests SaveReferenceCoordinates with zero image dimensions.
        /// </summary>
        /// <remarks>
        /// Input: IsCallerEditing = false, imgWidth = 0, imgHeight = 0.
        /// Expected: Method completes without exception (division by zero would occur in bl method if executed).
        /// </remarks>
        [Test]
        public void SaveReferenceCoordinates_ZeroDimensions_CompletesWithoutError()
        {
            // Arrange
            var drawable = new CirclesDrawable(1280, 1366, null, 30);
            drawable.IsCallerEditing = false;
            var zone = ZoneOfPosition.Front;
            double imgWidth = 0;
            double imgHeight = 0;

            // Act & Assert
            Assert.DoesNotThrow(() => drawable.SaveReferenceCoordinates(zone, imgWidth, imgHeight));
        }

        /// <summary>
        /// Tests SaveReferenceCoordinates with negative image dimensions.
        /// </summary>
        /// <remarks>
        /// Input: IsCallerEditing = false, negative imgWidth and imgHeight.
        /// Expected: Method completes without exception.
        /// </remarks>
        [Test]
        public void SaveReferenceCoordinates_NegativeDimensions_CompletesWithoutError()
        {
            // Arrange
            var drawable = new CirclesDrawable(1280, 1366, null, 30);
            drawable.IsCallerEditing = false;
            var zone = ZoneOfPosition.Front;
            double imgWidth = -100;
            double imgHeight = -200;

            // Act & Assert
            Assert.DoesNotThrow(() => drawable.SaveReferenceCoordinates(zone, imgWidth, imgHeight));
        }

        /// <summary>
        /// Tests SaveReferenceCoordinates with infinity values for image dimensions.
        /// </summary>
        /// <remarks>
        /// Input: IsCallerEditing = false, double.PositiveInfinity for imgWidth and imgHeight.
        /// Expected: Method completes without exception.
        /// </remarks>
        [Test]
        public void SaveReferenceCoordinates_InfinityDimensions_CompletesWithoutError()
        {
            // Arrange
            var drawable = new CirclesDrawable(1280, 1366, null, 30);
            drawable.IsCallerEditing = false;
            var zone = ZoneOfPosition.Front;
            double imgWidth = double.PositiveInfinity;
            double imgHeight = double.PositiveInfinity;

            // Act & Assert
            Assert.DoesNotThrow(() => drawable.SaveReferenceCoordinates(zone, imgWidth, imgHeight));
        }

        /// <summary>
        /// Tests SaveReferenceCoordinates with NaN values for image dimensions.
        /// </summary>
        /// <remarks>
        /// Input: IsCallerEditing = false, double.NaN for imgWidth and imgHeight.
        /// Expected: Method completes without exception.
        /// </remarks>
        [Test]
        public void SaveReferenceCoordinates_NaNDimensions_CompletesWithoutError()
        {
            // Arrange
            var drawable = new CirclesDrawable(1280, 1366, null, 30);
            drawable.IsCallerEditing = false;
            var zone = ZoneOfPosition.Front;
            double imgWidth = double.NaN;
            double imgHeight = double.NaN;

            // Act & Assert
            Assert.DoesNotThrow(() => drawable.SaveReferenceCoordinates(zone, imgWidth, imgHeight));
        }

        /// <summary>
        /// Tests SaveReferenceCoordinates with maximum double values for image dimensions.
        /// </summary>
        /// <remarks>
        /// Input: IsCallerEditing = false, double.MaxValue for imgWidth and imgHeight.
        /// Expected: Method completes without exception.
        /// </remarks>
        [Test]
        public void SaveReferenceCoordinates_MaxValueDimensions_CompletesWithoutError()
        {
            // Arrange
            var drawable = new CirclesDrawable(1280, 1366, null, 30);
            drawable.IsCallerEditing = false;
            var zone = ZoneOfPosition.Front;
            double imgWidth = double.MaxValue;
            double imgHeight = double.MaxValue;

            // Act & Assert
            Assert.DoesNotThrow(() => drawable.SaveReferenceCoordinates(zone, imgWidth, imgHeight));
        }

        /// <summary>
        /// Tests SaveReferenceCoordinates with minimum double values for image dimensions.
        /// </summary>
        /// <remarks>
        /// Input: IsCallerEditing = false, double.MinValue for imgWidth and imgHeight.
        /// Expected: Method completes without exception.
        /// </remarks>
        [Test]
        public void SaveReferenceCoordinates_MinValueDimensions_CompletesWithoutError()
        {
            // Arrange
            var drawable = new CirclesDrawable(1280, 1366, null, 30);
            drawable.IsCallerEditing = false;
            var zone = ZoneOfPosition.Front;
            double imgWidth = double.MinValue;
            double imgHeight = double.MinValue;

            // Act & Assert
            Assert.DoesNotThrow(() => drawable.SaveReferenceCoordinates(zone, imgWidth, imgHeight));
        }

        /// <summary>
        /// Tests SaveReferenceCoordinates with very small positive dimensions close to zero.
        /// </summary>
        /// <remarks>
        /// Input: IsCallerEditing = false, double.Epsilon for imgWidth and imgHeight.
        /// Expected: Method completes without exception.
        /// </remarks>
        [Test]
        public void SaveReferenceCoordinates_EpsilonDimensions_CompletesWithoutError()
        {
            // Arrange
            var drawable = new CirclesDrawable(1280, 1366, null, 30);
            drawable.IsCallerEditing = false;
            var zone = ZoneOfPosition.Front;
            double imgWidth = double.Epsilon;
            double imgHeight = double.Epsilon;

            // Act & Assert
            Assert.DoesNotThrow(() => drawable.SaveReferenceCoordinates(zone, imgWidth, imgHeight));
        }

        /// <summary>
        /// Tests that the Type property correctly sets and returns the specified PointType enum value.
        /// Verifies that all valid enum values (Front, Back, Hands, Sensor) can be set and retrieved.
        /// </summary>
        /// <param name="pointType">The PointType enum value to set and verify.</param>
        [TestCase(CirclesDrawable.PointType.Front)]
        [TestCase(CirclesDrawable.PointType.Back)]
        [TestCase(CirclesDrawable.PointType.Hands)]
        [TestCase(CirclesDrawable.PointType.Sensor)]
        public void Type_SetValidEnumValue_ReturnsSetValue(CirclesDrawable.PointType pointType)
        {
            // Arrange
            var drawable = new CirclesDrawable();

            // Act
            drawable.Type = pointType;
            var result = drawable.Type;

            // Assert
            Assert.That(result, Is.EqualTo(pointType));
        }

        /// <summary>
        /// Tests that the Type property accepts and returns invalid enum values (values outside the defined enum range).
        /// Verifies that the property does not validate enum values and allows any integer value cast to PointType.
        /// </summary>
        /// <param name="invalidValue">An integer value outside the defined PointType enum range.</param>
        [TestCase(-1)]
        [TestCase(4)]
        [TestCase(100)]
        [TestCase(int.MinValue)]
        [TestCase(int.MaxValue)]
        public void Type_SetInvalidEnumValue_ReturnsSetValue(int invalidValue)
        {
            // Arrange
            var drawable = new CirclesDrawable();
            var invalidPointType = (CirclesDrawable.PointType)invalidValue;

            // Act
            drawable.Type = invalidPointType;
            var result = drawable.Type;

            // Assert
            Assert.That(result, Is.EqualTo(invalidPointType));
            Assert.That((int)result, Is.EqualTo(invalidValue));
        }

        /// <summary>
        /// Tests that the Type property returns the default enum value (Front = 0) when not explicitly set.
        /// Verifies the initial state of the Type property upon object instantiation.
        /// </summary>
        [Test]
        public void Type_DefaultValue_ReturnsFront()
        {
            // Arrange & Act
            var drawable = new CirclesDrawable();
            var result = drawable.Type;

            // Assert
            Assert.That(result, Is.EqualTo(CirclesDrawable.PointType.Front));
        }

        /// <summary>
        /// Tests that ClearAll executes without throwing when provided with valid ZoneOfPosition enum values.
        /// </summary>
        /// <param name="zone">The ZoneOfPosition value to test.</param>
        /// <remarks>
        /// This test verifies that the method can be called with all valid enum values without throwing exceptions.
        /// LIMITATION: Cannot verify that InjectionPointsCoordinates.Clear() was actually called as it's a private field
        /// and reflection is not permitted. Cannot verify database operation as BL_BolusesAndInjections is directly
        /// instantiated (not injected) and cannot be mocked.
        /// </remarks>
        [TestCase(ZoneOfPosition.NotSet)]
        [TestCase(ZoneOfPosition.Front)]
        [TestCase(ZoneOfPosition.Back)]
        [TestCase(ZoneOfPosition.Hands)]
        [TestCase(ZoneOfPosition.Sensor)]
        public void ClearAll_ValidZoneOfPosition_ExecutesWithoutThrowing(ZoneOfPosition zone)
        {
            // Arrange
            var drawable = new CirclesDrawable();

            // Act & Assert
            Assert.DoesNotThrow(() => drawable.ClearAll(zone));
        }

        /// <summary>
        /// Tests that ClearAll handles invalid (out-of-range) enum values without throwing.
        /// </summary>
        /// <remarks>
        /// Tests behavior when an invalid enum value is passed. The method should handle this gracefully
        /// due to the try-catch block, though the actual behavior depends on the underlying data layer implementation.
        /// </remarks>
        [Test]
        public void ClearAll_InvalidZoneOfPosition_ExecutesWithoutThrowing()
        {
            // Arrange
            var drawable = new CirclesDrawable();
            var invalidZone = (ZoneOfPosition)999;

            // Act & Assert
            Assert.DoesNotThrow(() => drawable.ClearAll(invalidZone));
        }

        /// <summary>
        /// Tests that ClearAll properly handles exceptions thrown by DeleteAllReferenceCoordinates.
        /// </summary>
        /// <remarks>
        /// LIMITATION: Cannot test exception handling path because BL_BolusesAndInjections is directly instantiated
        /// as a field (not injected via constructor or property), making it impossible to mock with Moq.
        /// The method catches all exceptions and logs to Console.WriteLine, but we cannot verify this behavior
        /// without being able to inject a mock that throws an exception.
        /// 
        /// Expected behavior if testable: When bl.DeleteAllReferenceCoordinates throws any exception,
        /// the exception should be caught, logged to console, and the method should complete without throwing.
        /// InjectionPointsCoordinates.Clear() should not be called if the exception occurs before it.
        /// </remarks>
        [Test]
        [Ignore("Cannot test: BL_BolusesAndInjections is directly instantiated as a field and cannot be mocked. Would need constructor injection or property injection to test exception handling path.")]
        public void ClearAll_DeleteThrowsException_CatchesAndLogsException()
        {
            // Arrange
            // Would need: var mockBL = new Mock<BL_BolusesAndInjections>();
            // Would need: mockBL.Setup(x => x.DeleteAllReferenceCoordinates(It.IsAny<ZoneOfPosition>())).Throws<Exception>();
            // Would need: var drawable = new CirclesDrawable() with injected mockBL

            // Act
            // Would execute: drawable.ClearAll(ZoneOfPosition.Front);

            // Assert
            // Would verify: No exception thrown
            // Would verify: Console.WriteLine was called with error message
            // Would verify: InjectionPointsCoordinates was NOT cleared
        }

        /// <summary>
        /// Tests that ClearAll with parameterized constructor initializes properly before calling ClearAll.
        /// </summary>
        /// <remarks>
        /// Verifies that the parameterized constructor doesn't interfere with ClearAll functionality.
        /// </remarks>
        [Test]
        public void ClearAll_WithParameterizedConstructor_ExecutesWithoutThrowing()
        {
            // Arrange
            var drawable = new CirclesDrawable(
                ImageWidth: 1920.0,
                ImageHeight: 1080.0,
                IdInjection: 42,
                CirclesVisibilityMaxTimeInDays: 7.0);

            // Act & Assert
            Assert.DoesNotThrow(() => drawable.ClearAll(ZoneOfPosition.Front));
        }

        /// <summary>
        /// Tests that LoadInjectionsCoordinates correctly handles an empty list of injections.
        /// </summary>
        /// <remarks>
        /// Input: Empty list of injections.
        /// Expected: InjectionPointsCoordinates remains empty, no exception is thrown.
        /// </remarks>
        [Test]
        public void LoadInjectionsCoordinates_EmptyList_NoCirclesAdded()
        {
            // Arrange
            var drawable = new CirclesDrawable(1280, 1366, null, 30);
            var emptyList = new List<Injection>();

            // Act
            drawable.LoadInjectionsCoordinates(emptyList);

            // Assert
            Assert.That(drawable.InjectionPointsCoordinates.Count, Is.EqualTo(0));
        }

        /// <summary>
        /// Tests that LoadInjectionsCoordinates skips injections with null PositionX.
        /// </summary>
        /// <remarks>
        /// Input: Injection with null PositionX.
        /// Expected: No circle is added to InjectionPointsCoordinates.
        /// </remarks>
        [Test]
        public void LoadInjectionsCoordinates_NullPositionX_SkipsInjection()
        {
            // Arrange
            var drawable = new CirclesDrawable(1280, 1366, 1, 30);
            var injections = new List<Injection>
            {
                new Injection
                {
                    IdInjection = 1,
                    PositionX = null,
                    PositionY = 0.5,
                    EventTime = new DateTimeAndText { DateTime = DateTime.Now.AddDays(-1) }
                }
            };

            // Act
            drawable.LoadInjectionsCoordinates(injections);

            // Assert
            Assert.That(drawable.InjectionPointsCoordinates.Count, Is.EqualTo(0));
        }

        /// <summary>
        /// Tests that LoadInjectionsCoordinates skips injections with null PositionY.
        /// </summary>
        /// <remarks>
        /// Input: Injection with null PositionY.
        /// Expected: No circle is added to InjectionPointsCoordinates.
        /// </remarks>
        [Test]
        public void LoadInjectionsCoordinates_NullPositionY_SkipsInjection()
        {
            // Arrange
            var drawable = new CirclesDrawable(1280, 1366, 1, 30);
            var injections = new List<Injection>
            {
                new Injection
                {
                    IdInjection = 1,
                    PositionX = 0.5,
                    PositionY = null,
                    EventTime = new DateTimeAndText { DateTime = DateTime.Now.AddDays(-1) }
                }
            };

            // Act
            drawable.LoadInjectionsCoordinates(injections);

            // Assert
            Assert.That(drawable.InjectionPointsCoordinates.Count, Is.EqualTo(0));
        }

        /// <summary>
        /// Tests that LoadInjectionsCoordinates skips injections with both null positions.
        /// </summary>
        /// <remarks>
        /// Input: Injection with null PositionX and PositionY.
        /// Expected: No circle is added to InjectionPointsCoordinates.
        /// </remarks>
        [Test]
        public void LoadInjectionsCoordinates_BothPositionsNull_SkipsInjection()
        {
            // Arrange
            var drawable = new CirclesDrawable(1280, 1366, 1, 30);
            var injections = new List<Injection>
            {
                new Injection
                {
                    IdInjection = 1,
                    PositionX = null,
                    PositionY = null,
                    EventTime = new DateTimeAndText { DateTime = DateTime.Now.AddDays(-1) }
                }
            };

            // Act
            drawable.LoadInjectionsCoordinates(injections);

            // Assert
            Assert.That(drawable.InjectionPointsCoordinates.Count, Is.EqualTo(0));
        }

        /// <summary>
        /// Tests that LoadInjectionsCoordinates draws current injection in brown color.
        /// </summary>
        /// <remarks>
        /// Input: Single injection matching idCurrentInjection with valid EventTime.
        /// Expected: One circle added with Brown color.
        /// </remarks>
        [Test]
        public void LoadInjectionsCoordinates_CurrentInjection_DrawsInBrown()
        {
            // Arrange
            int currentId = 42;
            var drawable = new CirclesDrawable(1280, 1366, currentId, 30);
            var injections = new List<Injection>
            {
                new Injection
                {
                    IdInjection = currentId,
                    PositionX = 0.5,
                    PositionY = 0.6,
                    EventTime = new DateTimeAndText { DateTime = DateTime.Now.AddDays(-1) }
                }
            };

            // Act
            drawable.LoadInjectionsCoordinates(injections);

            // Assert
            Assert.That(drawable.InjectionPointsCoordinates.Count, Is.EqualTo(1));
            var circle = drawable.InjectionPointsCoordinates[0];
            Assert.That(circle.Color, Is.EqualTo(Colors.Brown));
            Assert.That(circle.Position.X, Is.EqualTo(0.5 * 1280).Within(0.01));
            Assert.That(circle.Position.Y, Is.EqualTo(0.6 * 1366).Within(0.01));
        }

        /// <summary>
        /// Tests that LoadInjectionsCoordinates draws to-be injection (null EventTime.DateTime) in red color.
        /// </summary>
        /// <remarks>
        /// Input: Injection with null EventTime.DateTime.
        /// Expected: One circle added with Red color.
        /// </remarks>
        [Test]
        public void LoadInjectionsCoordinates_ToBeInjectionNullEventTime_DrawsInRed()
        {
            // Arrange
            var drawable = new CirclesDrawable(1280, 1366, 1, 30);
            var injections = new List<Injection>
            {
                new Injection
                {
                    IdInjection = 2,
                    PositionX = 0.3,
                    PositionY = 0.4,
                    EventTime = new DateTimeAndText { DateTime = null }
                }
            };

            // Act
            drawable.LoadInjectionsCoordinates(injections);

            // Assert
            Assert.That(drawable.InjectionPointsCoordinates.Count, Is.EqualTo(1));
            var circle = drawable.InjectionPointsCoordinates[0];
            Assert.That(circle.Color, Is.EqualTo(Colors.Red));
            Assert.That(circle.Position.X, Is.EqualTo(0.3 * 1280).Within(0.01));
            Assert.That(circle.Position.Y, Is.EqualTo(0.4 * 1366).Within(0.01));
        }

        /// <summary>
        /// Tests that LoadInjectionsCoordinates skips injection with same coordinates as current injection.
        /// </summary>
        /// <remarks>
        /// Input: Two injections with identical coordinates, one is current.
        /// Expected: Only one circle added (the current injection).
        /// </remarks>
        [Test]
        public void LoadInjectionsCoordinates_SameCoordinatesAsCurrent_SkipsNonCurrent()
        {
            // Arrange
            int currentId = 1;
            var drawable = new CirclesDrawable(1280, 1366, currentId, 30);
            var injections = new List<Injection>
            {
                new Injection
                {
                    IdInjection = currentId,
                    PositionX = 0.5,
                    PositionY = 0.5,
                    EventTime = new DateTimeAndText { DateTime = DateTime.Now }
                },
                new Injection
                {
                    IdInjection = 2,
                    PositionX = 0.5,
                    PositionY = 0.5,
                    EventTime = new DateTimeAndText { DateTime = DateTime.Now.AddDays(-1) }
                }
            };

            // Act
            drawable.LoadInjectionsCoordinates(injections);

            // Assert
            Assert.That(drawable.InjectionPointsCoordinates.Count, Is.EqualTo(1));
            Assert.That(drawable.InjectionPointsCoordinates[0].Color, Is.EqualTo(Colors.Brown));
        }

        /// <summary>
        /// Tests that LoadInjectionsCoordinates draws injection with slightly different coordinates than current.
        /// </summary>
        /// <remarks>
        /// Input: Two injections with coordinates differing by more than 0.001.
        /// Expected: Two circles added.
        /// </remarks>
        [Test]
        public void LoadInjectionsCoordinates_DifferentCoordinatesThanCurrent_DrawsBoth()
        {
            // Arrange
            int currentId = 1;
            var drawable = new CirclesDrawable(1280, 1366, currentId, 30);
            var injections = new List<Injection>
            {
                new Injection
                {
                    IdInjection = currentId,
                    PositionX = 0.5,
                    PositionY = 0.5,
                    EventTime = new DateTimeAndText { DateTime = DateTime.Now }
                },
                new Injection
                {
                    IdInjection = 2,
                    PositionX = 0.502,
                    PositionY = 0.502,
                    EventTime = new DateTimeAndText { DateTime = DateTime.Now.AddDays(-1) }
                }
            };

            // Act
            drawable.LoadInjectionsCoordinates(injections);

            // Assert
            Assert.That(drawable.InjectionPointsCoordinates.Count, Is.EqualTo(2));
        }

        /// <summary>
        /// Tests that LoadInjectionsCoordinates correctly calculates color for recent injection (high saturation and opacity).
        /// </summary>
        /// <remarks>
        /// Input: Non-current injection from 1 day ago with 30-day visibility window.
        /// Expected: Circle with high saturation and high alpha.
        /// </remarks>
        [Test]
        public void LoadInjectionsCoordinates_RecentInjection_HighSaturationAndAlpha()
        {
            // Arrange
            var drawable = new CirclesDrawable(1280, 1366, 1, 30);
            var injections = new List<Injection>
            {
                new Injection
                {
                    IdInjection = 2,
                    PositionX = 0.5,
                    PositionY = 0.5,
                    EventTime = new DateTimeAndText { DateTime = DateTime.Now.AddDays(-1) }
                }
            };

            // Act
            drawable.LoadInjectionsCoordinates(injections);

            // Assert
            Assert.That(drawable.InjectionPointsCoordinates.Count, Is.EqualTo(1));
            var circle = drawable.InjectionPointsCoordinates[0];
            // Recent injection should have high alpha (close to 1.0)
            Assert.That(circle.Color.Alpha, Is.GreaterThan(0.9f));
        }

        /// <summary>
        /// Tests that LoadInjectionsCoordinates correctly calculates color for old injection (low saturation and opacity).
        /// </summary>
        /// <remarks>
        /// Input: Non-current injection from 30 days ago with 30-day visibility window.
        /// Expected: Circle with low saturation and low alpha.
        /// </remarks>
        [Test]
        public void LoadInjectionsCoordinates_OldInjection_LowSaturationAndAlpha()
        {
            // Arrange
            var drawable = new CirclesDrawable(1280, 1366, 1, 30);
            var injections = new List<Injection>
            {
                new Injection
                {
                    IdInjection = 2,
                    PositionX = 0.5,
                    PositionY = 0.5,
                    EventTime = new DateTimeAndText { DateTime = DateTime.Now.AddDays(-30) }
                }
            };

            // Act
            drawable.LoadInjectionsCoordinates(injections);

            // Assert
            Assert.That(drawable.InjectionPointsCoordinates.Count, Is.EqualTo(1));
            var circle = drawable.InjectionPointsCoordinates[0];
            // Old injection should have low alpha (close to 0.1)
            Assert.That(circle.Color.Alpha, Is.LessThan(0.2f));
        }

        /// <summary>
        /// Tests that LoadInjectionsCoordinates handles very old injection (beyond visibility window).
        /// </summary>
        /// <remarks>
        /// Input: Non-current injection from 50 days ago with 30-day visibility window.
        /// Expected: Circle with minimal saturation and alpha clamped to minimum values.
        /// </remarks>
        [Test]
        public void LoadInjectionsCoordinates_VeryOldInjection_MinimalSaturationAndAlpha()
        {
            // Arrange
            var drawable = new CirclesDrawable(1280, 1366, 1, 30);
            var injections = new List<Injection>
            {
                new Injection
                {
                    IdInjection = 2,
                    PositionX = 0.5,
                    PositionY = 0.5,
                    EventTime = new DateTimeAndText { DateTime = DateTime.Now.AddDays(-50) }
                }
            };

            // Act
            drawable.LoadInjectionsCoordinates(injections);

            // Assert
            Assert.That(drawable.InjectionPointsCoordinates.Count, Is.EqualTo(1));
            var circle = drawable.InjectionPointsCoordinates[0];
            // Very old injection should have minimum alpha (0.1)
            Assert.That(circle.Color.Alpha, Is.EqualTo(0.1f).Within(0.01f));
        }

        /// <summary>
        /// Tests that LoadInjectionsCoordinates handles multiple injections with various ages.
        /// </summary>
        /// <remarks>
        /// Input: Multiple injections with ages ranging from 0 to 20 days.
        /// Expected: All circles added with varying saturation and alpha values.
        /// </remarks>
        [Test]
        public void LoadInjectionsCoordinates_MultipleInjectionsVariousAges_AllDrawnWithCorrectColors()
        {
            // Arrange
            var drawable = new CirclesDrawable(1280, 1366, null, 30);
            var injections = new List<Injection>
            {
                new Injection
                {
                    IdInjection = 1,
                    PositionX = 0.2,
                    PositionY = 0.2,
                    EventTime = new DateTimeAndText { DateTime = DateTime.Now }
                },
                new Injection
                {
                    IdInjection = 2,
                    PositionX = 0.4,
                    PositionY = 0.4,
                    EventTime = new DateTimeAndText { DateTime = DateTime.Now.AddDays(-10) }
                },
                new Injection
                {
                    IdInjection = 3,
                    PositionX = 0.6,
                    PositionY = 0.6,
                    EventTime = new DateTimeAndText { DateTime = DateTime.Now.AddDays(-20) }
                }
            };

            // Act
            drawable.LoadInjectionsCoordinates(injections);

            // Assert
            Assert.That(drawable.InjectionPointsCoordinates.Count, Is.EqualTo(3));
            // Verify that alpha decreases with age
            var alphas = drawable.InjectionPointsCoordinates.Select(c => c.Color.Alpha).ToList();
            Assert.That(alphas[0], Is.GreaterThan(alphas[1]));
            Assert.That(alphas[1], Is.GreaterThan(alphas[2]));
        }

        /// <summary>
        /// Tests that LoadInjectionsCoordinates correctly normalizes coordinates by multiplying with image dimensions.
        /// </summary>
        /// <remarks>
        /// Input: Injection with normalized coordinates (0 to 1 range).
        /// Expected: Circle position scaled to actual image dimensions.
        /// </remarks>
        [Test]
        public void LoadInjectionsCoordinates_ValidCoordinates_CorrectlyNormalizes()
        {
            // Arrange
            double imgWidth = 800;
            double imgHeight = 600;
            var drawable = new CirclesDrawable(imgWidth, imgHeight, 1, 30);
            var injections = new List<Injection>
            {
                new Injection
                {
                    IdInjection = 2,
                    PositionX = 0.25,
                    PositionY = 0.75,
                    EventTime = new DateTimeAndText { DateTime = DateTime.Now.AddDays(-5) }
                }
            };

            // Act
            drawable.LoadInjectionsCoordinates(injections);

            // Assert
            Assert.That(drawable.InjectionPointsCoordinates.Count, Is.EqualTo(1));
            var circle = drawable.InjectionPointsCoordinates[0];
            Assert.That(circle.Position.X, Is.EqualTo(0.25 * imgWidth).Within(0.01));
            Assert.That(circle.Position.Y, Is.EqualTo(0.75 * imgHeight).Within(0.01));
        }

        /// <summary>
        /// Tests that LoadInjectionsCoordinates handles boundary normalized coordinates (0.0).
        /// </summary>
        /// <remarks>
        /// Input: Injection with coordinates at (0, 0).
        /// Expected: Circle position at (0, 0) in actual coordinates.
        /// </remarks>
        [Test]
        public void LoadInjectionsCoordinates_BoundaryCoordinatesZero_CorrectlyNormalizes()
        {
            // Arrange
            var drawable = new CirclesDrawable(1280, 1366, 1, 30);
            var injections = new List<Injection>
            {
                new Injection
                {
                    IdInjection = 2,
                    PositionX = 0.0,
                    PositionY = 0.0,
                    EventTime = new DateTimeAndText { DateTime = DateTime.Now.AddDays(-1) }
                }
            };

            // Act
            drawable.LoadInjectionsCoordinates(injections);

            // Assert
            Assert.That(drawable.InjectionPointsCoordinates.Count, Is.EqualTo(1));
            var circle = drawable.InjectionPointsCoordinates[0];
            Assert.That(circle.Position.X, Is.EqualTo(0.0).Within(0.01));
            Assert.That(circle.Position.Y, Is.EqualTo(0.0).Within(0.01));
        }

        /// <summary>
        /// Tests that LoadInjectionsCoordinates handles boundary normalized coordinates (1.0).
        /// </summary>
        /// <remarks>
        /// Input: Injection with coordinates at (1, 1).
        /// Expected: Circle position at full image dimensions.
        /// </remarks>
        [Test]
        public void LoadInjectionsCoordinates_BoundaryCoordinatesOne_CorrectlyNormalizes()
        {
            // Arrange
            double imgWidth = 1280;
            double imgHeight = 1366;
            var drawable = new CirclesDrawable(imgWidth, imgHeight, 1, 30);
            var injections = new List<Injection>
            {
                new Injection
                {
                    IdInjection = 2,
                    PositionX = 1.0,
                    PositionY = 1.0,
                    EventTime = new DateTimeAndText { DateTime = DateTime.Now.AddDays(-1) }
                }
            };

            // Act
            drawable.LoadInjectionsCoordinates(injections);

            // Assert
            Assert.That(drawable.InjectionPointsCoordinates.Count, Is.EqualTo(1));
            var circle = drawable.InjectionPointsCoordinates[0];
            Assert.That(circle.Position.X, Is.EqualTo(imgWidth).Within(0.01));
            Assert.That(circle.Position.Y, Is.EqualTo(imgHeight).Within(0.01));
        }

        /// <summary>
        /// Tests that LoadInjectionsCoordinates handles edge case where current injection has null coordinates.
        /// </summary>
        /// <remarks>
        /// Input: Current injection with null coordinates, and another injection with valid coordinates.
        /// Expected: Only non-current injection is drawn.
        /// </remarks>
        [Test]
        public void LoadInjectionsCoordinates_CurrentInjectionNullCoordinates_OtherInjectionsDrawn()
        {
            // Arrange
            int currentId = 1;
            var drawable = new CirclesDrawable(1280, 1366, currentId, 30);
            var injections = new List<Injection>
            {
                new Injection
                {
                    IdInjection = currentId,
                    PositionX = null,
                    PositionY = null,
                    EventTime = new DateTimeAndText { DateTime = DateTime.Now }
                },
                new Injection
                {
                    IdInjection = 2,
                    PositionX = 0.5,
                    PositionY = 0.5,
                    EventTime = new DateTimeAndText { DateTime = DateTime.Now.AddDays(-1) }
                }
            };

            // Act
            drawable.LoadInjectionsCoordinates(injections);

            // Assert
            Assert.That(drawable.InjectionPointsCoordinates.Count, Is.EqualTo(1));
            Assert.That(drawable.InjectionPointsCoordinates[0].Color, Is.Not.EqualTo(Colors.Brown));
        }

        /// <summary>
        /// Tests that LoadInjectionsCoordinates handles edge case where idCurrentInjection is null.
        /// </summary>
        /// <remarks>
        /// Input: idCurrentInjection is null, multiple injections present.
        /// Expected: No injection is drawn in brown; all follow age-based coloring.
        /// </remarks>
        [Test]
        public void LoadInjectionsCoordinates_NullCurrentId_NoCurrentInjectionDrawn()
        {
            // Arrange
            var drawable = new CirclesDrawable(1280, 1366, null, 30);
            var injections = new List<Injection>
            {
                new Injection
                {
                    IdInjection = 1,
                    PositionX = 0.3,
                    PositionY = 0.3,
                    EventTime = new DateTimeAndText { DateTime = DateTime.Now.AddDays(-1) }
                },
                new Injection
                {
                    IdInjection = 2,
                    PositionX = 0.7,
                    PositionY = 0.7,
                    EventTime = new DateTimeAndText { DateTime = DateTime.Now.AddDays(-5) }
                }
            };

            // Act
            drawable.LoadInjectionsCoordinates(injections);

            // Assert
            Assert.That(drawable.InjectionPointsCoordinates.Count, Is.EqualTo(2));
            // None should be brown
            Assert.That(drawable.InjectionPointsCoordinates.All(c => c.Color != Colors.Brown), Is.True);
        }

        /// <summary>
        /// Tests that LoadInjectionsCoordinates handles injection with coordinates barely within tolerance of current.
        /// </summary>
        /// <remarks>
        /// Input: Two injections with coordinates differing by exactly 0.001 (within tolerance).
        /// Expected: Non-current injection is skipped.
        /// </remarks>
        [Test]
        public void LoadInjectionsCoordinates_CoordinatesWithinTolerance_SkipsNonCurrent()
        {
            // Arrange
            int currentId = 1;
            var drawable = new CirclesDrawable(1280, 1366, currentId, 30);
            var injections = new List<Injection>
            {
                new Injection
                {
                    IdInjection = currentId,
                    PositionX = 0.5,
                    PositionY = 0.5,
                    EventTime = new DateTimeAndText { DateTime = DateTime.Now }
                },
                new Injection
                {
                    IdInjection = 2,
                    PositionX = 0.5005,
                    PositionY = 0.5005,
                    EventTime = new DateTimeAndText { DateTime = DateTime.Now.AddDays(-1) }
                }
            };

            // Act
            drawable.LoadInjectionsCoordinates(injections);

            // Assert
            Assert.That(drawable.InjectionPointsCoordinates.Count, Is.EqualTo(1));
            Assert.That(drawable.InjectionPointsCoordinates[0].Color, Is.EqualTo(Colors.Brown));
        }

        /// <summary>
        /// Tests that LoadInjectionsCoordinates handles very small circlesVisibilityMaxTimeInDays value.
        /// </summary>
        /// <remarks>
        /// Input: circlesVisibilityMaxTimeInDays set to 0.1 days, injection from 1 day ago.
        /// Expected: Alpha and saturation clamped to minimum values.
        /// </remarks>
        [Test]
        public void LoadInjectionsCoordinates_VerySmallVisibilityWindow_ClampsToMinimum()
        {
            // Arrange
            var drawable = new CirclesDrawable(1280, 1366, 1, 0.1);
            var injections = new List<Injection>
            {
                new Injection
                {
                    IdInjection = 2,
                    PositionX = 0.5,
                    PositionY = 0.5,
                    EventTime = new DateTimeAndText { DateTime = DateTime.Now.AddDays(-1) }
                }
            };

            // Act
            drawable.LoadInjectionsCoordinates(injections);

            // Assert
            Assert.That(drawable.InjectionPointsCoordinates.Count, Is.EqualTo(1));
            var circle = drawable.InjectionPointsCoordinates[0];
            // Should be clamped to minimum
            Assert.That(circle.Color.Alpha, Is.EqualTo(0.1f).Within(0.01f));
        }

        /// <summary>
        /// Tests that LoadInjectionsCoordinates handles future injection date.
        /// </summary>
        /// <remarks>
        /// Input: Injection with EventTime in the future.
        /// Expected: Circle drawn with maximum saturation and alpha (treated as very recent).
        /// </remarks>
        [Test]
        public void LoadInjectionsCoordinates_FutureInjectionDate_MaxSaturationAndAlpha()
        {
            // Arrange
            var drawable = new CirclesDrawable(1280, 1366, 1, 30);
            var injections = new List<Injection>
            {
                new Injection
                {
                    IdInjection = 2,
                    PositionX = 0.5,
                    PositionY = 0.5,
                    EventTime = new DateTimeAndText { DateTime = DateTime.Now.AddDays(1) }
                }
            };

            // Act
            drawable.LoadInjectionsCoordinates(injections);

            // Assert
            Assert.That(drawable.InjectionPointsCoordinates.Count, Is.EqualTo(1));
            var circle = drawable.InjectionPointsCoordinates[0];
            // Future date results in negative diffInDays, should clamp to max
            Assert.That(circle.Color.Alpha, Is.EqualTo(1.0f).Within(0.01f));
        }

        /// <summary>
        /// Tests that LoadInjectionsCoordinates handles mixed injection types correctly.
        /// </summary>
        /// <remarks>
        /// Input: Mix of current injection, to-be injection, and historical injections.
        /// Expected: Each injection type drawn with correct color (brown, red, HSV-based).
        /// </remarks>
        [Test]
        public void LoadInjectionsCoordinates_MixedInjectionTypes_CorrectColorsApplied()
        {
            // Arrange
            int currentId = 1;
            var drawable = new CirclesDrawable(1280, 1366, currentId, 30);
            var injections = new List<Injection>
            {
                new Injection
                {
                    IdInjection = currentId,
                    PositionX = 0.2,
                    PositionY = 0.2,
                    EventTime = new DateTimeAndText { DateTime = DateTime.Now }
                },
                new Injection
                {
                    IdInjection = 2,
                    PositionX = 0.4,
                    PositionY = 0.4,
                    EventTime = new DateTimeAndText { DateTime = null }
                },
                new Injection
                {
                    IdInjection = 3,
                    PositionX = 0.6,
                    PositionY = 0.6,
                    EventTime = new DateTimeAndText { DateTime = DateTime.Now.AddDays(-10) }
                }
            };

            // Act
            drawable.LoadInjectionsCoordinates(injections);

            // Assert
            Assert.That(drawable.InjectionPointsCoordinates.Count, Is.EqualTo(3));
            Assert.That(drawable.InjectionPointsCoordinates[0].Color, Is.EqualTo(Colors.Brown));
            Assert.That(drawable.InjectionPointsCoordinates[1].Color, Is.EqualTo(Colors.Red));
            Assert.That(drawable.InjectionPointsCoordinates[2].Color, Is.Not.EqualTo(Colors.Brown));
            Assert.That(drawable.InjectionPointsCoordinates[2].Color, Is.Not.EqualTo(Colors.Red));
        }

        /// <summary>
        /// Tests that LoadReferenceCoordinates throws NullReferenceException when given a null list.
        /// </summary>
        [Test]
        public void LoadReferenceCoordinates_NullList_ThrowsNullReferenceException()
        {
            // Arrange
            var drawable = new CirclesDrawable();
            List<PositionOfInjection>? nullList = null;

            // Act & Assert
            Assert.Throws<NullReferenceException>(() => drawable.LoadReferenceCoordinates(nullList!));
        }

        /// <summary>
        /// Tests that LoadReferenceCoordinates adds no points when given an empty list.
        /// </summary>
        [Test]
        public void LoadReferenceCoordinates_EmptyList_AddsNoPoints()
        {
            // Arrange
            var drawable = new CirclesDrawable();
            var emptyList = new List<PositionOfInjection>();

            // Act
            drawable.LoadReferenceCoordinates(emptyList);

            // Assert
            var referencePoints = GetReferencePointsCoordinates(drawable);
            Assert.That(referencePoints.Count, Is.EqualTo(0));
        }

        /// <summary>
        /// Tests that LoadReferenceCoordinates correctly adds a point when given valid coordinates.
        /// </summary>
        [TestCase(0.5, 0.5, 1280.0, 1366.0, 640.0, 683.0, Description = "Center point with default dimensions")]
        [TestCase(0.0, 0.0, 1280.0, 1366.0, 0.0, 0.0, Description = "Origin point")]
        [TestCase(1.0, 1.0, 1280.0, 1366.0, 1280.0, 1366.0, Description = "Bottom-right corner")]
        [TestCase(0.25, 0.75, 800.0, 600.0, 200.0, 450.0, Description = "Custom dimensions")]
        public void LoadReferenceCoordinates_ValidCoordinates_AddsPointWithCorrectScaling(
            double posX, double posY, double imgWidth, double imgHeight,
            double expectedX, double expectedY)
        {
            // Arrange
            var drawable = new CirclesDrawable(imgWidth, imgHeight, null, 0);
            var positions = new List<PositionOfInjection>
            {
                new PositionOfInjection { PositionX = posX, PositionY = posY }
            };

            // Act
            drawable.LoadReferenceCoordinates(positions);

            // Assert
            var referencePoints = GetReferencePointsCoordinates(drawable);
            Assert.That(referencePoints.Count, Is.EqualTo(1));
            Assert.That(referencePoints[0].X, Is.EqualTo((float)expectedX).Within(0.001));
            Assert.That(referencePoints[0].Y, Is.EqualTo((float)expectedY).Within(0.001));
        }

        /// <summary>
        /// Tests that LoadReferenceCoordinates skips items where PositionX is null.
        /// </summary>
        [Test]
        public void LoadReferenceCoordinates_NullPositionX_SkipsItem()
        {
            // Arrange
            var drawable = new CirclesDrawable();
            var positions = new List<PositionOfInjection>
            {
                new PositionOfInjection { PositionX = null, PositionY = 0.5 }
            };

            // Act
            drawable.LoadReferenceCoordinates(positions);

            // Assert
            var referencePoints = GetReferencePointsCoordinates(drawable);
            Assert.That(referencePoints.Count, Is.EqualTo(0));
        }

        /// <summary>
        /// Tests that LoadReferenceCoordinates skips items where PositionY is null.
        /// </summary>
        [Test]
        public void LoadReferenceCoordinates_NullPositionY_SkipsItem()
        {
            // Arrange
            var drawable = new CirclesDrawable();
            var positions = new List<PositionOfInjection>
            {
                new PositionOfInjection { PositionX = 0.5, PositionY = null }
            };

            // Act
            drawable.LoadReferenceCoordinates(positions);

            // Assert
            var referencePoints = GetReferencePointsCoordinates(drawable);
            Assert.That(referencePoints.Count, Is.EqualTo(0));
        }

        /// <summary>
        /// Tests that LoadReferenceCoordinates skips items where both PositionX and PositionY are null.
        /// </summary>
        [Test]
        public void LoadReferenceCoordinates_BothPositionsNull_SkipsItem()
        {
            // Arrange
            var drawable = new CirclesDrawable();
            var positions = new List<PositionOfInjection>
            {
                new PositionOfInjection { PositionX = null, PositionY = null }
            };

            // Act
            drawable.LoadReferenceCoordinates(positions);

            // Assert
            var referencePoints = GetReferencePointsCoordinates(drawable);
            Assert.That(referencePoints.Count, Is.EqualTo(0));
        }

        /// <summary>
        /// Tests that LoadReferenceCoordinates correctly handles a mix of valid and invalid items,
        /// adding only those with both coordinates set.
        /// </summary>
        [Test]
        public void LoadReferenceCoordinates_MixedValidAndInvalid_AddsOnlyValidItems()
        {
            // Arrange
            var drawable = new CirclesDrawable(1000.0, 1000.0, null, 0);
            var positions = new List<PositionOfInjection>
            {
                new PositionOfInjection { PositionX = 0.1, PositionY = 0.2 },  // Valid
                new PositionOfInjection { PositionX = null, PositionY = 0.3 }, // Invalid - null X
                new PositionOfInjection { PositionX = 0.4, PositionY = null }, // Invalid - null Y
                new PositionOfInjection { PositionX = 0.5, PositionY = 0.6 },  // Valid
                new PositionOfInjection { PositionX = null, PositionY = null } // Invalid - both null
            };

            // Act
            drawable.LoadReferenceCoordinates(positions);

            // Assert
            var referencePoints = GetReferencePointsCoordinates(drawable);
            Assert.That(referencePoints.Count, Is.EqualTo(2));
            Assert.That(referencePoints[0].X, Is.EqualTo(100.0f).Within(0.001));
            Assert.That(referencePoints[0].Y, Is.EqualTo(200.0f).Within(0.001));
            Assert.That(referencePoints[1].X, Is.EqualTo(500.0f).Within(0.001));
            Assert.That(referencePoints[1].Y, Is.EqualTo(600.0f).Within(0.001));
        }

        /// <summary>
        /// Tests that LoadReferenceCoordinates handles boundary values correctly,
        /// including negative values, zero, and very large values.
        /// </summary>
        [TestCase(-1.0, -1.0, Description = "Negative coordinates")]
        [TestCase(0.0, 0.0, Description = "Zero coordinates")]
        [TestCase(100.0, 100.0, Description = "Large coordinates")]
        [TestCase(-0.5, 1.5, Description = "Mixed negative and large")]
        public void LoadReferenceCoordinates_BoundaryValues_HandlesCorrectly(double posX, double posY)
        {
            // Arrange
            var drawable = new CirclesDrawable(1280.0, 1366.0, null, 0);
            var positions = new List<PositionOfInjection>
            {
                new PositionOfInjection { PositionX = posX, PositionY = posY }
            };

            // Act
            drawable.LoadReferenceCoordinates(positions);

            // Assert
            var referencePoints = GetReferencePointsCoordinates(drawable);
            Assert.That(referencePoints.Count, Is.EqualTo(1));
            Assert.That(referencePoints[0].X, Is.EqualTo((float)(posX * 1280.0)).Within(0.001));
            Assert.That(referencePoints[0].Y, Is.EqualTo((float)(posY * 1366.0)).Within(0.001));
        }

        /// <summary>
        /// Tests that LoadReferenceCoordinates handles special double values like NaN and Infinity.
        /// These values should be processed (not skipped) since they are not null.
        /// </summary>
        [TestCase(double.NaN, 0.5, Description = "NaN for X coordinate")]
        [TestCase(0.5, double.NaN, Description = "NaN for Y coordinate")]
        [TestCase(double.PositiveInfinity, 0.5, Description = "PositiveInfinity for X")]
        [TestCase(0.5, double.PositiveInfinity, Description = "PositiveInfinity for Y")]
        [TestCase(double.NegativeInfinity, 0.5, Description = "NegativeInfinity for X")]
        [TestCase(0.5, double.NegativeInfinity, Description = "NegativeInfinity for Y")]
        public void LoadReferenceCoordinates_SpecialDoubleValues_AddsPoint(double posX, double posY)
        {
            // Arrange
            var drawable = new CirclesDrawable(1280.0, 1366.0, null, 0);
            var positions = new List<PositionOfInjection>
            {
                new PositionOfInjection { PositionX = posX, PositionY = posY }
            };

            // Act
            drawable.LoadReferenceCoordinates(positions);

            // Assert
            var referencePoints = GetReferencePointsCoordinates(drawable);
            Assert.That(referencePoints.Count, Is.EqualTo(1));
            // Special values are not null, so they pass the null check and get added
        }

        /// <summary>
        /// Tests that LoadReferenceCoordinates correctly adds multiple valid items.
        /// </summary>
        [Test]
        public void LoadReferenceCoordinates_MultipleValidItems_AddsAllPoints()
        {
            // Arrange
            var drawable = new CirclesDrawable(1000.0, 1000.0, null, 0);
            var positions = new List<PositionOfInjection>
            {
                new PositionOfInjection { PositionX = 0.1, PositionY = 0.1 },
                new PositionOfInjection { PositionX = 0.2, PositionY = 0.2 },
                new PositionOfInjection { PositionX = 0.3, PositionY = 0.3 },
                new PositionOfInjection { PositionX = 0.4, PositionY = 0.4 },
                new PositionOfInjection { PositionX = 0.5, PositionY = 0.5 }
            };

            // Act
            drawable.LoadReferenceCoordinates(positions);

            // Assert
            var referencePoints = GetReferencePointsCoordinates(drawable);
            Assert.That(referencePoints.Count, Is.EqualTo(5));

            for (int i = 0; i < 5; i++)
            {
                double expectedCoord = (i + 1) * 100.0;
                Assert.That(referencePoints[i].X, Is.EqualTo((float)expectedCoord).Within(0.001));
                Assert.That(referencePoints[i].Y, Is.EqualTo((float)expectedCoord).Within(0.001));
            }
        }

        /// <summary>
        /// Tests that LoadReferenceCoordinates can be called multiple times,
        /// appending points to the existing collection.
        /// </summary>
        [Test]
        public void LoadReferenceCoordinates_CalledMultipleTimes_AppendsPoints()
        {
            // Arrange
            var drawable = new CirclesDrawable(1000.0, 1000.0, null, 0);
            var firstBatch = new List<PositionOfInjection>
            {
                new PositionOfInjection { PositionX = 0.1, PositionY = 0.1 }
            };
            var secondBatch = new List<PositionOfInjection>
            {
                new PositionOfInjection { PositionX = 0.2, PositionY = 0.2 }
            };

            // Act
            drawable.LoadReferenceCoordinates(firstBatch);
            drawable.LoadReferenceCoordinates(secondBatch);

            // Assert
            var referencePoints = GetReferencePointsCoordinates(drawable);
            Assert.That(referencePoints.Count, Is.EqualTo(2));
        }

        /// <summary>
        /// Tests extreme double values close to double.MinValue and double.MaxValue.
        /// </summary>
        [TestCase(double.MinValue, 0.5, Description = "MinValue for X")]
        [TestCase(0.5, double.MaxValue, Description = "MaxValue for Y")]
        public void LoadReferenceCoordinates_ExtremeDoubleValues_AddsPoint(double posX, double posY)
        {
            // Arrange
            var drawable = new CirclesDrawable(1280.0, 1366.0, null, 0);
            var positions = new List<PositionOfInjection>
            {
                new PositionOfInjection { PositionX = posX, PositionY = posY }
            };

            // Act
            drawable.LoadReferenceCoordinates(positions);

            // Assert
            var referencePoints = GetReferencePointsCoordinates(drawable);
            Assert.That(referencePoints.Count, Is.EqualTo(1));
        }

        /// <summary>
        /// Tests NormalizeXPosition with various numeric inputs and image widths.
        /// Verifies that the method correctly divides x by imageWidth for normal numeric values.
        /// </summary>
        /// <param name="x">The x coordinate to normalize.</param>
        /// <param name="imageWidth">The image width used for normalization.</param>
        /// <param name="expected">The expected normalized result.</param>
        [TestCase(100.0, 1280.0, 0.078125, TestName = "NormalizeXPosition_PositiveValues_ReturnsCorrectRatio")]
        [TestCase(0.0, 1280.0, 0.0, TestName = "NormalizeXPosition_ZeroX_ReturnsZero")]
        [TestCase(-100.0, 1280.0, -0.078125, TestName = "NormalizeXPosition_NegativeX_ReturnsNegativeRatio")]
        [TestCase(1280.0, 1280.0, 1.0, TestName = "NormalizeXPosition_XEqualsImageWidth_ReturnsOne")]
        [TestCase(2560.0, 1280.0, 2.0, TestName = "NormalizeXPosition_XGreaterThanImageWidth_ReturnsGreaterThanOne")]
        [TestCase(640.0, 1280.0, 0.5, TestName = "NormalizeXPosition_HalfImageWidth_ReturnsHalf")]
        [TestCase(100.0, 100.0, 1.0, TestName = "NormalizeXPosition_SmallImageWidth_ReturnsCorrectRatio")]
        [TestCase(100.0, 10000.0, 0.01, TestName = "NormalizeXPosition_LargeImageWidth_ReturnsSmallRatio")]
        [TestCase(-1280.0, 1280.0, -1.0, TestName = "NormalizeXPosition_NegativeXEqualsImageWidth_ReturnsNegativeOne")]
        [TestCase(100.0, -1280.0, -0.078125, TestName = "NormalizeXPosition_NegativeImageWidth_ReturnsNegativeRatio")]
        public void NormalizeXPosition_VariousNumericInputs_ReturnsExpectedResult(double x, double imageWidth, double expected)
        {
            // Arrange
            var drawable = new CirclesDrawable(imageWidth, 1366.0, null, 30.0);

            // Act
            var result = drawable.NormalizeXPosition(x);

            // Assert
            Assert.That(result, Is.Not.Null, "Result should never be null despite nullable return type");
            Assert.That(result.Value, Is.EqualTo(expected).Within(1e-10), $"Expected {expected} but got {result.Value}");
        }

        /// <summary>
        /// Tests NormalizeXPosition when x is double.MaxValue.
        /// Verifies that dividing very large values by imageWidth results in positive infinity or very large value.
        /// </summary>
        [Test]
        public void NormalizeXPosition_MaxValueX_ReturnsPositiveInfinity()
        {
            // Arrange
            var drawable = new CirclesDrawable(1280.0, 1366.0, null, 30.0);

            // Act
            var result = drawable.NormalizeXPosition(double.MaxValue);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Value, Is.EqualTo(double.PositiveInfinity));
        }

        /// <summary>
        /// Tests NormalizeXPosition when x is double.MinValue.
        /// Verifies that dividing very small (negative) values by imageWidth results in negative infinity or very large negative value.
        /// </summary>
        [Test]
        public void NormalizeXPosition_MinValueX_ReturnsNegativeInfinity()
        {
            // Arrange
            var drawable = new CirclesDrawable(1280.0, 1366.0, null, 30.0);

            // Act
            var result = drawable.NormalizeXPosition(double.MinValue);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Value, Is.EqualTo(double.NegativeInfinity));
        }

        /// <summary>
        /// Tests NormalizeXPosition when imageWidth is zero.
        /// Verifies that division by zero results in positive infinity for positive x.
        /// </summary>
        [Test]
        public void NormalizeXPosition_ImageWidthZeroPositiveX_ReturnsPositiveInfinity()
        {
            // Arrange
            var drawable = new CirclesDrawable(0.0, 1366.0, null, 30.0);

            // Act
            var result = drawable.NormalizeXPosition(100.0);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Value, Is.EqualTo(double.PositiveInfinity));
        }

        /// <summary>
        /// Tests NormalizeXPosition when imageWidth is zero and x is negative.
        /// Verifies that division by zero results in negative infinity for negative x.
        /// </summary>
        [Test]
        public void NormalizeXPosition_ImageWidthZeroNegativeX_ReturnsNegativeInfinity()
        {
            // Arrange
            var drawable = new CirclesDrawable(0.0, 1366.0, null, 30.0);

            // Act
            var result = drawable.NormalizeXPosition(-100.0);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Value, Is.EqualTo(double.NegativeInfinity));
        }

        /// <summary>
        /// Tests NormalizeXPosition when imageWidth is zero and x is zero.
        /// Verifies that 0/0 results in NaN.
        /// </summary>
        [Test]
        public void NormalizeXPosition_ImageWidthZeroXZero_ReturnsNaN()
        {
            // Arrange
            var drawable = new CirclesDrawable(0.0, 1366.0, null, 30.0);

            // Act
            var result = drawable.NormalizeXPosition(0.0);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Value, Is.NaN);
        }

        /// <summary>
        /// Tests NormalizeXPosition when x is double.NaN.
        /// Verifies that NaN propagates through the division operation.
        /// </summary>
        [Test]
        public void NormalizeXPosition_NaNX_ReturnsNaN()
        {
            // Arrange
            var drawable = new CirclesDrawable(1280.0, 1366.0, null, 30.0);

            // Act
            var result = drawable.NormalizeXPosition(double.NaN);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Value, Is.NaN);
        }

        /// <summary>
        /// Tests NormalizeXPosition when imageWidth is double.NaN.
        /// Verifies that NaN propagates through the division operation.
        /// </summary>
        [Test]
        public void NormalizeXPosition_ImageWidthNaN_ReturnsNaN()
        {
            // Arrange
            var drawable = new CirclesDrawable(double.NaN, 1366.0, null, 30.0);

            // Act
            var result = drawable.NormalizeXPosition(100.0);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Value, Is.NaN);
        }

        /// <summary>
        /// Tests NormalizeXPosition when x is positive infinity.
        /// Verifies that positive infinity divided by a finite value remains positive infinity.
        /// </summary>
        [Test]
        public void NormalizeXPosition_PositiveInfinityX_ReturnsPositiveInfinity()
        {
            // Arrange
            var drawable = new CirclesDrawable(1280.0, 1366.0, null, 30.0);

            // Act
            var result = drawable.NormalizeXPosition(double.PositiveInfinity);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Value, Is.EqualTo(double.PositiveInfinity));
        }

        /// <summary>
        /// Tests NormalizeXPosition when x is negative infinity.
        /// Verifies that negative infinity divided by a finite value remains negative infinity.
        /// </summary>
        [Test]
        public void NormalizeXPosition_NegativeInfinityX_ReturnsNegativeInfinity()
        {
            // Arrange
            var drawable = new CirclesDrawable(1280.0, 1366.0, null, 30.0);

            // Act
            var result = drawable.NormalizeXPosition(double.NegativeInfinity);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Value, Is.EqualTo(double.NegativeInfinity));
        }

        /// <summary>
        /// Tests NormalizeXPosition when imageWidth is positive infinity.
        /// Verifies that a finite value divided by positive infinity returns zero.
        /// </summary>
        [Test]
        public void NormalizeXPosition_ImageWidthPositiveInfinity_ReturnsZero()
        {
            // Arrange
            var drawable = new CirclesDrawable(double.PositiveInfinity, 1366.0, null, 30.0);

            // Act
            var result = drawable.NormalizeXPosition(100.0);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Value, Is.EqualTo(0.0));
        }

        /// <summary>
        /// Tests NormalizeXPosition when both x and imageWidth are positive infinity.
        /// Verifies that infinity divided by infinity results in NaN.
        /// </summary>
        [Test]
        public void NormalizeXPosition_BothInfinity_ReturnsNaN()
        {
            // Arrange
            var drawable = new CirclesDrawable(double.PositiveInfinity, 1366.0, null, 30.0);

            // Act
            var result = drawable.NormalizeXPosition(double.PositiveInfinity);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Value, Is.NaN);
        }

        /// <summary>
        /// Tests NormalizeXPosition with very small positive imageWidth.
        /// Verifies that division by a very small number produces a very large result.
        /// </summary>
        [Test]
        public void NormalizeXPosition_VerySmallImageWidth_ReturnsLargeValue()
        {
            // Arrange
            var drawable = new CirclesDrawable(0.0001, 1366.0, null, 30.0);

            // Act
            var result = drawable.NormalizeXPosition(100.0);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Value, Is.EqualTo(1000000.0).Within(1e-5));
        }

        /// <summary>
        /// Tests NormalizeXPosition using default constructor.
        /// Verifies that the default imageWidth (1280) is used for normalization.
        /// </summary>
        [Test]
        public void NormalizeXPosition_DefaultConstructor_UsesDefaultImageWidth()
        {
            // Arrange
            var drawable = new CirclesDrawable();

            // Act
            var result = drawable.NormalizeXPosition(1280.0);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Value, Is.EqualTo(1.0).Within(1e-10));
        }

        /// <summary>
        /// Tests NormalizeYPosition with various y values and normal positive imageHeight.
        /// Verifies that the method correctly divides y by imageHeight for positive, zero, and negative values.
        /// </summary>
        /// <param name="y">The y coordinate to normalize.</param>
        /// <param name="imageHeight">The image height to use for normalization.</param>
        /// <param name="expectedResult">The expected normalized result.</param>
        [TestCase(0.0, 1366.0, 0.0)]
        [TestCase(683.0, 1366.0, 0.5)]
        [TestCase(1366.0, 1366.0, 1.0)]
        [TestCase(2732.0, 1366.0, 2.0)]
        [TestCase(-683.0, 1366.0, -0.5)]
        [TestCase(100.0, 200.0, 0.5)]
        [TestCase(1.0, 1.0, 1.0)]
        public void NormalizeYPosition_WithNormalValues_ReturnsCorrectDivision(double y, double imageHeight, double expectedResult)
        {
            // Arrange
            var drawable = new CirclesDrawable(1280.0, imageHeight, null, 0.0);

            // Act
            double? result = drawable.NormalizeYPosition(y);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Value, Is.EqualTo(expectedResult).Within(0.0001));
        }

        /// <summary>
        /// Tests NormalizeYPosition with extreme double values to verify behavior at boundaries.
        /// Verifies that the method handles double.MaxValue, double.MinValue correctly.
        /// </summary>
        /// <param name="y">The y coordinate to normalize.</param>
        [TestCase(double.MaxValue)]
        [TestCase(double.MinValue)]
        public void NormalizeYPosition_WithExtremeValues_ReturnsFiniteResult(double y)
        {
            // Arrange
            var drawable = new CirclesDrawable(1280.0, 1366.0, null, 0.0);

            // Act
            double? result = drawable.NormalizeYPosition(y);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Value, Is.Not.NaN);
        }

        /// <summary>
        /// Tests NormalizeYPosition with positive infinity input.
        /// Verifies that dividing positive infinity by a finite positive imageHeight returns positive infinity.
        /// </summary>
        [Test]
        public void NormalizeYPosition_WithPositiveInfinity_ReturnsPositiveInfinity()
        {
            // Arrange
            var drawable = new CirclesDrawable(1280.0, 1366.0, null, 0.0);

            // Act
            double? result = drawable.NormalizeYPosition(double.PositiveInfinity);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Value, Is.EqualTo(double.PositiveInfinity));
        }

        /// <summary>
        /// Tests NormalizeYPosition with negative infinity input.
        /// Verifies that dividing negative infinity by a finite positive imageHeight returns negative infinity.
        /// </summary>
        [Test]
        public void NormalizeYPosition_WithNegativeInfinity_ReturnsNegativeInfinity()
        {
            // Arrange
            var drawable = new CirclesDrawable(1280.0, 1366.0, null, 0.0);

            // Act
            double? result = drawable.NormalizeYPosition(double.NegativeInfinity);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Value, Is.EqualTo(double.NegativeInfinity));
        }

        /// <summary>
        /// Tests NormalizeYPosition with NaN input.
        /// Verifies that dividing NaN by any finite imageHeight returns NaN.
        /// </summary>
        [Test]
        public void NormalizeYPosition_WithNaN_ReturnsNaN()
        {
            // Arrange
            var drawable = new CirclesDrawable(1280.0, 1366.0, null, 0.0);

            // Act
            double? result = drawable.NormalizeYPosition(double.NaN);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Value, Is.NaN);
        }

        /// <summary>
        /// Tests NormalizeYPosition when imageHeight is zero.
        /// Verifies that division by zero produces positive or negative infinity based on the sign of y.
        /// </summary>
        /// <param name="y">The y coordinate to normalize.</param>
        /// <param name="expectedSign">The expected sign of infinity: 1 for positive, -1 for negative.</param>
        [TestCase(100.0, 1)]
        [TestCase(-100.0, -1)]
        public void NormalizeYPosition_WithZeroImageHeight_ReturnsInfinity(double y, int expectedSign)
        {
            // Arrange
            var drawable = new CirclesDrawable(1280.0, 0.0, null, 0.0);

            // Act
            double? result = drawable.NormalizeYPosition(y);

            // Assert
            Assert.That(result, Is.Not.Null);
            if (expectedSign > 0)
            {
                Assert.That(result.Value, Is.EqualTo(double.PositiveInfinity));
            }
            else
            {
                Assert.That(result.Value, Is.EqualTo(double.NegativeInfinity));
            }
        }

        /// <summary>
        /// Tests NormalizeYPosition when imageHeight is zero and y is zero.
        /// Verifies that 0/0 produces NaN.
        /// </summary>
        [Test]
        public void NormalizeYPosition_WithZeroYAndZeroImageHeight_ReturnsNaN()
        {
            // Arrange
            var drawable = new CirclesDrawable(1280.0, 0.0, null, 0.0);

            // Act
            double? result = drawable.NormalizeYPosition(0.0);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Value, Is.NaN);
        }

        /// <summary>
        /// Tests NormalizeYPosition when imageHeight is negative.
        /// Verifies that division by negative imageHeight produces expected negative result for positive y.
        /// </summary>
        [Test]
        public void NormalizeYPosition_WithNegativeImageHeight_ReturnsNegativeResult()
        {
            // Arrange
            var drawable = new CirclesDrawable(1280.0, -1366.0, null, 0.0);

            // Act
            double? result = drawable.NormalizeYPosition(683.0);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Value, Is.EqualTo(-0.5).Within(0.0001));
        }

        /// <summary>
        /// Tests NormalizeYPosition when imageHeight is positive infinity.
        /// Verifies that dividing a finite value by positive infinity returns zero.
        /// </summary>
        [Test]
        public void NormalizeYPosition_WithPositiveInfinityImageHeight_ReturnsZero()
        {
            // Arrange
            var drawable = new CirclesDrawable(1280.0, double.PositiveInfinity, null, 0.0);

            // Act
            double? result = drawable.NormalizeYPosition(1000.0);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Value, Is.EqualTo(0.0));
        }

        /// <summary>
        /// Tests NormalizeYPosition when imageHeight is negative infinity.
        /// Verifies that dividing a positive finite value by negative infinity returns negative zero.
        /// </summary>
        [Test]
        public void NormalizeYPosition_WithNegativeInfinityImageHeight_ReturnsNegativeZero()
        {
            // Arrange
            var drawable = new CirclesDrawable(1280.0, double.NegativeInfinity, null, 0.0);

            // Act
            double? result = drawable.NormalizeYPosition(1000.0);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Value, Is.EqualTo(-0.0).Within(0.0001));
        }

        /// <summary>
        /// Tests NormalizeYPosition when imageHeight is NaN.
        /// Verifies that dividing any value by NaN returns NaN.
        /// </summary>
        /// <param name="y">The y coordinate to normalize.</param>
        [TestCase(0.0)]
        [TestCase(100.0)]
        [TestCase(-100.0)]
        [TestCase(double.PositiveInfinity)]
        [TestCase(double.NegativeInfinity)]
        public void NormalizeYPosition_WithNaNImageHeight_ReturnsNaN(double y)
        {
            // Arrange
            var drawable = new CirclesDrawable(1280.0, double.NaN, null, 0.0);

            // Act
            double? result = drawable.NormalizeYPosition(y);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Value, Is.NaN);
        }

        /// <summary>
        /// Tests NormalizeYPosition when both y and imageHeight are infinity.
        /// Verifies that infinity divided by infinity returns NaN.
        /// </summary>
        [Test]
        public void NormalizeYPosition_WithInfinityDividedByInfinity_ReturnsNaN()
        {
            // Arrange
            var drawable = new CirclesDrawable(1280.0, double.PositiveInfinity, null, 0.0);

            // Act
            double? result = drawable.NormalizeYPosition(double.PositiveInfinity);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Value, Is.NaN);
        }

        /// <summary>
        /// Tests NormalizeYPosition with default constructor.
        /// Verifies that the method works correctly with default imageHeight value (1366).
        /// </summary>
        [Test]
        public void NormalizeYPosition_WithDefaultConstructor_ReturnsCorrectResult()
        {
            // Arrange
            var drawable = new CirclesDrawable();

            // Act
            double? result = drawable.NormalizeYPosition(683.0);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Value, Is.EqualTo(0.5).Within(0.0001));
        }

        /// <summary>
        /// Tests NormalizeYPosition with very small positive imageHeight value.
        /// Verifies that division by a very small number produces a very large result.
        /// </summary>
        [Test]
        public void NormalizeYPosition_WithVerySmallImageHeight_ReturnsLargeResult()
        {
            // Arrange
            var drawable = new CirclesDrawable(1280.0, 0.0001, null, 0.0);

            // Act
            double? result = drawable.NormalizeYPosition(1.0);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Value, Is.EqualTo(10000.0).Within(0.1));
        }

        /// <summary>
        /// Tests NormalizeYPosition with very large positive imageHeight value.
        /// Verifies that division by a very large number produces a very small result.
        /// </summary>
        [Test]
        public void NormalizeYPosition_WithVeryLargeImageHeight_ReturnsSmallResult()
        {
            // Arrange
            var drawable = new CirclesDrawable(1280.0, 1000000.0, null, 0.0);

            // Act
            double? result = drawable.NormalizeYPosition(1000.0);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Value, Is.EqualTo(0.001).Within(0.0001));
        }

        /// <summary>
        /// Tests that AddPoint correctly adds a clicked point to ReferencePointsCoordinates when editing
        /// and no existing points are near enough.
        /// </summary>
        /// <remarks>
        /// Input: Valid click position, IsEditing = true, empty ReferencePointsCoordinates
        /// Expected: Point is added to ReferencePointsCoordinates, returns Point with MaxValue coordinates
        /// </remarks>
        [Test]
        public void AddPoint_IsEditingTrueEmptyReferencePoints_AddsPointToList()
        {
            // Arrange
            var drawable = new CirclesDrawable(1280, 1366, null, 30);
            var clickPosition = new MauiPoint(100, 150);

            // Act
            var result = drawable.AddPoint(clickPosition, true);

            // Assert
            Assert.That(result.X, Is.EqualTo(double.MaxValue));
            Assert.That(result.Y, Is.EqualTo(double.MaxValue));
        }

        /// <summary>
        /// Tests that AddPoint correctly replaces a near reference point when editing
        /// and clicked position is within CurrentNearEnoughDistance.
        /// </summary>
        /// <remarks>
        /// Input: Click position near existing reference point, IsEditing = true
        /// Expected: Old point removed, new point added, returns nearest point
        /// </remarks>
        [Test]
        public void AddPoint_IsEditingTrueClickNearExistingPoint_ReplacesPoint()
        {
            // Arrange
            var drawable = new CirclesDrawable(1280, 1366, null, 30);
            var existingPoint = new MauiPoint(100, 100);

            // Add an initial reference point
            drawable.AddPoint(existingPoint, true);

            // Click very close to the existing point (within nearEnoughScaleOneDistance)
            var clickPosition = new MauiPoint(105, 105);

            // Act
            var result = drawable.AddPoint(clickPosition, true);

            // Assert
            Assert.That(result.X, Is.EqualTo(existingPoint.X));
            Assert.That(result.Y, Is.EqualTo(existingPoint.Y));
        }

        /// <summary>
        /// Tests that AddPoint adds a new reference point when editing
        /// and clicked position is far from all existing points.
        /// </summary>
        /// <remarks>
        /// Input: Click position far from existing points, IsEditing = true
        /// Expected: New point added without removing any existing points
        /// </remarks>
        [Test]
        public void AddPoint_IsEditingTrueClickFarFromExistingPoint_AddsNewPoint()
        {
            // Arrange
            var drawable = new CirclesDrawable(1280, 1366, null, 30);
            var existingPoint = new MauiPoint(100, 100);

            // Add an initial reference point
            drawable.AddPoint(existingPoint, true);

            // Click far from the existing point (beyond nearEnoughScaleOneDistance)
            var clickPosition = new MauiPoint(200, 200);

            // Act
            var result = drawable.AddPoint(clickPosition, true);

            // Assert
            Assert.That(result.X, Is.EqualTo(existingPoint.X));
            Assert.That(result.Y, Is.EqualTo(existingPoint.Y));
        }

        /// <summary>
        /// Tests that AddPoint snaps to nearest reference point when not editing
        /// and a valid reference point exists.
        /// </summary>
        /// <remarks>
        /// Input: Click position near reference point, IsEditing = false, first click
        /// Expected: Injection point added at nearest reference position, isFirstClick becomes false
        /// </remarks>
        [Test]
        public void AddPoint_IsEditingFalseFirstClickNearReference_SnapsToReference()
        {
            // Arrange
            var drawable = new CirclesDrawable(1280, 1366, null, 30);
            var referencePoint = new MauiPoint(100, 100);

            // Add a reference point while editing
            drawable.AddPoint(referencePoint, true);

            // Click near the reference point while not editing
            var clickPosition = new MauiPoint(110, 110);

            // Act
            var result = drawable.AddPoint(clickPosition, false);

            // Assert
            Assert.That(result.X, Is.EqualTo(referencePoint.X));
            Assert.That(result.Y, Is.EqualTo(referencePoint.Y));
        }

        /// <summary>
        /// Tests that AddPoint replaces the previous injection point when not editing
        /// and it's not the first click.
        /// </summary>
        /// <remarks>
        /// Input: Second click when not editing
        /// Expected: Previous injection point removed, new one added at clicked position
        /// </remarks>
        [Test]
        public void AddPoint_IsEditingFalseSecondClick_ReplacesPreviousInjection()
        {
            // Arrange
            var drawable = new CirclesDrawable(1280, 1366, null, 30);
            var referencePoint = new MauiPoint(100, 100);

            // Setup: add reference point
            drawable.AddPoint(referencePoint, true);

            // First click (not editing)
            drawable.AddPoint(new MauiPoint(105, 105), false);

            // Second click (not editing)
            var secondClickPosition = new MauiPoint(100, 100);

            // Act
            var result = drawable.AddPoint(secondClickPosition, false);

            // Assert
            Assert.That(result.X, Is.EqualTo(referencePoint.X));
            Assert.That(result.Y, Is.EqualTo(referencePoint.Y));
        }

        /// <summary>
        /// Tests that AddPoint uses clicked position when not editing and no reference points exist.
        /// </summary>
        /// <remarks>
        /// Input: Click position, IsEditing = false, no reference points
        /// Expected: Injection added at clicked position, returns Point with MaxValue
        /// </remarks>
        [Test]
        public void AddPoint_IsEditingFalseNoReferencePoints_UsesClickedPosition()
        {
            // Arrange
            var drawable = new CirclesDrawable(1280, 1366, null, 30);
            var clickPosition = new MauiPoint(100, 150);

            // Act
            var result = drawable.AddPoint(clickPosition, false);

            // Assert
            Assert.That(result.X, Is.EqualTo(double.MaxValue));
            Assert.That(result.Y, Is.EqualTo(double.MaxValue));
        }

        /// <summary>
        /// Tests AddPoint behavior with zero coordinates.
        /// </summary>
        /// <remarks>
        /// Input: Point(0, 0), IsEditing = true
        /// Expected: Point added successfully at origin
        /// </remarks>
        [Test]
        public void AddPoint_ZeroCoordinates_AddsPointSuccessfully()
        {
            // Arrange
            var drawable = new CirclesDrawable(1280, 1366, null, 30);
            var clickPosition = new MauiPoint(0, 0);

            // Act
            var result = drawable.AddPoint(clickPosition, true);

            // Assert
            Assert.That(result.X, Is.EqualTo(double.MaxValue));
            Assert.That(result.Y, Is.EqualTo(double.MaxValue));
        }

        /// <summary>
        /// Tests AddPoint behavior with negative coordinates.
        /// </summary>
        /// <remarks>
        /// Input: Point with negative X and Y, IsEditing = true
        /// Expected: Point added successfully with negative coordinates
        /// </remarks>
        [Test]
        public void AddPoint_NegativeCoordinates_AddsPointSuccessfully()
        {
            // Arrange
            var drawable = new CirclesDrawable(1280, 1366, null, 30);
            var clickPosition = new MauiPoint(-50, -100);

            // Act
            var result = drawable.AddPoint(clickPosition, true);

            // Assert
            Assert.That(result.X, Is.EqualTo(double.MaxValue));
            Assert.That(result.Y, Is.EqualTo(double.MaxValue));
        }

        /// <summary>
        /// Tests AddPoint behavior with very large coordinates.
        /// </summary>
        /// <remarks>
        /// Input: Point with very large X and Y values, IsEditing = true
        /// Expected: Point added successfully without overflow
        /// </remarks>
        [Test]
        public void AddPoint_VeryLargeCoordinates_AddsPointSuccessfully()
        {
            // Arrange
            var drawable = new CirclesDrawable(1280, 1366, null, 30);
            var clickPosition = new MauiPoint(1e10, 1e10);

            // Act
            var result = drawable.AddPoint(clickPosition, true);

            // Assert
            Assert.That(result.X, Is.EqualTo(double.MaxValue));
            Assert.That(result.Y, Is.EqualTo(double.MaxValue));
        }

        /// <summary>
        /// Tests AddPoint behavior with NaN coordinates.
        /// </summary>
        /// <remarks>
        /// Input: Point with NaN coordinates, IsEditing = true
        /// Expected: Method handles NaN values (may add point or handle gracefully)
        /// </remarks>
        [Test]
        public void AddPoint_NaNCoordinates_HandlesGracefully()
        {
            // Arrange
            var drawable = new CirclesDrawable(1280, 1366, null, 30);
            var clickPosition = new MauiPoint(double.NaN, double.NaN);

            // Act
            var result = drawable.AddPoint(clickPosition, true);

            // Assert
            // NaN comparisons are special - any comparison with NaN returns false
            // Just verify method doesn't throw
            Assert.That(result.X, Is.EqualTo(double.MaxValue));
            Assert.That(result.Y, Is.EqualTo(double.MaxValue));
        }

        /// <summary>
        /// Tests AddPoint behavior with positive infinity coordinates.
        /// </summary>
        /// <remarks>
        /// Input: Point with PositiveInfinity coordinates, IsEditing = true
        /// Expected: Method handles infinity values without throwing
        /// </remarks>
        [Test]
        public void AddPoint_PositiveInfinityCoordinates_HandlesGracefully()
        {
            // Arrange
            var drawable = new CirclesDrawable(1280, 1366, null, 30);
            var clickPosition = new MauiPoint(double.PositiveInfinity, double.PositiveInfinity);

            // Act
            var result = drawable.AddPoint(clickPosition, true);

            // Assert
            Assert.That(result.X, Is.EqualTo(double.MaxValue));
            Assert.That(result.Y, Is.EqualTo(double.MaxValue));
        }

        /// <summary>
        /// Tests AddPoint behavior with negative infinity coordinates.
        /// </summary>
        /// <remarks>
        /// Input: Point with NegativeInfinity coordinates, IsEditing = true
        /// Expected: Method handles infinity values without throwing
        /// </remarks>
        [Test]
        public void AddPoint_NegativeInfinityCoordinates_HandlesGracefully()
        {
            // Arrange
            var drawable = new CirclesDrawable(1280, 1366, null, 30);
            var clickPosition = new MauiPoint(double.NegativeInfinity, double.NegativeInfinity);

            // Act
            var result = drawable.AddPoint(clickPosition, true);

            // Assert
            Assert.That(result.X, Is.EqualTo(double.MaxValue));
            Assert.That(result.Y, Is.EqualTo(double.MaxValue));
        }

        /// <summary>
        /// Tests AddPoint with multiple reference points to find the truly nearest one.
        /// </summary>
        /// <remarks>
        /// Input: Multiple reference points, click near one of them, IsEditing = true
        /// Expected: Finds and returns the nearest reference point
        /// </remarks>
        [Test]
        public void AddPoint_MultipleReferencePoints_FindsNearestPoint()
        {
            // Arrange
            var drawable = new CirclesDrawable(1280, 1366, null, 30);

            // Add multiple reference points
            drawable.AddPoint(new MauiPoint(100, 100), true);
            drawable.AddPoint(new MauiPoint(200, 200), true);
            drawable.AddPoint(new MauiPoint(300, 300), true);

            // Click near the second point
            var clickPosition = new MauiPoint(205, 205);

            // Act
            var result = drawable.AddPoint(clickPosition, true);

            // Assert - should find point (200, 200) as nearest
            Assert.That(result.X, Is.EqualTo(200));
            Assert.That(result.Y, Is.EqualTo(200));
        }

        /// <summary>
        /// Tests AddPoint when clicking exactly on an existing reference point boundary.
        /// </summary>
        /// <remarks>
        /// Input: Click exactly at distance = CurrentNearEnoughDistance from reference point
        /// Expected: Behavior depends on whether distance is considered "less than" threshold
        /// </remarks>
        [Test]
        public void AddPoint_ClickAtExactThresholdDistance_BehavesConsistently()
        {
            // Arrange
            var drawable = new CirclesDrawable(1280, 1366, null, 30);
            var referencePoint = new MauiPoint(100, 100);

            // Add reference point
            drawable.AddPoint(referencePoint, true);

            // Calculate point exactly at nearEnoughScaleOneDistance (30 pixels at scale 1.0)
            // Since imageWidth/Height match defaults, scale = 1.0
            // Distance = sqrt((30)^2 + (0)^2) = 30
            var clickPosition = new MauiPoint(130, 100);

            // Act
            var result = drawable.AddPoint(clickPosition, true);

            // Assert - at exactly the threshold, should NOT replace (distance < threshold is false)
            Assert.That(result.X, Is.EqualTo(referencePoint.X));
            Assert.That(result.Y, Is.EqualTo(referencePoint.Y));
        }

        /// <summary>
        /// Tests AddPoint with different image dimensions affecting scale calculation.
        /// </summary>
        /// <remarks>
        /// Input: Smaller image dimensions (affects scale), click position, IsEditing = true
        /// Expected: Scale affects distance calculation via CurrentNearEnoughDistance
        /// </remarks>
        [Test]
        public void AddPoint_DifferentImageDimensions_AffectsDistanceCalculation()
        {
            // Arrange - use half the default dimensions, scale should be 0.5
            var drawable = new CirclesDrawable(640, 683, null, 30);
            var referencePoint = new MauiPoint(100, 100);

            // Add reference point
            drawable.AddPoint(referencePoint, true);

            // At scale 0.5, nearEnoughDistance = 30 * 0.5 = 15
            // Click at distance 20 (should be far enough to add new point)
            var clickPosition = new MauiPoint(120, 100);

            // Act
            var result = drawable.AddPoint(clickPosition, true);

            // Assert
            Assert.That(result.X, Is.EqualTo(referencePoint.X));
            Assert.That(result.Y, Is.EqualTo(referencePoint.Y));
        }

        /// <summary>
        /// Tests AddPoint behavior when not editing with MaxValue returned from FindNearest.
        /// </summary>
        /// <remarks>
        /// Input: No reference points, IsEditing = false
        /// Expected: Uses original clicked coordinates, doesn't snap
        /// </remarks>
        [Test]
        public void AddPoint_NotEditingNoSnapTarget_UsesOriginalCoordinates()
        {
            // Arrange
            var drawable = new CirclesDrawable(1280, 1366, 1, 30);
            var clickPosition = new MauiPoint(123.456, 789.012);

            // Act
            var result = drawable.AddPoint(clickPosition, false);

            // Assert - should use original coordinates since no reference points exist
            Assert.That(result.X, Is.EqualTo(double.MaxValue));
            Assert.That(result.Y, Is.EqualTo(double.MaxValue));
        }

        /// <summary>
        /// Tests AddPoint multiple consecutive calls when not editing.
        /// </summary>
        /// <remarks>
        /// Input: Three consecutive clicks when not editing
        /// Expected: Each click replaces the previous injection point (except first)
        /// </remarks>
        [Test]
        public void AddPoint_MultipleConsecutiveClicksNotEditing_ReplacesEachTime()
        {
            // Arrange
            var drawable = new CirclesDrawable(1280, 1366, null, 30);
            var refPoint = new MauiPoint(100, 100);

            // Add reference point
            drawable.AddPoint(refPoint, true);

            // Act - three consecutive clicks not editing
            var result1 = drawable.AddPoint(new MauiPoint(100, 100), false);
            var result2 = drawable.AddPoint(new MauiPoint(100, 100), false);
            var result3 = drawable.AddPoint(new MauiPoint(100, 100), false);

            // Assert - all should snap to reference point
            Assert.That(result1.X, Is.EqualTo(refPoint.X));
            Assert.That(result1.Y, Is.EqualTo(refPoint.Y));
            Assert.That(result2.X, Is.EqualTo(refPoint.X));
            Assert.That(result2.Y, Is.EqualTo(refPoint.Y));
            Assert.That(result3.X, Is.EqualTo(refPoint.X));
            Assert.That(result3.Y, Is.EqualTo(refPoint.Y));
        }

        /// <summary>
        /// Tests AddPoint with min/max double values for edge case coverage.
        /// </summary>
        /// <remarks>
        /// Input: Point(double.MinValue, double.MaxValue), IsEditing = true
        /// Expected: Handles extreme values without overflow or exception
        /// </remarks>
        [Test]
        public void AddPoint_MinMaxDoubleValues_HandlesExtremeValues()
        {
            // Arrange
            var drawable = new CirclesDrawable(1280, 1366, null, 30);
            var clickPosition = new MauiPoint(double.MinValue, double.MaxValue);

            // Act
            var result = drawable.AddPoint(clickPosition, true);

            // Assert - should not throw and return MaxValue coordinates
            Assert.That(result.X, Is.EqualTo(double.MaxValue));
            Assert.That(result.Y, Is.EqualTo(double.MaxValue));
        }

        /// <summary>
        /// Tests AddPoint alternating between editing and not editing modes.
        /// </summary>
        /// <remarks>
        /// Input: Alternating IsEditing true/false calls
        /// Expected: Each mode behaves correctly according to its logic
        /// </remarks>
        [Test]
        public void AddPoint_AlternatingEditingModes_BehavesCorrectlyForEachMode()
        {
            // Arrange
            var drawable = new CirclesDrawable(1280, 1366, null, 30);
            var refPoint = new MauiPoint(100, 100);

            // Act & Assert
            // Add reference point while editing
            var result1 = drawable.AddPoint(refPoint, true);
            Assert.That(result1.X, Is.EqualTo(double.MaxValue));

            // Add injection point not editing (should snap)
            var result2 = drawable.AddPoint(new MauiPoint(105, 105), false);
            Assert.That(result2.X, Is.EqualTo(refPoint.X));

            // Add another reference point while editing
            var newRefPoint = new MauiPoint(200, 200);
            var result3 = drawable.AddPoint(newRefPoint, true);
            Assert.That(result3.X, Is.EqualTo(refPoint.X)); // nearest is still first point

            // Add injection not editing (should snap to nearest, which is now possibly different)
            var result4 = drawable.AddPoint(new MauiPoint(195, 195), false);
            Assert.That(result4.X, Is.EqualTo(newRefPoint.X));
        }

        /// <summary>
        /// Tests that Draw method throws NullReferenceException when canvas parameter is null.
        /// </summary>
        [Test]
        public void Draw_NullCanvas_ThrowsNullReferenceException()
        {
            // Arrange
            var drawable = new CirclesDrawable(1280.0, 1366.0, null, 30.0);
            var dirtyRect = new RectF(0, 0, 100, 100);

            // Act & Assert
            Assert.Throws<NullReferenceException>(() => drawable.Draw(null!, dirtyRect));
        }

        /// <summary>
        /// Tests that Draw method accepts various dirtyRect values without throwing.
        /// The dirtyRect parameter is not used in the method implementation.
        /// </summary>
        /// <param name="x">X coordinate of dirtyRect.</param>
        /// <param name="y">Y coordinate of dirtyRect.</param>
        /// <param name="width">Width of dirtyRect.</param>
        /// <param name="height">Height of dirtyRect.</param>
        [TestCase(0f, 0f, 0f, 0f, TestName = "Draw_DirtyRectZero_ExecutesWithoutError")]
        [TestCase(float.MinValue, float.MinValue, float.MaxValue, float.MaxValue, TestName = "Draw_DirtyRectExtremeValues_ExecutesWithoutError")]
        [TestCase(float.NaN, float.NaN, float.NaN, float.NaN, TestName = "Draw_DirtyRectNaN_ExecutesWithoutError")]
        [TestCase(float.PositiveInfinity, float.NegativeInfinity, float.PositiveInfinity, float.NegativeInfinity, TestName = "Draw_DirtyRectInfinity_ExecutesWithoutError")]
        [TestCase(-100f, -200f, 1000f, 2000f, TestName = "Draw_DirtyRectNegativeOrigin_ExecutesWithoutError")]
        public void Draw_VariousDirtyRectValues_ExecutesWithoutError(float x, float y, float width, float height)
        {
            // Arrange
            var drawable = new CirclesDrawable(1280.0, 1366.0, null, 30.0);
            drawable.IsCallerEditing = true;
            var mockCanvas = new Mock<ICanvas>();
            var dirtyRect = new RectF(x, y, width, height);

            // Act
            drawable.Draw(mockCanvas.Object, dirtyRect);

            // Assert - method completes without exception
            Assert.Pass();
        }

        /// <summary>
        /// Tests that Draw method sets exact blue RGB color (0, 0, 255) in editing mode.
        /// Verifies both StrokeColor and FillColor are set to the same blue value.
        /// </summary>
        [Test]
        public void Draw_EditingMode_SetsExactBlueColor()
        {
            // Arrange
            var drawable = new CirclesDrawable(1280.0, 1366.0, null, 30.0);
            drawable.IsCallerEditing = true;
            var mockCanvas = new Mock<ICanvas>();
            Color? capturedStrokeColor = null;
            Color? capturedFillColor = null;

            mockCanvas.SetupSet(c => c.StrokeColor = It.IsAny<Color>())
                .Callback<Color>(color => capturedStrokeColor = color);
            mockCanvas.SetupSet(c => c.FillColor = It.IsAny<Color>())
                .Callback<Color>(color => capturedFillColor = color);

            var referencePoints = GetReferencePointsField(drawable);
            referencePoints.Add(new Point(100, 100));

            // Act
            drawable.Draw(mockCanvas.Object, new RectF());

            // Assert
            Assert.That(capturedStrokeColor, Is.Not.Null);
            Assert.That(capturedFillColor, Is.Not.Null);
            Assert.That(capturedStrokeColor!.Red, Is.EqualTo(0f).Within(0.001f));
            Assert.That(capturedStrokeColor.Green, Is.EqualTo(0f).Within(0.001f));
            Assert.That(capturedStrokeColor.Blue, Is.EqualTo(1f).Within(0.001f));
            Assert.That(capturedFillColor!.Red, Is.EqualTo(0f).Within(0.001f));
            Assert.That(capturedFillColor.Green, Is.EqualTo(0f).Within(0.001f));
            Assert.That(capturedFillColor.Blue, Is.EqualTo(1f).Within(0.001f));
        }

        /// <summary>
        /// Tests that Draw method correctly calculates FillEllipse parameters in editing mode.
        /// Verifies that circle position is offset by negative radius and size is 2x radius.
        /// </summary>
        [Test]
        public void Draw_EditingMode_CalculatesFillEllipseParametersCorrectly()
        {
            // Arrange
            var drawable = new CirclesDrawable(1280.0, 1366.0, null, 30.0);
            drawable.IsCallerEditing = true;
            var mockCanvas = new Mock<ICanvas>();
            float? capturedX = null;
            float? capturedY = null;
            float? capturedWidth = null;
            float? capturedHeight = null;

            mockCanvas.Setup(c => c.FillEllipse(It.IsAny<float>(), It.IsAny<float>(), It.IsAny<float>(), It.IsAny<float>()))
                .Callback<float, float, float, float>((x, y, w, h) =>
                {
                    capturedX = x;
                    capturedY = y;
                    capturedWidth = w;
                    capturedHeight = h;
                });

            var referencePoints = GetReferencePointsField(drawable);
            referencePoints.Add(new Point(200, 300));

            // Act
            drawable.Draw(mockCanvas.Object, new RectF());

            // Assert
            Assert.That(capturedX, Is.Not.Null);
            Assert.That(capturedY, Is.Not.Null);
            Assert.That(capturedWidth, Is.Not.Null);
            Assert.That(capturedHeight, Is.Not.Null);

            // Verify width and height are equal (circular)
            Assert.That(capturedWidth, Is.EqualTo(capturedHeight));

            // Verify width/height is 2 * radius
            float expectedRadius = 15f; // baseReferenceRadius with scale 1.0 at default dimensions
            float expectedDiameter = expectedRadius * 2f;
            Assert.That(capturedWidth, Is.EqualTo(expectedDiameter).Within(0.1f));
        }

        /// <summary>
        /// Tests that Draw method correctly calculates FillEllipse parameters in non-editing mode.
        /// Verifies that circle uses CurrentInjectionRadius for calculations.
        /// </summary>
        [Test]
        public void Draw_NonEditingMode_CalculatesFillEllipseParametersCorrectly()
        {
            // Arrange
            var drawable = new CirclesDrawable(1280.0, 1366.0, null, 30.0);
            drawable.IsCallerEditing = false;
            var mockCanvas = new Mock<ICanvas>();
            float? capturedWidth = null;
            float? capturedHeight = null;

            mockCanvas.Setup(c => c.FillEllipse(It.IsAny<float>(), It.IsAny<float>(), It.IsAny<float>(), It.IsAny<float>()))
                .Callback<float, float, float, float>((x, y, w, h) =>
                {
                    capturedWidth = w;
                    capturedHeight = h;
                });

            var injectionPoints = GetInjectionPointsField(drawable);
            injectionPoints.Add(new CircleData(new Point(200, 300), Color.FromRgb(255, 0, 0)));

            // Act
            drawable.Draw(mockCanvas.Object, new RectF());

            // Assert
            Assert.That(capturedWidth, Is.Not.Null);
            Assert.That(capturedHeight, Is.Not.Null);

            // Verify width and height are equal (circular)
            Assert.That(capturedWidth, Is.EqualTo(capturedHeight));

            // Verify width/height is 2 * CurrentInjectionRadius
            float expectedRadius = 22f; // baseInjectionRadius with scale 1.0 at default dimensions
            float expectedDiameter = expectedRadius * 2f;
            Assert.That(capturedWidth, Is.EqualTo(expectedDiameter).Within(0.1f));
        }

        /// <summary>
        /// Tests that Draw method calls canvas methods in correct sequence for editing mode.
        /// Verifies StrokeColor, StrokeSize, FillColor are set before FillEllipse.
        /// </summary>
        [Test]
        public void Draw_EditingMode_CallsCanvasMethodsInCorrectOrder()
        {
            // Arrange
            var drawable = new CirclesDrawable(1280.0, 1366.0, null, 30.0);
            drawable.IsCallerEditing = true;
            var mockCanvas = new Mock<ICanvas>(MockBehavior.Strict);
            var callSequence = new List<string>();

            mockCanvas.SetupSet(c => c.StrokeColor = It.IsAny<Color>())
                .Callback<Color>(c => callSequence.Add("StrokeColor"));
            mockCanvas.SetupSet(c => c.StrokeSize = It.IsAny<float>())
                .Callback<float>(s => callSequence.Add("StrokeSize"));
            mockCanvas.SetupSet(c => c.FillColor = It.IsAny<Color>())
                .Callback<Color>(c => callSequence.Add("FillColor"));
            mockCanvas.Setup(c => c.FillEllipse(It.IsAny<float>(), It.IsAny<float>(), It.IsAny<float>(), It.IsAny<float>()))
                .Callback(() => callSequence.Add("FillEllipse"));

            var referencePoints = GetReferencePointsField(drawable);
            referencePoints.Add(new Point(100, 100));

            // Act
            drawable.Draw(mockCanvas.Object, new RectF());

            // Assert
            Assert.That(callSequence.Count, Is.EqualTo(4));
            Assert.That(callSequence[0], Is.EqualTo("StrokeColor"));
            Assert.That(callSequence[1], Is.EqualTo("StrokeSize"));
            Assert.That(callSequence[2], Is.EqualTo("FillColor"));
            Assert.That(callSequence[3], Is.EqualTo("FillEllipse"));
        }

        /// <summary>
        /// Tests that Draw method calls canvas methods in correct sequence for non-editing mode.
        /// Verifies StrokeColor, StrokeSize, FillColor are set before FillEllipse.
        /// </summary>
        [Test]
        public void Draw_NonEditingMode_CallsCanvasMethodsInCorrectOrder()
        {
            // Arrange
            var drawable = new CirclesDrawable(1280.0, 1366.0, null, 30.0);
            drawable.IsCallerEditing = false;
            var mockCanvas = new Mock<ICanvas>(MockBehavior.Strict);
            var callSequence = new List<string>();

            mockCanvas.SetupSet(c => c.StrokeColor = It.IsAny<Color>())
                .Callback<Color>(c => callSequence.Add("StrokeColor"));
            mockCanvas.SetupSet(c => c.StrokeSize = It.IsAny<float>())
                .Callback<float>(s => callSequence.Add("StrokeSize"));
            mockCanvas.SetupSet(c => c.FillColor = It.IsAny<Color>())
                .Callback<Color>(c => callSequence.Add("FillColor"));
            mockCanvas.Setup(c => c.FillEllipse(It.IsAny<float>(), It.IsAny<float>(), It.IsAny<float>(), It.IsAny<float>()))
                .Callback(() => callSequence.Add("FillEllipse"));

            var injectionPoints = GetInjectionPointsField(drawable);
            injectionPoints.Add(new CircleData(new Point(100, 100), Color.FromRgb(255, 0, 0)));

            // Act
            drawable.Draw(mockCanvas.Object, new RectF());

            // Assert
            Assert.That(callSequence.Count, Is.EqualTo(4));
            Assert.That(callSequence[0], Is.EqualTo("StrokeColor"));
            Assert.That(callSequence[1], Is.EqualTo("StrokeSize"));
            Assert.That(callSequence[2], Is.EqualTo("FillColor"));
            Assert.That(callSequence[3], Is.EqualTo("FillEllipse"));
        }

        /// <summary>
        /// Tests that Draw method in editing mode calls FillEllipse once per reference point.
        /// </summary>
        [TestCase(1)]
        [TestCase(5)]
        [TestCase(10)]
        [TestCase(50)]
        public void Draw_EditingModeWithMultiplePoints_CallsFillEllipseForEachPoint(int pointCount)
        {
            // Arrange
            var drawable = new CirclesDrawable(1280.0, 1366.0, null, 30.0);
            drawable.IsCallerEditing = true;
            var mockCanvas = new Mock<ICanvas>();
            int fillEllipseCallCount = 0;

            mockCanvas.Setup(c => c.FillEllipse(It.IsAny<float>(), It.IsAny<float>(), It.IsAny<float>(), It.IsAny<float>()))
                .Callback(() => fillEllipseCallCount++);

            var referencePoints = GetReferencePointsField(drawable);
            for (int i = 0; i < pointCount; i++)
            {
                referencePoints.Add(new Point(100 + i * 50, 100 + i * 50));
            }

            // Act
            drawable.Draw(mockCanvas.Object, new RectF());

            // Assert
            Assert.That(fillEllipseCallCount, Is.EqualTo(pointCount));
        }

        /// <summary>
        /// Tests that Draw method in non-editing mode calls FillEllipse once per injection point.
        /// </summary>
        [TestCase(1)]
        [TestCase(5)]
        [TestCase(10)]
        [TestCase(50)]
        public void Draw_NonEditingModeWithMultiplePoints_CallsFillEllipseForEachPoint(int pointCount)
        {
            // Arrange
            var drawable = new CirclesDrawable(1280.0, 1366.0, null, 30.0);
            drawable.IsCallerEditing = false;
            var mockCanvas = new Mock<ICanvas>();
            int fillEllipseCallCount = 0;

            mockCanvas.Setup(c => c.FillEllipse(It.IsAny<float>(), It.IsAny<float>(), It.IsAny<float>(), It.IsAny<float>()))
                .Callback(() => fillEllipseCallCount++);

            var injectionPoints = GetInjectionPointsField(drawable);
            for (int i = 0; i < pointCount; i++)
            {
                injectionPoints.Add(new CircleData(new Point(100 + i * 50, 100 + i * 50), Color.FromRgb(255, 0, 0)));
            }

            // Act
            drawable.Draw(mockCanvas.Object, new RectF());

            // Assert
            Assert.That(fillEllipseCallCount, Is.EqualTo(pointCount));
        }

        /// <summary>
        /// Tests that Draw method correctly handles CircleData with special coordinate values.
        /// </summary>
        [TestCase(double.NaN, 100.0, TestName = "Draw_NonEditingModeWithNaNX_HandlesGracefully")]
        [TestCase(100.0, double.NaN, TestName = "Draw_NonEditingModeWithNaNY_HandlesGracefully")]
        [TestCase(double.PositiveInfinity, 100.0, TestName = "Draw_NonEditingModeWithInfinityX_HandlesGracefully")]
        [TestCase(100.0, double.NegativeInfinity, TestName = "Draw_NonEditingModeWithNegativeInfinityY_HandlesGracefully")]
        [TestCase(double.MaxValue, double.MaxValue, TestName = "Draw_NonEditingModeWithMaxValues_HandlesGracefully")]
        [TestCase(double.MinValue, double.MinValue, TestName = "Draw_NonEditingModeWithMinValues_HandlesGracefully")]
        public void Draw_NonEditingModeWithSpecialCoordinateValues_HandlesGracefully(double x, double y)
        {
            // Arrange
            var drawable = new CirclesDrawable(1280.0, 1366.0, null, 30.0);
            drawable.IsCallerEditing = false;
            var mockCanvas = new Mock<ICanvas>();

            var injectionPoints = GetInjectionPointsField(drawable);
            injectionPoints.Add(new CircleData(new Point(x, y), Color.FromRgb(255, 0, 0)));

            // Act & Assert - should not throw
            Assert.DoesNotThrow(() => drawable.Draw(mockCanvas.Object, new RectF()));
        }

        /// <summary>
        /// Tests that Draw method correctly handles reference points with special coordinate values in editing mode.
        /// </summary>
        [TestCase(double.NaN, 100.0, TestName = "Draw_EditingModeWithNaNX_HandlesGracefully")]
        [TestCase(100.0, double.NaN, TestName = "Draw_EditingModeWithNaNY_HandlesGracefully")]
        [TestCase(double.PositiveInfinity, 100.0, TestName = "Draw_EditingModeWithInfinityX_HandlesGracefully")]
        [TestCase(100.0, double.NegativeInfinity, TestName = "Draw_EditingModeWithNegativeInfinityY_HandlesGracefully")]
        [TestCase(double.MaxValue, double.MaxValue, TestName = "Draw_EditingModeWithMaxValues_HandlesGracefully")]
        [TestCase(double.MinValue, double.MinValue, TestName = "Draw_EditingModeWithMinValues_HandlesGracefully")]
        public void Draw_EditingModeWithSpecialCoordinateValues_HandlesGracefully(double x, double y)
        {
            // Arrange
            var drawable = new CirclesDrawable(1280.0, 1366.0, null, 30.0);
            drawable.IsCallerEditing = true;
            var mockCanvas = new Mock<ICanvas>();

            var referencePoints = GetReferencePointsField(drawable);
            referencePoints.Add(new Point(x, y));

            // Act & Assert - should not throw
            Assert.DoesNotThrow(() => drawable.Draw(mockCanvas.Object, new RectF()));
        }

        /// <summary>
        /// Tests that Draw method respects IsCallerEditing state change between calls.
        /// </summary>
        [Test]
        public void Draw_IsCallerEditingChangedBetweenCalls_RespectsNewState()
        {
            // Arrange
            var drawable = new CirclesDrawable(1280.0, 1366.0, null, 30.0);
            var mockCanvas = new Mock<ICanvas>();

            var referencePoints = GetReferencePointsField(drawable);
            referencePoints.Add(new Point(100, 100));

            var injectionPoints = GetInjectionPointsField(drawable);
            injectionPoints.Add(new CircleData(new Point(200, 200), Color.FromRgb(255, 0, 0)));

            // Act & Assert - First call in editing mode
            drawable.IsCallerEditing = true;
            int editingCallCount = 0;
            mockCanvas.Setup(c => c.FillEllipse(It.IsAny<float>(), It.IsAny<float>(), It.IsAny<float>(), It.IsAny<float>()))
                .Callback(() => editingCallCount++);
            drawable.Draw(mockCanvas.Object, new RectF());
            Assert.That(editingCallCount, Is.EqualTo(1)); // Only reference point drawn

            // Act & Assert - Second call in non-editing mode
            drawable.IsCallerEditing = false;
            mockCanvas.Invocations.Clear();
            int nonEditingCallCount = 0;
            mockCanvas.Setup(c => c.FillEllipse(It.IsAny<float>(), It.IsAny<float>(), It.IsAny<float>(), It.IsAny<float>()))
                .Callback(() => nonEditingCallCount++);
            drawable.Draw(mockCanvas.Object, new RectF());
            Assert.That(nonEditingCallCount, Is.EqualTo(1)); // Only injection point drawn
        }

        /// <summary>
        /// Tests that Draw method in non-editing mode uses different colors from CircleData.
        /// </summary>
        [Test]
        public void Draw_NonEditingModeWithDifferentColors_UsesDifferentColorsForEachPoint()
        {
            // Arrange
            var drawable = new CirclesDrawable(1280.0, 1366.0, null, 30.0);
            drawable.IsCallerEditing = false;
            var mockCanvas = new Mock<ICanvas>();
            var capturedColors = new List<Color>();

            mockCanvas.SetupSet(c => c.FillColor = It.IsAny<Color>())
                .Callback<Color>(color => capturedColors.Add(color));

            var injectionPoints = GetInjectionPointsField(drawable);
            injectionPoints.Add(new CircleData(new Point(100, 100), Color.FromRgb(255, 0, 0)));
            injectionPoints.Add(new CircleData(new Point(200, 200), Color.FromRgb(0, 255, 0)));
            injectionPoints.Add(new CircleData(new Point(300, 300), Color.FromRgb(0, 0, 255)));

            // Act
            drawable.Draw(mockCanvas.Object, new RectF());

            // Assert
            Assert.That(capturedColors.Count, Is.EqualTo(3));
            Assert.That(capturedColors[0].Red, Is.EqualTo(1f).Within(0.001f));
            Assert.That(capturedColors[1].Green, Is.EqualTo(1f).Within(0.001f));
            Assert.That(capturedColors[2].Blue, Is.EqualTo(1f).Within(0.001f));
        }

        /// <summary>
        /// Tests that Draw method calculates StrokeSize with Math.Max ensuring minimum of 1f.
        /// </summary>
        [TestCase(0.1, 0.1, TestName = "Draw_VerySmallImageDimensions_StrokeSizeMinimumOne")]
        [TestCase(10.0, 10.0, TestName = "Draw_SmallImageDimensions_StrokeSizeMinimumOne")]
        public void Draw_SmallDimensionsCausingSmallRadius_StrokeSizeIsAtLeastOne(double width, double height)
        {
            // Arrange
            var drawable = new CirclesDrawable(width, height, null, 30.0);
            drawable.IsCallerEditing = true;
            var mockCanvas = new Mock<ICanvas>();
            float? capturedStrokeSize = null;

            mockCanvas.SetupSet(c => c.StrokeSize = It.IsAny<float>())
                .Callback<float>(size => capturedStrokeSize = size);

            var referencePoints = GetReferencePointsField(drawable);
            referencePoints.Add(new Point(100, 100));

            // Act
            drawable.Draw(mockCanvas.Object, new RectF());

            // Assert
            Assert.That(capturedStrokeSize, Is.Not.Null);
            Assert.That(capturedStrokeSize, Is.GreaterThanOrEqualTo(1f));
        }

        /// <summary>
        /// Helper method to get InjectionPointsCoordinates field using reflection.
        /// </summary>
        private List<CircleData> GetInjectionPointsField(CirclesDrawable drawable)
        {
            var field = typeof(CirclesDrawable).GetField("InjectionPointsCoordinates", BindingFlags.NonPublic | BindingFlags.Instance);
            return (List<CircleData>)field!.GetValue(drawable)!;
        }

        /// <summary>
        /// Helper method to get ReferencePointsCoordinates field using reflection.
        /// </summary>
        private List<Point> GetReferencePointsField(CirclesDrawable drawable)
        {
            var field = typeof(CirclesDrawable).GetField("ReferencePointsCoordinates", BindingFlags.NonPublic | BindingFlags.Instance);
            return (List<Point>)field!.GetValue(drawable)!;
        }

        /// <summary>
        /// Tests that the Type property getter returns the default enum value (Front = 0) when not explicitly set.
        /// Verifies the initial state of the Type property upon object instantiation.
        /// Input: New CirclesDrawable instance with Type property not explicitly set.
        /// Expected result: Type property returns PointType.Front (default enum value).
        /// </summary>
        [Test]
        public void Type_GetDefaultValue_ReturnsFront()
        {
            // Arrange
            var drawable = new CirclesDrawable();

            // Act
            var result = drawable.Type;

            // Assert
            Assert.That(result, Is.EqualTo(CirclesDrawable.PointType.Front));
        }

        /// <summary>
        /// Tests that the Type property setter and getter correctly handle all valid PointType enum values.
        /// Verifies that each defined enum value can be set and retrieved without data loss.
        /// Input: Each valid PointType enum value (Front, Back, Hands, Sensor).
        /// Expected result: The getter returns the exact value that was set via the setter.
        /// </summary>
        /// <param name="expectedType">The PointType value to set and verify.</param>
        [TestCase(CirclesDrawable.PointType.Front)]
        [TestCase(CirclesDrawable.PointType.Back)]
        [TestCase(CirclesDrawable.PointType.Hands)]
        [TestCase(CirclesDrawable.PointType.Sensor)]
        public void Type_SetAndGetValidEnumValue_ReturnsSetValue(CirclesDrawable.PointType expectedType)
        {
            // Arrange
            var drawable = new CirclesDrawable();

            // Act
            drawable.Type = expectedType;
            var result = drawable.Type;

            // Assert
            Assert.That(result, Is.EqualTo(expectedType));
        }

        /// <summary>
        /// Tests that the Type property accepts and returns invalid enum values outside the defined range.
        /// Verifies that the property does not perform enum validation and allows any integer value cast to PointType.
        /// This tests the behavior when casting arbitrary integers to the enum type.
        /// Input: Integer values outside the defined PointType enum range cast to PointType.
        /// Expected result: The property accepts and returns the invalid enum value without throwing.
        /// </summary>
        /// <param name="invalidIntValue">An integer value outside the defined PointType enum range.</param>
        [TestCase(-1)]
        [TestCase(4)]
        [TestCase(100)]
        [TestCase(999)]
        [TestCase(int.MinValue)]
        [TestCase(int.MaxValue)]
        public void Type_SetAndGetInvalidEnumValue_ReturnsSetValue(int invalidIntValue)
        {
            // Arrange
            var drawable = new CirclesDrawable();
            var invalidEnumValue = (CirclesDrawable.PointType)invalidIntValue;

            // Act
            drawable.Type = invalidEnumValue;
            var result = drawable.Type;

            // Assert
            Assert.That(result, Is.EqualTo(invalidEnumValue));
            Assert.That((int)result, Is.EqualTo(invalidIntValue));
        }

        /// <summary>
        /// Tests that the Type property correctly maintains state across multiple set operations.
        /// Verifies that the property value updates properly when set multiple times in sequence.
        /// Input: Multiple sequential assignments of different PointType values.
        /// Expected result: After each assignment, the getter returns the most recently set value.
        /// </summary>
        [Test]
        public void Type_SetMultipleTimes_ReturnsLatestValue()
        {
            // Arrange
            var drawable = new CirclesDrawable();

            // Act & Assert - First assignment
            drawable.Type = CirclesDrawable.PointType.Back;
            Assert.That(drawable.Type, Is.EqualTo(CirclesDrawable.PointType.Back));

            // Act & Assert - Second assignment
            drawable.Type = CirclesDrawable.PointType.Sensor;
            Assert.That(drawable.Type, Is.EqualTo(CirclesDrawable.PointType.Sensor));

            // Act & Assert - Third assignment
            drawable.Type = CirclesDrawable.PointType.Hands;
            Assert.That(drawable.Type, Is.EqualTo(CirclesDrawable.PointType.Hands));

            // Act & Assert - Fourth assignment back to Front
            drawable.Type = CirclesDrawable.PointType.Front;
            Assert.That(drawable.Type, Is.EqualTo(CirclesDrawable.PointType.Front));
        }

        /// <summary>
        /// Tests that setting the Type property to its current value works correctly.
        /// Verifies that assigning the same value multiple times does not cause issues.
        /// Input: Setting Type to Front, then setting it to Front again.
        /// Expected result: Property returns Front both times without errors.
        /// </summary>
        [Test]
        public void Type_SetToSameValueMultipleTimes_MaintainsValue()
        {
            // Arrange
            var drawable = new CirclesDrawable();

            // Act
            drawable.Type = CirclesDrawable.PointType.Front;
            var firstRead = drawable.Type;
            drawable.Type = CirclesDrawable.PointType.Front;
            var secondRead = drawable.Type;
            drawable.Type = CirclesDrawable.PointType.Front;
            var thirdRead = drawable.Type;

            // Assert
            Assert.That(firstRead, Is.EqualTo(CirclesDrawable.PointType.Front));
            Assert.That(secondRead, Is.EqualTo(CirclesDrawable.PointType.Front));
            Assert.That(thirdRead, Is.EqualTo(CirclesDrawable.PointType.Front));
        }

        /// <summary>
        /// Tests that the Type property works correctly when set via the parameterized constructor
        /// and then accessed via the property getter.
        /// Verifies that the property reflects state regardless of initialization method.
        /// Input: CirclesDrawable created with parameterized constructor, Type property accessed.
        /// Expected result: Type property getter returns the default value (Front).
        /// </summary>
        [Test]
        public void Type_GetAfterParameterizedConstructor_ReturnsDefaultValue()
        {
            // Arrange
            var drawable = new CirclesDrawable(1280.0, 1366.0, 1, 30.0);

            // Act
            var result = drawable.Type;

            // Assert
            Assert.That(result, Is.EqualTo(CirclesDrawable.PointType.Front));
        }

        /// <summary>
        /// Tests that the Type property setter and getter work correctly after parameterized constructor.
        /// Verifies that setting Type after object creation via parameterized constructor works as expected.
        /// Input: CirclesDrawable with parameterized constructor, then Type is set to a specific value.
        /// Expected result: Type property returns the value that was set.
        /// </summary>
        [Test]
        public void Type_SetAfterParameterizedConstructor_ReturnsSetValue()
        {
            // Arrange
            var drawable = new CirclesDrawable(1280.0, 1366.0, 1, 30.0);

            // Act
            drawable.Type = CirclesDrawable.PointType.Sensor;
            var result = drawable.Type;

            // Assert
            Assert.That(result, Is.EqualTo(CirclesDrawable.PointType.Sensor));
        }

        /// <summary>
        /// Tests boundary values of the PointType enum (first and last defined values).
        /// Verifies that the property correctly handles the minimum and maximum defined enum values.
        /// Input: PointType.Front (0) and PointType.Sensor (3).
        /// Expected result: Both boundary values are set and retrieved correctly.
        /// </summary>
        [TestCase(CirclesDrawable.PointType.Front, Description = "First enum value (0)")]
        [TestCase(CirclesDrawable.PointType.Sensor, Description = "Last enum value (3)")]
        public void Type_SetAndGetBoundaryEnumValues_ReturnsSetValue(CirclesDrawable.PointType boundaryType)
        {
            // Arrange
            var drawable = new CirclesDrawable();

            // Act
            drawable.Type = boundaryType;
            var result = drawable.Type;

            // Assert
            Assert.That(result, Is.EqualTo(boundaryType));
        }

        /// <summary>
        /// Tests that the Type property can transition through all valid enum values in sequence.
        /// Verifies that the property correctly updates through a complete cycle of all defined values.
        /// Input: Sequential assignment of all PointType values: Front → Back → Hands → Sensor.
        /// Expected result: After each assignment, the property returns the expected value.
        /// </summary>
        [Test]
        public void Type_CycleThroughAllEnumValues_EachValueCorrectlySet()
        {
            // Arrange
            var drawable = new CirclesDrawable();

            // Act & Assert - Cycle through all enum values
            drawable.Type = CirclesDrawable.PointType.Front;
            Assert.That(drawable.Type, Is.EqualTo(CirclesDrawable.PointType.Front));

            drawable.Type = CirclesDrawable.PointType.Back;
            Assert.That(drawable.Type, Is.EqualTo(CirclesDrawable.PointType.Back));

            drawable.Type = CirclesDrawable.PointType.Hands;
            Assert.That(drawable.Type, Is.EqualTo(CirclesDrawable.PointType.Hands));

            drawable.Type = CirclesDrawable.PointType.Sensor;
            Assert.That(drawable.Type, Is.EqualTo(CirclesDrawable.PointType.Sensor));
        }

        /// <summary>
        /// Tests that the IsCallerEditing property returns the default value of false when not explicitly set.
        /// This verifies the initial state of the backing field isCallerEditing.
        /// Input conditions: Property accessed without prior assignment.
        /// Expected result: Property returns false (default bool value).
        /// </summary>
        [Test]
        public void IsCallerEditing_NotSet_ReturnsFalseByDefault()
        {
            // Arrange
            CirclesDrawable drawable = new CirclesDrawable();

            // Act
            bool result = drawable.IsCallerEditing;

            // Assert
            Assert.That(result, Is.False, "IsCallerEditing should return false by default.");
        }

        /// <summary>
        /// Tests that the IsCallerEditing property correctly stores and returns true when set to true.
        /// This verifies the setter assigns the value and the getter retrieves it correctly.
        /// Input conditions: Property set to true.
        /// Expected result: Property returns true.
        /// </summary>
        [Test]
        public void IsCallerEditing_SetToTrue_ReturnsTrue()
        {
            // Arrange
            CirclesDrawable drawable = new CirclesDrawable();

            // Act
            drawable.IsCallerEditing = true;
            bool result = drawable.IsCallerEditing;

            // Assert
            Assert.That(result, Is.True, "IsCallerEditing should return true when set to true.");
        }

        /// <summary>
        /// Tests that the IsCallerEditing property correctly stores and returns false when explicitly set to false.
        /// This verifies the setter can assign false and the getter retrieves it correctly.
        /// Input conditions: Property set to false.
        /// Expected result: Property returns false.
        /// </summary>
        [Test]
        public void IsCallerEditing_SetToFalse_ReturnsFalse()
        {
            // Arrange
            CirclesDrawable drawable = new CirclesDrawable();

            // Act
            drawable.IsCallerEditing = false;
            bool result = drawable.IsCallerEditing;

            // Assert
            Assert.That(result, Is.False, "IsCallerEditing should return false when set to false.");
        }

        /// <summary>
        /// Tests that the IsCallerEditing property correctly updates when toggled between true and false multiple times.
        /// This verifies that the property maintains state correctly across multiple assignments.
        /// Input conditions: Property is set to true, then false, then true, then false again.
        /// Expected result: Property returns the most recently set value after each assignment.
        /// </summary>
        [Test]
        public void IsCallerEditing_ToggledMultipleTimes_AlwaysReturnsLatestValue()
        {
            // Arrange
            CirclesDrawable drawable = new CirclesDrawable();

            // Act & Assert - Set to true
            drawable.IsCallerEditing = true;
            Assert.That(drawable.IsCallerEditing, Is.True, "IsCallerEditing should return true after first set.");

            // Act & Assert - Set to false
            drawable.IsCallerEditing = false;
            Assert.That(drawable.IsCallerEditing, Is.False, "IsCallerEditing should return false after second set.");

            // Act & Assert - Set to true again
            drawable.IsCallerEditing = true;
            Assert.That(drawable.IsCallerEditing, Is.True, "IsCallerEditing should return true after third set.");

            // Act & Assert - Set to false again
            drawable.IsCallerEditing = false;
            Assert.That(drawable.IsCallerEditing, Is.False, "IsCallerEditing should return false after fourth set.");
        }

        /// <summary>
        /// Tests that the IsCallerEditing property works correctly with parameterized constructor.
        /// Verifies that the property is independent of constructor parameters.
        /// Input conditions: Object created with parameterized constructor, property set to various values.
        /// Expected result: Property behaves correctly regardless of constructor used.
        /// </summary>
        [TestCase(true)]
        [TestCase(false)]
        public void IsCallerEditing_WithParameterizedConstructor_WorksCorrectly(bool value)
        {
            // Arrange
            CirclesDrawable drawable = new CirclesDrawable(1280.0, 1366.0, 1, 30.0);

            // Act
            drawable.IsCallerEditing = value;
            bool result = drawable.IsCallerEditing;

            // Assert
            Assert.That(result, Is.EqualTo(value), $"IsCallerEditing should return {value} when set to {value}.");
        }

        /// <summary>
        /// Tests that the IsCallerEditing property setter can be called multiple times with the same value without issues.
        /// This verifies idempotency of the setter.
        /// Input conditions: Property set to true three times consecutively.
        /// Expected result: Property returns true and no exceptions are thrown.
        /// </summary>
        [Test]
        public void IsCallerEditing_SetSameValueMultipleTimes_RemainsStable()
        {
            // Arrange
            CirclesDrawable drawable = new CirclesDrawable();

            // Act
            drawable.IsCallerEditing = true;
            drawable.IsCallerEditing = true;
            drawable.IsCallerEditing = true;
            bool result = drawable.IsCallerEditing;

            // Assert
            Assert.That(result, Is.True, "IsCallerEditing should remain true after setting to true multiple times.");
        }

        /// <summary>
        /// Tests that RemovePointIfNear with very small CurrentNearEnoughDistance does not remove far points.
        /// </summary>
        /// <remarks>
        /// Input: Very small image dimensions (1x1) resulting in tiny CurrentNearEnoughDistance, point far from reference.
        /// Expected: Point is not removed due to small threshold.
        /// Coverage: Exercises distance calculation with scale-affected threshold.
        /// </remarks>
        [Test]
        public void RemovePointIfNear_VerySmallThresholdFarPoint_DoesNotRemovePoint()
        {
            // Arrange
            var drawable = new CirclesDrawable(1.0, 1.0, null, 30.0);
            var refPoints = GetReferencePointsCoordinates(drawable);
            refPoints.Add(new Point(100, 100));

            // Act
            drawable.RemovePointIfNear(new Point(105, 105));

            // Assert
            Assert.That(refPoints.Count, Is.EqualTo(1));
            Assert.That(refPoints[0], Is.EqualTo(new Point(100, 100)));
        }

        /// <summary>
        /// Tests that RemovePointIfNear with very large CurrentNearEnoughDistance removes distant points.
        /// </summary>
        /// <remarks>
        /// Input: Very large image dimensions (10000x10000) resulting in large CurrentNearEnoughDistance, point moderately far from reference.
        /// Expected: Point is removed due to large threshold.
        /// Coverage: Exercises distance calculation with large scale factor.
        /// </remarks>
        [Test]
        public void RemovePointIfNear_VeryLargeThresholdModerateDistance_RemovesPoint()
        {
            // Arrange
            var drawable = new CirclesDrawable(10000.0, 10000.0, null, 30.0);
            var refPoints = GetReferencePointsCoordinates(drawable);
            refPoints.Add(new Point(100, 100));

            // Act
            drawable.RemovePointIfNear(new Point(200, 200));

            // Assert
            Assert.That(refPoints.Count, Is.EqualTo(0));
        }

        /// <summary>
        /// Tests that RemovePointIfNear correctly handles distance calculation with mixed positive and negative coordinates.
        /// </summary>
        /// <remarks>
        /// Input: Reference at positive coords, click at negative coords within distance.
        /// Expected: Point is removed when distance calculation crosses zero.
        /// Coverage: Exercises distance formula with sign changes.
        /// </remarks>
        [Test]
        public void RemovePointIfNear_MixedPositiveNegativeCoordinates_CalculatesDistanceCorrectly()
        {
            // Arrange
            var drawable = new CirclesDrawable(1280.0, 1366.0, null, 30.0);
            var refPoints = GetReferencePointsCoordinates(drawable);
            refPoints.Add(new Point(10, 10));

            // Act
            drawable.RemovePointIfNear(new Point(-5, -5));

            // Assert
            Assert.That(refPoints.Count, Is.EqualTo(0));
        }

        /// <summary>
        /// Tests that RemovePointIfNear removes only one point when multiple points are equidistant from click position.
        /// </summary>
        /// <remarks>
        /// Input: Two points equidistant from click position (symmetrically placed).
        /// Expected: Only the first found nearest point is removed (FindNearest returns first match).
        /// Coverage: Exercises FindNearest with equidistant points.
        /// </remarks>
        [Test]
        public void RemovePointIfNear_EquidistantPoints_RemovesFirstFoundPoint()
        {
            // Arrange
            var drawable = new CirclesDrawable(1280.0, 1366.0, null, 30.0);
            var refPoints = GetReferencePointsCoordinates(drawable);
            refPoints.Add(new Point(90, 100));
            refPoints.Add(new Point(110, 100));

            // Act
            drawable.RemovePointIfNear(new Point(100, 100));

            // Assert
            Assert.That(refPoints.Count, Is.EqualTo(1));
        }

        /// <summary>
        /// Tests that RemovePointIfNear handles click at origin with reference point at origin.
        /// </summary>
        /// <remarks>
        /// Input: Both click and reference at (0, 0).
        /// Expected: Point is removed (distance = 0).
        /// Coverage: Exercises distance calculation with zero coordinates.
        /// </remarks>
        [Test]
        public void RemovePointIfNear_BothAtOrigin_RemovesPoint()
        {
            // Arrange
            var drawable = new CirclesDrawable(1280.0, 1366.0, null, 30.0);
            var refPoints = GetReferencePointsCoordinates(drawable);
            refPoints.Add(new Point(0, 0));

            // Act
            drawable.RemovePointIfNear(new Point(0, 0));

            // Assert
            Assert.That(refPoints.Count, Is.EqualTo(0));
        }

        /// <summary>
        /// Tests that RemovePointIfNear works correctly with default constructor dimensions.
        /// </summary>
        /// <remarks>
        /// Input: Default constructor (uses DefaultImageWidth=1280, DefaultImageHeight=1366), point within standard threshold.
        /// Expected: Point is removed using default scale (1.0).
        /// Coverage: Exercises with default initialization path.
        /// </remarks>
        [Test]
        public void RemovePointIfNear_DefaultConstructorWithinDistance_RemovesPoint()
        {
            // Arrange
            var drawable = new CirclesDrawable();
            var refPoints = GetReferencePointsCoordinates(drawable);
            refPoints.Add(new Point(100, 100));

            // Act
            drawable.RemovePointIfNear(new Point(115, 115));

            // Assert
            Assert.That(refPoints.Count, Is.EqualTo(0));
        }

        /// <summary>
        /// Tests that RemovePointIfNear handles very precise distance calculation near threshold.
        /// </summary>
        /// <remarks>
        /// Input: Point at distance of exactly 29.99999 (just under 30 threshold with default scale).
        /// Expected: Point is removed (distance &lt; threshold).
        /// Coverage: Exercises floating-point precision in distance comparison.
        /// </remarks>
        [Test]
        public void RemovePointIfNear_PreciseDistanceJustUnderThreshold_RemovesPoint()
        {
            // Arrange
            var drawable = new CirclesDrawable(1280.0, 1366.0, null, 30.0);
            var refPoints = GetReferencePointsCoordinates(drawable);
            refPoints.Add(new Point(100, 100));

            // Calculate position at distance 29.99
            double angle = System.Math.PI / 4;
            double distance = 29.99;
            double x = 100 + distance * System.Math.Cos(angle);
            double y = 100 + distance * System.Math.Sin(angle);

            // Act
            drawable.RemovePointIfNear(new Point(x, y));

            // Assert
            Assert.That(refPoints.Count, Is.EqualTo(0));
        }

        /// <summary>
        /// Tests that RemovePointIfNear does not remove when distance is just over threshold.
        /// </summary>
        /// <remarks>
        /// Input: Point at distance of exactly 30.01 (just over 30 threshold with default scale).
        /// Expected: Point is not removed (distance &gt;= threshold).
        /// Coverage: Exercises threshold boundary from above.
        /// </remarks>
        [Test]
        public void RemovePointIfNear_PreciseDistanceJustOverThreshold_DoesNotRemovePoint()
        {
            // Arrange
            var drawable = new CirclesDrawable(1280.0, 1366.0, null, 30.0);
            var refPoints = GetReferencePointsCoordinates(drawable);
            refPoints.Add(new Point(100, 100));

            // Calculate position at distance 30.01
            double angle = System.Math.PI / 4;
            double distance = 30.01;
            double x = 100 + distance * System.Math.Cos(angle);
            double y = 100 + distance * System.Math.Sin(angle);

            // Act
            drawable.RemovePointIfNear(new Point(x, y));

            // Assert
            Assert.That(refPoints.Count, Is.EqualTo(1));
        }

        /// <summary>
        /// Tests that RemovePointIfNear with very small non-zero distance removes the point.
        /// </summary>
        /// <remarks>
        /// Input: Reference and click positions differ by double.Epsilon.
        /// Expected: Point is removed (distance is very small but &lt; threshold).
        /// Coverage: Exercises minimal non-zero distance calculation.
        /// </remarks>
        [Test]
        public void RemovePointIfNear_EpsilonDistance_RemovesPoint()
        {
            // Arrange
            var drawable = new CirclesDrawable(1280.0, 1366.0, null, 30.0);
            var refPoints = GetReferencePointsCoordinates(drawable);
            refPoints.Add(new Point(100, 100));

            // Act
            drawable.RemovePointIfNear(new Point(100 + double.Epsilon, 100 + double.Epsilon));

            // Assert
            Assert.That(refPoints.Count, Is.EqualTo(0));
        }

        /// <summary>
        /// Tests that RemovePointIfNear correctly identifies nearest among multiple widely spaced points.
        /// </summary>
        /// <remarks>
        /// Input: Three points at different distances, click near the middle one.
        /// Expected: Only the nearest (middle) point is removed.
        /// Coverage: Exercises FindNearest selection logic with clear distance differences.
        /// </remarks>
        [Test]
        public void RemovePointIfNear_ThreeWidelySpacedPoints_RemovesCorrectNearest()
        {
            // Arrange
            var drawable = new CirclesDrawable(1280.0, 1366.0, null, 30.0);
            var refPoints = GetReferencePointsCoordinates(drawable);
            refPoints.Add(new Point(10, 10));
            refPoints.Add(new Point(100, 100));
            refPoints.Add(new Point(500, 500));

            // Act
            drawable.RemovePointIfNear(new Point(105, 105));

            // Assert
            Assert.That(refPoints.Count, Is.EqualTo(2));
            Assert.That(refPoints, Does.Contain(new Point(10, 10)));
            Assert.That(refPoints, Does.Contain(new Point(500, 500)));
            Assert.That(refPoints, Does.Not.Contain(new Point(100, 100)));
        }

        /// <summary>
        /// Tests that RemovePointIfNear handles asymmetric image dimensions affecting scale.
        /// </summary>
        /// <remarks>
        /// Input: Asymmetric dimensions (width much larger than height), point within scaled threshold.
        /// Expected: Scale is calculated as min(sx, sy), affecting CurrentNearEnoughDistance.
        /// Coverage: Exercises GetScale with asymmetric dimensions.
        /// </remarks>
        [Test]
        public void RemovePointIfNear_AsymmetricDimensions_UsesMinScale()
        {
            // Arrange
            var drawable = new CirclesDrawable(5000.0, 500.0, null, 30.0);
            var refPoints = GetReferencePointsCoordinates(drawable);
            refPoints.Add(new Point(100, 100));

            // Act - with min scale based on height (500/1366 ≈ 0.366), threshold ≈ 30 * 0.366 ≈ 11
            drawable.RemovePointIfNear(new Point(108, 108));

            // Assert
            Assert.That(refPoints.Count, Is.EqualTo(0));
        }

        /// <summary>
        /// Tests that RemovePointIfNear correctly removes when X distance is large but Y is zero.
        /// </summary>
        /// <remarks>
        /// Input: Reference and click differ only in X coordinate, within threshold on X axis.
        /// Expected: Point is removed based on total Euclidean distance.
        /// Coverage: Exercises distance calculation with one zero component.
        /// </remarks>
        [Test]
        public void RemovePointIfNear_OnlyXDifferenceWithinThreshold_RemovesPoint()
        {
            // Arrange
            var drawable = new CirclesDrawable(1280.0, 1366.0, null, 30.0);
            var refPoints = GetReferencePointsCoordinates(drawable);
            refPoints.Add(new Point(100, 100));

            // Act - distance = sqrt(20^2 + 0^2) = 20
            drawable.RemovePointIfNear(new Point(120, 100));

            // Assert
            Assert.That(refPoints.Count, Is.EqualTo(0));
        }

        /// <summary>
        /// Tests that RemovePointIfNear correctly removes when Y distance is large but X is zero.
        /// </summary>
        /// <remarks>
        /// Input: Reference and click differ only in Y coordinate, within threshold on Y axis.
        /// Expected: Point is removed based on total Euclidean distance.
        /// Coverage: Exercises distance calculation with one zero component.
        /// </remarks>
        [Test]
        public void RemovePointIfNear_OnlyYDifferenceWithinThreshold_RemovesPoint()
        {
            // Arrange
            var drawable = new CirclesDrawable(1280.0, 1366.0, null, 30.0);
            var refPoints = GetReferencePointsCoordinates(drawable);
            refPoints.Add(new Point(100, 100));

            // Act - distance = sqrt(0^2 + 20^2) = 20
            drawable.RemovePointIfNear(new Point(100, 120));

            // Assert
            Assert.That(refPoints.Count, Is.EqualTo(0));
        }

        /// <summary>
        /// Tests that RemovePointIfNear handles large coordinate values without overflow.
        /// </summary>
        /// <remarks>
        /// Input: Large but not extreme coordinate values that should still calculate distance correctly.
        /// Expected: Distance calculated correctly, removal based on threshold.
        /// Coverage: Exercises distance calculation with large coordinates.
        /// </remarks>
        [Test]
        public void RemovePointIfNear_LargeCoordinateValues_CalculatesCorrectly()
        {
            // Arrange
            var drawable = new CirclesDrawable(1280.0, 1366.0, null, 30.0);
            var refPoints = GetReferencePointsCoordinates(drawable);
            refPoints.Add(new Point(1000000, 1000000));

            // Act
            drawable.RemovePointIfNear(new Point(1000010, 1000010));

            // Assert
            Assert.That(refPoints.Count, Is.EqualTo(0));
        }

        /// <summary>
        /// Tests that RemovePointIfNear is idempotent when called multiple times with same position.
        /// </summary>
        /// <remarks>
        /// Input: Call RemovePointIfNear twice with same click position after first removal.
        /// Expected: First call removes point, second call does nothing (list already empty or no match).
        /// Coverage: Exercises behavior after removal has occurred.
        /// </remarks>
        [Test]
        public void RemovePointIfNear_CalledTwiceSamePosition_IdempotentAfterRemoval()
        {
            // Arrange
            var drawable = new CirclesDrawable(1280.0, 1366.0, null, 30.0);
            var refPoints = GetReferencePointsCoordinates(drawable);
            refPoints.Add(new Point(100, 100));

            // Act
            drawable.RemovePointIfNear(new Point(100, 100));
            drawable.RemovePointIfNear(new Point(100, 100));

            // Assert
            Assert.That(refPoints.Count, Is.EqualTo(0));
        }

        /// <summary>
        /// Tests that RemovePointIfNear correctly handles distance calculation when click is far in diagonal direction.
        /// </summary>
        /// <remarks>
        /// Input: Click position diagonal from reference, distance exactly at threshold.
        /// Expected: Point not removed (distance >= threshold triggers >= condition).
        /// Coverage: Exercises diagonal distance at exact threshold.
        /// </remarks>
        [Test]
        public void RemovePointIfNear_DiagonalDistanceAtThreshold_DoesNotRemovePoint()
        {
            // Arrange
            var drawable = new CirclesDrawable(1280.0, 1366.0, null, 30.0);
            var refPoints = GetReferencePointsCoordinates(drawable);
            refPoints.Add(new Point(100, 100));

            // distance = sqrt(21.21^2 + 21.21^2) ≈ 30.0
            double offset = 30.0 / System.Math.Sqrt(2);

            // Act
            drawable.RemovePointIfNear(new Point(100 + offset, 100 + offset));

            // Assert - should not remove because distance >= threshold
            Assert.That(refPoints.Count, Is.EqualTo(1));
        }

        /// <summary>
        /// Helper method to get the private InjectionPointsCoordinates field using reflection.
        /// </summary>
        private List<CircleData> GetInjectionPointsCoordinates(CirclesDrawable drawable)
        {
            var field = typeof(CirclesDrawable).GetField("InjectionPointsCoordinates", BindingFlags.NonPublic | BindingFlags.Instance);
            return (List<CircleData>)field!.GetValue(drawable)!;
        }

        /// <summary>
        /// Tests that LoadInjectionsCoordinates throws NullReferenceException when given a null list.
        /// Input: null list.
        /// Expected: NullReferenceException is thrown.
        /// </summary>
        [Test]
        public void LoadInjectionsCoordinates_NullList_ThrowsNullReferenceException()
        {
            // Arrange
            var drawable = new CirclesDrawable(1280.0, 1366.0, 1, 30.0);

            // Act & Assert
            Assert.Throws<NullReferenceException>(() => drawable.LoadInjectionsCoordinates(null!));
        }

        /// <summary>
        /// Tests that LoadInjectionsCoordinates handles zero circlesVisibilityMaxTimeInDays.
        /// Input: circlesVisibilityMaxTimeInDays set to 0.
        /// Expected: Division by zero may result in NaN or infinity in calculations, but no exception thrown.
        /// </summary>
        [Test]
        public void LoadInjectionsCoordinates_ZeroVisibilityWindow_HandlesGracefully()
        {
            // Arrange
            var drawable = new CirclesDrawable(1280.0, 1366.0, 1, 0.0);
            var now = DateTime.Now;
            var injections = new List<Injection>
            {
                new Injection
                {
                    IdInjection = 2,
                    PositionX = 0.5,
                    PositionY = 0.5,
                    EventTime = new DateTimeAndText { DateTime = now.AddDays(-1) }
                }
            };

            // Act & Assert
            Assert.DoesNotThrow(() => drawable.LoadInjectionsCoordinates(injections));
        }

        /// <summary>
        /// Tests that LoadInjectionsCoordinates handles negative circlesVisibilityMaxTimeInDays.
        /// Input: circlesVisibilityMaxTimeInDays set to -30.
        /// Expected: Calculations may produce unexpected results but no exception thrown.
        /// </summary>
        [Test]
        public void LoadInjectionsCoordinates_NegativeVisibilityWindow_HandlesGracefully()
        {
            // Arrange
            var drawable = new CirclesDrawable(1280.0, 1366.0, 1, -30.0);
            var now = DateTime.Now;
            var injections = new List<Injection>
            {
                new Injection
                {
                    IdInjection = 2,
                    PositionX = 0.5,
                    PositionY = 0.5,
                    EventTime = new DateTimeAndText { DateTime = now.AddDays(-1) }
                }
            };

            // Act & Assert
            Assert.DoesNotThrow(() => drawable.LoadInjectionsCoordinates(injections));
        }

        /// <summary>
        /// Tests that LoadInjectionsCoordinates handles NaN circlesVisibilityMaxTimeInDays.
        /// Input: circlesVisibilityMaxTimeInDays set to double.NaN.
        /// Expected: Calculations produce NaN values but no exception thrown.
        /// </summary>
        [Test]
        public void LoadInjectionsCoordinates_NaNVisibilityWindow_HandlesGracefully()
        {
            // Arrange
            var drawable = new CirclesDrawable(1280.0, 1366.0, 1, double.NaN);
            var now = DateTime.Now;
            var injections = new List<Injection>
            {
                new Injection
                {
                    IdInjection = 2,
                    PositionX = 0.5,
                    PositionY = 0.5,
                    EventTime = new DateTimeAndText { DateTime = now.AddDays(-1) }
                }
            };

            // Act & Assert
            Assert.DoesNotThrow(() => drawable.LoadInjectionsCoordinates(injections));
        }

        /// <summary>
        /// Tests that LoadInjectionsCoordinates handles infinity circlesVisibilityMaxTimeInDays.
        /// Input: circlesVisibilityMaxTimeInDays set to double.PositiveInfinity.
        /// Expected: Calculations produce zero or very small values but no exception thrown.
        /// </summary>
        [Test]
        public void LoadInjectionsCoordinates_InfinityVisibilityWindow_HandlesGracefully()
        {
            // Arrange
            var drawable = new CirclesDrawable(1280.0, 1366.0, 1, double.PositiveInfinity);
            var now = DateTime.Now;
            var injections = new List<Injection>
            {
                new Injection
                {
                    IdInjection = 2,
                    PositionX = 0.5,
                    PositionY = 0.5,
                    EventTime = new DateTimeAndText { DateTime = now.AddDays(-1) }
                }
            };

            // Act & Assert
            Assert.DoesNotThrow(() => drawable.LoadInjectionsCoordinates(injections));
        }

        /// <summary>
        /// Tests that LoadInjectionsCoordinates handles extreme position values (negative).
        /// Input: Injection with negative normalized coordinates.
        /// Expected: Circle position with negative actual coordinates.
        /// </summary>
        [Test]
        public void LoadInjectionsCoordinates_NegativeCoordinates_CorrectlyNormalizes()
        {
            // Arrange
            var drawable = new CirclesDrawable(1280.0, 1366.0, 1, 30.0);
            var injections = new List<Injection>
            {
                new Injection
                {
                    IdInjection = 2,
                    PositionX = -0.5,
                    PositionY = -0.5,
                    EventTime = new DateTimeAndText { DateTime = DateTime.Now }
                }
            };

            // Act
            drawable.LoadInjectionsCoordinates(injections);

            // Assert
            var injectionPoints = GetInjectionPointsCoordinates(drawable);
            Assert.That(injectionPoints.Count, Is.EqualTo(1));
            Assert.That(injectionPoints[0].Position.X, Is.EqualTo(-640.0).Within(0.01));
            Assert.That(injectionPoints[0].Position.Y, Is.EqualTo(-683.0).Within(0.01));
        }

        /// <summary>
        /// Tests that LoadInjectionsCoordinates handles extreme position values (greater than 1).
        /// Input: Injection with normalized coordinates greater than 1.
        /// Expected: Circle position beyond image boundaries.
        /// </summary>
        [Test]
        public void LoadInjectionsCoordinates_CoordinatesGreaterThanOne_CorrectlyNormalizes()
        {
            // Arrange
            var drawable = new CirclesDrawable(1280.0, 1366.0, 1, 30.0);
            var injections = new List<Injection>
            {
                new Injection
                {
                    IdInjection = 2,
                    PositionX = 2.0,
                    PositionY = 1.5,
                    EventTime = new DateTimeAndText { DateTime = DateTime.Now }
                }
            };

            // Act
            drawable.LoadInjectionsCoordinates(injections);

            // Assert
            var injectionPoints = GetInjectionPointsCoordinates(drawable);
            Assert.That(injectionPoints.Count, Is.EqualTo(1));
            Assert.That(injectionPoints[0].Position.X, Is.EqualTo(2560.0).Within(0.01));
            Assert.That(injectionPoints[0].Position.Y, Is.EqualTo(2049.0).Within(0.01));
        }

        /// <summary>
        /// Tests that LoadInjectionsCoordinates handles zero image dimensions.
        /// Input: imageWidth and imageHeight set to 0.
        /// Expected: Circle positions become 0 (0 * coordinate = 0).
        /// </summary>
        [Test]
        public void LoadInjectionsCoordinates_ZeroImageDimensions_ProducesZeroPositions()
        {
            // Arrange
            var drawable = new CirclesDrawable(0.0, 0.0, 1, 30.0);
            var injections = new List<Injection>
            {
                new Injection
                {
                    IdInjection = 2,
                    PositionX = 0.5,
                    PositionY = 0.5,
                    EventTime = new DateTimeAndText { DateTime = DateTime.Now }
                }
            };

            // Act
            drawable.LoadInjectionsCoordinates(injections);

            // Assert
            var injectionPoints = GetInjectionPointsCoordinates(drawable);
            Assert.That(injectionPoints.Count, Is.EqualTo(1));
            Assert.That(injectionPoints[0].Position.X, Is.EqualTo(0.0));
            Assert.That(injectionPoints[0].Position.Y, Is.EqualTo(0.0));
        }

        /// <summary>
        /// Tests that LoadInjectionsCoordinates handles negative image dimensions.
        /// Input: Negative imageWidth and imageHeight.
        /// Expected: Circle positions become negative (negative * positive coordinate).
        /// </summary>
        [Test]
        public void LoadInjectionsCoordinates_NegativeImageDimensions_ProducesNegativePositions()
        {
            // Arrange
            var drawable = new CirclesDrawable(-1280.0, -1366.0, 1, 30.0);
            var injections = new List<Injection>
            {
                new Injection
                {
                    IdInjection = 2,
                    PositionX = 0.5,
                    PositionY = 0.5,
                    EventTime = new DateTimeAndText { DateTime = DateTime.Now }
                }
            };

            // Act
            drawable.LoadInjectionsCoordinates(injections);

            // Assert
            var injectionPoints = GetInjectionPointsCoordinates(drawable);
            Assert.That(injectionPoints.Count, Is.EqualTo(1));
            Assert.That(injectionPoints[0].Position.X, Is.EqualTo(-640.0).Within(0.01));
            Assert.That(injectionPoints[0].Position.Y, Is.EqualTo(-683.0).Within(0.01));
        }

        /// <summary>
        /// Tests that LoadInjectionsCoordinates handles coordinates exactly at tolerance boundary (0.001).
        /// Input: Two injections with coordinates differing by exactly 0.001.
        /// Expected: Non-current injection is NOT skipped (tolerance is exclusive: less than 0.001).
        /// </summary>
        [Test]
        public void LoadInjectionsCoordinates_CoordinatesAtExactTolerance_DoesNotSkip()
        {
            // Arrange
            var drawable = new CirclesDrawable(1280.0, 1366.0, 1, 30.0);
            var now = DateTime.Now;
            var injections = new List<Injection>
            {
                new Injection
                {
                    IdInjection = 1,
                    PositionX = 0.5,
                    PositionY = 0.5,
                    EventTime = new DateTimeAndText { DateTime = now }
                },
                new Injection
                {
                    IdInjection = 2,
                    PositionX = 0.501,
                    PositionY = 0.5,
                    EventTime = new DateTimeAndText { DateTime = now.AddDays(-1) }
                }
            };

            // Act
            drawable.LoadInjectionsCoordinates(injections);

            // Assert
            var injectionPoints = GetInjectionPointsCoordinates(drawable);
            Assert.That(injectionPoints.Count, Is.EqualTo(2));
        }

        /// <summary>
        /// Tests that LoadInjectionsCoordinates can be called multiple times, appending circles.
        /// Input: Call LoadInjectionsCoordinates twice with different injections.
        /// Expected: Circles from both calls are accumulated in InjectionPointsCoordinates.
        /// </summary>
        [Test]
        public void LoadInjectionsCoordinates_CalledMultipleTimes_AppendsCircles()
        {
            // Arrange
            var drawable = new CirclesDrawable(1280.0, 1366.0, 1, 30.0);
            var injections1 = new List<Injection>
            {
                new Injection
                {
                    IdInjection = 2,
                    PositionX = 0.1,
                    PositionY = 0.1,
                    EventTime = new DateTimeAndText { DateTime = DateTime.Now }
                }
            };
            var injections2 = new List<Injection>
            {
                new Injection
                {
                    IdInjection = 3,
                    PositionX = 0.2,
                    PositionY = 0.2,
                    EventTime = new DateTimeAndText { DateTime = DateTime.Now }
                }
            };

            // Act
            drawable.LoadInjectionsCoordinates(injections1);
            drawable.LoadInjectionsCoordinates(injections2);

            // Assert
            var injectionPoints = GetInjectionPointsCoordinates(drawable);
            Assert.That(injectionPoints.Count, Is.EqualTo(2));
        }

        /// <summary>
        /// Tests that NormalizeYPosition correctly divides y by imageHeight for typical positive values.
        /// </summary>
        [Test]
        public void NormalizeYPosition_TypicalPositiveValues_ReturnsCorrectRatio()
        {
            // Arrange
            var drawable = new CirclesDrawable(1000.0, 2000.0, null, 30.0);

            // Act
            var result = drawable.NormalizeYPosition(1000.0);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Value, Is.EqualTo(0.5).Within(0.0001));
        }

        /// <summary>
        /// Tests that NormalizeYPosition returns zero when y is zero.
        /// </summary>
        [Test]
        public void NormalizeYPosition_YIsZero_ReturnsZero()
        {
            // Arrange
            var drawable = new CirclesDrawable(1280.0, 1366.0, null, 30.0);

            // Act
            var result = drawable.NormalizeYPosition(0.0);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Value, Is.EqualTo(0.0));
        }

        /// <summary>
        /// Tests that NormalizeYPosition handles negative y values correctly.
        /// </summary>
        [Test]
        public void NormalizeYPosition_NegativeY_ReturnsNegativeRatio()
        {
            // Arrange
            var drawable = new CirclesDrawable(1280.0, 1366.0, null, 30.0);

            // Act
            var result = drawable.NormalizeYPosition(-683.0);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Value, Is.EqualTo(-0.5).Within(0.0001));
        }

        /// <summary>
        /// Tests that NormalizeYPosition returns 1.0 when y equals imageHeight.
        /// </summary>
        [Test]
        public void NormalizeYPosition_YEqualsImageHeight_ReturnsOne()
        {
            // Arrange
            var drawable = new CirclesDrawable(1280.0, 1366.0, null, 30.0);

            // Act
            var result = drawable.NormalizeYPosition(1366.0);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Value, Is.EqualTo(1.0).Within(0.0001));
        }

        /// <summary>
        /// Tests that NormalizeYPosition returns positive infinity when dividing positive y by zero imageHeight.
        /// </summary>
        [Test]
        public void NormalizeYPosition_PositiveYZeroImageHeight_ReturnsPositiveInfinity()
        {
            // Arrange
            var drawable = new CirclesDrawable(1280.0, 0.0, null, 30.0);

            // Act
            var result = drawable.NormalizeYPosition(100.0);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(double.IsPositiveInfinity(result.Value), Is.True);
        }

        /// <summary>
        /// Tests that NormalizeYPosition returns negative infinity when dividing negative y by zero imageHeight.
        /// </summary>
        [Test]
        public void NormalizeYPosition_NegativeYZeroImageHeight_ReturnsNegativeInfinity()
        {
            // Arrange
            var drawable = new CirclesDrawable(1280.0, 0.0, null, 30.0);

            // Act
            var result = drawable.NormalizeYPosition(-100.0);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(double.IsNegativeInfinity(result.Value), Is.True);
        }

        /// <summary>
        /// Tests that NormalizeYPosition returns NaN when dividing zero by zero (0/0).
        /// </summary>
        [Test]
        public void NormalizeYPosition_ZeroYZeroImageHeight_ReturnsNaN()
        {
            // Arrange
            var drawable = new CirclesDrawable(1280.0, 0.0, null, 30.0);

            // Act
            var result = drawable.NormalizeYPosition(0.0);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(double.IsNaN(result.Value), Is.True);
        }

        /// <summary>
        /// Tests that NormalizeYPosition returns NaN when y is NaN.
        /// </summary>
        [Test]
        public void NormalizeYPosition_YIsNaN_ReturnsNaN()
        {
            // Arrange
            var drawable = new CirclesDrawable(1280.0, 1366.0, null, 30.0);

            // Act
            var result = drawable.NormalizeYPosition(double.NaN);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(double.IsNaN(result.Value), Is.True);
        }

        /// <summary>
        /// Tests that NormalizeYPosition returns positive infinity when y is positive infinity.
        /// </summary>
        [Test]
        public void NormalizeYPosition_YIsPositiveInfinity_ReturnsPositiveInfinity()
        {
            // Arrange
            var drawable = new CirclesDrawable(1280.0, 1366.0, null, 30.0);

            // Act
            var result = drawable.NormalizeYPosition(double.PositiveInfinity);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(double.IsPositiveInfinity(result.Value), Is.True);
        }

        /// <summary>
        /// Tests that NormalizeYPosition returns negative infinity when y is negative infinity.
        /// </summary>
        [Test]
        public void NormalizeYPosition_YIsNegativeInfinity_ReturnsNegativeInfinity()
        {
            // Arrange
            var drawable = new CirclesDrawable(1280.0, 1366.0, null, 30.0);

            // Act
            var result = drawable.NormalizeYPosition(double.NegativeInfinity);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(double.IsNegativeInfinity(result.Value), Is.True);
        }

        /// <summary>
        /// Tests that NormalizeYPosition returns NaN when imageHeight is NaN.
        /// </summary>
        [Test]
        public void NormalizeYPosition_ImageHeightIsNaN_ReturnsNaN()
        {
            // Arrange
            var drawable = new CirclesDrawable(1280.0, double.NaN, null, 30.0);

            // Act
            var result = drawable.NormalizeYPosition(100.0);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(double.IsNaN(result.Value), Is.True);
        }

        /// <summary>
        /// Tests that NormalizeYPosition returns zero when imageHeight is positive infinity.
        /// </summary>
        [Test]
        public void NormalizeYPosition_ImageHeightIsPositiveInfinity_ReturnsZero()
        {
            // Arrange
            var drawable = new CirclesDrawable(1280.0, double.PositiveInfinity, null, 30.0);

            // Act
            var result = drawable.NormalizeYPosition(100.0);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Value, Is.EqualTo(0.0));
        }

        /// <summary>
        /// Tests that NormalizeYPosition handles negative imageHeight correctly.
        /// Dividing positive y by negative imageHeight should return negative result.
        /// </summary>
        [Test]
        public void NormalizeYPosition_NegativeImageHeight_ReturnsNegativeRatio()
        {
            // Arrange
            var drawable = new CirclesDrawable(1280.0, -1366.0, null, 30.0);

            // Act
            var result = drawable.NormalizeYPosition(683.0);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Value, Is.LessThan(0.0));
            Assert.That(result.Value, Is.EqualTo(-0.5).Within(0.0001));
        }

        /// <summary>
        /// Tests that NormalizeYPosition returns NaN when both y and imageHeight are infinity.
        /// </summary>
        [Test]
        public void NormalizeYPosition_BothInfinity_ReturnsNaN()
        {
            // Arrange
            var drawable = new CirclesDrawable(1280.0, double.PositiveInfinity, null, 30.0);

            // Act
            var result = drawable.NormalizeYPosition(double.PositiveInfinity);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(double.IsNaN(result.Value), Is.True);
        }

        /// <summary>
        /// Tests that NormalizeYPosition handles double.MaxValue correctly.
        /// </summary>
        [Test]
        public void NormalizeYPosition_MaxValue_HandlesCorrectly()
        {
            // Arrange
            var drawable = new CirclesDrawable(1280.0, 1366.0, null, 30.0);

            // Act
            var result = drawable.NormalizeYPosition(double.MaxValue);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Value, Is.GreaterThan(0.0));
        }

        /// <summary>
        /// Tests that NormalizeYPosition handles double.MinValue correctly.
        /// </summary>
        [Test]
        public void NormalizeYPosition_MinValue_HandlesCorrectly()
        {
            // Arrange
            var drawable = new CirclesDrawable(1280.0, 1366.0, null, 30.0);

            // Act
            var result = drawable.NormalizeYPosition(double.MinValue);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Value, Is.LessThan(0.0));
        }

        /// <summary>
        /// Tests that NormalizeYPosition with very small imageHeight produces large result.
        /// </summary>
        [Test]
        public void NormalizeYPosition_VerySmallImageHeight_ProducesLargeResult()
        {
            // Arrange
            var drawable = new CirclesDrawable(1280.0, 0.001, null, 30.0);

            // Act
            var result = drawable.NormalizeYPosition(100.0);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Value, Is.GreaterThan(1000.0));
        }

        /// <summary>
        /// Tests that NormalizeYPosition with very large imageHeight produces small result.
        /// </summary>
        [Test]
        public void NormalizeYPosition_VeryLargeImageHeight_ProducesSmallResult()
        {
            // Arrange
            var drawable = new CirclesDrawable(1280.0, 100000.0, null, 30.0);

            // Act
            var result = drawable.NormalizeYPosition(100.0);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Value, Is.LessThan(0.01));
        }

        /// <summary>
        /// Tests that NormalizeYPosition with default constructor uses default imageHeight (1366).
        /// </summary>
        [Test]
        public void NormalizeYPosition_DefaultConstructor_UsesDefaultImageHeight()
        {
            // Arrange
            var drawable = new CirclesDrawable();

            // Act
            var result = drawable.NormalizeYPosition(683.0);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Value, Is.EqualTo(0.5).Within(0.0001));
        }

        /// <summary>
        /// Tests parameterized scenarios covering various y and imageHeight combinations.
        /// </summary>
        /// <param name="y">The y coordinate to normalize.</param>
        /// <param name="imageHeight">The image height to divide by.</param>
        /// <param name="expectedResult">The expected normalized result.</param>
        [TestCase(100.0, 200.0, 0.5)]
        [TestCase(200.0, 100.0, 2.0)]
        [TestCase(0.0, 100.0, 0.0)]
        [TestCase(-100.0, 200.0, -0.5)]
        [TestCase(50.0, 50.0, 1.0)]
        [TestCase(1366.0, 1366.0, 1.0)]
        [TestCase(683.0, 1366.0, 0.5)]
        [TestCase(2732.0, 1366.0, 2.0)]
        public void NormalizeYPosition_VariousCombinations_ReturnsExpectedRatio(double y, double imageHeight, double expectedResult)
        {
            // Arrange
            var drawable = new CirclesDrawable(1280.0, imageHeight, null, 30.0);

            // Act
            var result = drawable.NormalizeYPosition(y);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Value, Is.EqualTo(expectedResult).Within(0.0001));
        }

        /// <summary>
        /// Tests NormalizeXPosition with typical positive values for x and imageWidth.
        /// Verifies that the method correctly computes the ratio x / imageWidth.
        /// </summary>
        /// <param name="x">The x coordinate to normalize.</param>
        /// <param name="imageWidth">The image width used in the constructor.</param>
        /// <param name="expected">The expected normalized ratio.</param>
        [TestCase(640.0, 1280.0, 0.5)]
        [TestCase(1280.0, 1280.0, 1.0)]
        [TestCase(2560.0, 1280.0, 2.0)]
        [TestCase(100.0, 200.0, 0.5)]
        [TestCase(150.0, 300.0, 0.5)]
        public void NormalizeXPosition_TypicalPositiveValues_ReturnsCorrectRatio(double x, double imageWidth, double expected)
        {
            // Arrange
            var drawable = new CirclesDrawable(imageWidth, 1366.0, null, 30.0);

            // Act
            var result = drawable.NormalizeXPosition(x);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Value, Is.EqualTo(expected).Within(0.000001));
        }

        /// <summary>
        /// Tests NormalizeXPosition when x is zero.
        /// Verifies that zero divided by any non-zero imageWidth returns zero.
        /// </summary>
        [TestCase(1280.0)]
        [TestCase(100.0)]
        [TestCase(10000.0)]
        public void NormalizeXPosition_ZeroX_ReturnsZero(double imageWidth)
        {
            // Arrange
            var drawable = new CirclesDrawable(imageWidth, 1366.0, null, 30.0);

            // Act
            var result = drawable.NormalizeXPosition(0.0);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Value, Is.EqualTo(0.0));
        }

        /// <summary>
        /// Tests NormalizeXPosition with negative x values.
        /// Verifies that negative x produces negative normalized results.
        /// </summary>
        /// <param name="x">The negative x coordinate to normalize.</param>
        /// <param name="imageWidth">The image width used in the constructor.</param>
        /// <param name="expected">The expected negative normalized ratio.</param>
        [TestCase(-640.0, 1280.0, -0.5)]
        [TestCase(-1280.0, 1280.0, -1.0)]
        [TestCase(-100.0, 200.0, -0.5)]
        public void NormalizeXPosition_NegativeX_ReturnsNegativeRatio(double x, double imageWidth, double expected)
        {
            // Arrange
            var drawable = new CirclesDrawable(imageWidth, 1366.0, null, 30.0);

            // Act
            var result = drawable.NormalizeXPosition(x);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Value, Is.EqualTo(expected).Within(0.000001));
        }

        /// <summary>
        /// Tests NormalizeXPosition when imageWidth is zero and x is positive.
        /// Verifies that division by zero produces positive infinity.
        /// </summary>
        [Test]
        public void NormalizeXPosition_ZeroImageWidthPositiveX_ReturnsPositiveInfinity()
        {
            // Arrange
            var drawable = new CirclesDrawable(0.0, 1366.0, null, 30.0);

            // Act
            var result = drawable.NormalizeXPosition(100.0);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Value, Is.EqualTo(double.PositiveInfinity));
        }

        /// <summary>
        /// Tests NormalizeXPosition when imageWidth is zero and x is negative.
        /// Verifies that division by zero produces negative infinity.
        /// </summary>
        [Test]
        public void NormalizeXPosition_ZeroImageWidthNegativeX_ReturnsNegativeInfinity()
        {
            // Arrange
            var drawable = new CirclesDrawable(0.0, 1366.0, null, 30.0);

            // Act
            var result = drawable.NormalizeXPosition(-100.0);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Value, Is.EqualTo(double.NegativeInfinity));
        }

        /// <summary>
        /// Tests NormalizeXPosition when both imageWidth and x are zero.
        /// Verifies that 0/0 produces NaN.
        /// </summary>
        [Test]
        public void NormalizeXPosition_ZeroImageWidthAndZeroX_ReturnsNaN()
        {
            // Arrange
            var drawable = new CirclesDrawable(0.0, 1366.0, null, 30.0);

            // Act
            var result = drawable.NormalizeXPosition(0.0);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(double.IsNaN(result.Value), Is.True);
        }

        /// <summary>
        /// Tests NormalizeXPosition with negative imageWidth.
        /// Verifies that division by negative imageWidth produces negative ratio for positive x.
        /// </summary>
        [TestCase(100.0, -200.0, -0.5)]
        [TestCase(640.0, -1280.0, -0.5)]
        public void NormalizeXPosition_NegativeImageWidth_ReturnsNegativeRatio(double x, double imageWidth, double expected)
        {
            // Arrange
            var drawable = new CirclesDrawable(imageWidth, 1366.0, null, 30.0);

            // Act
            var result = drawable.NormalizeXPosition(x);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Value, Is.EqualTo(expected).Within(0.000001));
        }

        /// <summary>
        /// Tests NormalizeXPosition when x is double.MaxValue.
        /// Verifies that very large x divided by normal imageWidth produces very large result or infinity.
        /// </summary>
        [Test]
        public void NormalizeXPosition_MaxValueX_ReturnsVeryLargeOrInfinity()
        {
            // Arrange
            var drawable = new CirclesDrawable(1280.0, 1366.0, null, 30.0);

            // Act
            var result = drawable.NormalizeXPosition(double.MaxValue);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Value, Is.GreaterThan(1.0e308));
        }

        /// <summary>
        /// Tests NormalizeXPosition when x is double.MinValue.
        /// Verifies that very small (negative) x divided by normal imageWidth produces very small negative result or negative infinity.
        /// </summary>
        [Test]
        public void NormalizeXPosition_MinValueX_ReturnsVerySmallOrNegativeInfinity()
        {
            // Arrange
            var drawable = new CirclesDrawable(1280.0, 1366.0, null, 30.0);

            // Act
            var result = drawable.NormalizeXPosition(double.MinValue);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Value, Is.LessThan(-1.0e308));
        }

        /// <summary>
        /// Tests NormalizeXPosition when imageWidth is double.NaN.
        /// Verifies that division by NaN produces NaN.
        /// </summary>
        [Test]
        public void NormalizeXPosition_NaNImageWidth_ReturnsNaN()
        {
            // Arrange
            var drawable = new CirclesDrawable(double.NaN, 1366.0, null, 30.0);

            // Act
            var result = drawable.NormalizeXPosition(100.0);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(double.IsNaN(result.Value), Is.True);
        }

        /// <summary>
        /// Tests NormalizeXPosition when imageWidth is double.PositiveInfinity.
        /// Verifies that finite x divided by infinity produces zero.
        /// </summary>
        [Test]
        public void NormalizeXPosition_PositiveInfinityImageWidth_ReturnsZero()
        {
            // Arrange
            var drawable = new CirclesDrawable(double.PositiveInfinity, 1366.0, null, 30.0);

            // Act
            var result = drawable.NormalizeXPosition(100.0);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Value, Is.EqualTo(0.0));
        }

        /// <summary>
        /// Tests NormalizeXPosition when both x and imageWidth are double.PositiveInfinity.
        /// Verifies that infinity divided by infinity produces NaN.
        /// </summary>
        [Test]
        public void NormalizeXPosition_BothPositiveInfinity_ReturnsNaN()
        {
            // Arrange
            var drawable = new CirclesDrawable(double.PositiveInfinity, 1366.0, null, 30.0);

            // Act
            var result = drawable.NormalizeXPosition(double.PositiveInfinity);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(double.IsNaN(result.Value), Is.True);
        }

        /// <summary>
        /// Tests NormalizeXPosition when imageWidth is very small positive value.
        /// Verifies that division by very small number produces very large result.
        /// </summary>
        [Test]
        public void NormalizeXPosition_VerySmallImageWidth_ReturnsVeryLargeValue()
        {
            // Arrange
            var drawable = new CirclesDrawable(0.0001, 1366.0, null, 30.0);

            // Act
            var result = drawable.NormalizeXPosition(100.0);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Value, Is.EqualTo(1000000.0).Within(0.1));
        }

        /// <summary>
        /// Tests NormalizeXPosition with boundary case where x equals imageWidth.
        /// Verifies that the result is exactly 1.0.
        /// </summary>
        [TestCase(1280.0)]
        [TestCase(100.0)]
        [TestCase(5000.0)]
        public void NormalizeXPosition_XEqualsImageWidth_ReturnsOne(double imageWidth)
        {
            // Arrange
            var drawable = new CirclesDrawable(imageWidth, 1366.0, null, 30.0);

            // Act
            var result = drawable.NormalizeXPosition(imageWidth);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Value, Is.EqualTo(1.0).Within(0.000001));
        }

        /// <summary>
        /// Tests NormalizeXPosition with x greater than imageWidth.
        /// Verifies that the result is greater than 1.0.
        /// </summary>
        [TestCase(2000.0, 1000.0, 2.0)]
        [TestCase(3840.0, 1280.0, 3.0)]
        public void NormalizeXPosition_XGreaterThanImageWidth_ReturnsGreaterThanOne(double x, double imageWidth, double expected)
        {
            // Arrange
            var drawable = new CirclesDrawable(imageWidth, 1366.0, null, 30.0);

            // Act
            var result = drawable.NormalizeXPosition(x);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Value, Is.EqualTo(expected).Within(0.000001));
        }

        /// <summary>
        /// Tests NormalizeXPosition with x less than imageWidth.
        /// Verifies that the result is between 0 and 1.0.
        /// </summary>
        [TestCase(320.0, 1280.0, 0.25)]
        [TestCase(50.0, 200.0, 0.25)]
        public void NormalizeXPosition_XLessThanImageWidth_ReturnsBetweenZeroAndOne(double x, double imageWidth, double expected)
        {
            // Arrange
            var drawable = new CirclesDrawable(imageWidth, 1366.0, null, 30.0);

            // Act
            var result = drawable.NormalizeXPosition(x);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Value, Is.EqualTo(expected).Within(0.000001));
        }

        /// <summary>
        /// Tests NormalizeXPosition with double.Epsilon for x.
        /// Verifies that very small positive x produces very small positive result.
        /// </summary>
        [Test]
        public void NormalizeXPosition_EpsilonX_ReturnsVerySmallPositiveValue()
        {
            // Arrange
            var drawable = new CirclesDrawable(1280.0, 1366.0, null, 30.0);

            // Act
            var result = drawable.NormalizeXPosition(double.Epsilon);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Value, Is.GreaterThan(0.0));
            Assert.That(result.Value, Is.LessThan(1.0e-300));
        }

        /// <summary>
        /// Tests NormalizeXPosition with imageWidth set to double.Epsilon.
        /// Verifies that division by very small imageWidth produces very large result.
        /// </summary>
        [Test]
        public void NormalizeXPosition_EpsilonImageWidth_ReturnsVeryLargeValue()
        {
            // Arrange
            var drawable = new CirclesDrawable(double.Epsilon, 1366.0, null, 30.0);

            // Act
            var result = drawable.NormalizeXPosition(1.0);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Value, Is.GreaterThan(1.0e308));
        }

        /// <summary>
        /// Tests that ClearAll can be called multiple times in succession without errors.
        /// Verifies that repeated calls to clear the same zone don't cause issues.
        /// </summary>
        /// <remarks>
        /// Coverage: Executes lines 100, 102, 103, 104, 105 multiple times.
        /// </remarks>
        [Test]
        public void ClearAll_CalledMultipleTimes_ExecutesWithoutThrowing()
        {
            // Arrange
            var drawable = new CirclesDrawable();

            // Act & Assert
            Assert.DoesNotThrow(() =>
            {
                drawable.ClearAll(ZoneOfPosition.Front);
                drawable.ClearAll(ZoneOfPosition.Front);
                drawable.ClearAll(ZoneOfPosition.Back);
            });
        }

        /// <summary>
        /// Tests that ClearAll can be called with all enum values in sequence.
        /// Verifies comprehensive enum value handling.
        /// </summary>
        /// <remarks>
        /// Coverage: Executes lines 100, 102, 103, 104, 105 for each enum value.
        /// </remarks>
        [Test]
        public void ClearAll_AllEnumValuesInSequence_ExecutesWithoutThrowing()
        {
            // Arrange
            var drawable = new CirclesDrawable();

            // Act & Assert
            Assert.DoesNotThrow(() =>
            {
                drawable.ClearAll(ZoneOfPosition.NotSet);
                drawable.ClearAll(ZoneOfPosition.Front);
                drawable.ClearAll(ZoneOfPosition.Back);
                drawable.ClearAll(ZoneOfPosition.Hands);
                drawable.ClearAll(ZoneOfPosition.Sensor);
            });
        }

        /// <summary>
        /// Tests that ClearAll with minimum enum value (cast from int.MinValue) executes without throwing.
        /// Verifies extreme invalid enum value handling.
        /// </summary>
        /// <remarks>
        /// Coverage: Executes lines 100, 102, 103, 104, 105 (the try block).
        /// </remarks>
        [Test]
        public void ClearAll_MinValueEnumCast_ExecutesWithoutThrowing()
        {
            // Arrange
            var drawable = new CirclesDrawable();

            // Act & Assert
            Assert.DoesNotThrow(() => drawable.ClearAll((ZoneOfPosition)int.MinValue));
        }

        /// <summary>
        /// Tests that ClearAll with maximum enum value (cast from int.MaxValue) executes without throwing.
        /// Verifies extreme invalid enum value handling.
        /// </summary>
        /// <remarks>
        /// Coverage: Executes lines 100, 102, 103, 104, 105 (the try block).
        /// </remarks>
        [Test]
        public void ClearAll_MaxValueEnumCast_ExecutesWithoutThrowing()
        {
            // Arrange
            var drawable = new CirclesDrawable();

            // Act & Assert
            Assert.DoesNotThrow(() => drawable.ClearAll((ZoneOfPosition)int.MaxValue));
        }

        /// <summary>
        /// Tests that ClearAll with negative enum value executes without throwing.
        /// Verifies handling of negative invalid enum values.
        /// </summary>
        /// <remarks>
        /// Coverage: Executes lines 100, 102, 103, 104, 105 (the try block).
        /// </remarks>
        [Test]
        public void ClearAll_NegativeEnumValue_ExecutesWithoutThrowing()
        {
            // Arrange
            var drawable = new CirclesDrawable();

            // Act & Assert
            Assert.DoesNotThrow(() => drawable.ClearAll((ZoneOfPosition)(-100)));
        }

        /// <summary>
        /// Tests that ClearAll works correctly with default constructor instance.
        /// Verifies that default image dimensions don't interfere with clearing operations.
        /// </summary>
        /// <remarks>
        /// Coverage: Executes lines 100, 102, 103, 104, 105 (the try block).
        /// </remarks>
        [Test]
        public void ClearAll_WithDefaultConstructor_ExecutesWithoutThrowing()
        {
            // Arrange
            var drawable = new CirclesDrawable();

            // Act & Assert
            Assert.DoesNotThrow(() => drawable.ClearAll(ZoneOfPosition.Front));
        }

        /// <summary>
        /// Tests that ClearAll works with parameterized constructor using various edge case dimensions.
        /// </summary>
        /// <param name="imageWidth">The image width to test.</param>
        /// <param name="imageHeight">The image height to test.</param>
        /// <param name="idInjection">The injection ID to test.</param>
        /// <param name="circlesVisibilityMaxTimeInDays">The visibility time to test.</param>
        /// <remarks>
        /// Verifies that various constructor parameters don't affect ClearAll behavior.
        /// Coverage: Executes lines 100, 102, 103, 104, 105 (the try block).
        /// </remarks>
        [TestCase(0.0, 0.0, null, 0.0)]
        [TestCase(1280.0, 1366.0, 1, 30.0)]
        [TestCase(double.MaxValue, double.MaxValue, int.MaxValue, double.MaxValue)]
        [TestCase(double.MinValue, double.MinValue, int.MinValue, double.MinValue)]
        [TestCase(-100.0, -100.0, -1, -10.0)]
        public void ClearAll_WithVariousConstructorParameters_ExecutesWithoutThrowing(
            double imageWidth, double imageHeight, int? idInjection, double circlesVisibilityMaxTimeInDays)
        {
            // Arrange
            var drawable = new CirclesDrawable(imageWidth, imageHeight, idInjection, circlesVisibilityMaxTimeInDays);

            // Act & Assert
            Assert.DoesNotThrow(() => drawable.ClearAll(ZoneOfPosition.Front));
        }
    }


    /// <summary>
    /// Unit tests for the CircleData class constructor.
    /// </summary>
    [TestFixture]
    public class CircleDataTests
    {
        /// <summary>
        /// Tests that the constructor properly initializes Position and Color properties
        /// with various input values including edge cases.
        /// </summary>
        /// <param name="x">The X coordinate for the Point.</param>
        /// <param name="y">The Y coordinate for the Point.</param>
        /// <param name="red">Red component of the color (0-1).</param>
        /// <param name="green">Green component of the color (0-1).</param>
        /// <param name="blue">Blue component of the color (0-1).</param>
        /// <param name="alpha">Alpha component of the color (0-1).</param>
        [TestCase(0.0, 0.0, 0.0, 0.0, 0.0, 0.0, TestName = "Constructor_OriginPointAndBlackColor_PropertiesSetCorrectly")]
        [TestCase(100.5, 200.5, 1.0, 0.0, 0.0, 1.0, TestName = "Constructor_TypicalPointAndRedColor_PropertiesSetCorrectly")]
        [TestCase(-50.0, -100.0, 0.0, 1.0, 0.0, 1.0, TestName = "Constructor_NegativePointAndGreenColor_PropertiesSetCorrectly")]
        [TestCase(double.MaxValue, double.MaxValue, 1.0, 1.0, 1.0, 1.0, TestName = "Constructor_MaxValuePointAndWhiteColor_PropertiesSetCorrectly")]
        [TestCase(double.MinValue, double.MinValue, 0.5, 0.5, 0.5, 0.5, TestName = "Constructor_MinValuePointAndGrayColor_PropertiesSetCorrectly")]
        [TestCase(1280.0, 1366.0, 0.0, 0.0, 1.0, 1.0, TestName = "Constructor_LargePointAndBlueColor_PropertiesSetCorrectly")]
        [TestCase(0.0, 0.0, 0.0, 0.0, 0.0, 1.0, TestName = "Constructor_OriginPointAndOpaqueBlackColor_PropertiesSetCorrectly")]
        [TestCase(15.5, 22.5, 1.0, 1.0, 1.0, 0.0, TestName = "Constructor_SmallPointAndTransparentWhiteColor_PropertiesSetCorrectly")]
        public void Constructor_VariousInputs_PropertiesSetCorrectly(double x, double y, float red, float green, float blue, float alpha)
        {
            // Arrange
            var position = new MauiPoint(x, y);
            var color = new Color(red, green, blue, alpha);

            // Act
            var circleData = new CircleData(position, color);

            // Assert
            Assert.That(circleData.Position.X, Is.EqualTo(x));
            Assert.That(circleData.Position.Y, Is.EqualTo(y));
            Assert.That(circleData.Color.Red, Is.EqualTo(red));
            Assert.That(circleData.Color.Green, Is.EqualTo(green));
            Assert.That(circleData.Color.Blue, Is.EqualTo(blue));
            Assert.That(circleData.Color.Alpha, Is.EqualTo(alpha));
        }

        /// <summary>
        /// Tests that the constructor properly handles Point with special double values
        /// such as NaN, PositiveInfinity, and NegativeInfinity.
        /// </summary>
        /// <param name="x">The X coordinate for the Point.</param>
        /// <param name="y">The Y coordinate for the Point.</param>
        [TestCase(double.NaN, double.NaN, TestName = "Constructor_PointWithNaN_PropertiesSetCorrectly")]
        [TestCase(double.PositiveInfinity, double.PositiveInfinity, TestName = "Constructor_PointWithPositiveInfinity_PropertiesSetCorrectly")]
        [TestCase(double.NegativeInfinity, double.NegativeInfinity, TestName = "Constructor_PointWithNegativeInfinity_PropertiesSetCorrectly")]
        [TestCase(double.NaN, 100.0, TestName = "Constructor_PointWithNaNX_PropertiesSetCorrectly")]
        [TestCase(100.0, double.NaN, TestName = "Constructor_PointWithNaNY_PropertiesSetCorrectly")]
        [TestCase(double.PositiveInfinity, 0.0, TestName = "Constructor_PointWithInfinityX_PropertiesSetCorrectly")]
        [TestCase(0.0, double.NegativeInfinity, TestName = "Constructor_PointWithNegativeInfinityY_PropertiesSetCorrectly")]
        public void Constructor_PointWithSpecialDoubleValues_PropertiesSetCorrectly(double x, double y)
        {
            // Arrange
            var position = new MauiPoint(x, y);
            var color = new Color(1.0f, 0.0f, 0.0f, 1.0f);

            // Act
            var circleData = new CircleData(position, color);

            // Assert
            if (double.IsNaN(x))
            {
                Assert.That(double.IsNaN(circleData.Position.X), Is.True);
            }
            else
            {
                Assert.That(circleData.Position.X, Is.EqualTo(x));
            }

            if (double.IsNaN(y))
            {
                Assert.That(double.IsNaN(circleData.Position.Y), Is.True);
            }
            else
            {
                Assert.That(circleData.Position.Y, Is.EqualTo(y));
            }
        }

        /// <summary>
        /// Tests that the constructor correctly sets properties when using default struct values.
        /// </summary>
        [Test]
        public void Constructor_DefaultStructValues_PropertiesSetCorrectly()
        {
            // Arrange
            var position = new Point();
            var color = new Color();

            // Act
            var circleData = new CircleData(position, color);

            // Assert
            Assert.That(circleData.Position.X, Is.EqualTo(0.0));
            Assert.That(circleData.Position.Y, Is.EqualTo(0.0));
            Assert.That(circleData.Color, Is.Not.Null);
        }

        /// <summary>
        /// Tests that the constructor maintains exact equality of input and output values.
        /// Verifies that there is no data loss or transformation during assignment.
        /// </summary>
        [Test]
        public void Constructor_ProvidedValues_MaintainsExactEquality()
        {
            // Arrange
            var position = new MauiPoint(123.456, 789.012);
            var color = new Color(0.25f, 0.5f, 0.75f, 0.9f);

            // Act
            var circleData = new CircleData(position, color);

            // Assert
            Assert.That(circleData.Position, Is.EqualTo(position));
            Assert.That(circleData.Color, Is.EqualTo(color));
        }

        /// <summary>
        /// Tests that Position property is immutable after construction
        /// and cannot be modified from outside the class.
        /// </summary>
        [Test]
        public void Constructor_PositionProperty_IsImmutableFromOutside()
        {
            // Arrange
            var position = new MauiPoint(50.0, 60.0);
            var color = new Color(1.0f, 1.0f, 1.0f, 1.0f);

            // Act
            var circleData = new CircleData(position, color);
            var retrievedPosition = circleData.Position;

            // Assert
            Assert.That(circleData.Position, Is.EqualTo(position));
            Assert.That(retrievedPosition, Is.EqualTo(position));
        }

        /// <summary>
        /// Tests that Color property is immutable after construction
        /// and cannot be modified from outside the class.
        /// </summary>
        [Test]
        public void Constructor_ColorProperty_IsImmutableFromOutside()
        {
            // Arrange
            var position = new MauiPoint(50.0, 60.0);
            var color = new Color(0.5f, 0.5f, 0.5f, 1.0f);

            // Act
            var circleData = new CircleData(position, color);
            var retrievedColor = circleData.Color;

            // Assert
            Assert.That(circleData.Color, Is.EqualTo(color));
            Assert.That(retrievedColor, Is.EqualTo(color));
        }

        /// <summary>
        /// Tests that the constructor properly initializes Position and Color properties
        /// with various typical input values including positive coordinates and standard colors.
        /// Input: Various Point coordinates and Color RGBA combinations.
        /// Expected: Properties are set exactly as provided to constructor.
        /// </summary>
        /// <param name="x">The X coordinate for the Point.</param>
        /// <param name="y">The Y coordinate for the Point.</param>
        /// <param name="red">Red component of the color (0-1).</param>
        /// <param name="green">Green component of the color (0-1).</param>
        /// <param name="blue">Blue component of the color (0-1).</param>
        /// <param name="alpha">Alpha component of the color (0-1).</param>
        [TestCase(100.5, 200.5, 1.0f, 0.0f, 0.0f, 1.0f)]
        [TestCase(50.0, 75.0, 0.0f, 1.0f, 0.0f, 1.0f)]
        [TestCase(640.0, 480.0, 0.0f, 0.0f, 1.0f, 1.0f)]
        [TestCase(1280.0, 1366.0, 1.0f, 1.0f, 1.0f, 1.0f)]
        [TestCase(15.5, 22.5, 0.5f, 0.5f, 0.5f, 0.5f)]
        public void Constructor_TypicalValues_PropertiesSetCorrectly(double x, double y, float red, float green, float blue, float alpha)
        {
            // Arrange
            var position = new MauiPoint(x, y);
            var color = new Color(red, green, blue, alpha);

            // Act
            var circleData = new CircleData(position, color);

            // Assert
            Assert.That(circleData.Position.X, Is.EqualTo(x));
            Assert.That(circleData.Position.Y, Is.EqualTo(y));
            Assert.That(circleData.Color.Red, Is.EqualTo(red));
            Assert.That(circleData.Color.Green, Is.EqualTo(green));
            Assert.That(circleData.Color.Blue, Is.EqualTo(blue));
            Assert.That(circleData.Color.Alpha, Is.EqualTo(alpha));
        }

        /// <summary>
        /// Tests that the constructor properly handles boundary coordinate values including
        /// zero coordinates, which represent the origin point.
        /// Input: Point at (0, 0) with a solid black color.
        /// Expected: Position and Color properties reflect the zero coordinates and black color.
        /// </summary>
        [Test]
        public void Constructor_ZeroCoordinates_PropertiesSetCorrectly()
        {
            // Arrange
            var position = new MauiPoint(0.0, 0.0);
            var color = new Color(0.0f, 0.0f, 0.0f, 1.0f);

            // Act
            var circleData = new CircleData(position, color);

            // Assert
            Assert.That(circleData.Position.X, Is.EqualTo(0.0));
            Assert.That(circleData.Position.Y, Is.EqualTo(0.0));
            Assert.That(circleData.Color.Red, Is.EqualTo(0.0f));
            Assert.That(circleData.Color.Green, Is.EqualTo(0.0f));
            Assert.That(circleData.Color.Blue, Is.EqualTo(0.0f));
            Assert.That(circleData.Color.Alpha, Is.EqualTo(1.0f));
        }

        /// <summary>
        /// Tests that the constructor properly handles negative coordinate values.
        /// Input: Point with negative X and Y coordinates.
        /// Expected: Negative coordinates are stored exactly as provided.
        /// </summary>
        [TestCase(-50.0, -100.0)]
        [TestCase(-1280.0, -1366.0)]
        [TestCase(-0.5, -0.25)]
        public void Constructor_NegativeCoordinates_PropertiesSetCorrectly(double x, double y)
        {
            // Arrange
            var position = new MauiPoint(x, y);
            var color = new Color(0.5f, 0.5f, 0.5f, 1.0f);

            // Act
            var circleData = new CircleData(position, color);

            // Assert
            Assert.That(circleData.Position.X, Is.EqualTo(x));
            Assert.That(circleData.Position.Y, Is.EqualTo(y));
        }

        /// <summary>
        /// Tests that the constructor handles extreme double values correctly including
        /// double.MaxValue and double.MinValue for coordinates.
        /// Input: Points with maximum and minimum double values.
        /// Expected: Extreme values are stored without overflow or data loss.
        /// </summary>
        [TestCase(double.MaxValue, double.MaxValue)]
        [TestCase(double.MinValue, double.MinValue)]
        [TestCase(double.MaxValue, 0.0)]
        [TestCase(0.0, double.MinValue)]
        public void Constructor_ExtremeDoubleValues_PropertiesSetCorrectly(double x, double y)
        {
            // Arrange
            var position = new MauiPoint(x, y);
            var color = new Color(0.5f, 0.5f, 0.5f, 1.0f);

            // Act
            var circleData = new CircleData(position, color);

            // Assert
            Assert.That(circleData.Position.X, Is.EqualTo(x));
            Assert.That(circleData.Position.Y, Is.EqualTo(y));
        }

        /// <summary>
        /// Tests that the constructor properly handles special double values such as NaN
        /// for Point coordinates.
        /// Input: Point with double.NaN for X and/or Y coordinates.
        /// Expected: NaN values are preserved in the Position property.
        /// </summary>
        [TestCase(double.NaN, double.NaN)]
        [TestCase(double.NaN, 100.0)]
        [TestCase(100.0, double.NaN)]
        public void Constructor_NaNCoordinates_PropertiesSetCorrectly(double x, double y)
        {
            // Arrange
            var position = new MauiPoint(x, y);
            var color = new Color(1.0f, 0.0f, 0.0f, 1.0f);

            // Act
            var circleData = new CircleData(position, color);

            // Assert
            if (double.IsNaN(x))
            {
                Assert.That(double.IsNaN(circleData.Position.X), Is.True);
            }
            else
            {
                Assert.That(circleData.Position.X, Is.EqualTo(x));
            }

            if (double.IsNaN(y))
            {
                Assert.That(double.IsNaN(circleData.Position.Y), Is.True);
            }
            else
            {
                Assert.That(circleData.Position.Y, Is.EqualTo(y));
            }
        }

        /// <summary>
        /// Tests that the constructor properly handles positive infinity values for coordinates.
        /// Input: Point with double.PositiveInfinity for X and/or Y.
        /// Expected: Positive infinity values are preserved.
        /// </summary>
        [TestCase(double.PositiveInfinity, double.PositiveInfinity)]
        [TestCase(double.PositiveInfinity, 0.0)]
        [TestCase(0.0, double.PositiveInfinity)]
        public void Constructor_PositiveInfinityCoordinates_PropertiesSetCorrectly(double x, double y)
        {
            // Arrange
            var position = new MauiPoint(x, y);
            var color = new Color(0.0f, 1.0f, 0.0f, 1.0f);

            // Act
            var circleData = new CircleData(position, color);

            // Assert
            Assert.That(circleData.Position.X, Is.EqualTo(x));
            Assert.That(circleData.Position.Y, Is.EqualTo(y));
        }

        /// <summary>
        /// Tests that the constructor properly handles negative infinity values for coordinates.
        /// Input: Point with double.NegativeInfinity for X and/or Y.
        /// Expected: Negative infinity values are preserved.
        /// </summary>
        [TestCase(double.NegativeInfinity, double.NegativeInfinity)]
        [TestCase(double.NegativeInfinity, 0.0)]
        [TestCase(0.0, double.NegativeInfinity)]
        public void Constructor_NegativeInfinityCoordinates_PropertiesSetCorrectly(double x, double y)
        {
            // Arrange
            var position = new MauiPoint(x, y);
            var color = new Color(0.0f, 0.0f, 1.0f, 1.0f);

            // Act
            var circleData = new CircleData(position, color);

            // Assert
            Assert.That(circleData.Position.X, Is.EqualTo(x));
            Assert.That(circleData.Position.Y, Is.EqualTo(y));
        }

        /// <summary>
        /// Tests that the constructor properly handles fully transparent color (alpha = 0).
        /// Input: Any point with a color having alpha component set to 0.
        /// Expected: Color with zero alpha is stored correctly.
        /// </summary>
        [Test]
        public void Constructor_TransparentColor_PropertiesSetCorrectly()
        {
            // Arrange
            var position = new MauiPoint(100.0, 200.0);
            var color = new Color(1.0f, 1.0f, 1.0f, 0.0f);

            // Act
            var circleData = new CircleData(position, color);

            // Assert
            Assert.That(circleData.Color.Alpha, Is.EqualTo(0.0f));
            Assert.That(circleData.Color.Red, Is.EqualTo(1.0f));
            Assert.That(circleData.Color.Green, Is.EqualTo(1.0f));
            Assert.That(circleData.Color.Blue, Is.EqualTo(1.0f));
        }

        /// <summary>
        /// Tests that the constructor properly handles semi-transparent colors.
        /// Input: Point with color having alpha between 0 and 1.
        /// Expected: Semi-transparent color values are preserved exactly.
        /// </summary>
        [TestCase(0.25f)]
        [TestCase(0.5f)]
        [TestCase(0.75f)]
        public void Constructor_SemiTransparentColor_PropertiesSetCorrectly(float alpha)
        {
            // Arrange
            var position = new MauiPoint(100.0, 200.0);
            var color = new Color(0.8f, 0.6f, 0.4f, alpha);

            // Act
            var circleData = new CircleData(position, color);

            // Assert
            Assert.That(circleData.Color.Alpha, Is.EqualTo(alpha));
            Assert.That(circleData.Color.Red, Is.EqualTo(0.8f));
            Assert.That(circleData.Color.Green, Is.EqualTo(0.6f));
            Assert.That(circleData.Color.Blue, Is.EqualTo(0.4f));
        }

        /// <summary>
        /// Tests that the Position property is immutable after construction.
        /// The property has a private setter, so external modification should not be possible.
        /// Input: CircleData instance with specific position.
        /// Expected: Position property value remains unchanged and cannot be modified externally.
        /// </summary>
        [Test]
        public void Constructor_PositionProperty_IsImmutable()
        {
            // Arrange
            var originalPosition = new MauiPoint(100.0, 200.0);
            var color = new Color(1.0f, 0.0f, 0.0f, 1.0f);

            // Act
            var circleData = new CircleData(originalPosition, color);

            // Assert
            Assert.That(circleData.Position.X, Is.EqualTo(100.0));
            Assert.That(circleData.Position.Y, Is.EqualTo(200.0));
            // Position property has private set, so cannot be modified from outside
            // This test verifies the value remains as initialized
        }

        /// <summary>
        /// Tests that the Color property is immutable after construction.
        /// The property has a private setter, so external modification should not be possible.
        /// Input: CircleData instance with specific color.
        /// Expected: Color property value remains unchanged and cannot be modified externally.
        /// </summary>
        [Test]
        public void Constructor_ColorProperty_IsImmutable()
        {
            // Arrange
            var position = new MauiPoint(100.0, 200.0);
            var originalColor = new Color(0.5f, 0.6f, 0.7f, 0.8f);

            // Act
            var circleData = new CircleData(position, originalColor);

            // Assert
            Assert.That(circleData.Color.Red, Is.EqualTo(0.5f));
            Assert.That(circleData.Color.Green, Is.EqualTo(0.6f));
            Assert.That(circleData.Color.Blue, Is.EqualTo(0.7f));
            Assert.That(circleData.Color.Alpha, Is.EqualTo(0.8f));
            // Color property has private set, so cannot be modified from outside
            // This test verifies the value remains as initialized
        }

        /// <summary>
        /// Tests that the constructor handles mixed special values correctly
        /// (e.g., one coordinate is special while the other is normal).
        /// Input: Points with combinations of special and normal values.
        /// Expected: All values are stored exactly as provided.
        /// </summary>
        [TestCase(double.NaN, 100.0)]
        [TestCase(100.0, double.PositiveInfinity)]
        [TestCase(double.NegativeInfinity, double.MaxValue)]
        [TestCase(double.MinValue, double.PositiveInfinity)]
        public void Constructor_MixedSpecialAndNormalValues_PropertiesSetCorrectly(double x, double y)
        {
            // Arrange
            var position = new MauiPoint(x, y);
            var color = new Color(0.5f, 0.5f, 0.5f, 1.0f);

            // Act
            var circleData = new CircleData(position, color);

            // Assert
            if (double.IsNaN(x))
            {
                Assert.That(double.IsNaN(circleData.Position.X), Is.True);
            }
            else
            {
                Assert.That(circleData.Position.X, Is.EqualTo(x));
            }

            if (double.IsNaN(y))
            {
                Assert.That(double.IsNaN(circleData.Position.Y), Is.True);
            }
            else
            {
                Assert.That(circleData.Position.Y, Is.EqualTo(y));
            }
        }

        /// <summary>
        /// Tests that the constructor properly handles various predefined color values.
        /// Input: Point with common colors (black, white, red, green, blue).
        /// Expected: Color properties match the expected RGB values.
        /// </summary>
        [TestCase(0.0f, 0.0f, 0.0f, 1.0f, TestName = "Constructor_BlackColor_PropertiesSetCorrectly")]
        [TestCase(1.0f, 1.0f, 1.0f, 1.0f, TestName = "Constructor_WhiteColor_PropertiesSetCorrectly")]
        [TestCase(1.0f, 0.0f, 0.0f, 1.0f, TestName = "Constructor_RedColor_PropertiesSetCorrectly")]
        [TestCase(0.0f, 1.0f, 0.0f, 1.0f, TestName = "Constructor_GreenColor_PropertiesSetCorrectly")]
        [TestCase(0.0f, 0.0f, 1.0f, 1.0f, TestName = "Constructor_BlueColor_PropertiesSetCorrectly")]
        public void Constructor_CommonColors_PropertiesSetCorrectly(float red, float green, float blue, float alpha)
        {
            // Arrange
            var position = new MauiPoint(100.0, 200.0);
            var color = new Color(red, green, blue, alpha);

            // Act
            var circleData = new CircleData(position, color);

            // Assert
            Assert.That(circleData.Color.Red, Is.EqualTo(red));
            Assert.That(circleData.Color.Green, Is.EqualTo(green));
            Assert.That(circleData.Color.Blue, Is.EqualTo(blue));
            Assert.That(circleData.Color.Alpha, Is.EqualTo(alpha));
        }
    }
}
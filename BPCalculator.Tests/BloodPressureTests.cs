using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using BPCalculator;
using Xunit;

namespace BPCalculator.Tests
{
    public class BloodPressureTests
    {
        // Helper for DataAnnotations validation
        private static IList<ValidationResult> Validate(BloodPressure bp, out bool isValid)
        {
            var results = new List<ValidationResult>();
            var ctx = new ValidationContext(bp);
            isValid = Validator.TryValidateObject(bp, ctx, results, true);
            return results;
        }

        // -------------------------------------------------------
        // Tests for BP Category Calculation
        // -------------------------------------------------------

        [Fact]
        public void Category_LowBloodPressure_ReturnsBPCategoryLow()
        {
            // Arrange
            var bp = new BloodPressure { Systolic = 90, Diastolic = 60 };

            // Act
            var result = bp.Category;

            // Assert
            Assert.Equal(BPCategory.Low, result);
        }

        [Fact]
        public void Category_IdealBloodPressure_ReturnsBPCategoryIdeal()
        {
            // Arrange
            var bp = new BloodPressure { Systolic = 120, Diastolic = 80 };

            // Act
            var result = bp.Category;

            // Assert
            Assert.Equal(BPCategory.Ideal, result);
        }

        [Fact]
        public void Category_PreHighBloodPressure_ReturnsBPCategoryPreHigh()
        {
            // Arrange
            var bp = new BloodPressure { Systolic = 139, Diastolic = 89 };

            // Act
            var result = bp.Category;

            // Assert
            Assert.Equal(BPCategory.PreHigh, result);
        }

        [Fact]
        public void Category_HighBloodPressure_ReturnsBPCategoryHigh()
        {
            // Arrange
            var bp = new BloodPressure { Systolic = 150, Diastolic = 95 };

            // Act
            var result = bp.Category;

            // Assert
            Assert.Equal(BPCategory.High, result);
        }

        [Fact]
        public void Category_SystolicBoundaryLow_ReturnsBPCategoryLow()
        {
            // Arrange
            var bp = new BloodPressure { Systolic = 70, Diastolic = 40 };

            // Act
            var result = bp.Category;

            // Assert
            Assert.Equal(BPCategory.Low, result);
        }

        [Fact]
        public void Category_SystolicBoundaryIdeal_ReturnsBPCategoryIdeal()
        {
            // Arrange
            var bp = new BloodPressure { Systolic = 121, Diastolic = 79 };

            // Act
            var result = bp.Category;

            // Assert
            Assert.Equal(BPCategory.PreHigh, result);
        }

        // -------------------------------------------------------
        // Tests for Heart Risk Level
        // -------------------------------------------------------

        [Fact]
        public void HeartRiskLevel_LowCategory_ReturnsLowRiskMessage()
        {
            // Arrange
            var bp = new BloodPressure { Systolic = 90, Diastolic = 60 };

            // Act
            var result = bp.HeartRiskLevel;

            // Assert
            Assert.Equal("Low risk - Maintain hydration and regular meals.", result);
        }

        [Fact]
        public void HeartRiskLevel_IdealCategory_ReturnsHealthyMessage()
        {
            // Arrange
            var bp = new BloodPressure { Systolic = 110, Diastolic = 70 };

            // Act
            var result = bp.HeartRiskLevel;

            // Assert
            Assert.Equal("Healthy - Keep up the good work!", result);
        }

        [Fact]
        public void HeartRiskLevel_PreHighCategory_ReturnsModerateRiskMessage()
        {
            // Arrange
            var bp = new BloodPressure { Systolic = 130, Diastolic = 85 };

            // Act
            var result = bp.HeartRiskLevel;

            // Assert
            Assert.Equal("Moderate risk - Consider lifestyle adjustments.", result);
        }

        [Fact]
        public void HeartRiskLevel_HighCategory_ReturnsHighRiskMessage()
        {
            // Arrange
            var bp = new BloodPressure { Systolic = 160, Diastolic = 100 };

            // Act
            var result = bp.HeartRiskLevel;

            // Assert
            Assert.Equal("High risk - Consult your doctor.", result);
        }

        // -------------------------------------------------------
        // Tests for Cardiovascular Risk
        // -------------------------------------------------------

        [Fact]
        public void CardiovascularRisk_LowScore_ReturnsLowLongTermRisk()
        {
            // Arrange
            var bp = new BloodPressure { Systolic = 100, Diastolic = 70 };

            // Act
            var result = bp.CardiovascularRisk;

            // Assert
            Assert.Equal("Low long-term cardiovascular risk.", result);
        }

        [Fact]
        public void CardiovascularRisk_ModerateScore_ReturnsModerateRisk()
        {
            // Arrange
            var bp = new BloodPressure { Systolic = 120, Diastolic = 80 };

            // Act
            var result = bp.CardiovascularRisk;

            // Assert
            Assert.Equal("Moderate long-term cardiovascular risk.", result);
        }

        [Fact]
        public void CardiovascularRisk_HighScore_ReturnsHighRisk()
        {
            // Arrange
            var bp = new BloodPressure { Systolic = 160, Diastolic = 95 };

            // Act
            var result = bp.CardiovascularRisk;

            // Assert
            Assert.Equal("High long-term cardiovascular risk.", result);
        }

        [Fact]
        public void CardiovascularRisk_PreHighCategory_AddsExtraScore()
        {
            // Arrange
            var bp = new BloodPressure { Systolic = 135, Diastolic = 85 };

            // Act
            var result = bp.CardiovascularRisk;

            // Assert
            // Score: (135-100)/10 + (85-70)/10 + 2 (for PreHigh) = 3 + 1 + 2 = 6
            Assert.Equal("High long-term cardiovascular risk.", result);
        }

        [Fact]
        public void CardiovascularRisk_HighCategory_AddsMaxScore()
        {
            // Arrange
            var bp = new BloodPressure { Systolic = 150, Diastolic = 95 };

            // Act
            var result = bp.CardiovascularRisk;

            // Assert
            // Score: (150-100)/10 + (95-70)/10 + 4 (for High) = 5 + 2 + 4 = 11
            Assert.Equal("High long-term cardiovascular risk.", result);
        }

        // -------------------------------------------------------
        // Tests for Edge Cases and Boundaries
        // -------------------------------------------------------

        [Fact]
        public void BloodPressure_MinimumValues_ShouldBeValid()
        {
            // Arrange & Act
            var bp = new BloodPressure { Systolic = 70, Diastolic = 40 };

            // Assert
            Assert.Equal(70, bp.Systolic);
            Assert.Equal(40, bp.Diastolic);
            Assert.Equal(BPCategory.Low, bp.Category);
        }

        [Fact]
        public void BloodPressure_MaximumValues_ShouldBeValid()
        {
            // Arrange & Act
            var bp = new BloodPressure { Systolic = 190, Diastolic = 100 };

            // Assert
            Assert.Equal(190, bp.Systolic);
            Assert.Equal(100, bp.Diastolic);
            Assert.Equal(BPCategory.High, bp.Category);
        }

        [Fact]
        public void BloodPressure_NormalValues_ShouldBeValid()
        {
            // Arrange & Act
            var bp = new BloodPressure { Systolic = 119, Diastolic = 79 };

            // Assert
            Assert.Equal(119, bp.Systolic);
            Assert.Equal(79, bp.Diastolic);
            Assert.Equal(BPCategory.Ideal, bp.Category);
        }

        // -------------------------------------------------------
        // Tests for Systolic and Diastolic Thresholds
        // -------------------------------------------------------

        [Theory]
        [InlineData(70, 40, BPCategory.Low)]
        [InlineData(90, 60, BPCategory.Low)]
        [InlineData(91, 61, BPCategory.Ideal)]
        [InlineData(120, 80, BPCategory.Ideal)]
        [InlineData(121, 81, BPCategory.PreHigh)]
        [InlineData(139, 89, BPCategory.PreHigh)]
        [InlineData(140, 90, BPCategory.High)]
        [InlineData(190, 100, BPCategory.High)]
        public void Category_VariousReadings_CorrectCategoryReturned(int systolic, int diastolic, BPCategory expected)
        {
            // Arrange
            var bp = new BloodPressure { Systolic = systolic, Diastolic = diastolic };

            // Act
            var result = bp.Category;

            // Assert
            Assert.Equal(expected, result);
        }

        // -------------------------------------------------------
        // NEW: Validation tests for invalid values
        // -------------------------------------------------------

        [Theory]
        [InlineData(69)]
        [InlineData(0)]
        [InlineData(191)]
        [InlineData(1000)]
        public void Systolic_Invalid_Values_Are_Rejected(int systolic)
        {
            // Arrange
            var bp = new BloodPressure { Systolic = systolic, Diastolic = 70 };

            // Act
            var results = Validate(bp, out var isValid);

            // Assert
            Assert.False(isValid);
            Assert.Contains(results, r => r.ErrorMessage == "Invalid Systolic Value");
        }

        [Theory]
        [InlineData(39)]
        [InlineData(0)]
        [InlineData(101)]
        [InlineData(500)]
        public void Diastolic_Invalid_Values_Are_Rejected(int diastolic)
        {
            // Arrange
            var bp = new BloodPressure { Systolic = 120, Diastolic = diastolic };

            // Act
            var results = Validate(bp, out var isValid);

            // Assert
            Assert.False(isValid);
            Assert.Contains(results, r => r.ErrorMessage == "Invalid Diastolic Value");
        }
    }
}

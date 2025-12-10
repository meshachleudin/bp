using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using BPCalculator;           // adjust namespace to where your BloodPressure class lives
using TechTalk.SpecFlow;
using Xunit;

namespace BPCalculator.Specs.Steps
{
    [Binding]
    public class BloodPressureSteps
    {
        private readonly BloodPressure _bp = new BloodPressure();

        private string _categoryResult = string.Empty;
        private string _heartRiskResult = string.Empty;
        private string _cardioRiskResult = string.Empty;

        private bool _isValid;
        private IList<ValidationResult> _validationResults = new List<ValidationResult>();

        // -------------------------------------------------------
        // Existing GIVEN steps
        // -------------------------------------------------------
        [Given(@"my systolic pressure is (.*)")]
        public void GivenMySystolicPressureIs(int systolic)
        {
            _bp.Systolic = systolic;
        }

        [Given(@"my diastolic pressure is (.*)")]
        public void GivenMyDiastolicPressureIs(int diastolic)
        {
            _bp.Diastolic = diastolic;
        }

        // New combined GIVEN used in boundary/validation scenarios
        [Given(@"I have entered a systolic value of (.*) and a diastolic value of (.*)")]
        public void GivenIHaveEnteredASystolicValueOfAndADiastolicValueOf(int systolic, int diastolic)
        {
            _bp.Systolic = systolic;
            _bp.Diastolic = diastolic;
        }

        // -------------------------------------------------------
        // WHEN steps
        // -------------------------------------------------------
        [When(@"I evaluate my blood pressure")]
        public void WhenIEvaluateMyBloodPressure()
        {
            // Read properties from your model
            _categoryResult = _bp.Category.ToString();
            _heartRiskResult = _bp.HeartRiskLevel;
            _cardioRiskResult = _bp.CardiovascularRisk;
        }

        [When(@"I validate the blood pressure reading")]
        public void WhenIValidateTheBloodPressureReading()
        {
            _validationResults = new List<ValidationResult>();
            var context = new ValidationContext(_bp);
            _isValid = Validator.TryValidateObject(_bp, context, _validationResults, validateAllProperties: true);
        }

        // -------------------------------------------------------
        // THEN steps
        // -------------------------------------------------------
        [Then(@"the category should be ""(.*)""")]
        public void ThenTheCategoryShouldBe(string expectedCategory)
        {
            Assert.Equal(expectedCategory, _categoryResult);
        }

        [Then(@"the heart risk level should be ""(.*)""")]
        public void ThenTheHeartRiskLevelShouldBe(string expectedHeartRisk)
        {
            Assert.Equal(expectedHeartRisk, _heartRiskResult);
        }

        [Then(@"the cardiovascular risk should be ""(.*)""")]
        public void ThenTheCardiovascularRiskShouldBe(string expectedCardioRisk)
        {
            Assert.Equal(expectedCardioRisk, _cardioRiskResult);
        }

        [Then(@"the validation should fail with error ""(.*)""")]
        public void ThenTheValidationShouldFailWithError(string expectedErrorMessage)
        {
            Assert.False(_isValid);
            Assert.Contains(_validationResults, v => v.ErrorMessage == expectedErrorMessage);
        }
    }
}

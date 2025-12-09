using BPCalculator;           // adjust namespace to where your BloodPressure class lives
using TechTalk.SpecFlow;
using Xunit;

namespace BPCalculator.Specs.Steps
{
    [Binding]
    public class BloodPressureSteps
    {
        private readonly BloodPressure _bp = new BloodPressure();

        private string _categoryResult;
        private string _heartRiskResult;
        private string _cardioRiskResult;

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

        [When(@"I evaluate my blood pressure")]
        public void WhenIEvaluateMyBloodPressure()
        {
            // Read properties from your model
            _categoryResult = _bp.Category.ToString();
            _heartRiskResult = _bp.HeartRiskLevel;
            _cardioRiskResult = _bp.CardiovascularRisk;
        }

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
    }
}
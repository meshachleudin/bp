using System;
using System.Threading.Tasks;
using Microsoft.Playwright;
using Microsoft.Playwright.NUnit;
using NUnit.Framework;
using static Microsoft.Playwright.Assertions;

namespace BPCalculator.E2E
{
    // Inherit from PageTest to get Playwright plumbing (browser, context, page)
    [TestFixture]
    public class BPCalculatorE2ETests : PageTest
    {
        // Default URL for local run; can be overridden by BP_APP_URL env var (used in CI)
        private const string DefaultAppUrl = "http://localhost:5000";
        private string AppUrl => Environment.GetEnvironmentVariable("BP_APP_URL") ?? DefaultAppUrl;

        private async Task EnterBloodPressureValues(string systolic, string diastolic)
        {
            await Page.GotoAsync(AppUrl);
            await Page.GetByLabel("Systolic").FillAsync(systolic);
            await Page.GetByLabel("Diastolic").FillAsync(diastolic);
            await Page.GetByRole(AriaRole.Button, new() { Name = "Submit" }).ClickAsync();
        }

        [Test]
        public async Task LowBloodPressure_ShowsLowCategory()
        {
            // Test case: Systolic <= 90 and Diastolic <= 60
            await EnterBloodPressureValues("85", "55");

            var resultText = Page.GetByText("Maintain hydration and regular meals.");
            await Expect(resultText).ToBeVisibleAsync();
        }

        [Test]
        public async Task LowBloodPressure_BoundaryValues()
        {
            // Test boundary: Systolic = 90, Diastolic = 60
            await EnterBloodPressureValues("90", "60");

            var resultText = Page.GetByText("Maintain hydration and regular meals.");
            await Expect(resultText).ToBeVisibleAsync();
        }

        [Test]
        public async Task IdealBloodPressure_ShowsIdealCategory()
        {
            // Test case: 90 < Systolic <= 120 and 60 < Diastolic <= 80
            await EnterBloodPressureValues("118", "75");

            var resultText = Page.GetByText("Keep up the good work!");
            await Expect(resultText).ToBeVisibleAsync();
        }

        [Test]
        public async Task IdealBloodPressure_BoundaryValuesLower()
        {
            // Test boundary: Systolic = 91, Diastolic = 61 (just above Low)
            await EnterBloodPressureValues("91", "61");

            var resultText = Page.GetByText("Keep up the good work!");
            await Expect(resultText).ToBeVisibleAsync();
        }

        [Test]
        public async Task IdealBloodPressure_BoundaryValuesUpper()
        {
            // Test boundary: Systolic = 120, Diastolic = 80 (upper limit)
            await EnterBloodPressureValues("120", "80");

            var resultText = Page.GetByText("Keep up the good work!");
            await Expect(resultText).ToBeVisibleAsync();
        }

        [Test]
        public async Task PreHighBloodPressure_ShowsPreHighCategory()
        {
            // Test case: 120 < Systolic <= 139 and 80 < Diastolic <= 89
            await EnterBloodPressureValues("130", "85");

            var resultText = Page.GetByText("Consider lifestyle adjustments.");
            await Expect(resultText).ToBeVisibleAsync();
        }

        [Test]
        public async Task PreHighBloodPressure_BoundaryValuesLower()
        {
            // Test boundary: Systolic = 121, Diastolic = 81 (just above Ideal)
            await EnterBloodPressureValues("121", "81");

            var resultText = Page.GetByText("Consider lifestyle adjustments.");
            await Expect(resultText).ToBeVisibleAsync();
        }

        [Test]
        public async Task PreHighBloodPressure_BoundaryValuesUpper()
        {
            // Test boundary: Systolic = 139, Diastolic = 89 (upper limit)
            await EnterBloodPressureValues("139", "89");

            var resultText = Page.GetByText("Consider lifestyle adjustments.");
            await Expect(resultText).ToBeVisibleAsync();
        }

        [Test]
        public async Task HighBloodPressure_ShowsHighCategory()
        {
            // Test case: Systolic > 139 or Diastolic > 89
            await EnterBloodPressureValues("150", "95");

            var resultText = Page.GetByText("Consult your doctor.");
            await Expect(resultText).ToBeVisibleAsync();
        }

        [Test]
        public async Task HighBloodPressure_BoundaryValuesLower()
        {
            // Test boundary: Systolic = 140, Diastolic = 90 (just above PreHigh)
            await EnterBloodPressureValues("140", "90");

            var resultText = Page.GetByText("Consult your doctor.");
            await Expect(resultText).ToBeVisibleAsync();
        }

        [Test]
        public async Task HighBloodPressure_HighSystolicOnly()
        {
            // Test case: High systolic with normal diastolic
            await EnterBloodPressureValues("160", "75");

            var resultText = Page.GetByText("Consult your doctor.");
            await Expect(resultText).ToBeVisibleAsync();
        }

        [Test]
        public async Task HighBloodPressure_HighDiastolicOnly()
        {
            // Test case: Normal systolic with high diastolic
            await EnterBloodPressureValues("100", "95");

            var resultText = Page.GetByText("Consult your doctor.");
            await Expect(resultText).ToBeVisibleAsync();
        }

        [Test]
        public async Task BloodPressure_ChartDisplays()
        {
            // Test that the blood pressure chart canvas is rendered when values are entered
            await EnterBloodPressureValues("120", "80");

            var canvas = Page.Locator("canvas#bpChart");
            await Expect(canvas).ToBeVisibleAsync();
        }

        [Test]
        public async Task BloodPressure_ClearButton()
        {
            // Test that Clear button resets the form
            await EnterBloodPressureValues("130", "85");

            // Verify results are displayed
            var resultText = Page.GetByText("Consider lifestyle adjustments.");
            await Expect(resultText).ToBeVisibleAsync();

            // Click Clear button
            await Page.GetByRole(AriaRole.Button, new() { Name = "Clear" }).ClickAsync();

            // Results should no longer be visible
            await Expect(resultText).Not.ToBeVisibleAsync();
        }

        [Test]
        public async Task BloodPressure_MultipleCalculations()
        {
            // Test that multiple calculations work correctly (state management)
            // First calculation - Low BP
            await EnterBloodPressureValues("80", "50");
            var lowRiskText = Page.GetByText("Maintain hydration and regular meals.");
            await Expect(lowRiskText).ToBeVisibleAsync();

            // Second calculation - High BP (on the same page)
            await Page.GetByLabel("Systolic").FillAsync("150");
            await Page.GetByLabel("Diastolic").FillAsync("95");
            await Page.GetByRole(AriaRole.Button, new() { Name = "Submit" }).ClickAsync();

            var highRiskText = Page.GetByText("Consult your doctor.");
            await Expect(highRiskText).ToBeVisibleAsync();
        }
    }
}

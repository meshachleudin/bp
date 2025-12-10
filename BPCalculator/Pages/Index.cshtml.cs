using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.ApplicationInsights;
using System.Collections.Generic;

namespace BPCalculator.Pages
{
    public class BloodPressureModel : PageModel
    {
        private readonly TelemetryClient _telemetryClient;

        // Inject TelemetryClient
        public BloodPressureModel(TelemetryClient telemetryClient)
        {
            _telemetryClient = telemetryClient;
        }

        [BindProperty]   // bound on POST
        public BloodPressure BP { get; set; }

        // POST, validate
        public IActionResult OnPost()
        {
            // extra validation
            if (!(BP.Systolic > BP.Diastolic))
            {
                ModelState.AddModelError("", "Systolic must be greater than Diastolic");
            }

            // Determine the category (if your BloodPressure class has Category property, it will fill automatically)
            string category = BP.Category.ToString();

            // Prepare event data for Application Insights
            var properties = new Dictionary<string, string>
            {
                { "Systolic", BP.Systolic.ToString() },
                { "Diastolic", BP.Diastolic.ToString() },
                { "Category", category }
            };

            // Track the custom event
            _telemetryClient.TrackEvent("BloodPressureCalculated", properties);

            return Page();
        }

        public IActionResult OnPostClear()
        {
            BP = new BloodPressure();
            ModelState.Clear();
            return RedirectToPage();
        }
    }
}

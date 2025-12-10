using System;
using System.ComponentModel.DataAnnotations;

namespace BPCalculator
{
    // BP categories
    public enum BPCategory
    {
        [Display(Name = "Low Blood Pressure - Low risk - Maintain hydration and regular meals.")]
        Low,

        [Display(Name = "Ideal Blood Pressure - Healthy - Keep up the good work!")]
        Ideal,

        [Display(Name = "Pre-High Blood Pressure - Moderate risk - Consider lifestyle adjustments.")]
        PreHigh,

        [Display(Name = "High Blood Pressure - High risk - Consult your doctor.")]
        High
    };

    public class BloodPressure
    {
        public const int SystolicMin = 70;
        public const int SystolicMax = 190;
        public const int DiastolicMin = 40;
        public const int DiastolicMax = 100;

        [Range(SystolicMin, SystolicMax, ErrorMessage = "Invalid Systolic Value")]
        public int Systolic { get; set; }

        [Range(DiastolicMin, DiastolicMax, ErrorMessage = "Invalid Diastolic Value")]
        public int Diastolic { get; set; }

        // -------------------------------------------------------
        // Blood Pressure Category
        // -------------------------------------------------------
        public BPCategory Category
        {
            get
            {
                if (Systolic <= 90 && Diastolic <= 60)
                    return BPCategory.Low;

                if (Systolic <= 120 && Diastolic <= 80)
                    return BPCategory.Ideal;

                if (Systolic <= 139 && Diastolic <= 89)
                    return BPCategory.PreHigh;

                return BPCategory.High;
            }
        }

        // -------------------------------------------------------
        // Short-term heart risk feedback
        // -------------------------------------------------------
        public string HeartRiskLevel
        {
            get
            {
                return Category switch
                {
                    BPCategory.Low => "Low risk - Maintain hydration and regular meals.",
                    BPCategory.Ideal => "Healthy - Keep up the good work!",
                    BPCategory.PreHigh => "Moderate risk - Consider lifestyle adjustments.",
                    _ => "High risk - Consult your doctor."
                };
            }
        }

        // -------------------------------------------------------
        // Long-term cardiovascular risk score
        // -------------------------------------------------------
        public string CardiovascularRisk
        {
            get
            {
                int score = 0;

                // Safe scoring (integers only)
                score += Math.Max(0, (Systolic - 100) / 10);
                score += Math.Max(0, (Diastolic - 70) / 10);

                // Category weighting
                if (Category == BPCategory.PreHigh)
                    score += 2;

                if (Category == BPCategory.High)
                    score += 4;

                if (score < 2)
                    return "Low long-term cardiovascular risk.";
                else if (score < 6)
                    return "Moderate long-term cardiovascular risk.";
                else
                    return "High long-term cardiovascular risk.";
            }
        }
    }
}

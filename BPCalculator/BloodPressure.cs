using System;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;

namespace BPCalculator
{
    // BP categories
    public enum BPCategory
    {
        [Display(Name="Low Blood Pressure - Low risk - Maintain hydration and regular meals.")] Low,
        [Display(Name="Ideal Blood Pressure - Healthy - Keep up the good work!")]  Ideal,
        [Display(Name="Pre-High Blood Pressure - Moderate risk - Consider lifestyle adjustments.")] PreHigh,
        [Display(Name ="High Blood Pressure - High risk - Consult your doctor.")]  High
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

        public int systolic;
        public int diastolic;

        // calculate BP category
        public BPCategory Category
        {
            get
            {
                if (Systolic <= 90 && Diastolic <= 60)
                    return BPCategory.Low;
                else if (Systolic <= 120 && Diastolic <= 80)
                    return BPCategory.Ideal;
                else if (Systolic <= 139 && Diastolic <= 89)
                    return BPCategory.PreHigh;
                else
                    return BPCategory.High;
            }
        }

        public string HeartRiskLevel
        {
            get
            {
                if (Category == BPCategory.Low) return "Low risk - Maintain hydration and regular meals.";
                if (Category == BPCategory.Ideal) return "Healthy - Keep up the good work!";
                if (Category == BPCategory.PreHigh) return "Moderate risk - Consider lifestyle adjustments.";
                return "High risk - Consult your doctor.";
            }
        }

        // -------------------------------------------------------
        // Simple Cardiovascular Risk Score (long-term risk)
        // -------------------------------------------------------
        public string CardiovascularRisk
        {
            get
            {
                int score = 0;

                // Base scoring from blood pressure values
                score += (Systolic - 100) / 10;
                score += (Diastolic - 70) / 10;

                // Additional weight based on BP category
                if (Category == BPCategory.PreHigh) score += 2;
                if (Category == BPCategory.High) score += 4;

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

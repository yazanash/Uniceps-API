using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Uniceps.AnalysisEngine.Services
{
    public class MetricAnalysisService
    {
        public double CalculateBMI(double weightKg, double heightCm)
        => weightKg / Math.Pow(heightCm / 100.0, 2);

        public double CalculateBMR(double weightKg, double heightCm, int age, string gender)
            => gender == "Male"
                ? (10 * weightKg) + (6.25 * heightCm) - (5 * age) + 5
                : (10 * weightKg) + (6.25 * heightCm) - (5 * age) - 161;

        public double CalculateTDEE(double bmr, double activityFactor) => bmr * activityFactor;

        public double CalculateBodyFat(
            string gender, double heightCm, double waistCm, double neckCm, double? hipCm = null)
        {
            return gender == "Male"
                ? 86.010 * Math.Log10(waistCm - neckCm) - 70.041 * Math.Log10(heightCm)
                : 163.205 * Math.Log10(waistCm + (hipCm ?? 0) - neckCm) - 97.684 * Math.Log10(heightCm);
        }
    }
}

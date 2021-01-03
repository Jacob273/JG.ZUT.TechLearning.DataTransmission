using System;
using System.Collections.Generic;
using System.Linq;

namespace JG.TechLearning.DataTransmission
{
    public static class Fourier
    {
        public static void DiscreteTransform(List<Sample> signalDataInTimeDomain, out FourierResult fourier)
        {
            int nSamples = signalDataInTimeDomain.Count();
            fourier = new FourierResult();

            for (int k = 0; k < nSamples; k++)
            {
                double sumReal = 0;
                double sumImaginary = 0;
                for (int j = 0; j < nSamples; j++)
                {
                    double angle = 2 * Math.PI * j * k / nSamples;
                    sumReal += signalDataInTimeDomain[j].Value * Math.Cos(angle) + signalDataInTimeDomain[j].Value * Math.Sin(angle);
                    sumImaginary += signalDataInTimeDomain[j].Value * Math.Cos(angle) - signalDataInTimeDomain[j].Value * Math.Sin(angle);
                }
                fourier.RealComponents.Add(new Sample(sumReal, 0));
                fourier.ImaginaryComponents.Add(new Sample(sumImaginary, 0));

                var module = Math.Sqrt(Math.Pow(sumReal, 2) + Math.Pow(sumImaginary, 2));
                fourier.RealAndImagSummed.Add(new Sample(module, 0));
            }
        }
    }
}

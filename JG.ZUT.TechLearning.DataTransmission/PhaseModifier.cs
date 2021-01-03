using System;
using System.Collections.Generic;

namespace JG.TechLearning.DataTransmission
{
    public class PhaseModifier
    {
        /// <summary>
        /// Modulator fazy w sygnale.
        /// </summary>
        /// <param name="signalToModulate">Sygnal informacyjny.</param>
        /// <param name="carrierSignal">Sygnal nosny.</param>
        /// <param name="modulationDepthFactor">Wspolczynnik glebokosci.</param>
        /// <returns>Sygnal zmodulowany.</returns>
        public static List<Sample> Modulate(List<Sample> signalToModulate, List<Sample> carrierSignal, double modulationDepthFactor, double carrierSignalFrequency, double amplitudeOfInfoSignal)
        {
            if (signalToModulate.Count != carrierSignal.Count)
            {
                throw new Exception("Number of samples must be equal!");
            }

            List<Sample> modulatedSignal = new List<Sample>();
            for (int i = 0; i < signalToModulate.Count; i++)
            {
                var angle = (2 * Math.PI * carrierSignalFrequency * carrierSignal[i].DeltaTime) + (modulationDepthFactor * carrierSignal[i].Value);
                var time = signalToModulate[i].DeltaTime;
                var modulatedSampleValue = amplitudeOfInfoSignal * Math.Cos(angle);

                var sample = new Sample(modulatedSampleValue, time);
                modulatedSignal.Add(sample);
            }
            return modulatedSignal;
        }
    }
}

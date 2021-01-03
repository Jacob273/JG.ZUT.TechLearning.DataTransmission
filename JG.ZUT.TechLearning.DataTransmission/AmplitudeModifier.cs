using System;
using System.Collections.Generic;

namespace JG.TechLearning.DataTransmission
{
    /// <summary>
    /// Modulator amplitudy w sygnale.
    /// </summary>
    public static class AmplitudeModifier
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="signalToModulate">Sygnal informacyjny.</param>
        /// <param name="carrierSignal">Sygnal nosny.</param>
        /// <param name="modulationDepthFactor">Wspolczynnik glebokosci.</param>
        /// <returns>Sygnal zmodulowany.</returns>
        public static List<Sample> Modulate(List<Sample> signalToModulate, List<Sample> carrierSignal, double modulationDepthFactor)
        {
            if(signalToModulate.Count != carrierSignal.Count)
            {
                throw new Exception("Number of samples must be equal!");
            }

            List<Sample> modulatedSignal = new List<Sample>();
            for (int i = 0; i < signalToModulate.Count; i++)
            {
                var result = carrierSignal[i].Value * (modulationDepthFactor * signalToModulate[i].Value + 1);
                var time = signalToModulate[i].DeltaTime;
                var sample = new Sample(result, time);
                modulatedSignal.Add(sample);
            }
            return modulatedSignal;
        }
    }
}

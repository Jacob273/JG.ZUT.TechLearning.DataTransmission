using System;
using System.Collections.Generic;

namespace JG.TechLearning.DataTransmission
{

    /// <summary>
    /// Generator probek, dla sygnalu harmonicznego.
    /// </summary>
    public class SamplesGenerator
    {
        /// Czas przebiegu symulacji tonu prostego.
        /// </summary>
        public static float GlobalSimulationTimeSeconds { get; set; } = 0;

        public static float GlobalSamplingRate { get; set; } = 0;

        /// <summary>
        /// Realizuje funkcje: x(t) = A * cos(omega + fi), gdzie: omega = 2 * PI * f.        
        /// /// </summary>
        /// <param name="ampl">Amplituda (najwieksze wychylenie).</param>
        /// <param name="frequency">Czestotliwosc (czestosc) probkowania. Wyrazana w [Hz]. Czestotliwosc 1 herca odpowiada wystepowaniu jednego cyklu (zdarzenia) w ciagu 1s.</param>
        /// <param name="period">Okres T. Czas, w ktorym zachodzi jeden cykl drgañ. </param>
        /// <param name="phase"> Faza poczatkowa. </param>
        /// <returns>Zwraca ciag liczbowy. Kazda wartosc ciagu nazywa sie probka.</returns>
        public static List<Sample> GenerateCarrierSignal(double ampl, double frequency, out SignalData input, double phase = 0)
        {
            if (GlobalSamplingRate < frequency * 2)
            {
                throw new Exception($"Czestotliwosc probkowania musi byc co najmniej 2x wieksza niz dwukrotnosc czestotliwosci sygnalu. Zadana nieprawidlowa czestotliwosc: {frequency}");
            }

            List<Sample> samples = new List<Sample>();

            //this is unsused, just for the output data
            double period = 1 / frequency;

            double pulsation = 2 * Math.PI * frequency;

            double deltaTime = GlobalSimulationTimeSeconds / GlobalSamplingRate;

            double time = deltaTime;
            for (int sample = 1; sample <= GlobalSamplingRate; sample++)
            {
                var sampleValue = ampl * Math.Cos(pulsation * time + phase);
                samples.Add(new Sample(sampleValue, time));
                time += deltaTime;
            }

            input = new SignalData()
            {
                Frequency = frequency,
                Pulsation = pulsation,
                SimulationTime = GlobalSimulationTimeSeconds,
                SamplingRate = GlobalSamplingRate,
                Phase = phase,
                Amplitude = ampl,
                Period = period
            };
            return samples;
        }

        /// <summary>
        /// Generator wartosci liczbowych, dla zadanych parametrow.
        /// Realizuje funkcje: x(t) = A * sin(omega + fi), gdzie: omega = 2 * PI * f.        
        /// /// </summary>
        /// <param name="ampl">Amplituda (najwieksze wychylenie).</param>
        /// <param name="frequency">Czestotliwosc (czestosc) probkowania. Wyrazana w [Hz]. Czestotliwosc 1 herca odpowiada wystepowaniu jednego cyklu (zdarzenia) w ciagu 1s.</param>
        /// <param name="period">Okres T. Czas, w ktorym zachodzi jeden cykl drgañ. </param>
        /// <param name="phase"> Faza poczatkowa. </param>
        /// <returns>Zwraca ciag liczbowy. Kazda wartosc ciagu nazywa sie probka.</returns>
        public static List<Sample> GenerateSamples(double ampl, double frequency, out SignalData input, double phase = 0)
        {
            if(GlobalSamplingRate < frequency * 2)
            {
                throw new Exception($"Czestotliwosc probkowania musi byc co najmniej 2x wieksza niz dwukrotnosc czestotliwosci sygnalu. Zadana nieprawidlowa czestotliwosc: {frequency}");
            }

            List<Sample> samples = new List<Sample>();

            //this is unsused, just for the output data
            double period = 1 / frequency;

            double pulsation = 2 * Math.PI * frequency;

            double deltaTime = GlobalSimulationTimeSeconds / GlobalSamplingRate;

            double time = deltaTime;
            for (int sample = 1; sample <= GlobalSamplingRate; sample++)
            {
                var sampleValue = ampl * Math.Sin(pulsation * time + phase);
                samples.Add(new Sample(sampleValue, time));
                time += deltaTime;
            }

            input = new SignalData()
            {
                Frequency = frequency,
                Pulsation = pulsation,
                SimulationTime = GlobalSimulationTimeSeconds,
                SamplingRate = GlobalSamplingRate,
                Phase = phase,
                Amplitude = ampl,
                Period = period
            };
            return samples;
        }

        /// <summary>
        /// Sumowanie dwoch tonow prostych.
        /// Musza miec ta sama czestotliwosc probkowania! x1(t) + x2(t)
        /// </summary>
        public static List<Sample> Sum(List<Sample> samples1, List<Sample> samples2)
        {
            List<Sample> resultSamples = new List<Sample>();

            if (samples1.Count == samples2.Count)
            {
                for(int i= 0; i < samples1.Count; i ++)
                {
                    resultSamples.Add(new Sample(samples1[i].Value + samples2[i].Value, samples1[i].DeltaTime));
                }
            }
            else
            {
                throw new Exception("Liczba probek dla sygnalu 1 i sygnalu 2 musi sie zgadzac.");
            }

            return resultSamples;
        }

        /// <summary>
        /// Odejmowanie dwoch sygnalow...
        /// </summary>
        public static List<Sample> Substract(List<Sample> samples1, List<Sample> samples2)
        {
            List<Sample> resultSamples = new List<Sample>();

            if (samples1.Count == samples2.Count)
            {
                for (int i = 0; i < samples1.Count; i++)
                {
                    resultSamples.Add(new Sample(samples1[i].Value - samples2[i].Value, samples1[i].DeltaTime));
                }
            }

            return resultSamples;
        }

        public static List<Sample> GenerateBits(int numberOfProbes)
        {
            List<Sample> samples = new List<Sample>(numberOfProbes);
            Random r = new Random();

            double deltaTime = GlobalSimulationTimeSeconds / GlobalSamplingRate;

            double time = deltaTime;
            for (int sample = 1; sample <= GlobalSamplingRate; sample++)
            {
                samples.Add(new Sample(r.Next(0,1), time));
                time += deltaTime;
            }

            return samples;
        }

    }
}

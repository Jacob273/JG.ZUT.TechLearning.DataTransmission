using System;

namespace JG.TechLearning.DataTransmission
{
    /// <summary>
    /// Program edukacyjny pozwalajacy zapoznac sie z podstawami tworzenia, dodawania, odejmowania sygnalow a takze
    /// poddawania ich modulacji czy tez transformacie fouriera.
    /// </summary>
    class Program
    { 
        static void Main(string[] args)
        {
            for(int i = 0; i < 5; i ++)
            {
                Console.WriteLine("Pamietaj! Czestotliwosc probkowania musi byc co najmniej 2x wieksza niz najwyzsza w sygnale!!!!!!");
            }


            /*
             * 
             * Ustawienia globalne
             * 
             */
            SamplesGenerator.GlobalSamplingRate = 440;
            SamplesGenerator.GlobalSimulationTimeSeconds = 1;

            /*
             * Tworzenie sygnalu informacyjnego
             */
            var signal1 = SamplesGenerator.GenerateSamples(ampl: 3, frequency: 7, out SignalData signalData1, phase: 0);
            var signal2 = SamplesGenerator.GenerateSamples(ampl: 9, frequency: 14, out SignalData signalData2, phase: 0);

            var summedInformationSignal = SamplesGenerator.Sum(signal1, signal2);

            /*
             * Tworzenie sygnalu nosnego z o wiele wieksza czestotliwoscia
             */
            var carrierSignal = SamplesGenerator.GenerateCarrierSignal(ampl: 20, frequency: 200, out SignalData carrierSignalData, 0);

            /*
             * Wygenerowanie zmodulowanego sygnalu z uwzglednieniem wsp. glebokosci modulacji
             */
            double modulationDepthFactor = 2;
            var modulatedSignal = AmplitudeModifier.Modulate(summedInformationSignal, carrierSignal, modulationDepthFactor);

            /*
             * Transformata zmodulowanego i zapisanie do excela
             */
            Fourier.DiscreteTransform(modulatedSignal, out var fourier_modulated);
           
            /*
             * Wprowadzenie danych do EXCEL'A
             */
            ExcelWriter.WriteSamples(CustomFullPath, "Sygnal1.xlsx", signal1, signalData1);
            ExcelWriter.WriteSamples(CustomFullPath, "Sygnal2.xlsx", signal2, signalData2);
            ExcelWriter.WriteSamples(CustomFullPath, "SygnalNosny.xlsx", carrierSignal, carrierSignalData);
            ExcelWriter.WriteSamples(CustomFullPath, "SygnalInformacyjny.xlsx", summedInformationSignal, signalData1, signalData2, null,null, signal1, signal2);

            AdditionalSignalData additionalData = new AdditionalSignalData();
            additionalData.ModulationDepthFactor = modulationDepthFactor;
            ExcelWriter.WriteSamples(CustomFullPath, "Zmodulowany_Amplituda.xlsx", modulatedSignal, carrierSignalData, signalData1, signalData2, additionalData, summedInformationSignal, carrierSignal);
            ExcelWriter.WriteSignalDataAndDFTResult(CustomFullPath, "Zmodulowany_DFT.xlsx", fourier_modulated, carrierSignalData);

        }

        public static string Lab01FullPath
        {
            get
            {
                return PTDMainDirectoryFolder + @"\" + Lab01Folder;
            }
        }

        public static string Lab02FullPath
        {
            get
            {
                return PTDMainDirectoryFolder + @"\" + Lab02Folder;
            }
        }

        public static string Lab03FullPath
        {
            get
            {
                return PTDMainDirectoryFolder + @"\" + Lab03Folder;
            }
        }

        public static string CustomFullPath
        {
            get
            {
                return PTDMainDirectoryFolder + @"\" + CustomFolder;
            }
        }

        private static string PTDMainDirectoryFolder = @"C:\PTD";
        private static string Lab01Folder = "Lab01";
        private static string Lab02Folder = "Lab02";
        private static string Lab03Folder = "Lab03";
        private static string CustomFolder = "Custom01";
    }
}

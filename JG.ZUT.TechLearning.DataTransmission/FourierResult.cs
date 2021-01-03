using System.Collections.Generic;

namespace JG.TechLearning.DataTransmission
{
    public class FourierResult
    {
        public List<Sample> RealComponents { get; set; } = new List<Sample>();
        public List<Sample> ImaginaryComponents { get; set; } = new List<Sample>();

        public List<Sample> RealAndImagSummed { get; set; } = new List<Sample>();
    }
}

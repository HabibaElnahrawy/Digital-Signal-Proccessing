using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSPAlgorithms.DataStructures;

namespace DSPAlgorithms.Algorithms
{
    public class TimeDelay:Algorithm
    {
        public Signal InputSignal1 { get; set; }
        public Signal InputSignal2 { get; set; }
        public float InputSamplingPeriod { get; set; }
        public float OutputTimeDelay { get; set; }

        public override void Run()
        {
            //throw new NotImplementedException();

            DirectCorrelation directCorrelation = new DirectCorrelation();
            directCorrelation.InputSignal1 = new Signal(InputSignal1.Samples, InputSignal1.Periodic);
            directCorrelation.InputSignal2 = new Signal(InputSignal2.Samples, InputSignal2.Periodic);
            directCorrelation.Run();

            float absoute_max = Math.Abs(directCorrelation.OutputNormalizedCorrelation[0]);
            int j = 0;
            for (int i = 0; i < directCorrelation.OutputNormalizedCorrelation.Count(); i++)
            {
                while (Math.Abs(directCorrelation.OutputNormalizedCorrelation[i]) > absoute_max)
                {
                    absoute_max = Math.Abs(directCorrelation.OutputNormalizedCorrelation[i]);
                    j = i;
                }
            }
            OutputTimeDelay = InputSamplingPeriod * j;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSPAlgorithms.DataStructures;

namespace DSPAlgorithms.Algorithms
{
    public class Subtractor : Algorithm
    {
        public Signal InputSignal1 { get; set; }
        public Signal InputSignal2 { get; set; }
        public Signal OutputSignal { get; set; }

        /// <summary>
        /// To do: Subtract Signal2 from Signal1 
        /// i.e OutSig = Sig1 - Sig2 
        /// </summary>
        public override void Run()
        {
            int size_1 = InputSignal1.Samples.Count;
            int size_2 = InputSignal2.Samples.Count;

            OutputSignal = new Signal(new List<float>(), false);
          
            for (int i = 0; i < size_2; i++)
            {
                InputSignal2.Samples[i] = InputSignal2.Samples[i] * -1;

            }
            for (int i = 0; i < size_1; i++)
            {
                float result ;
                result = InputSignal1.Samples[i] + InputSignal2.Samples[i];
                OutputSignal.Samples.Add(result);

            }
        }
    }
}
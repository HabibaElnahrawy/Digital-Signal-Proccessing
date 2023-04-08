using DSPAlgorithms.DataStructures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;

namespace DSPAlgorithms.Algorithms
{
    public class DCT : Algorithm
    {
        public Signal InputSignal { get; set; }
        public Signal OutputSignal { get; set; }

        public override void Run()
        {
            List<float> output_list = new List<float>();
            int N = InputSignal.Samples.Count;
            double temp = Math.Pow(2f / N,0.5);
            float pi = (float)Math.PI;
            for (int i = 0; i < N; i++)
            {
                double sum = 0;
                for (int j = 0; j < N; j++)
                {
                    sum += InputSignal.Samples[j] * Math.Cos((((2 * j) - 1) * ((2 * i) - 1) * (pi)) / (4 * N));
                }
                output_list.Add((float)(sum * temp));
            }
            OutputSignal = new Signal(output_list, false);

          
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSPAlgorithms.DataStructures;

namespace DSPAlgorithms.Algorithms
{
    public class MovingAverage : Algorithm
    {
        public Signal InputSignal { get; set; }
        public int InputWindowSize { get; set; }
        public Signal OutputAverageSignal { get; set; }
 
        public override void Run()
        {

            int size = InputSignal.Samples.Count();
            List<float> avg = new List<float>();

            for (int i = InputWindowSize / 2; i < size - (InputWindowSize / 2); i++)
            {
                float temp = 0;
                for (int j = i - (InputWindowSize / 2); j <= i + (InputWindowSize / 2); j++)
                {
                    temp += InputSignal.Samples[j];
                }
                temp = temp / InputWindowSize;
                avg.Add(temp);
            }


            OutputAverageSignal = new Signal(avg, false);
        }
    }
}

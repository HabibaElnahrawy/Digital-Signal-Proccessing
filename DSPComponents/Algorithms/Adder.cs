using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSPAlgorithms.DataStructures;

namespace DSPAlgorithms.Algorithms
{
    public class Adder : Algorithm
    {
        public List<Signal> InputSignals { get; set; }
        public Signal OutputSignal { get; set; }

        public override void Run()
        {
            int nb_inputSignal = InputSignals.Count();
            OutputSignal = new Signal(new List<float>(), false);
            float max = 0;
            int i = 0;
            while ( i < nb_inputSignal)
            {
                if (max < InputSignals[i].Samples.Count)
                {
                    max = InputSignals[i].Samples.Count;
                }
                i++;
            }

            for (int l = 0; l < max; l++)
            {
                float sum = 0;
                int index = 0;
                while (index < nb_inputSignal)
                {
                   sum = InputSignals[index].Samples[l] + sum;
                   index++;
                }
                OutputSignal.Samples.Add(sum);

            }
        }
    }
}
using System;
using System.Collections.Generic;
using System.IO;
using Utils.CSVReaderWriter;
using System.Linq;

namespace CycleFinder.Models
{
    public class InputSignal
    {


		public InputSignal()
		{

		}

		public List<double> inputSignalList { get; set; }
        public List<decimal> inputSignalListdecimal { get; set; }

		public static InputSignal GetInputData()
		{
			var inputSignal = new InputSignal();
			List<double> inputSignalList = new List<double> {
		    1.000000, 0.616019, -0.074742, -0.867709, -1.513756, -1.814072, -1.695685, -1.238285, -0.641981, -0.148568, 0.052986, -0.099981, -0.519991, -1.004504, -1.316210, -1.277204, -0.840320, -0.109751, 0.697148, 1.332076, 1.610114, 1.479484, 1.039674, 0.500934, 0.100986, 0.011428, 0.270337, 0.767317, 1.286847, 1.593006, 1.522570, 1.050172, 0.300089, -0.500000, -1.105360, -1.347092, -1.195502, -0.769329, -0.287350, 0.018736, -0.003863, -0.368315, -0.942240, -1.498921, -1.805718, -1.715243, -1.223769, -0.474092, 0.298324, 0.855015, 1.045127, 0.861789, 0.442361, 0.012549, -0.203743, -0.073667, 0.391081, 1.037403, 1.629420, 1.939760, 1.838000, 1.341801, 0.610829, -0.114220, -0.603767, -0.726857, -0.500000, -0.078413, 0.306847, 0.441288, 0.212848, -0.342305, -1.051947, -1.673286, -1.986306, -1.878657, -1.389067, -0.692377, -0.032016, 0.373796, 0.415623, 0.133682, -0.299863, -0.650208, -0.713739, -0.399757, 0.231814, 0.991509, 1.632070, 1.942987, 1.831075, 1.355754, 0.705338, 0.123579, -0.184921, -0.133598, 0.213573, 0.668583, 0.994522, 1.000000
		    };
			
            //List<double> inputSignalList = new List<double> { 1, 2, 3, 4 };
			inputSignal.inputSignalList = inputSignalList;
            inputSignal.inputSignalListdecimal = inputSignalList.Select(n => (decimal)(n+2)*1000000).ToList();

			return inputSignal;

		}

    

		public static InputSignal GetInputData(string fileName)
		{
			var inputSignal = new InputSignal();
            List<decimal> inputSignalList = new List<decimal>();
            var fileStream = File.OpenRead(fileName);

            var csvReader = new CsvFileReader(fileStream);
            var row = new CsvRow();
            var lineRead = csvReader.ReadRow(row); //ignore first line
            inputSignalList.Clear();
            while (csvReader.ReadRow(row))
            {
                inputSignalList.Add(decimal.Parse(row[4]));
            }
			
			inputSignal.inputSignalListdecimal = inputSignalList;
            inputSignal.inputSignalList = inputSignalList.Select(x => (double)x).ToList();

			return inputSignal;


		}






	}


    public abstract class Prequalifier
    {
        public string PrequalifierName { get; set; }
  
        //public Prequalifier(Contractor c)
        //{
        //    contractor = c;

        //}

        //public Prequalifier(Consultant c)
        //{
        //    consultant = c;
        //}
 
    }

    public class Contractor
    {
        
        public Prequalifier prequalifier { get; set; }
        public string contractname { get; set; }

        public Contractor(Prequalifier p)
        {
            prequalifier = p;
            Console.WriteLine(p.PrequalifierName);
        }
        
    }

    public class Consultant
    {
        
    }

/// //////////////////////////////////////////

    public abstract class Prequalifier2
    {
        
    }

    public class Consultant2:Prequalifier2
    {
        
    }

    public class Contractor2:Prequalifier2
    {
        public string contractname { get; set; }
        
    }


}

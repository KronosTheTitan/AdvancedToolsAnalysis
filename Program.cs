using System.Globalization;
// ReSharper disable SuggestVarOrType_BuiltInTypes

namespace AdvancedToolsAnalysis;

class Program
{
    public static void Main()
    {
        Program program = new Program();
        program.Run();
    }

    private readonly double[,] _data = new double[10,2500];
    private readonly double[] _averages = new double[10];
    private readonly int[] _peaks = new int[10];

    private double averageStatic = 0;
    private double averageNoStatic = 0;

    private const string DataAddress = "data.csv";
    private const double PeakThreshold = 1.1;

    private void Run()
    {
        LoadData();
        CalculateAverage();
        CalculateAveragePerCategory();
        CountPeaks();
    }

    private void LoadData()
    {
        string[] lines = File.ReadAllLines(DataAddress);

        for (int i = 1; i < lines.Length; i++)
        {
            string line = lines[i];

            string[] values = line.Split(',');

            for (int j = 1; j < values.Length; j++)
            {
                _data[j - 1, i - 1] = Convert.ToDouble(values[j],CultureInfo.InvariantCulture);
            }
        }
    }

    private void CalculateAverage()
    {
        for (int i = 0; i < 10; i++)
        {
            double total = 0;

            for (int j = 9; j < 2500; j++)
            {
                total += _data[i, j];
            }

            _averages[i] = (total / 2500);
        }
    }

    private void CalculateAveragePerCategory()
    {
        double total = 0;
        for (int i = 0; i < 5; i++)
        {
            for (int j = 9; j < 2500; j++)
            {
                total += _data[i, j];
            }

            _averages[i] = (total / 2500);
        }

        averageStatic = total / 5;
        
        
        total = 0;
        
        for (int i = 5; i < 10; i++)
        {

            for (int j = 9; j < 2500; j++)
            {
                total += _data[i, j];
            }

            _averages[i] = (total / 2500);
        }

        averageNoStatic = total / 5;
    }

    private void CountPeaks()
    {
        for (int i = 0; i < 10; i++)
        {
            double average = _averages[i];
            int peaks = 0;

            for (int j = 9; j < 25; j++)
            {
                if (_data[i, j] * PeakThreshold > average)
                {
                    peaks++;
                }
            }
            
            _peaks[i] = peaks;
        }
    }
    
    
}
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
    private readonly List<int>[] _peaksSeparations = new List<int>[10];

    private double _averageStatic;
    private double _averageNoStatic;

    private double _averagePeaksStatic;
    private double _averagePeaksNoStatic;

    private double _averagePeaksSeparationStatic;
    private double _averagePeaksSeparationNoStatic;

    private const string DataAddress = "data.csv";
    private const double PeakThreshold = 1.25;

    private void Run()
    {
        LoadData();
        CalculateAverage();
        CountPeaks();
        CalculateAveragePerCategory();
        Output();
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
            Console.WriteLine(_averages[i] * PeakThreshold);
        }
    }

    private void CalculateAveragePerCategory()
    {
        double totalAverageTime = 0;
        double totalPeaks = 0;
        int totalPeakSeparation = 0;
        for (int i = 0; i < 5; i++)
        {
            totalAverageTime += _averages[i];

            totalPeaks += _peaks[i];

            foreach (int separation in _peaksSeparations[i])
            {
                totalPeakSeparation += separation;
            }
        }
        
        _averagePeaksStatic = totalPeaks / 5;
        _averagePeaksSeparationStatic = totalPeakSeparation / totalPeaks;
        _averageStatic = totalAverageTime / 5;
        
        
        totalAverageTime = 0;
        totalPeaks = 0;
        totalPeakSeparation = 0;
        
        for (int i = 5; i < 10; i++)
        {
            totalAverageTime += _averages[i];

            totalPeaks += _peaks[i];

            foreach (int separation in _peaksSeparations[i])
            {
                totalPeakSeparation += separation;
            }
        }

        _averagePeaksNoStatic = totalPeaks / 5;
        _averagePeaksSeparationNoStatic = totalPeakSeparation / totalPeaks;
        _averageNoStatic = totalAverageTime / 5;
    }

    private void CountPeaks()
    {
        for (int i = 0; i < 10; i++)
        {
            _peaksSeparations[i] = new List<int>();
            
            double average = _averages[i];
            int peaks = 0;
            int framesSincePeak = 0;

            for (int j = 9; j < 2500; j++)
            {
                if (_data[i, j] > average * PeakThreshold)
                {
                    peaks++;
                    
                    _peaksSeparations[i].Add(framesSincePeak);
                    
                    framesSincePeak = 0;
                }
                else
                {
                    framesSincePeak++;
                }
            }
            
            _peaks[i] = peaks;
        }
    }

    private void Output()
    {
        List<string> lines =
        [
            "Static results: ",
            "    Average Number of peaks: " + _averagePeaksStatic,
            "    Average Frames between peaks: " + _averagePeaksSeparationStatic,
            "    Average Milliseconds per frame: " + _averageStatic,
            "Non Static results: ",
            "    Average Number of peaks: " + _averagePeaksNoStatic,
            "    Average Frames between peaks: " + _averagePeaksSeparationNoStatic,
            "    Average Milliseconds per frame: " + _averageNoStatic
        ];

        File.WriteAllLines("Output.txt", lines);
    }
}
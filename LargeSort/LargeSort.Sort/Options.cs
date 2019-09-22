using CommandLine;

namespace LargeSort.Sort
{
    class Options
    {
        [Option('i', "input", Required = true, HelpText = "Путь к входному файлу")]
        public string InputFile { get; set; }

        [Option('o', "output", Required = true, HelpText = "Путь к выходному файлу")]
        public string OutputFile { get; set; }
    }
}

using CommandLine;

namespace LargeSort.Generator
{
    internal class Options
    {
        [Option('s', "size", Required = true, HelpText = "Размер файла в GB")]
        public int SizeInGb { get; set; }

        [Option('d', "dictionary", Required = false, Default = "dictionary.txt", HelpText = "Файл словаря")]
        public string DictionaryFile { get; set; }

        [Option('o', "output", Default = "out", Required = false)]
        public string OutputFile { get; set; }
    }
}
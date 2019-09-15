using CommandLine;

namespace LargeSort.Sort
{
    class Options
    {
        [Option('i', "input", Required = true, HelpText = "Путь к входному файлу")]
        public string InputFile { get; set; }

        [Option('o', "output", Required = true, HelpText = "Путь к выходному файлу")]
        public string OutputFile { get; set; }

        [Option('m', "memory-limit", Required = false, Default = 4096, HelpText = "Примерный лимит памяти")]
        public int MemoryLimit { get; set; }

        [Option('t', "threads", Required = false, Default = 0, HelpText = "Максимальное число потоков сортировки. 0 - атоматически")]
        public int ThreadsCount { get; set; }
    }
}

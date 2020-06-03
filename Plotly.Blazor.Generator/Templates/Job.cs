using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Plotly.Blazor.Generator.Templates.Class;
using Plotly.Blazor.Generator.Templates.Enumerated;
using Plotly.Blazor.Generator.Templates.Flag;
using Plotly.Blazor.Generator.Templates.Interface;
using Stubble.Core;

namespace Plotly.Blazor.Generator.Templates
{
    /// <summary>
    ///     Type of the job.
    /// </summary>
    public enum JobType
    {
        Class,
        Enumerated,
        Flag,
        Interface
    }

    /// <summary>
    ///     Job Class
    /// </summary>
    public class Job
    {
        private readonly Data data;
        private readonly JobType type;

        /// <summary>
        ///     Creates a new job.
        /// </summary>
        /// <param name="data">Data of the file which will be generated.</param>
        public Job(Data data)
        {
            type = data switch
            {
                ClassData _ => JobType.Class,
                FlagData _ => JobType.Flag,
                EnumeratedData _ => JobType.Enumerated,
                InterfaceData _ => JobType.Interface,
                _ => type
            };
            this.data = data;
            Console.WriteLine($"Created job for {data.Namespace}.{data.Name}.");
        }

        /// <summary>
        ///     Executes the job.
        /// </summary>
        /// <param name="stubble">Stubble to use for file generation.</param>
        /// <returns>Awaitable task.</returns>
        public async Task Execute(StubbleVisitorRenderer stubble)
        {
            var templatePath = type switch
            {
                JobType.Class => @".\Templates\Class\Class.txt",
                JobType.Flag => @".\Templates\Flag\Flag.txt",
                JobType.Enumerated => @".\Templates\Enumerated\Enumerated.txt",
                JobType.Interface => @".\Templates\Interface\Interface.txt",
                _ => throw new ArgumentException()
            };

            var outputDir = data.Namespace.GetOutputPathByNameSpace();
            Directory.CreateDirectory(outputDir);

            using var streamReader = new StreamReader(templatePath, Encoding.UTF8);
            var output = await stubble.RenderAsync(await streamReader.ReadToEndAsync(), data);

            await File.WriteAllTextAsync($"{outputDir}\\{data.Name}.cs", output);
        }
    }
}
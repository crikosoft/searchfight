using searchfight.SearchRunners;
using searchfight.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace searchfight
{
    class Program
    {
        static void Main(string[] args)
        {

            try
            {


            Run(args);

            Console.WriteLine("Press enter to close...");
            Console.ReadLine();

            }
                catch (Exception ex)
            {
                Console.WriteLine();
                Console.WriteLine("An unexpected exception has occurred: " + ex.ToString());
            
            }

        }


        private static void Run(string[] args)
        {
            try
            {
                if (args.Length > 0)
                {

                    if (args.Length == 0)
                        throw new ConfigurationException("Expected at least one argument.");

                    //Read XML Configuration File
                    var runners = ReadConfiguration().SearchRunners.Where(runner => !runner.Disabled).ToList();
                    var results = CollectResults(args, runners).Result;

                    Console.WriteLine();
                    ConsoleHelpers.PrintAsTable(results.Languages, results.Runners, results.Counts); 
                    Console.WriteLine();

                    foreach (var winner in results.Winners)
                        Console.WriteLine("{0} winner: {1}", winner.Key, winner.Value);

                    Console.WriteLine();

                    Console.WriteLine("Total winner: {0}", results.Winner);

                }

            }
            catch (Exception ex)
            {
                Console.WriteLine();
                Console.WriteLine(ex.Message);

            }

        }

        private static async Task<Results> CollectResults(IReadOnlyList<string> languages, IReadOnlyList<ISearchRunner> runners)
        {
            using (var reporter = new ConsoleProgressReporter("Running..."))
            {
                return await Results.Collect(languages, runners, reporter);
            }
        }

        private static Configuration ReadConfiguration()
        {
            try
            {
                using (var stream = File.OpenRead("Configuration.xml"))
                {
                    try
                    {
                        var serializer = new XmlSerializer(typeof(Configuration));
                        return (Configuration)serializer.Deserialize(stream);
                    }
                    catch (InvalidOperationException ex)
                    {
                        throw new ConfigurationException("The configuration file is invalid. " + ex.Message, ex);
                    }
                }
            }
            catch (UnauthorizedAccessException ex)
            {
                throw new ConfigurationException("Unauthorized exception trying to access cofiguration file.", ex);
            }
            catch (FileNotFoundException ex)
            {
                throw new ConfigurationException("Could not find configuration file.", ex);
            }
            catch (IOException ex)
            {
                throw new ConfigurationException("An error occurred when reading configuration file.", ex);
            }
        }
    }
}

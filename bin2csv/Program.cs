using System;
using System.IO;
using TaxiMeter.Converter;

namespace BIN2CSV
{
	class MainClass
	{
		public static void PrintUsage ()
		{
			Console.WriteLine("使用方式：");
			Console.WriteLine("BIN2CSVCmd <path> <pattern>");
			Console.WriteLine();
			Console.WriteLine("EX: BIN2CSVCmd C:\\Data *.bin");
		}

		public static void Main (string[] args)
		{
			if (args.Length != 2) {
				Console.WriteLine("參數錯誤！");
				PrintUsage();
				Environment.Exit(1);
			}	

			BinFileConverter converter = new BinFileConverter ();
			string[] files = Directory.GetFiles(args [0], args [1]);
			foreach (string fileName in files) {
				Console.Write("Converting " + fileName + "...");
				converter.Convert(fileName);
				Console.WriteLine("Done.");
			}
		}
	}
}
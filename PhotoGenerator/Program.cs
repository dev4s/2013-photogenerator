using System;

namespace PhotoGenerator
{
	static class Program
	{
		static void Main()
		{

			Console.WriteLine("- Tworzenie bitmap (trochę to może potrwać :) [nawet całe życie :P]) -");

			/*
			 * 
			 * Why this is randomly?
			 * The answer is simple: we have RGB colors, we need to put it for every pixel, so
			 * computation for ex. for picture which has dimension: 640x480 px will be: 
			 * 
			 * RGB = 256 * 256 * 256
			 * (640 * 480) ^ RGB 
			 * ^^^^^^^^^^^^^ That is a lot (and I mean - really lot)
			 * 
			 */

			Bitmaps.GenerateRandomly();
			
			Console.Write("Aby zakończyć, wciśnij dowolny klawisz.\r\n");
			Console.ReadKey();
		}
	}
}

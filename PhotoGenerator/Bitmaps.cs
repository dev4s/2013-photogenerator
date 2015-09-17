using System;
using System.Collections.Concurrent;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace PhotoGenerator
{
	class Point
	{
		public int X { get; set; }
		public int Y { get; set; }
	}

	public static class Bitmaps
	{
		public static void GenerateRandomly()
		{
			var points = GeneratePoints();
			var bitmaps = new BlockingCollection<Bitmap>(Global.BitmapsInBlockingCollection);
			var colors = new BlockingCollection<Color>(Global.ColorsInBlockingCollection);

			Task.Factory.StartNew(() => GenerateColors(colors));
			Task.Factory.StartNew(() => ProduceBitmap(colors, bitmaps, points));
			Task.Factory.StartNew(() => SaveBitmap(bitmaps));
		}

		private static BlockingCollection<Point> GeneratePoints()
		{
			var collection = new BlockingCollection<Point>();

			for (var x = 0; x < Global.ImageX; x++)
			{
				for (var y = 0; y < Global.ImageY; y++)
				{
					collection.Add(new Point {X = x, Y = y});
				}
			}

			return collection;
		}

		private static void GenerateColors(BlockingCollection<Color> colors)
		{
			while (true)
			{
				colors.TryAdd(ColorTranslator.FromHtml(string.Format("#{0:X6}", GetNumber())));
			}
		}

		private static void ProduceBitmap(BlockingCollection<Color> colors, BlockingCollection<Bitmap> bitmaps, BlockingCollection<Point> points)
		{
			double counter = 0;

			while (true)
			{
				var b = new Bitmap(Global.ImageX, Global.ImageY);

				foreach (var p in points)
				{
					Color color;
					colors.TryTake(out color, Timeout.Infinite);

					b.SetPixel(p.X, p.Y, color);
				}

				bitmaps.TryAdd(b, Timeout.Infinite);
				Console.WriteLine("Utworzono plik graficzny nr.: {0}", ++counter);
			}
		}

		private static void SaveBitmap(BlockingCollection<Bitmap> bitmaps)
		{
			long showFileCounter = 0;
			long innerFileCounter = 0;
			long folderCounter = 0;
			var imagesPath = Environment.CurrentDirectory + @"\images\";
			var saveFolder = imagesPath + folderCounter.ToString(Global.FolderNameFormat) + @"\";
			Directory.CreateDirectory(saveFolder);

			foreach (var b in bitmaps.GetConsumingEnumerable())
			{
				var fileName = saveFolder + showFileCounter.ToString(Global.FileNameFormat) + ".jpg";
				b.Save(fileName, ImageFormat.Jpeg);
				Console.WriteLine("Zapisano plik graficzny nr.: {0}", ++showFileCounter);
				++innerFileCounter;

				var maxFilesInFolderComputation = innerFileCounter / Global.MaxFilesInFolder;
				if (maxFilesInFolderComputation == 0) continue;

				++folderCounter;
				innerFileCounter = 0;
				saveFolder = imagesPath + folderCounter.ToString(Global.FolderNameFormat) + @"\";
				Directory.CreateDirectory(saveFolder);
			}
		}

		private static readonly Random R = new Random();
		private static long GetNumber()
		{
			return R.Next(1, Global.RgbAsNumber);
		}
	}
}
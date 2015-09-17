namespace PhotoGenerator
{
	public static class Global
	{
		public const int ImageX = 800;
		public const int ImageY = 600;
		public const int RgbAsNumber = 256 * 256 * 256;

		public const int BitmapsInBlockingCollection = 50;
		public const int ColorsInBlockingCollection = 100000;

		public const long MaxFilesInFolder = 4200000000; //4,294,967,295 max number of files, in NTFS filesystem, in folder

		public const string FolderNameFormat = "000000000";
		public const string FileNameFormat = "00000000000000000000";
	}
}
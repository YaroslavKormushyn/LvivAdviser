﻿using System.Drawing;
using System.IO;

namespace LvivAdviser.Domain.Abstract
{
	public static class ImageConverter
	{
		public static byte[] ImageToByteArray(Image imageIn)
		{
			MemoryStream ms = new MemoryStream();
			imageIn.Save(ms, System.Drawing.Imaging.ImageFormat.Gif);
			return ms.ToArray();
		}

		public static Image ByteArrayToImage(byte[] byteArrayIn)
		{
			MemoryStream ms = new MemoryStream(byteArrayIn);
			Image returnImage = Image.FromStream(ms);
			return returnImage;
		}
	}
}

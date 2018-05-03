using System;
using System.Collections.Generic;
using System.Windows;

namespace Plate.Trainer
{
	public class Photo
	{
		public string Name { get; set; }
		public string Path { get; set; }
		public List<Tuple<Point, Point>> Numbers { get; set; }
	}
}
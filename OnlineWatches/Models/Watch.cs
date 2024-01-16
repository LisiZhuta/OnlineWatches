using System.ComponentModel.DataAnnotations;

namespace OnlineWatches.Models
{
	public class Watch
	{
		[Key]
		public int Id { get; set; }	
		public string Name {  get; set; }
		public string Price { get; set; }
		public int Year {  get; set; }
		public string? ImgPath {  get; set; }
		
	}
}

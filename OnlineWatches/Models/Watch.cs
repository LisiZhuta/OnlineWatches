using System.ComponentModel.DataAnnotations;

namespace OnlineWatches.Models
{
	public class Watch //watchmodel
	{
		[Key]
		public int Id { get; set; }	
		public string Name {  get; set; }
		public decimal Price { get; set; }
		public int Year {  get; set; }
		public string? ImgPath {  get; set; }
		public string? Status {  get; set; }
		public decimal? Discount { get; set; }
	}
}

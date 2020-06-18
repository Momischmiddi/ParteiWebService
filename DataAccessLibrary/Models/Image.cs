using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DataAccessLibrary.Models
{
    public class Image
    {
        [Key]
        public int ImageId { get; set; }
        [Display(Name = "ImageUrl")]
        public string ImageUrl { get; set; }
        [Display(Name = "ImageName")]
        public string ImageName { get; set; }
        [Display(Name = "ImageFileSize")]
        public int ImageFileSize { get; set; }
        [Display(Name = "ImageFileType")]
        public string ImageFileType { get; set; }

        public Travel Travel { get; set; }
        public int TravelId { get; set; }
    }
}

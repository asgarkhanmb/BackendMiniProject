﻿using System.ComponentModel.DataAnnotations;

namespace BackendMiniProject.ViewModels.Categories
{
    public class CategoryEditVM
    {
        public string Name { get; set; }
        public string Images { get; set; }

        public IFormFile NewImages { get; set; }
    }
}
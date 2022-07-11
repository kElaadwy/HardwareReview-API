﻿using HardwareReview.Data;
using HardwareReview.Models;

namespace HardwareReview.Repository
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly DataContext _context;

        public CategoryRepository(DataContext context)
        {
            _context = context;
        }
        public bool CategoryExists(int categoryId)
        {
            return _context.Categories.Any(x => x.Id == categoryId);
        }

        public ICollection<Category> GetCategories()
        {
            return _context.Categories.OrderBy(x => x.Id).ToList();
        }

        public Category GetCategoryById(int id)
        {
            return _context.Categories.FirstOrDefault(x => x.Id == id);
        }

        public ICollection<Hardware> GetHardwaresByCategory(int categoryId)
        {
           return _context.HardwareCategories.Where(x => x.CategoryId == categoryId).Select(y => y.Hardware).ToList();
        }
    }
}

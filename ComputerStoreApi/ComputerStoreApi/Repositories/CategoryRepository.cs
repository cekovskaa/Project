﻿using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ComputerStoreApi.Models;
using ComputerStoreApi.Repositories;

namespace ComputerStoreApi.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly ApplicationDbContext _context;

        public CategoryRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Category>> GetCategoriesAsync()
        {
            return await _context.Categories.ToListAsync();
        }

        public async Task<Category> GetCategoryByIdAsync(int id)
        {
            return await _context.Categories.FindAsync(id);
        }

        public async Task<Category> GetCategoryByNameAsync(string name)
        {
            return await _context.Categories.FirstOrDefaultAsync(c => c.Name == name);
        }

        public async Task CreateCategoryAsync(Category category)
        {
            _context.Categories.Add(category);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateCategoryAsync(Category category)
        {
            var existingCategory = await _context.Categories.FindAsync(category.Id);

            if (existingCategory == null)
            {
                throw new Exception("Category with the given ID not found.");
            }

            existingCategory.Name = category.Name;
            existingCategory.Description = category.Description;

            await _context.SaveChangesAsync();
        }

        public async Task DeleteCategoryAsync(int id)
        {
            var category = await _context.Categories.FindAsync(id);
            if (category == null)
            {
                throw new Exception("Category with the given ID not found.");
            }

            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();
        }
    }
}

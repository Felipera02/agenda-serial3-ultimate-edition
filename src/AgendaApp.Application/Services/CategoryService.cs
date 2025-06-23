using AgendaApp.Application.DTOs;
using AgendaApp.Domain.Entities;
using AgendaApp.Infrastructure.Data;
using AgendaApp.Infrastructure.Data.Repository;
using Microsoft.EntityFrameworkCore;

namespace AgendaApp.Application.Services
{
    public class CategoryService
    {
        private readonly GenericRepository<Category> _categoryRepository;
        private readonly AppDbContext _context;

        public CategoryService(GenericRepository<Category> categoryRepository, AppDbContext context)
        {
            _categoryRepository = categoryRepository;
            _context = context;
        }

        public async Task<IEnumerable<CategoryDto>> GetUserCategoriesAsync(string userId)
        {
            var categories = await _categoryRepository.FindAsync(c => c.UserId == userId);
            return categories.Select(c => new CategoryDto
            {
                Id = c.Id,
                Name = c.Name,
                Color = c.Color
            });
        }

        public async Task<CategoryDto> CreateCategoryAsync(CategoryDto categoryDto, string userId)
        {
            var category = new Category
            {
                Name = categoryDto.Name,
                Color = categoryDto.Color,
                UserId = userId
            };

            var createdCategory = await _categoryRepository.AddAsync(category);
            
            return new CategoryDto
            {
                Id = createdCategory.Id,
                Name = createdCategory.Name,
                Color = createdCategory.Color
            };
        }

        public async Task<CategoryDto> UpdateCategoryAsync(CategoryDto categoryDto, string userId)
        {
            var category = await _context.Categories
                .FirstOrDefaultAsync(c => c.Id == categoryDto.Id && c.UserId == userId);

            if (category == null)
                throw new UnauthorizedAccessException("Categoria não encontrada ou não pertence ao usuário");

            category.Name = categoryDto.Name;
            category.Color = categoryDto.Color;

            var updatedCategory = await _categoryRepository.UpdateAsync(category);
            
            return new CategoryDto
            {
                Id = updatedCategory.Id,
                Name = updatedCategory.Name,
                Color = updatedCategory.Color
            };
        }

        public async Task DeleteCategoryAsync(int categoryId, string userId)
        {
            var category = await _context.Categories
                .FirstOrDefaultAsync(c => c.Id == categoryId && c.UserId == userId);

            if (category == null)
                throw new UnauthorizedAccessException("Categoria não encontrada ou não pertence ao usuário");

            await _categoryRepository.DeleteAsync(category);
        }
    }
}

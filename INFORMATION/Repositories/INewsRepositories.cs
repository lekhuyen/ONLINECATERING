﻿using INFORMATIONAPI.Models;

namespace INFORMATIONAPI.Repositories
{
    public interface INewsRepositories
    {
        Task<IEnumerable<News>> GetAllAsync();
        Task<News> GetByIdAsync(string id);
        Task CreateAsync(News news, IFormFile? imageFile);
        Task<bool> UpdateAsync(string id, News news, IFormFile? imageFile);
        Task<bool> DeleteAsync(string id);


        // NewsType CRUD operations
        Task<IEnumerable<NewsType>> GetAllNewTypesAsync();
        Task<NewsType> GetNewTypeByIdAsync(string id);
        Task CreateNewTypeAsync(NewsType newType);
        Task<bool> UpdateNewTypeAsync(string id, NewsType newType);
        Task<bool> DeleteNewTypeAsync(string id);
    }
}

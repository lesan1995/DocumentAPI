using Dapper;
using DocumentAPI.Abstract;
using DocumentAPI.DAL;
using DocumentAPI.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocumentAPI.Concrete
{
    public class CategoryRepo : ICategoryRepo
    {
        private readonly string _connectionString;
        private readonly DocumentDbContext _context;
        public CategoryRepo(
            string connectionString,
            DocumentDbContext context
            )
        {
            _connectionString = connectionString;
            _context = context;
        }

        public List<Category> GetCategories()
        {
            using (var context = new SqlConnection(_connectionString))
            {
                //Get list category
                StringBuilder sbSelect = new StringBuilder();
                sbSelect.Append("SELECT Id ");
                sbSelect.Append("      ,Title ");
                sbSelect.Append("  FROM Categories ");
                sbSelect.Append("  ORDER BY Id DESC  ");

                string querySelect = sbSelect.ToString();

                List<Category> categories = context.Query<Category>(querySelect).ToList();

                return categories;
            }
        }

        public Category GetCategory(int categoryId)
        {
            return _context.Categories.FirstOrDefault(x => x.Id == categoryId);
        }

        public bool IsCategoryExist(int categoryId)
        {
            return _context.Categories.Any(x => x.Id == categoryId);
        }

        public Category AddCategory(Category category)
        {
            var result = _context.Categories.Add(category).Entity;
            _context.SaveChanges();
            return result;
        }

        public Category UpdateCategory(Category category)
        {
            _context.Entry(category).State = EntityState.Modified;
            _context.SaveChanges();
            return category;
        }

        public Category RemoveCategory(Category category)
        {
            _context.Categories.Remove(category);
            _context.SaveChanges();
            return category;
        }
    }
}

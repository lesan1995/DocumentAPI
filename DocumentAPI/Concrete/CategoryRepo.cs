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


        public Category AddCategory(Category category)
        {
            _context.Categories.Add(category);
            _context.SaveChanges();
            return category;
        }

        public Category UpdateCategory(Category category)
        {
            var categoryUpdate = _context.Categories.FirstOrDefault(x => x.Id == category.Id);
            if (categoryUpdate != null)
            {
                categoryUpdate.Title = category.Title;
            }
            _context.SaveChanges();
            return categoryUpdate;
        }

        public Category RemoveCategory(Category category)
        {
            _context.Categories.Remove(category);
            _context.SaveChanges();
            return category;
        }
    }
}

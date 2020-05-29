using DocumentAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DocumentAPI.Abstract
{
    public interface ICategoryRepo
    {
        List<Category> GetCategories();
        Category GetCategory(int categoryId);
        Category AddCategory(Category category);
        Category UpdateCategory(Category category);
        Category RemoveCategory(Category category);
    }
}

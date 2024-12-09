using BusinessLayer.Abstract;
using EntityLayer.Concrete;
using DataAccessLayer.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using DataAccessLayer.EntityFramework;
using DataAccessLayer.Concrete;

namespace BusinessLayer.Concrete
{
    public class CategoryManager : ICategoryService
    {
        private readonly EfCategoryDal _categoryDal;

        // Constructor Dependency Injection ile CategoryDal'i alıyoruz
        public CategoryManager(EfCategoryDal categoryDal)
        {
            _categoryDal = categoryDal;
        }

        public List<Category> GetAll()
        {
            using (var c = new Context())
            {
                return c.Categories.ToList();
            }
        }

        public List<Category> GetCategories()
        {
            return TGetList();  // Burada zaten var olan TGetList metodunu kullanıyoruz
        }

        public List<string> GetCategoryNames()
        {
            // Kategorilerin sadece isimlerini al
            return _categoryDal.GetList().Select(c => c.Name).ToList();
        }
        public void TAdd(Category t)
        {
            _categoryDal.Insert(t);
        }

        public void TDelete(Category t)
        {
            _categoryDal.Delete(t);
        }

      
        public Category TGetByID(List<int> categoryIds)
        {
            throw new NotImplementedException();
        }

        public Category TGetByID(int id)
        {
            using (var c = new Context())
            {
                return c.Categories.FirstOrDefault(c => c.CategoryId == id);
            }
        }

        public List<Category> TGetList()
        {
            return _categoryDal.GetAll(); // Tüm kategorileri alıyoruz
        }

        public void TUpdate(Category t)
        {
            _categoryDal.Update(t);
        }
    }
}

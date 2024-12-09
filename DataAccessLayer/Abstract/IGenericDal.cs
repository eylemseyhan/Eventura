﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;


namespace DataAccessLayer.Abstract
{
    public interface IGenericDal<T>
    {
        void Insert(T t);
        void Delete(T t);
        void Update(T t);
        List<T> GetList();
        T GetByID(int id);
        List<T> GetList(Func<T, bool> filter);
        List<T> GetTicketsWithEvents();
        List<T> GetAll(Expression<Func<T, bool>> filter = null);
        List<T> GetListByFilter(Expression<Func<T, bool>> filter);
    }
}
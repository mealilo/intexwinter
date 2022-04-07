﻿using intex2.Models;
using System;
using System.Linq;
using System.Linq.Expressions;
using static intex2.Models.AuxiliaryModels.DataTableModels;

namespace jQueryDatatableServerSideNetCore.Extensions
{
    public static class LinqExtensions
    {
        public static IQueryable<Accident> OrderByDynamic<Accident>(
            this IQueryable<Accident> query,
            string orderByMember,
            DtOrderDir ascendingDirection)
        {
            var param = Expression.Parameter(typeof(Accident), "c");

            var body = orderByMember.Split('.').Aggregate<string, Expression>(param, Expression.PropertyOrField);

            var queryable = ascendingDirection == DtOrderDir.Asc ?
                (IOrderedQueryable<Accident>)Queryable.OrderBy(query.AsQueryable(), (dynamic)Expression.Lambda(body, param)) :
                (IOrderedQueryable<Accident>)Queryable.OrderByDescending(query.AsQueryable(), (dynamic)Expression.Lambda(body, param));

            return queryable;
        }

        public static IQueryable<T> WhereDynamic<T>(
            this IQueryable<T> sourceList, string query)
        {

            if (string.IsNullOrEmpty(query))
            {
                return sourceList;
            }

            try
            {

                var properties = typeof(Accident).GetProperties()
                    .Where(x => x.CanRead && x.CanWrite && !x.GetGetMethod().IsVirtual);

                //Expression
                sourceList = sourceList.Where(c =>
                    properties.Any(p => p.GetValue(c) != null && p.GetValue(c).ToString()
                        .Contains(query, StringComparison.InvariantCultureIgnoreCase)));
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            return sourceList;
        }
    }
}
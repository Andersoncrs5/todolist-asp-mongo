using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using ToDoList.API.models;

namespace ToDoList.API.utils.pagination
{
    public class TaskFilter
    {
        public static Expression<Func<TaskModel, bool>>? ApplyFilter(TaskQueryStruct taskQuery, Expression<Func<TaskModel, bool>>? filter)
        {
            if (!string.IsNullOrEmpty(taskQuery.SearchTitle))   
            {
                filter = task => task.Name.ToLower().Contains(taskQuery.SearchTitle.ToLower());
            }

            if (taskQuery.Done.HasValue) 
            {
                filter = task => task.IsComplete.Equals(taskQuery.Done);
            }


            if (taskQuery.CreatedAfter.HasValue)
            {
                filter = task => task.CreatedAt.ToUniversalTime() >= taskQuery.CreatedAfter.Value.ToUniversalTime();
            }

            if (taskQuery.CreatedBefore.HasValue)
            {
                filter = task => task.CreatedAt.ToUniversalTime() <= taskQuery.CreatedBefore.Value.ToUniversalTime();
            }


            return filter;
        }
    }
}
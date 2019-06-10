using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NHibernate.Linq;
using WebApiNHibernateCrud.Data;
using WebApiNHibernateCrud.Entities;
using WebApiNHibernateCrud.Enums;

namespace WebApiNHibernateCrud.Infrastructure.Services
{
    public class TodoService
    {
        private readonly ISessionFactoryBuilder _sessionFactoryBuilder;

        public TodoService()
        {
            _sessionFactoryBuilder = new FluentSessionFactoryBuilder();
        }

        public async Task<List<Todo>> FetchMany(TodoShow show = TodoShow.All)
        {
            using (var session = _sessionFactoryBuilder.GetSessionFactory().OpenSession())
            {
                // Retrieve hwo many articles with our criteria(All, Completed or Pending)
                IQueryable<Todo> queryable = session.Query<Todo>().OrderBy(t => t.CreatedAt);

                if (show == TodoShow.Completed)
                    queryable = queryable.Where(t => t.Completed);
                else if (show == TodoShow.Pending)
                    queryable = queryable.Where(t => !t.Completed);


                List<Todo> todos;
                if (show != TodoShow.All)
                    // https://stackoverflow.com/questions/5325797/the-entity-cannot-be-constructed-in-a-linq-to-entities-query
                    // for complete/pending
                    todos = await queryable
                        .Select(t => new Todo
                        {
                            Id = t.Id,
                            Title = t.Title,
                            Completed = t.Completed,
                            CreatedAt = t.CreatedAt,
                            UpdatedAt = t.UpdatedAt
                        })
                        .ToListAsync();
                else
                    todos = await queryable.ToListAsync();


                return todos;
            }
        }


        public async Task<Todo> Get(int todoId)
        {
            using (var session = _sessionFactoryBuilder.GetSessionFactory().OpenSession())
            {
                return await session.Query<Todo>().FirstOrDefaultAsync(t => t.Id == todoId);
            }
        }

        public async Task<Todo> CreateTodo(Todo todo)
        {
            using (var session = _sessionFactoryBuilder.GetSessionFactory().OpenSession())
            {
                using (var transaction = session.BeginTransaction())
                {
                    await session.SaveAsync(todo);
                    return todo;
                }
            }
        }

        public async Task<Todo> Update(int id, Todo todoFromUserInput)
        {
            using (var session = _sessionFactoryBuilder.GetSessionFactory().OpenSession())
            {
                using (var transaction = session.BeginTransaction())
                {
                    var todoFromDb = await session.Query<Todo>().FirstAsync(t => t.Id == id);
                    todoFromDb.Title = todoFromUserInput.Title;
                    if (todoFromUserInput.Description != null)
                        todoFromDb.Description = todoFromUserInput.Description;
                    todoFromDb.Completed = todoFromUserInput.Completed;

                    // Not needed, it is set in ApplicationDbContext
                    await session.UpdateAsync(todoFromDb);
                    await transaction.CommitAsync();
                    return todoFromDb;
                }
            }
        }


        public async Task Delete(int todoId)
        {
            using (var session = _sessionFactoryBuilder.GetSessionFactory().OpenSession())
            {
                using (var transaction = session.BeginTransaction())
                {
                    var todoFromDb = await session.Query<Todo>().FirstAsync(t => t.Id == todoId);
                    await session.DeleteAsync(todoFromDb);
                    await transaction.CommitAsync();
                }
            }
        }

        public async Task DeleteAll()
        {
            using (var session = _sessionFactoryBuilder.GetSessionFactory().OpenSession())
            {
                using (var transaction = session.BeginTransaction())
                {
                    // Approach 1
                    // session.Query<Todo>().Delete();

                    // Approach 2
                    await session.DeleteAsync("from Todo t");
                    await session.FlushAsync();
                    await transaction.CommitAsync();
                }
            }
        }

        public async Task<Todo> Update(Todo todoFromDb, Todo todoFromUserInput)
        {
            using (var session = _sessionFactoryBuilder.GetSessionFactory().OpenSession())
            {
                using (var transaction = session.BeginTransaction())
                {
                    todoFromDb.Title = todoFromUserInput.Title;

                    if (todoFromUserInput.Description != null)
                        todoFromDb.Description = todoFromUserInput.Description;

                    todoFromDb.Completed = todoFromUserInput.Completed;

                    await session.UpdateAsync(todoFromDb);
                    await transaction.CommitAsync();
                    return todoFromDb;
                }
            }
        }

        public async Task Delete(Todo todo)
        {
            using (var session = _sessionFactoryBuilder.GetSessionFactory().OpenSession())
            {
                using (var transaction = session.BeginTransaction())
                {
                    await session.DeleteAsync(todo);
                    await transaction.CommitAsync();
                }
            }
        }
    }
}
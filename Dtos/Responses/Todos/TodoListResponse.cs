using System.Collections.Generic;
using WebApiNHibernateCrud.Entities;

namespace WebApiNHibernateCrud.Dtos.Responses.Todos
{
    public class TodoListResponse
    {
        public IEnumerable<TodoDto> Todos { get; set; }

        public static List<TodoDto> Build(List<Todo> todos)
        {
            var todoDtos = new List<TodoDto>(todos.Count);

            foreach (var todo in todos)
                todoDtos.Add(TodoDto.Build(todo));


            return todoDtos;
        }
    }
}
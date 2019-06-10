using System;
using WebApiNHibernateCrud.Dtos.Responses.Shared;
using WebApiNHibernateCrud.Entities;

namespace WebApiNHibernateCrud.Dtos.Responses.Todos
{
    public class TodoDetailsDto : SuccessResponse
    {
        public bool Completed { get; set; }

        public int Id { get; set; }

        public DateTime? UpdatedAt { get; set; }

        public string Description { get; set; }

        public DateTime? CreatedAt { get; set; }

        public string Title { get; set; }

        public static TodoDetailsDto Build(Todo todo)
        {
            return new TodoDetailsDto
            {
                Id = todo.Id,
                Title = todo.Title,
                Description = todo.Description,
                Completed = todo.Completed,
                CreatedAt = todo.CreatedAt,
                UpdatedAt = todo.UpdatedAt
            };
        }
    }
}
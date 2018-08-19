using System;

namespace OwinSelfHosted.Model
{
    public class Todo
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public bool IsDone { get; set; }

        public DateTime Created { get; set; }

        public override string ToString()
        {
            return $"{Id}, {Name}, {IsDone}";
        }
    }
}
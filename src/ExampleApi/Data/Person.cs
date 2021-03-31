using System;

namespace ExampleApi.Data
{
    public class Person
    {
        protected Person()
        {
            // Required by ORM
        }

        public Person(string firstName, string lastName)
        {
            Id = Guid.NewGuid();
            FirstName = firstName;
            LastName = lastName;
        }
        
        public Guid Id { get; protected set; }
        public string FirstName { get; protected set; }
        public string LastName { get; protected set; }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Specs
{
    public class Program
    {
        static void Main(string[] args)
        {
            var userRepository = new Repository<User>();

            var firat = new User
            {
                Name = "Firat",
                Age = 35,
                IsVerified = true
            };

            
            var bradPitt = new User
            {
                Name = "Brad",
                Age = 48,
                IsVerified = false
            };


            var leo = new User
            {
                Name = "Leo",
                Age = 44,
                IsVerified = true
            };

            userRepository.Add(firat);
            userRepository.Add(bradPitt);
            userRepository.Add(leo);

            var verificationSpec = new VerificationSpecification();
            var filteredUsers = userRepository.Filter(verificationSpec);

            var jason = new User
            {
                Name = "Json Statham",
                Age = 42,
                IsVerified = true
            };

            if(verificationSpec.IsSatisfiedBy(jason))
            {
                Console.WriteLine("User is verified");
            }

            Console.Read();
        }
    }

    public class User
    {
        public string Name { get; set; }
        public int Age { get; set; }
        public bool IsVerified { get; set; }
    }

    public class Repository<T>
    {
        private readonly IList<T> members;

        public Repository()
        {
            members = new List<T>();   
        }

        public void Add(T member )
        {
            members.Add(member);
        }

        public IEnumerable<T> Filter(Specification<T> spec)
        {
            var expr = spec.Expression().Compile();
            return members.Where(expr);
        }
    }

    public abstract class Specification<T>
    {
        public abstract Expression<Func<T, bool>> Expression();
 
        public bool IsSatisfiedBy(T entity)
        {
            Func<T, bool> predicate = Expression().Compile();
    
            return predicate(entity);
        }
    }

    public class VerificationSpecification : Specification<User>
    {
        public override Expression<Func<User, bool>> Expression()
        {
            return p => p.IsVerified == true;
        }
    }
}

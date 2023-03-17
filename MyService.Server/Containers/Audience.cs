using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MyService.Server.Models;
namespace MyService.Server.Containers
{

    public interface Context
    {

    }
    public class ApplicationContext 
    {
        public  List<User> user;
        public ApplicationContext() {
                this.user = new List<User>
            {
                new User {Id = new Guid() ,  Name ="Eugene" }
            };
        }

        public async Task<User?> checkuser(User user)
        {
            return this.user.FirstOrDefault(p => (user.Name == p.Name));
        }

      

    }
}

using System.Data.Entity;

namespace Homie.Model
{
    public class DatabaseInitializer : DropCreateDatabaseIfModelChanges<DatabaseContext>
    {
        protected override void Seed(DatabaseContext context)
        {
            context.Users.Add(new User()
            {
                Username = "lemked",
                EmailAddress = "lemked@web.de",
                FirstName = "Daniel",
                LastName = "Lemke"
            });

            context.Machines.Add(new Machine()
            {
                MacAddress = "00-00-00-00-00-00-00-E0",
                NameOrAddress = "machine1",
                Port = 1234
            });

            base.Seed(context);
        }
    }
}

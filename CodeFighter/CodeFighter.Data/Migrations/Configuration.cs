namespace CodeFighter.Data.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<CodeFighter.Data.CodeFighterContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(CodeFighter.Data.CodeFighterContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //

            /**** ACTIONS ****/
            context.ActionData.AddOrUpdate(x => x.Id,
                new ActionData() { Id = 1, Name = "RepairPart", Description = "Repair This Part", TargetSelfPart = true, ActionValuesJSON = @"""RepairAmount"":5" },
                new ActionData() { Id = 2, Name = "RepairShip", Description = "Repair This Ship", TargetSelfShip = true, ActionValuesJSON = @"""RepairAmount"":5" }
                );

            /**** PARTS ****/

            /**** SHIPHULLS ****/

            /**** SHIPS ****/

            /**** FEATURES ****/

        }
    }
}

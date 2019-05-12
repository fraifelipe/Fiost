using FeedService.Domain.Write.States;
using FluentNHibernate.Mapping;

namespace FeedService.Infrastructure.Persistence.Maps.Write
{
    public class PersonStateMap: ClassMap<PersonState>
    {
        public PersonStateMap()
        {
            Table("[Person]");
            Id(x => x.PersonId).GeneratedBy.GuidComb();
            Map(x => x.UserId).Not.Nullable();
            Map(x => x.Username);
            Map(x => x.Name);
            Map(x => x.Email);
            
            HasManyToMany(x => x.Following)
                .Cascade.All()
                //.Inverse()
                .ParentKeyColumn("FollowingId")
                .ChildKeyColumn("PersonId")
                .Table("Follows");
        }
    }
}